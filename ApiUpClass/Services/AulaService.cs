using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
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

        public async Task<ICollection<Aula>> FindAll()
        {
            return await _context.Aulas
                .Include(a => a.Modulo)
                .ThenInclude(m => m!.curso)
                .ToListAsync();
        }

        public async Task<Aula> FindById(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Modulo)
                .ThenInclude(m => m!.curso)
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

        public async Task<Aula> Create(AulaDto data)
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

            return aula;
        }

        public async Task<Aula> Update(int id, AulaUpdateDto data)
        {
            var aula = await FindById(id);

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

            return aula;
        }

        public async Task Remove(int id)
        {
            var aula = await FindById(id);

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
        }
    }
}