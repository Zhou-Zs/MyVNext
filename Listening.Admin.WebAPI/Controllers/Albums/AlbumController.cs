using AsmResolver.PE;
using Commons.Validators;
using Listening.Domain;
using Listening.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Admin.WebAPI.Controllers.Albums
{
    [Route("[controller]/[action]")]
    [ApiController]
    []
    public class AlbumController : ControllerBase
    {
        private readonly IListeningRepository _listeningRepository;
        public AlbumController(IListeningRepository listeningRepository)
        {
            _listeningRepository = listeningRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Album?>> FindById([RequiredGuid] Guid id)
        {
            var album = await _listeningRepository.GetAlbumByIdAsync(id);
            return album;
        }
    }
}
