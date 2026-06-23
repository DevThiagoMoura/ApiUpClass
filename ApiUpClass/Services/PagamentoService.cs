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
    public class PagamentoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PagamentoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<PagamentoResponseDto>> FindAll()
        {
            return await _context.Pagamentos
                .ProjectTo<PagamentoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PagamentoResponseDto> FindById(int id)
        {
            var pagamento = await FindEntityById(id);

            return _mapper.Map<PagamentoResponseDto>(pagamento);
        }

        private async Task<Pagamento> FindEntityById(int id)
        {
            var pagamento = await _context.Pagamentos
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pagamento is null)
            {
                throw new ErrorServiceException(
                    "Pagamento nao encontrado",
                    c => c.NotFound(new { message = $"Pagamento #{id} nao encontrado" })
                );
            }

            return pagamento;
        }

        public async Task<PagamentoResponseDto> Create(PagamentoDto data)
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

            if (!matriculaExiste)
            {
                throw new ErrorServiceException(
                    "Matricula nao encontrada",
                    c => c.Conflict(new { message = "O usuario precisa estar matriculado no curso para realizar pagamento" })
                );
            }

            var pagamento = _mapper.Map<Pagamento>(data);

            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();

            return await FindById(pagamento.Id);
        }

        public async Task<PagamentoResponseDto> Update(int id, PagamentoUpdateDto data)
        {
            var pagamento = await FindEntityById(id);

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

            if (!matriculaExiste)
            {
                throw new ErrorServiceException(
                    "Matricula nao encontrada",
                    c => c.Conflict(new { message = "O usuario precisa estar matriculado no curso para realizar pagamento" })
                );
            }

            _mapper.Map(data, pagamento);

            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();

            return await FindById(pagamento.Id);
        }

        public async Task Remove(int id)
        {
            var pagamento = await FindEntityById(id);

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
        }
    }
}
