using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace ReadifyBank
{
    // This class is the implementation class of IStatementRow infercace  
    class StatementRow : Interfaces.IStatementRow
    {
        private Account account;
        private decimal amount;
        private decimal balance;
        private DateTimeOffset date;
        private string description;

        /// <summary>
        /// Construct Method
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">The amount of money</param>
        /// <param name="balance">Balance</param>
        /// <param name="date">Transaction date</param>
        /// <param name="description">Description</param>
        public StatementRow(Account account, decimal amount, decimal balance, DateTimeOffset date, string description)
        {
            this.account = account;
            this.amount = amount;
            this.balance = balance;
            this.date = date;
            this.description = description;
        }

        /// <summary>
        /// Account on which the transaction is made
        /// </summary>
        public IAccount Account
        {
            get
            {
                return account;
            }
        }

        /// <summary>
        /// Amount of the operation
        /// </summary>
        public decimal Amount
        {
            get
            {
                return amount;
            }
        }

        /// <summary>
        /// Balance of the account after the transaction
        /// </summary>
        public decimal Balance
        {
            get
            {
                return balance;
            }
        }

        /// <summary>
        /// Date and time of the transaction
        /// </summary>
        public DateTimeOffset Date
        {
            get
            {
                return date;
            }
        }

        /// <summary>
        /// Description of the transaction
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }
    }
}
