using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class UserRole
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public long RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
