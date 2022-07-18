using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
