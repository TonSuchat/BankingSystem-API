
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class MasterIBAN
    {
        [Key]
        public string IBAN { get; set; }
        public bool Used { get; set; }
    }
}
