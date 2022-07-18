using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class Role
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserRole> UserRole { get; set; }
    }
}
