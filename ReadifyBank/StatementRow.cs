using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace ReadifyBank
{
    class StatementRow : Interfaces.IStatementRow
    {
        private Account account;
        private decimal amount;
        private decimal balance;
        private DateTimeOffset date;
        private string description;

        public StatementRow(Account account, decimal amount, decimal balance, DateTimeOffset date, string description)
        {
            this.account = account;
            this.amount = amount;
            this.balance = balance;
            this.date = date;
            this.description = description;
        }

        public IAccount Account
        {
            get
            {
                return account;
            }
        }

        public decimal Amount
        {
            get
            {
                return amount;
            }
        }

        public decimal Balance
        {
            get
            {
                return balance;
            }
        }

        public DateTimeOffset Date
        {
            get
            {
                return date;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }
    }
}
