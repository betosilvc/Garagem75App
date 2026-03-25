using Garagem75.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garagem75.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdminController : ControllerBase
    {
        private readonly Garagem75DBContext _context;

        public BlogAdminController(Garagem75DBContext dbContext)
        {
            _context = dbContext;
        }




    }
}
