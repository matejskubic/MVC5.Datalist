using Datalist;
using DatalistSamples.Context;
using DatalistSamples.Models;
using System.Linq;

namespace DatalistSamples.Datalists
{
    public class DefaultDatalist : GenericDatalist<UserModel>
    {
        protected override IQueryable<UserModel> GetModels()
        {
            return new UserRepository().Users();
        }
    }
}