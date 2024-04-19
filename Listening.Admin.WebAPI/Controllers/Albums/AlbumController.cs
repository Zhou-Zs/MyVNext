using ASPNETCore;
using Commons.Validators;
using Listening.Admin.WebAPI.Controllers.Albums.Request;
using Listening.Domain;
using Listening.Entities;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Admin.WebAPI.Controllers.Albums
{
    [Route("[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(ListeningDbContext))]
    public class AlbumController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private readonly IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _domainService;
        public AlbumController(IListeningRepository listeningRepository, ListeningDbContext dbCtx, ListeningDomainService domainService)
        {
            _listeningRepository = listeningRepository;
            _dbCtx = dbCtx;
            _domainService = domainService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Album?>> FindById([RequiredGuid] Guid id)
        {
            var album = await _listeningRepository.GetAlbumByIdAsync(id);
            return album;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add(AlbumAddRequest req)
        {
            Album album = await _domainService.AddAlbumAsync(req.CategoryId, req.Name);
            _dbCtx.Add(album);
            return album.Id;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([RequiredGuid] Guid id, AlbumUpdateRequest req)
        {
            var album = await _listeningRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound("id没找到");
            }
            album.ChangeName(req.Name);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> DeleteById([RequiredGuid] Guid id)
        {
            var album = await _listeningRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound("id没找到");
            }
            album.SoftDelete();
            return Ok();
        }
    }
}
