using BookLib_Auth_Mapp_NF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookLib_Auth_Mapp_NF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Admins")]
        //[Authorize(Roles = "Administrator")] для ограничения доступа к определенному методу или контроллеру на основе ролей пользователей
        // Атрибут применяется для того, чтобы разрешить доступ только тем пользователям, которые принадлежат к роли Administrator
        // Пользователь должен быть аутентифицирован, только после этого происходит проверка его ролей        
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.UserName}, you are an {currentUser.Role}"); 
        }

        [HttpGet("LibraryUser")]        
        public IActionResult LibraryUsersEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.UserName}, you are an {currentUser.Role}");
        }
       
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return null; 
            var usercClaims = identity.Claims;
            return new UserModel
            {
                UserName = usercClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value,
                HashPassword = usercClaims.FirstOrDefault(h=>h.Type==ClaimTypes.Hash)?.Value,   
                Role = usercClaims.FirstOrDefault(r => r.Type == ClaimTypes.Role)?.Value
            };
        }
    }
}
