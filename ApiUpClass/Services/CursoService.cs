using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class CursoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CursoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Curso>> FindAll()
        {
            return await _context.Cursos
                .Include(c => c.Categoria)
                .ToListAsync();
        }

        public async Task<Curso> FindById(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso is null)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{id} nao encontrado" })
                );
            }

            return curso;
        }

        public async Task<Curso> Create(CursoDto data)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == data.CategoriaId);

            if (!categoriaExiste)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} nao encontrada" })
                );
            }

            var curso = _mapper.Map<Curso>(data);

            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();

            return curso;
        }

        public async Task<Curso> Update(int id, CursoUpdateDto data)
        {
            var curso = await FindById(id);

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == data.CategoriaId);

            if (!categoriaExiste)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{data.CategoriaId} nao encontrada" })
                );
            }

            _mapper.Map(data, curso);

            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();

            return curso;
        }

        public async Task Remove(int id)
        {
            var curso = await FindById(id);

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }
    }
}
