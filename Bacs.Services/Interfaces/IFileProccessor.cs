using Bacs.Models.Interfaces;
using Bacs.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Services.Interfaces
{
    public interface IFileProccessor
    {
        public FileTransaction ReadFile(IFormFile formFile);
        public IFileResponse ProccessFile(IFormFile formFile);
    }
}
