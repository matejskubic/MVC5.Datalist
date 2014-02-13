using Datalist;
using MvcDatalist.Context;
using MvcDatalist.Models;
using System.Linq;

namespace MvcDatalist.Datalists
{
    public class DefaultDatalist : GenericDatalist<UserModel>
    {
        protected override IQueryable<UserModel> GetModels()
        {
            return new UserRepository().Users();
        }
    }
}