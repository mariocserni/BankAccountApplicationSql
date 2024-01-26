using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountApplicationSql
{
    public class Account
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int Money { get; set; }


        public Account(string username, string name, string password, string email, int age, int money)
        {
            Username = username;
            Name = name;
            Password = password;
            Email = email;
            Age = age;
            Money = money;
        }

    }
}
