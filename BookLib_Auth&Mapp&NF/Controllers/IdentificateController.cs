using AutoMapper;
using BookLib_Auth_Mapp_NF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookLib_Auth_Mapp_NF.Data;
using System.Globalization;
using System.Security.Cryptography;

namespace BookLib_Auth_Mapp_NF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificateController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;        
        public IdentificateController(IConfiguration configuration, UserContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;            
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public async Task<ActionResult> Authorize(string userName, string password)
        {
            string hashedPassword = HashPassword(password); 
            var currentUser = await _context.UserModel
           .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower()
                               && u.HashPassword.ToLower() == hashedPassword.ToLower());            
            if (currentUser == null) return NotFound("There's no user, please registrate");
            var token = GenerateToken(currentUser);
            return Ok(token);       
        }

        [AllowAnonymous]
        [HttpPost("Registrate")]
        public async Task<ActionResult> Registrate(string userName, string password)
        {
            string hashedPassword = HashPassword(password);
            string role = "LibraryUser";
            if (userName.Contains("Admin")) role = "Administrator";
            var checkUser = await _context.UserModel
           .FirstOrDefaultAsync
           (u => u.UserName.ToLower() == userName.ToLower() &&
            u.HashPassword.ToLower() == hashedPassword.ToLower());
            if (checkUser != null) return BadRequest("Please authorize, user already exists"); // проверили есть ли пользовель уже в бд 
            // если нет, то создаем нового и добавляем его в КОНТЕКСТ,  а потом в бд            
            var newUserModel = new UserModel// добавляем объект типа UserModel!
            {
                UserName = userName,
                HashPassword = hashedPassword,
                Role = role
            };
            _context.UserModel.Add(newUserModel);
            await _context.SaveChangesAsync();
            var token = GenerateToken(newUserModel);
            return Ok("You're registrated succesfully\n" + token);// yeeeeeee                 

        }
        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var token = new JwtSecurityToken(
                _configuration["jwt:Issuer"],
                _configuration["jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();// работает 
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
