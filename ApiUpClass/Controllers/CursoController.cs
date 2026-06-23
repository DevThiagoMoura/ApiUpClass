using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/cursos")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly CursoService _service;

        public CursoController(CursoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var cursos = await _service.FindAll();

                return Ok(cursos);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var curso = await _service.FindById(id);

                return Ok(curso);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("ativos")]
        public async Task<IActionResult> FindActive()
        {
            try
            {
                var cursos = await _service.FindActive();

                return Ok(cursos);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> FindByCategoria(int categoriaId)
        {
            try
            {
                var cursos = await _service.FindByCategoria(categoriaId);

                return Ok(cursos);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("tag/{tagId}")]
        public async Task<IActionResult> FindByTag(int tagId)
        {
            try
            {
                var cursos = await _service.FindByTag(tagId);

                return Ok(cursos);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("{id}/modulos")]
        public async Task<IActionResult> FindModulos(int id)
        {
            try
            {
                var modulos = await _service.FindModulos(id);

                return Ok(modulos);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Authorize(Roles = "instrutor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CursoDto novoCurso)
        {
            try
            {
                var curso = await _service.Create(novoCurso);

                return Created("", curso);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Authorize(Roles = "instrutor")]
        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTags(int id, [FromBody] CursoTagsDto tags)
        {
            try
            {
                var curso = await _service.AddTags(id, tags);

                return Ok(curso);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CursoUpdateDto cursoDto)
        {
            try
            {
                var curso = await _service.Update(id, cursoDto);

                return Ok(curso);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);

                return NoContent();
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
