using Bacs.Models.Models;
using Bacs.Services;
using Bacs.Services.Interfaces;
using Bacs.Services.Service;
using Bacs.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Bacs.Web.Tests
{
    public class FileProccessorServiceTests:BaseTest
    {
        private IFileProccessor _IFileProccessor;
        [SetUp]
        public void Setup()
        {
            var context = SetUpContext();
            _IFileProccessor = new FileProccessor(new FileTransactionService(new Data.Repository.FileTransactionRepository(context),new TransactionService(new Data.Repository.TransactionRepository(context))));
        }
         [TearDown]
        public void TearDown()
        {
            Dispose();
        }
        [Test]
        public  void _IFileProccessor_NullArgs_ReturnsOk()
        {
            var response=_IFileProccessor.ReadFile(null);

            Assert.AreEqual("no file provided", response.ResponseMessage);
            Assert.IsEmpty(response.Transactions);
            Assert.AreEqual(default(double),response.TotalAmount);
            Assert.AreNotEqual(Guid.Empty,response.FileId);
            Assert.IsTrue(string.IsNullOrEmpty(response.FileName));
        }

        [Test]
        public async Task FileProccessorController_MockFile_ReturnsOk()
        {
            string fileName = "source.csv";
            var formFiles = new List<IFormFile>();
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "source.csv");
            if (File.Exists(path))
            {
                var fileStream = File.Open(path, FileMode.Open);
                formFiles.Add(new FormFile(fileStream, 0, fileStream.Length, fileName, "source.csv"));
            }

            var result =_IFileProccessor.ReadFile(formFiles[0]);
            
            Assert.AreEqual(fileName, result.FileName);
            Assert.AreEqual(210.0, result.TotalAmount);
            Assert.GreaterOrEqual( result.TotalAmount, 1.00);
            Assert.LessOrEqual(result.TotalAmount, 20000000.00);
            Assert.IsNotEmpty(result.Transactions);
            Assert.AreEqual(21, result.Transactions.Count);
            Assert.IsNull(result.ResponseMessage);
            
        }


        [Test]
        public async Task ProccessFile()
        {
            string fileName = "source.csv";
            var formFiles = new List<IFormFile>();
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "source.csv");
            if (File.Exists(path))
            {
                var fileStream = File.Open(path, FileMode.Open);
                formFiles.Add(new FormFile(fileStream, 0, fileStream.Length, fileName, "source.csv"));
            }

            var result = _IFileProccessor.ProccessFile(formFiles[0]);

            Assert.IsTrue(result is FileResponseSuccess);
            Assert.False(string.IsNullOrEmpty(result?.ResponseMessage));
            Assert.AreEqual("successful upload", string.IsNullOrEmpty(result.ResponseMessage));

        }
    }
}