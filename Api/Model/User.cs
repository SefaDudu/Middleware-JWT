using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<UserRole> UserRole { get; set; }
    }
}
