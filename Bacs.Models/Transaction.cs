using Bacs.Models.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bacs.Models
{
    public class Transaction
    {

        [Key]
        public int Id { get; set; }
        public Guid TransactionID { get; set; } = Guid.NewGuid();
        public string code { get; set; }
        public string Name { get; set; }
        public string DirectDebitReference { get; set; }
        public double Value { get; set; }
        public FileTransaction FileTransaction { get; set; }

    }
}
