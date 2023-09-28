using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMBussinesObjects;
using ATMDataAccessLayer;

namespace ATMBussinessLogicLayer
{
    public class ATM_BLL
    {
        public static void createAccount(CustomerBO cBO)
        {
            ATM_DAL.createAccount(cBO);
        }
        public static void VerifyAdmin(AdminBO aBO)
        {
            Console.WriteLine($"Name is{aBO.Name} and pass is {aBO.Password}'");
             ATM_DAL.AdminVerification(aBO);
            
        }
        public static object customerLogin(CustomerBO cBO)
        {
            ATM_DAL.customerLogin(cBO);        
            return cBO;
        }
        public static void DeleteAccount(CustomerBO cBO)
        {
            ATM_DAL.DeleteAccount(cBO);
        }
        public static void UpdateAccount(CustomerBO cBO)
        {
            ATM_DAL.UpdateAccount(cBO);
        }
        public static void FastCash(CustomerBO cBO,int fcAmount)
        {
            ATM_DAL.FastCash(cBO,fcAmount);
        }
        public static void CashTransfer(CustomerBO cBO, CustomerBO recipientBO,int Amount)
        {
            ATM_DAL.CashTransfer(cBO, recipientBO,Amount);
        }
        public static void DepositCash(CustomerBO CBO, int amount)
        {
            ATM_DAL.DepositCash(CBO,amount);
        }
        public static void DisplayAmount(CustomerBO CBO)
        {
            ATM_DAL.DisplayAmount(CBO);
        }
        public static void SearchAccount(CustomerBO cBO)
        {
            ATM_DAL.SearchAccount(cBO);
        }
    }
   
}
