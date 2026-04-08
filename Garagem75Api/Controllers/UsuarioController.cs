using AutoMapper;
using Garagem75.Api.Data;
using Garagem75.Shared.Dtos;
using Garagem75.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Garagem75.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Garagem75DBContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsuarioController(Garagem75DBContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var lista = await _context.Usuarios
                //.Include(u => u.TipoUsuario)
                //.Where(u => u.Ativo) // 🔥 IMPORTANTE
                .ToListAsync();
            Console.WriteLine($"TOTAL DE USUARIOS NO BANCO: {lista.Count}");

            return Ok(_mapper.Map<List<UsuarioDto>>(lista));
        }

        [Authorize(Roles = "Administrador")]

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var item = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<UsuarioDto>(item));
        }
        [Authorize(Roles = "Administrador")]

        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto dto)
        {
            var entity = _mapper.Map<Usuario>(dto);

            _context.Usuarios.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UsuarioDto>(entity);

            return CreatedAtAction(nameof(GetById),
                new { id = result.IdUsuario }, result);
        }
        [Authorize(Roles = "Administrador")]

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest();

            var entity = await _context.Usuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize(Roles = "Administrador")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Usuarios.FindAsync(id);

            if (entity == null)
                return NotFound();

            entity.Ativo = false; // 🔥 NÃO DELETA MAIS

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize(Roles = "Administrador")]

        [HttpPut("{id}/reativar")]
        public async Task<IActionResult> Reativar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            usuario.Ativo = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {

            Console.WriteLine($"EMAIL: '{login.Email}'");
            Console.WriteLine($"SENHA: '{login.Senha}'");
            var user = await _context.Usuarios
    .Include(u => u.TipoUsuario)
    .FirstOrDefaultAsync(u =>
        u.Email.ToLower().Trim() == login.Email.ToLower().Trim() &&
        u.Senha.Trim() == login.Senha.Trim() &&
        u.Ativo);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            var chaveJwt = _configuration["Jwt:ChaveSecreta"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Nome),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.TipoUsuario.DescricaoTipoUsuario)
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                nome = user.Nome,
                tipo = user.TipoUsuario.DescricaoTipoUsuario
            });
        }
    }
}