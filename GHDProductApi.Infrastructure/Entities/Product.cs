using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHDProductApi.Infrastructure.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }
    }
}
