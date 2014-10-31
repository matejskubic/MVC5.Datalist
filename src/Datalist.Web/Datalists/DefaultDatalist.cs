using Datalist;
using Datalist.Web.Context;
using Datalist.Web.Models;
using System.Linq;

namespace Datalist.Web.Datalists
{
    public class DefaultDatalist : GenericDatalist<UserModel>
    {
        protected override IQueryable<UserModel> GetModels()
        {
            return new UserRepository().Users();
        }
    }
}