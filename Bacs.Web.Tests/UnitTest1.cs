using Bacs.Services;
using Bacs.Services.Interfaces;
using Bacs.Services.Service;
using Bacs.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Bacs.Web.Tests
{
    public class Tests:BaseTest
    {
        private FileProccessorController _fileProccessorController;
        private SqliteConnection sqliteConnection;
        [SetUp]
        public void Setup()
        {
            var context = SetUpContext();
            _fileProccessorController = new FileProccessorController(new FileProccessor(new FileTransactionService(new Data.Repository.FileTransactionRepository(context), new TransactionService(new Data.Repository.TransactionRepository(context)))));
        }
        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        [Test]
        public async Task FileProccessorController_NullArgs_ReturnsOk()
        {
            var result = _fileProccessorController.OnPostUpload(null) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200,result.StatusCode);
        }

        [Test]
        public async Task FileProccessorController_MockFile_ReturnsOk()
        {
            FormFile formFiles = default;
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "source.csv");
            if (File.Exists(path))
            {
                var fileStream = File.Open(path, FileMode.Open);
                formFiles = new FormFile(fileStream, 0, fileStream.Length, "someName", "source.csv");
            }
            
            
            var result = _fileProccessorController.OnPostUpload(formFiles) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}