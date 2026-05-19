using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class MatriculaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MatriculaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Matricula>> FindAll()
        {
            return await _context.Matriculas
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .ToListAsync();
        }

        public async Task<Matricula> FindById(int id)
        {
            var matricula = await _context.Matriculas
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (matricula is null)
            {
                throw new ErrorServiceException(
                    "Matricula nao encontrada",
                    c => c.NotFound(new { message = $"Matricula #{id} nao encontrada" })
                );
            }

            return matricula;
        }

        public async Task<Matricula> Create(MatriculaDto data)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(x => x.Id == data.UsuarioId);
            var cursoExiste = await _context.Cursos.AnyAsync(x => x.Id == data.CursoId);

            if (!usuarioExiste)
            {
                throw new ErrorServiceException(
                    "Usuario nao encontrado",
                    c => c.NotFound(new { message = $"Usuario #{data.UsuarioId} nao encontrado" })
                );
            }

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} nao encontrado" })
                );
            }

            var matriculaExiste = await _context.Matriculas
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId);

            if (matriculaExiste)
            {
                throw new ErrorServiceException(
                    "Matricula ja existente",
                    c => c.Conflict(new { message = "O usuario ja esta matriculado neste curso" })
                );
            }

            var matricula = _mapper.Map<Matricula>(data);

            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();

            return matricula;
        }

        public async Task<Matricula> Update(int id, MatriculaUpdateDto data)
        {
            var matricula = await FindById(id);

            var usuarioExiste = await _context.Usuarios.AnyAsync(x => x.Id == data.UsuarioId);
            var cursoExiste = await _context.Cursos.AnyAsync(x => x.Id == data.CursoId);

            if (!usuarioExiste)
            {
                throw new ErrorServiceException(
                    "Usuario nao encontrado",
                    c => c.NotFound(new { message = $"Usuario #{data.UsuarioId} nao encontrado" })
                );
            }

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso nao encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} nao encontrado" })
                );
            }

            _mapper.Map(data, matricula);

            _context.Matriculas.Update(matricula);
            await _context.SaveChangesAsync();

            return matricula;
        }

        public async Task Remove(int id)
        {
            var matricula = await FindById(id);

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();
        }
    }
}