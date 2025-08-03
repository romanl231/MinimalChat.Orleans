using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.GrainInterfaces;

namespace SiloHost.Grains
{
    public class UserDirectoryGrainState
    {
        public string? UserGrainId { get; set; }
    }

    public class UserDirectoryGrain : Grain<UserDirectoryGrainState>, IUserDirectoryGrain
    {
        private static string Normalize(string nickname) => nickname.Trim().ToLowerInvariant();

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task<bool> TryClaim(string userGrainId)
        {
            if(State.UserGrainId != null) return false;

            State.UserGrainId = userGrainId;
            await WriteStateAsync();
            return true;
        }

        public Task<string?> Resolve()
        {
            return Task.FromResult(State.UserGrainId);
        }

        public async Task<bool> Release(string userGrainId)
        {
            if (State.UserGrainId != userGrainId) return false;
            State.UserGrainId = null;
            await WriteStateAsync();
            return true;
        }
    }
}
