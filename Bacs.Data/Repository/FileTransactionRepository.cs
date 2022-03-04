using Bacs.Models;
using Bacs.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Bacs.Data.Repository
{
    public class FileTransactionRepository : GenericRepository<FileTransaction>
    {
        public FileTransactionRepository(BacsContext context) : base(context)
        {
        }
    }
}

