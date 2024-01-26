using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountApplicationSql
{
    internal class Utilities
    {
        private static List<Account> accounts = new List<Account>();
       

        internal static void Initialization()
        {
            DataAccesss db = new DataAccesss();
            accounts = db.GetAccount();
            string option;
            do
            {
                Console.WriteLine("1. Login into the bank account.");
                Console.WriteLine("2. Register a bank account.");
                Console.WriteLine("3. I forgot the password.");
                Console.WriteLine("4. Exit");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        if (Login())
                            option = "4";
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        ForgotPass();
                        break;
                    case "4":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid option!\n");
                        Console.ResetColor();
                        break;
                }
            }
            while (!option.Equals("4"));
            Console.WriteLine("\nThank you for using the application!");
        }

        internal static void Register()
        {
            Console.Clear();
            Console.WriteLine("Enter the username : ");
            string username = Console.ReadLine();
            if (CheckUsername(username) == false)
                return;

            Console.WriteLine("Enter the name : ");
            string name = Console.ReadLine();
            if (CheckName(name) == false)
                return;

            Console.WriteLine("Enter the password : ");
            string password = Console.ReadLine();
            if (CheckPassword(password) == false)
                return;

            Console.WriteLine("Confirm password : ");
            string confirmPassword = Console.ReadLine();
            if (CheckConfirmPassword(confirmPassword, password) == false)
                return;

            Console.WriteLine("Enter the email : ");
            string email = Console.ReadLine();
            if (CheckEmail(email) == false)
                return;

            Console.WriteLine("Enter the age : ");
            string age = Console.ReadLine();
            if (CheckAge(age) == false)
                return;
            int ageNumber = int.Parse(age);

            DataAccesss db = new DataAccesss();
            db.InsertAccount(username, name, password, email, ageNumber);
            Account account = new Account(username, name, password, email, ageNumber, 0);
            accounts.Add(account);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe account has been succesfully created!\n");
            Console.ResetColor();
        }

        internal static bool Login()
        {
            Console.Clear();
            Console.WriteLine("Username : ");
            string username = Console.ReadLine();
            int OK = 0;
            foreach (var account in accounts)
            {
                if (account.Username.Equals(username))
                {
                    OK = 1;
                    Console.WriteLine("Password : ");
                    string password = Console.ReadLine();
                    if (account.Password.Equals(password))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You have succesfully logged in!\n");
                        Console.ResetColor();
                        string option;
                        string message = String.Empty;
                        do
                        {
                            Console.Clear();
                            if (message != String.Empty)
                            {
                                Console.WriteLine(message + "\n");
                            }
                            Console.WriteLine($"Name : {account.Name}");
                            Console.WriteLine($"Email : {account.Email}");
                            Console.WriteLine($"Age : {account.Age}");
                            Console.WriteLine($"Money : {account.Money} LEI\n");
                            Console.WriteLine("1. Deposit money");
                            Console.WriteLine("2. Withdraw money");
                            Console.WriteLine("3. Exit");
                            option = Console.ReadLine();
                            DataAccesss db = new DataAccesss();
                            switch (option)
                            {
                                case "1":
                                    message = DepositMoney(account);
                                    db.UpdateAccountBalance(account);
                                    break;
                                case "2":
                                    message = WithdrawMoney(account);
                                    db.UpdateAccountBalance(account);
                                    break;
                                case "3":
                                    break;
                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nInvalid option!\n");
                                    Console.ResetColor();
                                    break;
                            }
                        } while (!option.Equals("3"));
                        return true;
                    }
                    else
                    {
                        OK = 2;
                        break;
                    }
                }
            }
            if (OK == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nUsername not found!\n");
                Console.ResetColor();
            }
            if (OK == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nWrong password!\n");
                Console.ResetColor();
            }
            return false;
        }

        private static void ForgotPass()
        {
            Console.Clear();
            Console.WriteLine("Enter the username : ");
            string username = Console.ReadLine();
            int OK = 0;
            foreach (var account in accounts)
            {
                if (account.Username.Equals(username))
                {
                    OK = 1;
                    Console.WriteLine("Enter the email : ");
                    string email = Console.ReadLine();
                    if (account.Email.Equals(email))
                    {
                        OK = 2;
                        Console.WriteLine($"The account's password is : {account.Password}\n");
                    }
                    else
                        break;
                }
            }
            if (OK == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid username!\n");
                Console.ResetColor();
            }
            else if (OK == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid email!\n");
                Console.ResetColor();
            }
        }

        private static String DepositMoney(Account account)
        {
            Console.WriteLine("\nHow much money do you want to deposit?");
            string amount = Console.ReadLine();
            int amountNumber;
            if (int.TryParse(amount, out amountNumber))
            {
                if (!(amountNumber >= 0 && amountNumber <= 10000))
                    return "The amount must be between 0 and 10000!";
            }
            else
            {
                return "The amount must be a number!";
            }
            account.Money += amountNumber;
            return "The amount was stored in the account!";
        }

        private static String WithdrawMoney(Account account)
        {
            Console.WriteLine("\nHow much money do you want to withdraw?");
            string amount = Console.ReadLine();
            int amountNumber;
            if (int.TryParse(amount, out amountNumber))
            {
                if (!(amountNumber >= 0 && amountNumber <= 10000))
                    return "The amount must be between 0 and 10000!";
            }
            else
            {
                return "The amount must be a number!";
            }
            if (amountNumber > account.Money)
            {
                return "You do not have enough money!";
            }
            else
                account.Money -= amountNumber;
            return "Withdraw succesfull!";
        }

        private static void ShowAllAcounts(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                Console.WriteLine($"Username : {account.Username}");
                Console.WriteLine($"Name : {account.Name}");
                Console.WriteLine($"Password : {account.Password}");
                Console.WriteLine($"Email : {account.Email}");
                Console.WriteLine($"Age : {account.Age}");
                Console.WriteLine($"Money : {account.Money}\n");
            }
        }

        private static bool CheckUsername(string username)
        {
            if (username.Length >= 3 && username.Length <= 20)
            {
                if (!username.Contains(" "))
                {
                    for (int i = 0; i < username.Length; i++)
                    {
                        if (Char.IsLetterOrDigit(username[i]) == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nThe username can only contain letters and digits!\n");
                            Console.ResetColor();
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nThe username can not contain white spaces!\n");
                    Console.ResetColor();
                    return false;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nThe username must be between 3 and 20 characters!\n");
                Console.ResetColor();
                return false;
            }

        }

        private static bool CheckName(string name)
        {
            if (name.Length >= 2 && name.Length <= 20)
            {
                for (int i = 0; i < name.Length; i++)
                {
                    if (Char.IsLetterOrDigit(name[i]) || name[i] == ' ')
                        continue;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nThe name can only contain letters, numbers and spaces\n");
                        Console.ResetColor();
                        return false;
                    }
                }
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nThe name must be between 2 and 20 characters!\n");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckPassword(string password)
        {
            if (password.Length >= 6)
            {
                if (password.Contains(" "))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nThe password can not contain spaces\n");
                    Console.ResetColor();
                    return false;
                }
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThe password must contain more than 6 characters!\n");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckConfirmPassword(string confirmPassword, string password)
        {
            if (confirmPassword.Equals(password))
                return true;
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThe passwords must be the same!\n");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckEmail(string email)
        {
            if (email.Contains("@") && email.Contains("."))
            {
                if (email.Contains(" "))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nIncorect format\n");
                    Console.ResetColor();
                    return false;
                }
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIncorect format!\n");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckAge(string age)
        {
            int ageNumber;
            if (int.TryParse(age, out ageNumber))
            {
                if (ageNumber >= 1 && ageNumber <= 100)
                    return true;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nThe age must be between 1 and 100!\n");
                    Console.ResetColor();
                    return false;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThe age must be a number!\n");
                Console.ResetColor();
                return false;
            }
        }
    }
}
