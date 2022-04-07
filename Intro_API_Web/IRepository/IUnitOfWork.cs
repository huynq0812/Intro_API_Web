using Intro_API_Web.Controllers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_API_Web.IRepository
{
    public interface IUnitOfWork :IDisposable
    {
        IGenericRepository<ApiUser> ApiUser { get; }
        Task Save();
    }
}
