using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcDatalist.Models
{
    public class UserModel
    {
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