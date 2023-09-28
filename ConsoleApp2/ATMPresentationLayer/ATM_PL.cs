using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMBussinesObjects;
using ATMBussinessLogicLayer;
using Microsoft.Data.SqlClient;

namespace ATMPresentationLayer
{ 
    public class ATMView
    {
        int accountLimit = 20000;
        int totalAmountWithdrawl = 0;
        public static void UserType()
        {
            Console.WriteLine("WELCOME TO ATM SYSTEM");
            Console.WriteLine("Select one of the following User Type");
            bool type=false;
            int count = 0;
            while (type==false)
            {
                Console.WriteLine("Press 1 to login as a Customer");
                Console.WriteLine("Press 2 to login as an Admin");
                Console.WriteLine("Press X to close the Program");
                string input=Console.ReadLine();
                if (input == "1"&&count<4)
                {
                    customerLogin();
                }
                else if (input == "2")
                {
                    AdminLogin();
                }
                else if (input=="X"|| input=="x")
                {
                    type=true;
                }
                else
                {
                    Console.WriteLine("Enter A Valid Input!!!");
                    count++;
                }
            }
        }
        public static void customerLogin()
        {
            Console.WriteLine("Enter your Login");
            string Login = Console.ReadLine();
            Console.WriteLine("Enter Pin Code");
            int PinCode = int.Parse(Console.ReadLine());
            CustomerBO cBO = new CustomerBO {Login = Login, PinCode = PinCode };
            ATM_BLL.customerLogin(cBO);
            if (cBO.AccountNo != 0)
            {
                customerMenu(cBO);
            }
            else
            {
                Console.WriteLine("Invalid pin or login");
            }
        }
        public static void customerMenu(CustomerBO cBO)
        {
            Console.WriteLine("Your Account ID is: ");
            Console.Write(cBO.AccountNo);
            bool menu=false;
            while (menu!=true)
            {
                Console.WriteLine("\nPlease select one of these options");
                Console.WriteLine("1--- Withdraw cash");
                Console.WriteLine("2--- Cash Transfer");
                Console.WriteLine("3--- Deposit cash");
                Console.WriteLine("4--- Display Balance");
                Console.WriteLine("5---Exit");
                string entry = Console.ReadLine();
                if (entry=="1")
                {
                    withdrawCash(cBO);
                }
                else if (entry=="2")
                {
                    CashTransfer(cBO);
                }
                else if (entry=="3")
                {
                    DepositCash(cBO);
                }
                else if (entry=="4")
                {
                    DisplayAmount(cBO);
                }
                else if (entry=="5")
                {
                    menu=true;
                }
                else
                {
                    Console.WriteLine("INVALID INPUT");
                }
            }
        }
        public static void AdminLogin()
        {
            Console.WriteLine("Enter your UserName");
            string UserName = Console.ReadLine();
            Console.WriteLine("Enter Password");
            string Password = Console.ReadLine();
            AdminBO aBO=new AdminBO { Name = UserName, Password = Password };
            ATM_BLL.VerifyAdmin(aBO);
            if (aBO.verify==true)
            {
                AdminMenu();
            }
            
        }
        public static void AdminMenu()
        {
            bool action = false;
            while (!action)
            {
                Console.WriteLine("\t\t---Administrator Menu---");
                Console.WriteLine("1--- Create New Account");
                Console.WriteLine("2--- Update Account Information");
                Console.WriteLine("3--- Delete Existing Account");
                Console.WriteLine("4--- Search for Account");
                Console.WriteLine("5--- View Reports");
                Console.WriteLine("6--- Exit");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    createAccouunt();
                }
                else if (input=="2")
                {
                    UpdateAccount();
                }
                else if (input=="3")
                {
                    DeleteAccount();
                }
                else if (input=="4")
                {
                    SearchAccount();
                }
                
            }
        }
        public static void createAccouunt()
        {
            Console.WriteLine("Login:\t");
            string Login = Console.ReadLine();
            Console.WriteLine("Encryped Login is:\t");
            //
            string str = Login;
            char[] ch = new char[str.Length];
            char[] ascii=new char[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                ch[i] = str[i];
            }
            foreach (char c in ch)
            {
                if (c=='0'||c=='1'||c=='2'||c=='3' || c =='4' || c =='5' || c =='6' || c =='7' || c =='8' || c == '9')
                {
                    Encrypted2(c);
                }

                else
                {
                    Encrypted(c);
                }
            }
            Console.WriteLine("\nPin Code:\t");
            int PinCode = int.Parse( Console.ReadLine());
            Console.WriteLine("Holder Name\t");
            string HolderName = Console.ReadLine();
            Console.WriteLine("Type(saving,current)\t");
            string Type = Console.ReadLine();
            Console.WriteLine("Starting Balance:\t");
            int Balance = int.Parse(Console.ReadLine());
            Console.WriteLine("Status:\t");
            string Status = Console.ReadLine();
            
            CustomerBO cBO = new CustomerBO {Login=Login,PinCode=PinCode,HolderName=HolderName,Type=Type,Balance=Balance,Status=Status };
            ATM_BLL.createAccount(cBO);
            
        }
        public static void DeleteAccount()
        {
            Console.WriteLine("Enter the account number to which you want to delete:");
            int acc = System.Convert.ToInt32(Console.ReadLine());
            CustomerBO cBO = new CustomerBO { AccountNo = acc };
            ATM_BLL.DeleteAccount(cBO);            
        }
        public static void UpdateAccount()
        {
            Console.WriteLine("Enter the Account Number:");
            int acc=System.Convert.ToInt32(Console.ReadLine());
            CustomerBO cBO = new CustomerBO { AccountNo = acc };
            ATM_BLL.UpdateAccount(cBO);
        }

        public static void SearchAccount()
        {

            CustomerBO cBO = new CustomerBO();
            Console.WriteLine("SEARCH MENU");
            Console.WriteLine("Account ID");
            string accID = string.Empty;
            accID=Console.ReadLine();
            if (accID==string.Empty)
            {
                cBO.AccountNo = 0;    
            }
            Console.WriteLine("Holder Name: ");
            string HolderName = string.Empty ;
            HolderName = Console.ReadLine();
            cBO.HolderName = HolderName;
            Console.WriteLine("Type(saving,current):  ");
            string type = string.Empty;
            type = Console.ReadLine();
            cBO.Type = type;
            Console.WriteLine("Account Balance: ");
            string balance = string.Empty;
            balance = Console.ReadLine();
            if (balance == string.Empty)
            {
                cBO.Balance = 0;
            }
            Console.WriteLine("Status:");
            string status = string.Empty;
            status = Console.ReadLine();
            cBO.Status = status;
            if (accID==String.Empty&&HolderName==String.Empty&&type==string.Empty&&balance==String.Empty&&status==String.Empty)
            {
                Console.WriteLine("You haven't provided enough data for Searching ");
            }
            else if (accID == String.Empty && HolderName == String.Empty && type == string.Empty && balance == String.Empty && status == String.Empty)
            {
                Console.WriteLine("Invalid Input");
            }
            else
                    {
                ATM_BLL.SearchAccount(cBO);

            }
            //Console.WriteLine("Login: "+cBO.Login);
            //Console.WriteLine("Acc #: " + cBO.AccountNo);
            //Console.WriteLine("TYPE: " + cBO.Type);
            //Console.WriteLine("NAME: " + cBO.HolderName);
            //Console.WriteLine("Balance: " + cBO.Balance);        
        }
        public static void withdrawCash(CustomerBO cBO)
        {
            Console.WriteLine("Please select a method of withdrawl:");
            Console.WriteLine("a) Fast Cash");
            Console.WriteLine("b) Normal Cash Cash");
            string input = Console.ReadLine();
            if (input == "a"||input=="A")
            {
                FastCash(cBO);
            }
            else if (input == "b"||input=="B")
            {
                NormalCash(cBO);
            }
            else
            {
                Console.WriteLine("You selected an invalid option");
            }
        }
        public static void NormalCash(CustomerBO cBO)
        {
            Console.WriteLine("Enter the Withdrawl Amount");
            int amount=System.Convert.ToInt32(Console.ReadLine());
            cBO.totalAmountWithdraw = cBO.totalAmountWithdraw + amount;
            ATM_BLL.FastCash(cBO, amount);

        }
        public static void FastCash(CustomerBO cBO)
        {
            Console.WriteLine("1----500");
            Console.WriteLine("2----1000");
            Console.WriteLine("3----2000");
            Console.WriteLine("4----5000");
            Console.WriteLine("5----1000");
            Console.WriteLine("6----15000");
            Console.WriteLine("7----20000");
            Console.WriteLine("Select one of the denominations of money: ");
            int choice = int.Parse(Console.ReadLine());
            choice--;
            int[] fcMoney = { 500, 1000, 2000, 5000, 10000, 15000, 20000 };
            Console.WriteLine($"Are you sure you want to withdraw '{fcMoney[choice]}? (Y/N)'");
            string fcConfirm = Console.ReadLine();
            if (fcConfirm == "Y" || fcConfirm == "y")
            {
                cBO.totalAmountWithdraw = cBO.totalAmountWithdraw + fcMoney[choice];
                ATM_BLL.FastCash(cBO,fcMoney[choice]);
            }
            else if (fcConfirm == "N" || fcConfirm == "n")
            {
                Console.WriteLine("Cash Withdrawl has been cancelled successfully");
            }
        }
        public static void CashTransfer(CustomerBO cBO)
        {
            Console.WriteLine("Enter The amount in multiples of 500: ");
            int ctAmount = int.Parse(Console.ReadLine());
            if (ctAmount%500==0) 
            {
                Console.WriteLine("amount is valid");
                Console.WriteLine("Enter the Acoount Id of the recipient ");
                int recipientID=int.Parse(Console.ReadLine());
                CustomerBO recipientBO=new CustomerBO { AccountNo = recipientID };
                ATM_BLL.CashTransfer(cBO,recipientBO,ctAmount);
            }
            else
            {
                Console.WriteLine("INVALID");
            }
        }
        public static void DepositCash(CustomerBO cBO)
        {
            Console.WriteLine("Enter the amount you want to deposite:  ");
            int dcAmount = int.Parse(Console.ReadLine());
            ATM_BLL.DepositCash(cBO,dcAmount);
        }
        public static void DisplayAmount (CustomerBO cBO)
        {
            ATM_BLL.DisplayAmount(cBO);

        }
        public static char Encrypted(char alphaEncrypt)
        {
            int charASCII=System.Convert.ToInt32( alphaEncrypt);
            int x = charASCII-65;
            
            int result = 90 - x;
            char encrypted = System.Convert.ToChar(result);
            Console.Write(encrypted);
            return encrypted;
            //Console.WriteLine("Decrypted code is");
            //Console.WriteLine(System.Convert.ToChar(result));

            //Console.WriteLine("   ");
            //string v = Console.ReadLine();
        }
        public static char Encrypted2(char numericEncrypt)
        {
            int charASCII = System.Convert.ToInt32(numericEncrypt);
            int x = charASCII - 48;

            int result = 57 - x;
            char encrypted = System.Convert.ToChar(result);
            Console.Write(encrypted);

            return encrypted;
            //Console.WriteLine("Decrypted code is");
            //Console.WriteLine(System.Convert.ToChar(result));

            //Console.WriteLine("   ");
            //string v = Console.ReadLine();


        }
    }
}
