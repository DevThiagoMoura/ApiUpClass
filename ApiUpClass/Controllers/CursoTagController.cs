using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/cursos-tags")]
    [ApiController]
    public class CursoTagController : ControllerBase
    {
        private readonly CursoTagService _service;

        public CursoTagController(CursoTagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try { return Ok(await _service.FindAll()); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CursoTagDto dto)
        {
            try { return Created("", await _service.Create(dto)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] int cursoId, [FromQuery] int tagId)
        {
            try { await _service.Remove(cursoId, tagId); return NoContent(); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }
    }
}