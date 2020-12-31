using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stratis.STOPlatform.Data.Entities
{
    public class Deposit : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string TransactionId { get; set; }
        public ulong Invested { get; set; }

        public ulong EarnedToken { get; set; }

        public ulong Refunded { get; set; }

        public DateTime Date { get; set; }
    }
}