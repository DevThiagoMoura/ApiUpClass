using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class CategoriaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Categoria>> FindAll()
        {
            return await _context.Categorias
                .Include(c => c.Cursos)
                .ToListAsync();
        }

        public async Task<Categoria> FindById(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Cursos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria is null)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{id} nao encontrada" })
                );
            }

            return categoria;
        }

        public async Task<Categoria> Create(CategoriaDto data)
        {
            var categoria = _mapper.Map<Categoria>(data);

            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria> Update(int id, CategoriaDto data)
        {
            var categoria = await FindById(id);

            _mapper.Map(data, categoria);

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task Remove(int id)
        {
            var categoria = await FindById(id);

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
