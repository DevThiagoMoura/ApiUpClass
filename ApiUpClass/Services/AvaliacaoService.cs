using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class AvaliacaoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AvaliacaoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Avaliacao>> FindAll()
        {
            return await _context.Avaliacoes
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .ToListAsync();
        }

        public async Task<Avaliacao> FindById(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (avaliacao is null)
            {
                throw new ErrorServiceException(
                    "Avaliação não encontrada",
                    c => c.NotFound(new { message = $"Avaliação #{id} não encontrada" })
                );
            }

            return avaliacao;
        }

        public async Task<Avaliacao> Create(AvaliacaoDto data)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(x => x.Id == data.UsuarioId);
            var cursoExiste = await _context.Cursos.AnyAsync(x => x.Id == data.CursoId);

            if (!usuarioExiste)
            {
                throw new ErrorServiceException(
                    "Usuario nao encontrado",
                    c => c.NotFound(new { message = $"Usuario #{data.UsuarioId} não encontrado" })
                );
            }

            if (!cursoExiste)
            {
                throw new ErrorServiceException(
                    "Curso não encontrado",
                    c => c.NotFound(new { message = $"Curso #{data.CursoId} não encontrado" })
                );
            }

            var avaliacaoExiste = await _context.Avaliacoes
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId);

            if (avaliacaoExiste)
            {
                throw new ErrorServiceException(
                    "Avaliação já existente",
                    c => c.Conflict(new { message = "O usuario já avaliou este curso" })
                );
            }

            var avaliacao = _mapper.Map<Avaliacao>(data);

            await _context.Avaliacoes.AddAsync(avaliacao);
            await _context.SaveChangesAsync();

            return avaliacao;
        }

        public async Task<Avaliacao> Update(int id, AvaliacaoUpdateDto data)
        {
            var avaliacao = await FindById(id);

            _mapper.Map(data, avaliacao);

            _context.Avaliacoes.Update(avaliacao);
            await _context.SaveChangesAsync();

            return avaliacao;
        }

        public async Task Remove(int id)
        {
            var avaliacao = await FindById(id);

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }
    }
}