using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank
{
    class Account : Interfaces.IAccount
    {
        private string accountNumber, customerName;
        private decimal balance;
        private DateTimeOffset openedDate;

        public Account(string accountNumber, string customerName, DateTimeOffset openedDate)
        {
            this.accountNumber = accountNumber;
            this.customerName = customerName;
            this.balance = 0;
            this.openedDate = openedDate;
        }
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
        }

        public decimal Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        public string CustomerName
        {
            get
            {
                return customerName;
            }
        }

        public DateTimeOffset OpenedDate
        {
            get
            {
                return openedDate;
            }
        }
    }
}
