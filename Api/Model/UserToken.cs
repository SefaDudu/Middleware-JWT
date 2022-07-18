using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class UserToken
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }

        public string Token { get; set; }
        public DateTime expiresTime { get; set; }
        public bool IsDelete { get; set; }

        public User User { get; set; }
    }
}
