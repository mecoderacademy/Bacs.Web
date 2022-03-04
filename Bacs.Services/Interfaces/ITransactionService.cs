using Bacs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bacs.Services.Interfaces
{
    public interface ITransactionService
    {
        void Insert(Transaction Entity);
    }
}
