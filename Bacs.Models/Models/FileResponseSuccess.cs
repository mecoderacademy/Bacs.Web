using Bacs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Models.Models
{
    public class FileResponseSuccess : IFileResponse
    {
        public string ResponseMessage { get; set; }
    }
}
