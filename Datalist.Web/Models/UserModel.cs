using Datalist;
using Datalist.Web.Datalists;
using System;

namespace Datalist.Web.Models
{
    public class UserModel
    {
        [Datalist(typeof(DefaultDatalist))]
        public String Id { get; set; }

        [DatalistColumn]
        public String FirstName { get; set; }

        [DatalistColumn]
        public String LastName { get; set; }

        [DatalistColumn(4, Format = "{0:d}")]
        public DateTime DateOfBirth { get; set; }

        [DatalistColumn(7, Relation = "LoginName")]
        public AccountModel Account { get; set; }
    }
}