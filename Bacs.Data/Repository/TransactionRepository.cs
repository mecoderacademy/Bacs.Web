using Bacs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Bacs.Data.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>
    {
        public TransactionRepository(BacsContext context) : base(context)
        {
        }

    }
}

