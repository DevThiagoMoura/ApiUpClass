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
    public class AvaliacaoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AvaliacaoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<AvaliacaoResponseDto>> FindAll()
        {
            return await _context.Avaliacoes
                .ProjectTo<AvaliacaoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AvaliacaoResponseDto> FindById(int id)
        {
            var avaliacao = await FindEntityById(id);

            return _mapper.Map<AvaliacaoResponseDto>(avaliacao);
        }

        private async Task<Avaliacao> FindEntityById(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (avaliacao is null)
            {
                throw new ErrorServiceException(
                    "Avaliacao nao encontrada",
                    c => c.NotFound(new { message = $"Avaliacao #{id} nao encontrada" })
                );
            }

            return avaliacao;
        }

        public async Task<AvaliacaoResponseDto> Create(AvaliacaoDto data)
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

            var matriculaAtiva = await _context.Matriculas
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId && x.Status == "ativo");

            if (!matriculaAtiva)
            {
                throw new ErrorServiceException(
                    "Matricula nao encontrada",
                    c => c.Conflict(new { message = "O usuario precisa estar matriculado no curso para avaliar" })
                );
            }

            var avaliacaoExiste = await _context.Avaliacoes
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId);

            if (avaliacaoExiste)
            {
                throw new ErrorServiceException(
                    "Avaliacao ja existente",
                    c => c.Conflict(new { message = "O usuario ja avaliou este curso" })
                );
            }

            var avaliacao = _mapper.Map<Avaliacao>(data);

            await _context.Avaliacoes.AddAsync(avaliacao);
            await _context.SaveChangesAsync();

            return await FindById(avaliacao.Id);
        }

        public async Task<AvaliacaoResponseDto> Update(int id, AvaliacaoUpdateDto data)
        {
            var avaliacao = await FindEntityById(id);

            var matriculaAtiva = await _context.Matriculas
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId && x.Status == "ativo");

            if (!matriculaAtiva)
            {
                throw new ErrorServiceException(
                    "Matricula nao encontrada",
                    c => c.Conflict(new { message = "O usuario precisa estar matriculado no curso para avaliar" })
                );
            }

            var avaliacaoExiste = await _context.Avaliacoes
                .AnyAsync(x => x.UsuarioId == data.UsuarioId && x.CursoId == data.CursoId && x.Id != id);

            if (avaliacaoExiste)
            {
                throw new ErrorServiceException(
                    "Avaliacao ja existente",
                    c => c.Conflict(new { message = "O usuario ja avaliou este curso" })
                );
            }

            _mapper.Map(data, avaliacao);

            _context.Avaliacoes.Update(avaliacao);
            await _context.SaveChangesAsync();

            return await FindById(avaliacao.Id);
        }

        public async Task Remove(int id)
        {
            var avaliacao = await FindEntityById(id);

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }
    }
}
