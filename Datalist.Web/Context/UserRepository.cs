using Datalist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datalist.Web.Context
{
    public class UserRepository
    {
        public IQueryable<UserModel> Users()
        {
            List<UserModel> users = new List<UserModel>();
            users.Add(new UserModel()
            {
                Id = "1",
                FirstName = "Tom",
                LastName = "Jecks",
                DateOfBirth = new DateTime(1998, 10, 10),
                Account = new AccountModel()
                {
                    LoginName = "Tommy"
                }
            });
            users.Add(new UserModel()
            {
                Id = "2",
                FirstName = "Len",
                DateOfBirth = new DateTime(1990, 1, 1),
                Account = new AccountModel()
                {
                    LoginName = "Lennox"
                }
            });
            users.Add(new UserModel()
            {
                Id = "3",
                FirstName = "Kim",
                LastName = "",
                DateOfBirth = new DateTime(1950, 5, 2),
                Account = new AccountModel()
                {
                    LoginName = ""
                }
            });
            users.Add(new UserModel()
            {
                Id = "4",
                FirstName = "Dred",
                LastName = "Insky",
                Account = new AccountModel()
                {
                    LoginName = null
                }
            });
            users.Add(new UserModel()
            {
                Id = "5",
                FirstName = "Pet",
                LastName = "Quacks",
                DateOfBirth = new DateTime(2000, 9, 18),
                Account = null
            });

            return users.AsQueryable();
        }
    }
}