using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public UsuarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<UsuarioResponseDto>> FindAll()
        {
            return await _context.Usuarios
                .ProjectTo<UsuarioResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UsuarioResponseDto> FindById(int id)
        {
            var usuario = await FindEntityById(id);

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        private async Task<Usuario> FindEntityById(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario is null)
            {
                throw new ErrorServiceException(
                    "Usuario nao encontrado",
                    c => c.NotFound(new { message = $"Usuario #{id} nao encontrado" })
                );
            }

            return usuario;
        }

        public async Task<UsuarioResponseDto> Create(UsuarioDto data)
        {
            var emailExiste = await _context.Usuarios.AnyAsync(x => x.Email == data.Email);

            if (emailExiste)
            {
                throw new ErrorServiceException(
                    "Email ja cadastrado",
                    c => c.Conflict(new { message = $"O email {data.Email} ja esta em uso" })
                );
            }

            var usuario = _mapper.Map<Usuario>(data);
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<UsuarioResponseDto> Update(int id, UsuarioUpdateDto data)
        {
            var usuario = await FindEntityById(id);

            var emailExiste = await _context.Usuarios.AnyAsync(x => x.Email == data.Email && x.Id != id);

            if (emailExiste)
            {
                throw new ErrorServiceException(
                    "Email ja cadastrado",
                    c => c.Conflict(new { message = $"O email {data.Email} ja esta em uso" })
                );
            }

            _mapper.Map(data, usuario);

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task Remove(int id)
        {
            var usuario = await FindEntityById(id);

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
