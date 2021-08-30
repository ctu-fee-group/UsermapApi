using System.Threading;
using System.Threading.Tasks;
using Usermap.Data;

namespace Usermap.Controllers
{
    public interface IUsermapPeopleApi
    {
        public Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default);
    }
}