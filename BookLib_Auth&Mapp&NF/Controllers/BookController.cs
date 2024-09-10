using BookLib_Auth_Mapp_NF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLib_Auth_Mapp_NF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _bookContext;
        public BookController(BookContext bookContext)
        {
            _bookContext = bookContext;
        }
        
        [HttpGet("Admins")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetBookAdminsEndpoint(string title)
        {
            var book = await _bookContext.Books.FirstOrDefaultAsync(b=>b.Title == title );
            if (book == null) return NotFound("This book not found"); 
            return Ok(book.Author);
        }

        [HttpGet("LibraryUser")]
        [Authorize(Roles = "LibraryUser")]
        public async Task<ActionResult> GetBookUsersEndpoint([FromBody] string title)
        {
            var book = await _bookContext.Books.FirstOrDefaultAsync(b => b.Title == title);
            if (book == null) return NotFound("This book not found");
            return Ok(book.Author);

        }


    }
}
