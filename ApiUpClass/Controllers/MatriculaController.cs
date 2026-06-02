using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/matriculas")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly MatriculaService _service;

        public MatriculaController(MatriculaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try { return Ok(await _service.FindAll()); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try { return Ok(await _service.FindById(id)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [Authorize(Roles = "aluno")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatriculaDto dto)
        {
            try { return Created("", await _service.Create(dto)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MatriculaUpdateDto dto)
        {
            try { return Ok(await _service.Update(id, dto)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try { await _service.Remove(id); return NoContent(); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }
    }
}
