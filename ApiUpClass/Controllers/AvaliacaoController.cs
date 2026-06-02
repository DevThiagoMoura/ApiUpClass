using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/avaliacoes")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AvaliacaoService _service;

        public AvaliacaoController(AvaliacaoService service)
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
        public async Task<IActionResult> Create([FromBody] AvaliacaoDto dto)
        {
            try { return Created("", await _service.Create(dto)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AvaliacaoUpdateDto dto)
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
