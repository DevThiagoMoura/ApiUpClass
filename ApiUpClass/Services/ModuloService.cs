using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class ModuloService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ModuloService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Modulo>> FindAll()
        {
            return await _context.Modulos
                .Include(m => m.curso)
                .Include(m => m.aulas)
                .ToListAsync();
        }

        public async Task<Modulo> FindById(int id)
        {
            var modulo = await _context.Modulos
                .Include(m => m.curso)
                .Include(m => m.aulas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modulo is null)
            {
                throw new ErrorServiceException(
                    "Modulo nao encontrado",
                    c => c.NotFound(new { message = $"Modulo #{id} nao encontrado" })
                );
            }

            return modulo;
        }

        public async Task<Modulo> Create(ModuloDto data)
        {
            var cursoExiste = await _context.Cursos
                .AnyAsync(c => c.Id == data.CursoId);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} nao encontrado" })
                );
            }

            var ordemJaExiste = await _context.Modulos
                .AnyAsync(m => m.cursoId == data.CursoId && m.Ordem == data.Ordem);

            if (ordemJaExiste)
            {
                throw new ErrorServiceException(
                    "Ordem ja utilizada neste curso",
                    c => c.Conflict(new { message = $"Ja existe um modulo com a ordem {data.Ordem} neste curso" })
                );
            }

            var modulo = _mapper.Map<Modulo>(data);

            await _context.Modulos.AddAsync(modulo);
            await _context.SaveChangesAsync();

            return modulo;
        }

        public async Task<Modulo> Update(int id, ModuloUpdateDto data)
        {
            var modulo = await FindById(id);

            var cursoExiste = await _context.Cursos
                .AnyAsync(c => c.Id == data.CursoId);

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} nao encontrado" })
                );
            }

            var ordemJaExiste = await _context.Modulos
                .AnyAsync(m => m.cursoId == data.CursoId && m.Ordem == data.Ordem && m.Id != id);

            if (ordemJaExiste)
            {
                throw new ErrorServiceException(
                    "Ordem ja utilizada neste curso",
                    c => c.Conflict(new { message = $"Ja existe um modulo com a ordem {data.Ordem} neste curso" })
                );
            }

            _mapper.Map(data, modulo);

            _context.Modulos.Update(modulo);
            await _context.SaveChangesAsync();

            return modulo;
        }

        public async Task Remove(int id)
        {
            var modulo = await FindById(id);

            _context.Modulos.Remove(modulo);
            await _context.SaveChangesAsync();
        }
    }
}