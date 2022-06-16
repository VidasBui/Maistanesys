using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maistanesys.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public string? Name { get; set; }

        public float Price { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public int Amount { get; set; }
        public virtual ICollection<Order>? order { get; set; }
    }
}
