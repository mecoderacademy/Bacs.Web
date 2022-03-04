using Bacs.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Web.Tests
{
    public class BaseTest
    {
        private SqliteConnection sqliteConnection;
        private BacsContext _context;
        public BacsContext SetUpContext()
        {
            sqliteConnection = new SqliteConnection("Filename=:memory:");
            sqliteConnection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var _contextOptions = new DbContextOptionsBuilder<Data.BacsContext>()
                 .UseSqlite(sqliteConnection)
                 .Options;
            _context = new Data.BacsContext(_contextOptions);
            _context.Database.EnsureCreated();
            return _context;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            sqliteConnection.Close();
            sqliteConnection.Dispose();
        }
    }
}
