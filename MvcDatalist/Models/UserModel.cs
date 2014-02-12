using Datalist;
using MvcDatalist.Datalists;
using System;

namespace MvcDatalist.Models
{
    public class UserModel
    {
        [Datalist(Type = typeof(DefaultDatalist))]
        public String Id { get; set; }

        [DatalistColumn]
        public String FirstName { get; set; }

        [DatalistColumn]
        public String LastName { get; set; }

        [DatalistColumn]
        public DateTime DateOfBirth { get; set; }

        [DatalistColumn(Relation = "LoginName")]
        public AccountModel Account { get; set; }
    }
}