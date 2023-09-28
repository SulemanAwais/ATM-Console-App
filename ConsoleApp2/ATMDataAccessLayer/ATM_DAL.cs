using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMBussinesObjects;
using Microsoft.Data.SqlClient;

namespace ATMDataAccessLayer
{
    public class ATM_DAL
    {
        public static void FastCash(CustomerBO cBO,int fcAmount)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Select * from Accounts where Id={cBO.AccountNo}";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            bool fc = false;
            
            int PreviousBalance=0;
            if (dr.Read())
            {
                PreviousBalance=System.Convert.ToInt32 (dr.GetValue(5));
                fc = true;
            }
            connection.Close();
            //Variable for reciept decision
            bool reciept = false;

            if (fc == true )
            {
                //Calculating Balance 
                int UpdatedBalance = PreviousBalance-fcAmount;
                connection.Open();
                string updateQuery = $"Update Accounts set StartingBalance={UpdatedBalance} where Id={cBO.AccountNo}";
                SqlCommand cmd1 = new SqlCommand(updateQuery, connection);
                int balanceUpdated = cmd1.ExecuteNonQuery();
                if (balanceUpdated >= 1)
                {
                    Console.WriteLine("Do you wish to print a receipt (Y/N)?");
                    string recieptOption = Console.ReadLine();
                    if (recieptOption=="Y"||recieptOption=="y")
                    {
                        Console.WriteLine("WIHDRAWL SUCCESSFULL");
                        reciept = true;
                    }
                    else
                    {
                        Console.WriteLine("Thank You For Cash withDrawl");
                    }  
                }
                connection.Close();
            }

            else
            {
                Console.WriteLine("FAST CASH FAILED");
            }
            if (reciept == true)
            {
                connection.Open();
                string Recieptquery = $"Select * from Accounts where Id={cBO.AccountNo}";
                SqlCommand cmd1 = new SqlCommand(Recieptquery, connection);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                DateTime now = DateTime.Now;
                if (dr1.Read())
                {
                    cBO.Balance = System.Convert.ToInt32(dr1.GetValue(5));
                    string showReciept = $"Account #: {dr1.GetValue(0)}\nDate:{now}\nWithDrawn: {fcAmount}\nBalance:{dr1.GetValue(5)}";
                    Console.WriteLine("--------RECIEPT--------");
                    Console.WriteLine(showReciept);
                }
                connection.Close();
            }
        }
        public static void CashTransfer(CustomerBO cBO, CustomerBO recipientBO,int Amount)
        {
            Console.WriteLine("Yous Id is");
            Console.WriteLine(cBO.AccountNo);
            Console.WriteLine("Reciptient id is");
            Console.WriteLine(recipientBO.AccountNo);

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Select * from Accounts where Id='{recipientBO.AccountNo}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            bool accConfirm;
            if (dr.Read())
            {
                string confirmation = $"Reciptient Account Holder: {dr.GetValue(1)}";
                recipientBO.HolderName=System.Convert.ToString(dr.GetValue(1));
                Console.WriteLine(confirmation);
                accConfirm = true;
            }
            else
            {
                Console.WriteLine("ACCOUNT NOT FOUND");
                accConfirm = false;
            }
            connection.Close();

            bool senderSide=false;
            Console.WriteLine($"Are you sure you want to transfer {Amount} to {recipientBO.HolderName}?");
            Console.WriteLine("If Yes then Re-enter the Reciptient ID");
            int reEnteredID=int.Parse(Console.ReadLine());
            //Calculating Balances
            int senderNewBalance = cBO.Balance - Amount;
            int recipterBalance = recipientBO.Balance + Amount;
            if (reEnteredID==recipientBO.AccountNo)
            {
               
                connection.Open();
                string updateQuery = $"Update Accounts set StartingBalance={senderNewBalance} where Id={cBO.AccountNo}";
                SqlCommand cmd1 = new SqlCommand(updateQuery, connection);
                int balanceUpdated = cmd1.ExecuteNonQuery();
                if (balanceUpdated >= 1)
                {
                    senderSide = true;
                    Console.WriteLine("Amount sent from sender side");
                }
                else
                {
                    Console.WriteLine("Amount not sent");
                }
                connection.Close();
                if (senderSide == true)
                {
                    connection.Open();
                    string ReciptentupdateQuery = $"Update Accounts set StartingBalance={recipterBalance} where Id={recipientBO.AccountNo}";
                    SqlCommand cmd2 = new SqlCommand(ReciptentupdateQuery, connection);
                    int ReciptentbalanceUpdated = cmd2.ExecuteNonQuery();
                    if (ReciptentbalanceUpdated>=1)
                    {
                        Console.WriteLine("Amount Recieved");
                    }
                    else
                    {
                        Console.WriteLine("Amount Not recieved");
                    }
                    connection.Close();
                }
                bool reciept = false;
                Console.WriteLine("DO YOU WANT RECIEPT?\t(Y/N)");
                string wantReciept = Console.ReadLine();
                if (wantReciept =="y" || wantReciept=="Y")
                {
                    reciept = true;
                }
                if (reciept == true)
                {
                    DateTime now = DateTime.Now;
                    connection.Open();
                    string Recieptquery = $"Select * from Accounts where Id={cBO.AccountNo}";
                    SqlCommand cmd3 = new SqlCommand(Recieptquery, connection);
                    SqlDataReader dr1 = cmd3.ExecuteReader();
                    if (dr1.Read())
                    {
                        cBO.Balance = System.Convert.ToInt32(dr1.GetValue(5));
                        string showReciept = $"Account #: {dr1.GetValue(0)}\nAmount Transfered: {Amount}\nDate:{now}\nBalance:{dr1.GetValue(5)}";
                        Console.WriteLine("--------RECIEPT--------");
                        Console.WriteLine(showReciept);
                        Console.WriteLine("\t\t Thank you for using our service");
                    }
                    connection.Close();
                }
            }   
        }
        public static void DepositCash(CustomerBO cBO, int amount)
        {
            bool recipte = false ;
            Console.WriteLine($"YOUR old balance was\t{cBO.Balance}");
            int newBalance = cBO.Balance + amount;
            cBO.Balance = newBalance;

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Update Accounts set StartingBalance ='{newBalance}' where Id='{cBO.AccountNo}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            int deposited = cmd.ExecuteNonQuery();
            if (deposited>=1)
            {
                Console.WriteLine("Cash Deposited Successfully ");
                
                Console.WriteLine("Do you want Recipt?\t(Y/N)");
                string wantRecipt = Console.ReadLine();
                if (wantRecipt=="Y"||wantRecipt=="y")
                {
                    recipte = true;

                }
                else if (wantRecipt=="N"||wantRecipt=="n")
                {
                    Console.WriteLine("Thank you for using our servise");

                }
            }
            else
            {
                Console.WriteLine("Deposie Failed");
            }
            connection.Close();
            if (recipte==true)
            {
                DateTime now = DateTime.Now;
                connection.Open();
                string Recieptquery = $"Select * from Accounts where Id={cBO.AccountNo}";
                SqlCommand cmd3 = new SqlCommand(Recieptquery, connection);
                SqlDataReader dr1 = cmd3.ExecuteReader();
                if (dr1.Read())
                {
                    cBO.Balance = System.Convert.ToInt32(dr1.GetValue(5));
                    string showReciept = $"Account #: {dr1.GetValue(0)}\nDate:{now}\nDeposited: {amount}\nBalance:{dr1.GetValue(5)}";
                    Console.WriteLine("--------RECIEPT--------");
                    Console.WriteLine(showReciept);
                    Console.WriteLine("\t Thank you for using our service");
                }
                connection.Close();
            }
        }
        public static void DisplayAmount(CustomerBO cBO)
        {
            DateTime now = DateTime.Now;
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string Recieptquery = $"Select * from Accounts where Id={cBO.AccountNo}";
            SqlCommand cmd3 = new SqlCommand(Recieptquery, connection);
            SqlDataReader dr1 = cmd3.ExecuteReader();
            if (dr1.Read())
            {
                cBO.Balance = System.Convert.ToInt32(dr1.GetValue(5));
                string showReciept = $"Account #: {dr1.GetValue(0)}\nDate:{now}\nBalance:{dr1.GetValue(5)}";
                Console.WriteLine("--------RECIEPT--------");
                Console.WriteLine(showReciept);
                Console.WriteLine("\t Thank you for using our service");
            }
            connection.Close();
        }
        public static object customerLogin(CustomerBO cBO)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Select * from Accounts where Login=@L and PinCode=@P";
            SqlParameter p1 = new SqlParameter("L", cBO.Login);
            SqlParameter p2 = new SqlParameter("P", cBO.PinCode);
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            
            if (dr.Read())
            {
                cBO.AccountNo= System.Convert.ToInt32(dr.GetValue(0));
                cBO.Balance= System.Convert.ToInt32(dr.GetValue(5));
                //cID = System.Convert.ToInt32(dr.GetValue(0));
            }
            else
            {
                Console.WriteLine("Invalid Login and password");
            }
            connection.Close();
            return cBO;
        }
        public static void UpdateAccount(CustomerBO cBO)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Select * from Accounts where Id='{cBO.AccountNo}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            //Storing the previosu data of customer
            string prevLogin = string.Empty;
            string prevStatus= string.Empty;
            string prevHolderName = string.Empty;
            string prevPinCode = string.Empty;

            string confirmation;
            bool verify = false;

            if (dr.Read())
            {
                confirmation = $"Account #: {dr.GetValue(0)}\nType:\t{dr.GetValue(4)} \nHolder:\t{dr.GetValue(1)} \nBalance:\t{dr.GetValue(5)} \nStatus:\t{dr.GetValue(4)}";
                Console.WriteLine(confirmation);
                verify= true; 
                Console.WriteLine();
                Console.WriteLine("Please enter in the fields you wish to update (leave blank otherwise): ");
                prevLogin = System.Convert.ToString(dr.GetValue(2));
                prevStatus = System.Convert.ToString(dr.GetValue(6));
                prevHolderName = System.Convert.ToString(dr.GetValue(1));
                prevPinCode = System.Convert.ToString(dr.GetValue(4));
            }
            else
            {
                verify = false;
                confirmation=("NO ACCOUNT FOUND");
                Console.WriteLine(confirmation);
            }
            connection.Close();
            if (verify==true)
            {
                Console.WriteLine("Login: ");
                string newLogin = string.Empty;
                newLogin=Console.ReadLine();
                Console.WriteLine("Pin Code: ");
                int newPinCode = 0;
                newPinCode=int.Parse(Console.ReadLine());
                Console.WriteLine("Holder Name: ");
                string newHolderName = string.Empty;
                newHolderName=Console.ReadLine();
                Console.WriteLine("Status: ");
                string newStatus = string.Empty;
                newStatus=Console.ReadLine();

                if (newLogin==string.Empty)
                {
                    newLogin = prevLogin;
                }
                else if(newPinCode==0)
                {
                    newPinCode = int.Parse(prevPinCode);
                }
                if (newHolderName == string.Empty)
                {
                    newHolderName = prevHolderName;
                }
                else if (newStatus == string.Empty)
                {
                    newStatus = prevStatus;
                }
                connection.Open();
                
                string updateQuery = $"Update Accounts set Login='{newLogin}',PinCode='{newPinCode}',Holder='{newHolderName}',Status='{newStatus}' where Id='{cBO.AccountNo}'";
                SqlCommand cmd1 = new SqlCommand(updateQuery, connection);
                int UpdateAccount = cmd1.ExecuteNonQuery();
                if (UpdateAccount >= 1)
                {
                    Console.WriteLine("Accouunt Updated");
                }
                else
                {
                    Console.WriteLine("Update Action Failed");
                }
                connection.Close();
            }

        }
        public static void DeleteAccount(CustomerBO cBO)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Select * from Accounts where Id='{cBO.AccountNo}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            bool accConfirm;
            if (dr.Read())
            {
                string confirmation = $"Account Holder: {dr.GetValue(1)}";
                Console.WriteLine(confirmation);
                accConfirm = true;
            }
            else
            {
                Console.WriteLine("ACCOUNT NOT FOUND");
                accConfirm = false;
            }
            connection.Close();
            if (accConfirm==true)
            {
                connection.Open();
                string query1 = $"Delete from Accounts where Id='{cBO.AccountNo}'";
                SqlCommand cmd1 = new SqlCommand(query1, connection);
                int DeleteAccount=cmd1.ExecuteNonQuery();
                if (DeleteAccount >= 1)
                {
                    Console.WriteLine("Accouunt Deleted");
                }
                else
                {
                    Console.WriteLine("Deletion Failed");
                }
            }
            connection.Close();

        }
        public static void AdminVerification(AdminBO aBO)
        {
            bool verify = false;
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = $"Select * from Admin where AdminName=@U and AdminPass=@P";
            SqlParameter p1 = new SqlParameter("U", aBO.Name);
            SqlParameter p2 = new SqlParameter("P", aBO.Password);
            SqlCommand cmd=new SqlCommand(query, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("DATABASE IS WORKING");
            if (dr.Read())
            {
                //if(dr.GetValue(1)==aBO.Name)
                Console.WriteLine("Admin Authenticated");
                Console.WriteLine();
                verify=true;

            }
            else
            {
                Console.WriteLine("Invalid Username and password");
            }
            connection.Close();
            aBO.verify=verify;
        }
        public static void createAccount(CustomerBO cBO)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"Insert into Accounts(Login,PinCode,Holder,Type,StartingBalance,Status)values('{cBO.Login}','{cBO.PinCode}','{cBO.HolderName}','{cBO.Type}','{cBO.Balance}','{cBO.Status}')";
            SqlCommand cmd = new SqlCommand(query, connection);
            int dr = cmd.ExecuteNonQuery();
            if (dr >= 1)
            {
                Console.WriteLine("Account Created Successfully");
                string x = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Action Failed!!");

            }
            connection.Close();
        }
        public static void SearchAccount(CustomerBO cBO)
        {
            string searchWith = string.Empty;
            string searchWithValue = string.Empty;
            int searchWithValue2 = 0;
            string aID=string.Empty, B = string.Empty, hN = string.Empty, T = string.Empty, S = string.Empty;
            if (cBO.Type!=String.Empty)
            {
                searchWith = "Type";
                searchWithValue = cBO.Type;
            }
            else if(cBO.HolderName!=String.Empty)
            {
                searchWith= "Holder";
                searchWithValue = cBO.HolderName;
            }
            else if (cBO.AccountNo!=0)
            {
                searchWith = "Id";
                searchWithValue2 = cBO.AccountNo;
            }
            else if (cBO.Balance!=0)
            {
                searchWith = "StartingBalance";
                searchWithValue2 = cBO.Balance;
            }
            else if (cBO.Status!=string.Empty)
            {
                searchWith = "Status";
                searchWithValue= cBO.Status; 
            }
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = string.Empty;
            if (searchWithValue!= String.Empty)
            {
                query = $"Select * from Accounts where {searchWith}='{searchWithValue}'";
            }
            else//This will search with account number or ablance
            {
                query = $"Select * from Accounts where '{searchWith}'='{searchWithValue2}'";

            }
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aID = System.Convert.ToString( dr.GetValue(0));
                hN = System.Convert.ToString(dr.GetValue(1));
                T = System.Convert.ToString(dr.GetValue(4));
                B = System.Convert.ToString(dr.GetValue(5));
                S = System.Convert.ToString(dr.GetValue(6));


                Console.WriteLine($"{dr.GetValue(0)}\t{dr.GetValue(1)}\t{dr.GetValue(4)}\t{dr.GetValue(5)}\t{dr.GetValue(6)}");
            }
            connection.Close();
            Console.WriteLine($"Account ID\tHolder Name\tType\tBalance\tStatus");
            Console.WriteLine($"{aID}\t{hN}\t{T}\t{B}\t{S}");
            Console.WriteLine("hdss");
            string b = Console.ReadLine();
            //showSearch(AccountFound);

        }
        public static void showSearch(string[] x)
        {
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine(x[i]);

            }
        }
    }
}
