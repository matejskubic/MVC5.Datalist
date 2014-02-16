using Datalist;
using DatalistSamples.Datalists;
using System;

namespace DatalistSamples.Models
{
    public class UserModel
    {
        [Datalist(typeof(DefaultDatalist))]
        public String Id { get; set; }

        [DatalistColumn]
        public String FirstName { get; set; }

        [DatalistColumn]
        public String LastName { get; set; }

        [DatalistColumn(4)]
        public DateTime DateOfBirth { get; set; }

        [DatalistColumn(7, Relation = "LoginName")]
        public AccountModel Account { get; set; }
    }
}