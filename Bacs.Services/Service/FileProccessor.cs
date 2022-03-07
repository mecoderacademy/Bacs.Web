using Bacs.Models.Interfaces;
using Bacs.Models.Models;
using Bacs.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Linq;
using System.IO;
using Bacs.Models;
using System.Reflection;
using System.Collections.Generic;

namespace Bacs.Services
{
    public class FileProccessor : IFileProccessor
    {
        private readonly IFileTransactionService _fileTransactionService;
        public FileProccessor(IFileTransactionService fileTransactionService)
        {
            _fileTransactionService = fileTransactionService;
        }
        public FileTransaction ReadFile(IFormFile formFile)
        {
            bool isValid = false;
            FileTransaction fileReadSuccess = new();

            if (formFile == null)
            {
                fileReadSuccess.ResponseMessage = "no file provided";// I would add these into a static constants class. would errors be logged somewhere for auditing?
                return fileReadSuccess;
            }

            fileReadSuccess.FileName = formFile.FileName;

            try
            {
                using (var reader = new StreamReader(formFile.OpenReadStream()))
                {
                    int readerCount = 0;
                    while (!reader.EndOfStream)
                    {
                        readerCount++;
                        var line = reader.ReadLine();
                        if (line != null && readerCount > 1)
                        {
                            var data = line.Split(','); // could validate using regular expression
                            double.TryParse(data[3], out double value);
                            if (value < 1.0 && value > 20000000.00) { fileReadSuccess.ResponseMessage = "Please supply a value between 1 and 20000000"; isValid = false; } // could add this to the transaction success model so each transaction can be validated
                            if (isValid)
                            {
                                fileReadSuccess.Transactions.Add(new Transaction()
                                {
                                    code = data[0],
                                    DirectDebitReference = data[1], // this proccess can be seperated into a new service for testing in isolation or test files can be provided for other edge cases
                                    Name = data[2],
                                    Value = value,
                                });
                            }
                          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fileReadSuccess.ResponseMessage = ex.Message;
                return fileReadSuccess;
            }
            
            fileReadSuccess.TotalAmount = fileReadSuccess.Transactions.Sum(x => x.Value);
           
            return fileReadSuccess;
        }

        private List<Transaction> ReadFile(IFormFile formFile)
        {

        }
        public IFileResponse ProccessFile(IFormFile formFile)
        {
            var fileReadResult = ReadFile(formFile);
            if (!string.IsNullOrEmpty(fileReadResult.ResponseMessage)) return new FileResponseBadValidation() { ResponseMessage = fileReadResult.ResponseMessage };
            _fileTransactionService.Insert(fileReadResult);
            var all = _fileTransactionService.GetAll();
            if(!ProccessBacsFile()) return new FileResponseBadValidation() { ResponseMessage = "Bacs File didnt proccess" };
            return new FileResponseSuccess { ResponseMessage = "successful upload" };
        }

        public bool ProccessBacsFile()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "backsFile.txt");
            if (!File.Exists(path))
                File.Create(path);
           
            var lineArray = new string[5];
            var all = _fileTransactionService.GetAll().ToList();
                int linecount = 0;
                using (var writer = new StreamWriter(path))
                {
                    try
                    {
                        all.FirstOrDefault().Transactions.ToList().ForEach(x =>
                        {
                            if (linecount== 0)
                            {
                                lineArray[0] = "Destination sort code  ";
                                lineArray[1] = "Destination account number ";
                                lineArray[2] = "Amount ";
                                lineArray[3] = "Reference ";
                                lineArray[4] = "Account name ";

                            }
                            else
                            {
                                lineArray[0] = "000000"; // validate 6 digits
                                lineArray[1] = $"0000000{new String(' ', 22)}"; // validate 7 digits ect..
                                lineArray[2] = $"{x.Value.ToString("0000000000.##")}{new String(' ', 22)}"; // validate 7 digits ect..
                                lineArray[3] = $"{x.code}{new String(' ', 1)}"; // validate 7 digits ect..
                                lineArray[4] = $"{x.Name}"; // validate 7 digits ect..
                            }

                            linecount++;
                            writer.WriteLine(String.Join("",lineArray));
                        });
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }

            
            return true;
        }
    }
}
