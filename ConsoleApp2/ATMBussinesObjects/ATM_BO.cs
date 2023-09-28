using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMBussinesObjects
{
   
        public class CustomerBO
        {
            public string HolderName { get; set; }
            public string Login { get; set; }
            public int PinCode { get; set; }
            public int Balance { get; set; }
            public string Type { get; set; }
            public int AccountNo { get; set; }
            public string Status { get; set; }
            public int UserId { get; set; }
        public int totalAmountWithdraw { get; set; }


    }
        public class AdminBO
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public bool verify { get; set; }

    }
    
}
