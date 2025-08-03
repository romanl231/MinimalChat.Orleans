using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GrainInterfaces
{
    public interface IUserDirectoryGrain : IGrainWithStringKey
    {
        Task<bool> TryClaim(string userGrainId);
        Task<string?> Resolve();
        Task<bool> Release(string userGrainId);
    }
}
