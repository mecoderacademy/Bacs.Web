using Bacs.Data;
using Bacs.Data.Repository;
using Bacs.Models;
using Bacs.Models.Models;
using Bacs.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Services.Service
{
    public class FileTransactionService: IFileTransactionService
    {
        private readonly FileTransactionRepository _fileTransactionRepository;
        private readonly ITransactionService _transactionService;
        public FileTransactionService(FileTransactionRepository fileTransactionRepository, ITransactionService transactionService)
        {
            _fileTransactionRepository=fileTransactionRepository;
            _transactionService = transactionService;
        }

        public IEnumerable<FileTransaction> GetAll()
        {
           return _fileTransactionRepository.GetAll();
        }

        public FileTransaction GetByID(int Id)
        {
            return _fileTransactionRepository.GetByID(Id);
        }

        public void Insert(FileTransaction fileTransaction)
        {
            fileTransaction.Transactions.ToList().ForEach(transaction =>
            {
                _transactionService.Insert(transaction);
            });
            _fileTransactionRepository.Insert(fileTransaction);
            _fileTransactionRepository.Save();
        }

    }
}
