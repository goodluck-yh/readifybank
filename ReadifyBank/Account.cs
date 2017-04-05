using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank
{
    // This class is the implementation class of IAccount interface\
    class Account : Interfaces.IAccount
    {
        private string accountNumber, customerName;
        private decimal balance;
        private DateTimeOffset openedDate;

        /// <summary>
        /// Construct Method
        /// </summary>
        /// <param name="accountNumber">Account number</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="openedDate">Opened Date</param>
        public Account(string accountNumber, string customerName, DateTimeOffset openedDate)
        {
            this.accountNumber = accountNumber;
            this.customerName = customerName;
            this.balance = 0;
            this.openedDate = openedDate;
        }

        /// <summary>
        /// Account number 
        /// It is formatted as follows: 2 characters for Account type, dash and 6 digits for account number starting from 1
        /// For home loan account it should start from "LN-000001"
        /// For saving account it should start from "SV-000001"
        /// </summary>
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
        }

        /// <summary>
        /// Current account balance
        /// </summary>
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

        /// <summary>
        /// Customer Name
        /// </summary>
        public string CustomerName
        {
            get
            {
                return customerName;
            }
        }

        /// <summary>
        /// The date when the account was opened
        /// </summary>
        public DateTimeOffset OpenedDate
        {
            get
            {
                return openedDate;
            }
            set
            {
                openedDate = value;
            }
        }
    }
}
