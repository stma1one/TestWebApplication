using Microsoft.AspNetCore.Mvc;
using ProjectBL.Models;
using System.Text.Json;

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

        //העלאה של קובץ
        //id- מזהה רשומה מהדאטבייס
        [Route("UploadImage")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromQuery]int Id,IFormFile file)
        {

            User u = this.context.Users.Find(Id);
            

            //check file size
            if (file.Length > 0)
            {
                // Generate unique file name
                string fileName = $"{u.Id}{Path.GetExtension(file.FileName)}";

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                try
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    return Ok();
                }
                catch(Exception ex) { Console.WriteLine(ex.Message); }
            }

            return BadRequest();
        }

        //כשרוצים קובץ וגם אובייקט
        //היוזר מגיע בפורמט json
        [Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file,[FromForm]string user)
        {

            User? p=JsonSerializer.Deserialize<User>(user);  
            User? u = this.context.Users.Find(p.Id);


            //check file size
            if (file.Length > 0)
            {
                // Generate unique file name
                string fileName = $"{u.Id}{Path.GetExtension(file.FileName)}";

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                try
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    return Ok();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            return BadRequest();
        }

    }


}