using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public object Email { get; private set; }
        public object Senha { get; private set; }

        // GET -> /api/usuarios
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_usuarioRepository.Listar());
        }

        // Novo código POST para auxiliar o método de Login.
        [HttpPost]
        public IActionResult Post(UsuariosController usuario)
        {
            Usuario usuarioBuscado = _usuarioRepository.Login(usuario.Email, usuario.Senha);

            if (usuarioBuscado == null)
            {
                return NotFound("E-mail ou senha inválidos!");
            }

            // Se o usuário for encontrado, segue a criação do token.
            // Define os dados que serão fornecidos no token - Payload.
            var claims = new[]
            {
                // Armazena na claim o e-mail do usuário autenticado.
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                // Armazena na claim o id do usuário autenticado.
                new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Id.ToString()),
            };

            // Define a chave de acesso ao token.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao"));

            // Define as credenciais do token.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Gera o token.
            var token = new JwtSecurityToken(
                issuer: "exoapi.webapi", // Emissor do token.
                audience: "exoapi.webapi", // Destinatário do token.
                claims: claims, // Dados definidos acima.
                expires: DateTime.Now.AddMinutes(30), // Tempo de expiração.
                signingCredentials: creds // Credenciais do token.
            );

            // Retorna ok com o token.
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        // Fim do novo código POST para auxiliar o método de Login.
    }

    internal class Usuario
    {
        public ClaimsIdentity Email { get; internal set; }
    }

    public class UsuarioRepository
    {
        internal object Listar()
        {
            throw new NotImplementedException();
        }

        internal Usuario Login(object email, object senha)
        {
            throw new NotImplementedException();
        }
    }
}
