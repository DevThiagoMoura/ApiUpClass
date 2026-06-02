using ApiUpClass.DataContexts;
using ApiUpClass.Dtos;
using ApiUpClass.Exceptions;
using ApiUpClass.Models;
using ApiUpClass.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public AuthService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> Login(LoginDto data)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == data.Email);

            if (usuario is null)
            {
                throw new ErrorServiceException(
                    "Credenciais invalidas",
                    c => c.Unauthorized(new { message = "Email ou senha invalidos" })
                );
            }

            var verificacao = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, data.Senha);

            if (verificacao == PasswordVerificationResult.Failed)
            {
                // Compatibilidade temporaria com usuarios antigos salvos em texto puro.
                if (usuario.SenhaHash != data.Senha)
                {
                    throw new ErrorServiceException(
                        "Credenciais invalidas",
                        c => c.Unauthorized(new { message = "Email ou senha invalidos" })
                    );
                }

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
            else if (verificacao == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, data.Senha);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }

            var token = _tokenService.GenerateToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Papel
                }
            };
        }
    }
}
