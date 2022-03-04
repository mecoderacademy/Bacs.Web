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
    public class TransactionService: ITransactionService { 
       
        private readonly TransactionRepository _transactionRepository;
        public TransactionService(TransactionRepository transactionRepository) => _transactionRepository = transactionRepository;

        public IEnumerable<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public Transaction GetByID(int Id)
        {
           return _transactionRepository.GetByID(Id);
        }

        public void Insert(Transaction Entity)
        {
            _transactionRepository.Insert(Entity);
            Save();
        }

        public void Save()
        {
            _transactionRepository.Save();
        }
    }
}
