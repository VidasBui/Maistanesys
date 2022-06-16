namespace Maistanesys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(12)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public string? Password { get; set; }

        public bool IsAdmin { get; set; }

        public int Phone { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public string? Email { get; set; }

    }
}
