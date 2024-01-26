using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountApplicationSql
{
    public class DataAccesss
    {
        public List<Account> GetAccount()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("BankDB")))
            {
                return connection.Query<Account>("Select * from account").ToList();
            }
        }

        public void InsertAccount(string username, string name, string password, string email, int age)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("BankDB")))
            {
                //Account newAccount = new Account(username, name, password, email, age, 0);
                //List<Account> accounts = new List<Account>();
                //accounts.Add(newAccount);

                //connection.Execute("dbo.account_insert @Username @Name @Password @Email @Age @Money", accounts);
                connection.Execute("dbo.account_insert", new { Username = username, Name = name, Password = password, Email = email, Age = age, Money = 0 }, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateAccountBalance(Account account)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("BankDB")))
            {
                connection.Execute("dbo.account_update", new { Money = account.Money, Username = account.Username, Name = account.Name, Password = account.Password }, commandType: CommandType.StoredProcedure);
            }
            
        }
    }
}
