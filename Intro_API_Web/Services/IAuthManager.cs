using Intro_API_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_API_Web.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO loginDTO);
        Task<string> CreateToken();
    }
}
