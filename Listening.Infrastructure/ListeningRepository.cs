using Listening.Domain;
using Listening.Domain.Entities;
using Listening.Entities;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure
{
    public class ListeningRepository : IListeningRepository
    {
        private readonly ListeningDbContext _context;
        public ListeningRepository(ListeningDbContext context) 
        {
            _context = context;
        }

        public async Task<Album?> GetAlbumByIdAsync(Guid albumId)
        {
            return await _context.FindAsync<Album>(albumId);
        }

        public async Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Albums.OrderBy(c => c.SequenceNumber).Where(a => a.CategoryId == categoryId).ToArrayAsync();
        }

        public async Task<Category[]> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(e=>e.SequenceNumber).ToArrayAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<Episode?> GetEpisodeByIdAsync(Guid episodeId)
        {
            return await _context.Episodes.FindAsync(episodeId);
        }

        public async Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId)
        {
            return await _context.Episodes.OrderBy(o => o.SequenceNumber).Where(c => c.AlbumId == albumId).ToArrayAsync();
        }

        public async Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId)
        {
            return (await _context.Albums.MaxAsync(c => (int?)c.SequenceNumber)) ?? 0;
        }

        public async Task<int> GetMaxSeqOfCategoriesAsync()
        {
            return (await _context.Categories.MaxAsync(c => (int?)c.SequenceNumber)) ?? 0;
        }

        public async Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId)
        {
            return (await _context.Episodes.MaxAsync(c => (int?)c.SequenceNumber)) ?? 0;
        }
    }
}
