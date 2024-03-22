using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json;

namespace AnyDBConfigProvider
{
    public class DBConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private DBConfigOptions _options;
        private ReaderWriterLockSlim lockObj = new ReaderWriterLockSlim();
        private bool isDisposed = false;

        public DBConfigurationProvider(DBConfigOptions options)
        {
            _options = options;

            TimeSpan interval = TimeSpan.FromSeconds(3);
            if (options.ReloadInterval != null)
            {
                interval = options.ReloadInterval.Value;
            }

            if (options.ReloadOnChange)
            {
                ThreadPool.QueueUserWorkItem(obj => {
                    while (!isDisposed)
                    {
                        Load();
                        Thread.Sleep(interval);
                    }
                });
            }
        }

        public void Dispose()
        {
            isDisposed = true;
        }

        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
        {
            lockObj.EnterReadLock();
            try
            {
                return base.GetChildKeys(earlierKeys, parentPath);
            }
            finally
            {
                lockObj.ExitReadLock();
            }
        }

        public override bool TryGet(string key, out string? value)
        {
            lockObj.EnterReadLock();
            try
            {
                return base.TryGet(key, out value);
            }
            finally
            {

                lockObj.ExitReadLock();
            }
        }

        public override void Load()
        {
            base.Load();
            IDictionary<string, string> clonedData = null;
            try
            {
                lockObj.EnterWriteLock();
                clonedData = Data.Clone();
                Data.Clear();

                string tableName = _options.TableName;
                using (var conn = _options.CreateDbConnection())
                {
                    conn.Open();
                    DoLoad(tableName, conn);
                }
            }
            catch (DbException)
            {
                Data = clonedData;
                throw;
            }
            finally
            { 
                lockObj.ExitWriteLock();
            }

            if (Helper.IsChange(clonedData, Data))
            {
                OnReload();
            }
        }

        private void DoLoad(string tableName, IDbConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select Name,Value from {tableName} where Id in(select Max(Id) from {tableName} group by Name)";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        string name = reader.GetString(0);
                        string value = reader.GetString(1);
                        if (value == null)
                        {
                            this.Data[name] = value;
                            continue;
                        }
                        value = value.Trim();
                        if ((value.StartsWith("[") && value.EndsWith("]")) || (value.StartsWith("{") && value.EndsWith("}")))
                        {
                            TryLoadAsJson(name,value);
                        }
                        else
                        {
                            Data[name] = value;
                        }
                    }
                }
            }
        }

        private void LoadJsonElement(string name, JsonElement jsonRoot)
        {
            if (jsonRoot.ValueKind == JsonValueKind.Array)
            {
                int index = 0;
                foreach (var item in jsonRoot.EnumerateArray())
                {
                    //https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/
                    //parse as "a:b:0"="hello";"a:b:1"="world"
                    string path = name + ConfigurationPath.KeyDelimiter + index;
                    LoadJsonElement(path, item);
                    index++;
                }

            }
            else if (jsonRoot.ValueKind == JsonValueKind.Object)
            {
                foreach (var jsonObj in jsonRoot.EnumerateObject())
                {
                    string pathOfObj = name + ConfigurationPath.KeyDelimiter + jsonObj.Name;
                    LoadJsonElement(pathOfObj, jsonObj.Value);
                }
            }
            else
            {
                Data[name] = jsonRoot.GetValueForConfig();
            }
        }
        private void TryLoadAsJson(string name, string value)
        {
            // json配置（允许逗号结尾，有注释的直接忽略跳过）
            var jsonOptions = new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip };
            try
            {
                var jsonRoot = JsonDocument.Parse(value, jsonOptions).RootElement;
                LoadJsonElement(name, jsonRoot);
            }
            catch (JsonException ex)
            {
                // if it is not valid json,parse it as plain string value
                Data[name] = value;
                Debug.WriteLine($"When trying to parss {value} as json object,exception was thrown.{ex} ");
            }

        }
    }
}
