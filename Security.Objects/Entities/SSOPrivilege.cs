using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Objects.Entities
{
    public class SSOPrivilege
    {
        [Key]
        [StringLength(200)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Operation { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool PortalAdminsOnly { get; set; }
    }
}