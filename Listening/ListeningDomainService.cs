
using DomainCommons;
using Listening.Entities;

namespace Listening.Domain
{
    public class ListeningDomainService
    {
        private readonly IListeningRepository _repository;
        public ListeningDomainService(IListeningRepository repository) {
            _repository = repository;
        }

        public async Task<Album> AddAlbumAsync(Guid categoryId, MultilingualString name)
        {
            int maxSeq = await _repository.GetMaxSeqOfAlbumsAsync(categoryId);
            var id = Guid.NewGuid();
            return Album.Create(id, maxSeq, name, categoryId);
        }
    }
}
