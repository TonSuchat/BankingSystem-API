
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class MasterIBAN
    {
        [Key]
        [MaxLength(100)]
        public string IBAN { get; set; }
        [Required]
        public bool Used { get; set; }
    }
}
