using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stratis.STOPlatform.Data.Entities
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : Entity
    {
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }
        public string WalletAddress { get; set; }

        [ConcurrencyCheck]
        public DateTime LastCheck { get; set; }

        public List<Deposit> Deposits { get; set; }
    }
}