using Microsoft.AspNetCore.Mvc;
using ProjectBL.Models;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WallaController : ControllerBase
    {
        WallaDbContext context;



        public WallaController(WallaDbContext context)
        {
            this.context = context;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] User usr)
        {

            User user = null;
            //Search for user
            //Check user name and password
            user = context.Users.Where((u) => u.Email == usr.Email && u.UserPswd == usr.UserPswd).FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetObject("user", user);
                return Ok(user);
            }


            return Forbid();

        }

        /// <summary>
        /// Retrieves user
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>user email</returns>
        /// <response code="200">email</response>
        /// <response code="404">no such user</response>

        [Route("GetUserEmail")]
        [HttpGet]

        public async Task<ActionResult> GetUserEmail([FromQuery] int id)
        {
            User u = context.Users.Find(id);
            if (u != null) { return Ok(u.Email); }

            return NotFound();
        }

    }


}