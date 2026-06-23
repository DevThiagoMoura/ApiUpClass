using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<ICollection<CategoriaResponseDto>> FindAll()
        {
            return await _context.Categorias
                .ProjectTo<CategoriaResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CategoriaResponseDto> FindById(int id)
        {
            var categoria = await FindEntityById(id);

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        private async Task<Categoria> FindEntityById(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria is null)
            {
                throw new ErrorServiceException(
                    "Categoria nao encontrada",
                    c => c.NotFound(new { message = $"Categoria #{id} nao encontrada" })
                );
            }

            return categoria;
        }

        public async Task<CategoriaResponseDto> Create(CategoriaDto data)
        {
            var categoria = _mapper.Map<Categoria>(data);

            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        public async Task<CategoriaResponseDto> Update(int id, CategoriaDto data)
        {
            var categoria = await FindEntityById(id);

            _mapper.Map(data, categoria);

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        public async Task Remove(int id)
        {
            var categoria = await FindEntityById(id);

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
