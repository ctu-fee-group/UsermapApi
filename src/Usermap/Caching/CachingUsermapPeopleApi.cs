using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usermap.Controllers;
using Usermap.Data;

namespace Usermap.Caching
{
    public class CachingUsermapPeopleApi : UsermapPeopleApi
    {
        private readonly UsermapCacheService _cacheService;
        
        public CachingUsermapPeopleApi(UsermapCacheService cacheService, UsermapHttpClient client, ILogger<UsermapPeopleApi> logger) : base(client, logger)
        {
            _cacheService = cacheService;
        }

        public override async Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default)
        {
            var identifier = $"people/{username}";
            if (_cacheService.TryGetValue<UsermapPerson?>(identifier, out var cachedPerson))
            {
                return cachedPerson;
            }
            
            return _cacheService.Cache(identifier, await base.GetPersonAsync(username, token));
        }
    }
}