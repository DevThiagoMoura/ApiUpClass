using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
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

        public async Task<ICollection<Pagamento>> FindAll()
        {
            return await _context.Pagamentos
                .Include(x => x.Usuario)
                .Include(x => x.Curso)
                .ToListAsync();
        }

        public async Task<Pagamento> FindById(int id)
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

        public async Task<Pagamento> Create(PagamentoDto data)
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

            var pagamento = _mapper.Map<Pagamento>(data);

            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();

            return pagamento;
        }

        public async Task<Pagamento> Update(int id, PagamentoUpdateDto data)
        {
            var pagamento = await FindById(id);

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

            _mapper.Map(data, pagamento);

            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();

            return pagamento;
        }

        public async Task Remove(int id)
        {
            var pagamento = await FindById(id);

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
        }
    }
}