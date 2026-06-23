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
    public class AulaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AulaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<AulaResponseDto>> FindAll()
        {
            return await _context.Aulas
                .ProjectTo<AulaResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AulaResponseDto> FindById(int id)
        {
            var aula = await FindEntityById(id);

            return _mapper.Map<AulaResponseDto>(aula);
        }

        private async Task<Aula> FindEntityById(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Modulo)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula is null)
            {
                throw new ErrorServiceException(
                    "Aula nao encontrada",
                    c => c.NotFound(new { message = $"Aula #{id} nao encontrada" })
                );
            }

            return aula;
        }

        public async Task<AulaResponseDto> Create(AulaDto data)
        {
            var moduloExiste = await _context.Modulos
                .AnyAsync(m => m.Id == data.ModuloId);

            if (!moduloExiste)
            {
                throw new ErrorServiceException(
                    "Modulo nao encontrado",
                    c => c.NotFound(new { message = $"Modulo #{data.ModuloId} nao encontrado" })
                );
            }

            var aula = _mapper.Map<Aula>(data);

            await _context.Aulas.AddAsync(aula);
            await _context.SaveChangesAsync();

            return await FindById(aula.Id);
        }

        public async Task<AulaResponseDto> Update(int id, AulaUpdateDto data)
        {
            var aula = await FindEntityById(id);

            var moduloExiste = await _context.Modulos
                .AnyAsync(m => m.Id == data.ModuloId);

            if (!moduloExiste)
            {
                throw new ErrorServiceException(
                    "Modulo nao encontrado",
                    c => c.NotFound(new { message = $"Modulo #{data.ModuloId} nao encontrado" })
                );
            }

            _mapper.Map(data, aula);

            _context.Aulas.Update(aula);
            await _context.SaveChangesAsync();

            return await FindById(aula.Id);
        }

        public async Task Remove(int id)
        {
            var aula = await FindEntityById(id);

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
        }
    }
}
