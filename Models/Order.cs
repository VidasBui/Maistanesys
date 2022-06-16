using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Maistanesys.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       /* Neapmoketas,
        Apmoketas, 
        Atsauktas,
        Gaminamas,
        Pagamintas,
        Pristatomas,
        Uzsakymas_atliktas*/
        public State State { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public string DeliveryAddress { get; set; }

        public DateTime EstimatedTime { get; set; }
        public virtual ICollection<Item>? item { get; set; }
        public int UserId  { get; set; }
    }
}
