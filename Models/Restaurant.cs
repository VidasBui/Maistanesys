namespace Maistanesys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(12)")]
        public string? Name { get; set; }
        [Column(TypeName = "nvarchar(12)")]
        public string? Address { get; set; }

    }
}
