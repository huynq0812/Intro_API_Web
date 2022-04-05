using Intro_API_Web.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUnitOfWork unitOfWork, ILogger<LoginController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        []
        public async Task<IActionResult> Login()
        {

        }
    }
}
