using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
