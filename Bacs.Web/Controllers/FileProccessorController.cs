using Bacs.Models.Models;
using Bacs.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacs.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileProccessorController : ControllerBase
    {

        private readonly IFileProccessor _fileProccessor;
        public FileProccessorController(IFileProccessor fileProccessor)
        {
            _fileProccessor = fileProccessor;
        }

        [HttpPost(Name = "PostCsv")]
        public IActionResult OnPostUpload(IFormFile fileToUpload)
        {
            if (fileToUpload == null) return BadRequest(new FileResponseBadValidation() { ResponseMessage = "NoFiles Received" }); // could test all edge cases
            var result = _fileProccessor.ProccessFile(fileToUpload);
            if (result == null || result is FileResponseBadValidation)
            {
                return BadRequest(result); // could have single model with is successful property
            }
            return Ok(result);
        }
    }
}
