using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _service;

        public TagController(TagService service)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagDto dto)
        {
            try { return Created("", await _service.Create(dto)); }
            catch (ErrorServiceException e) { return e.ToActionResult(this); }
            catch (Exception e) { return Problem(e.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TagDto dto)
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