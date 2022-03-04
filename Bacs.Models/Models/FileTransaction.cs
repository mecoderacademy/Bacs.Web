using Bacs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Models.Models
{
    public class FileTransaction: IFileResponse
    {
        [Key]
        public int Id { get; set; }
        public Guid FileId { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public double TotalAmount { get; set; }
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>(); 
        public string ResponseMessage { get; set; }
    }
}
