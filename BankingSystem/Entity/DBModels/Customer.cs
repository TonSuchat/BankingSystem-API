using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
