using ProjectBL.Models;

namespace TestWebApplication.DTO
{
    public class FileDto
    {
        public IFormFile file { get; set; }
        public User user { get; set; }
    }
}
