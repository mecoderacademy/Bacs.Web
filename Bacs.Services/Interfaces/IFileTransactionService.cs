using Bacs.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Services.Interfaces
{
    public interface IFileTransactionService
    {
        IEnumerable<FileTransaction> GetAll();


        FileTransaction GetByID(int Id);


        void Insert(FileTransaction Entity);
        
    }
}
