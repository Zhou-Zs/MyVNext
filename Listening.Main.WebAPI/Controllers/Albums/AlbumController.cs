using ASPNETCore;
using Commons.Validators;
using Listening.Domain;
using Listening.Entities;
using Listening.Main.WebAPI.Controllers.Albums.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Main.WebAPI.Controllers.Albums
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumController : ControllerBase
    {
        private readonly IListeningRepository _listeningRepository;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        public AlbumController(IListeningRepository listeningRepository, IMemoryCacheHelper memoryCacheHelper)
        {
            _memoryCacheHelper = memoryCacheHelper;
            _listeningRepository = listeningRepository;

        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<AlbumVM>> FindById([RequiredGuid] Guid id)
        {
            var album = await _memoryCacheHelper.GetOrCreateAsync($"AlbumController.FindById.{id}",
                async (e) => AlbumVM.Create(await _listeningRepository.GetAlbumByIdAsync(id)));
            if (album == null)
            {
                return NotFound();
            }
            return album;
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<ActionResult<AlbumVM[]>> FindByCategoryId([RequiredGuid] Guid categoryId)
        {
            //  写到单独的local函数的好处是避免回调中代码太复杂
            Task<Album[]> FindDataAsync()
            {
                return _listeningRepository.GetAlbumsByCategoryIdAsync(categoryId);
            }

            var task = _memoryCacheHelper.GetOrCreateAsync($"AlbumController.FindByCategoryId.{categoryId}",
                async (e) => AlbumVM.Create(await FindDataAsync()));

            return await task;
        }
    }
}
