using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Maistanesys.Models
{

    public class Courrier
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public string Name { get; set; }

        public bool IsAvailable { get; set; }
    }
}
