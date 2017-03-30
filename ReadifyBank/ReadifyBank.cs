﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace ReadifyBank
{
    class ReadifyBank : Interfaces.IReadifyBank
    {
        private IList<IAccount> accountList;
        private IList<IStatementRow> transactionLog;
        private int LNNum;
        private int SVNum;
        public ReadifyBank()
        {
            this.accountList = new List<IAccount>();
            this.transactionLog = new List<IStatementRow>();
            this.LNNum = 1;
            this.SVNum = 1;
        }

        public IList<IAccount> AccountList
        {
            get
            {
                return accountList;
            }
        }

        public IList<IStatementRow> TransactionLog
        {
            get
            {
                return transactionLog;
            }
        }

        public decimal CalculateInterestToDate(IAccount account, DateTimeOffset toDate)
        {
            if (account == null || accountList.Contains(account) == false)
            {
                Console.Error.WriteLine("Account does not exist");
                return 0;
            }

            DateTimeOffset openDate = account.OpenedDate;
            TimeSpan difference = toDate.Subtract(openDate);
            if (toDate.CompareTo(openDate) <= 0) 
            {
                return 0;
            }

            if (account.AccountNumber.StartsWith("LN"))
            {

            }
            else
            {

            }
        }

        public IEnumerable<IStatementRow> CloseAccount(IAccount account, DateTimeOffset closeDate)
        {
            throw new NotImplementedException();
        }

        public decimal GetBalance(IAccount account)
        {
            if(account == null || accountList.Contains(account) == false)
            {
                Console.Error.WriteLine("Account does not exist");
                return 0;
            }
            return account.Balance;
        }

        public IEnumerable<IStatementRow> GetMiniStatement(IAccount account)
        {
            return transactionLog.Where(x => x.Account.AccountNumber == account.AccountNumber).OrderBy(x=>x.Date).Take(5).ToList();
        }

        public IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            if(customerName == null || customerName.Length == 0)
            {
                Console.Error.WriteLine("The customer name should not be empty");
                return null;
            }
            if (openDate.CompareTo(DateTimeOffset.Now) > 0)
            {
                Console.Error.WriteLine("Open date should not be earlier than the current date");
                return null;
            }
            if(LNNum == 999999)
            {
                Console.Error.WriteLine("There is no available customer number for LN account");
                return null;
            }
            String accountNumber = "LN-" + LNNum.ToString("D6");
            LNNum++;
            Account newAccount = new Account(accountNumber, customerName, openDate);
            accountList.Add(newAccount);
            return newAccount;
        }

        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            if (customerName == null || customerName.Length == 0)
            {
                Console.Error.WriteLine("The customer name should not be empty");
                return null;
            }
            if (openDate.CompareTo(DateTimeOffset.Now) > 0)
            {
                Console.Error.WriteLine("Open date should not be earlier than the current date");
                return null;
            }
            if (SVNum == 999999)
            {
                Console.Error.WriteLine("There is no available customer number for SV account");
                return null;
            }
            String accountNumber = "SV-" + SVNum.ToString("D6");
            SVNum++;
            Account newAccount = new Account(accountNumber, customerName, openDate);
            accountList.Add(newAccount);
            return newAccount;
        }

        public void PerformDeposit(IAccount account, decimal amount, string description, DateTimeOffset depositDate)
        {
            if(amount <= 0)
            {
                Console.Error.WriteLine("The amount should be larger than zero!");
                return;
            }
            if(depositDate.CompareTo(DateTimeOffset.Now) > 0)
            {
                Console.Error.WriteLine("The deposite date should be earlier than today!");
                return;
            }
            if(account == null || this.accountList.Contains(account) == false)
            {
                Console.Error.WriteLine("The account does not exist!");
                return;

            }
            Account _account = (Account)account;
            _account.Balance += amount;
            StatementRow statementRow = new StatementRow(_account, amount, _account.Balance + amount, depositDate, description);
            transactionLog.Add(statementRow);
        }

        public void PerformTransfer(IAccount from, IAccount to, decimal amount, string description, DateTimeOffset transferDate)
        {
            if(from == null || this.accountList.Contains(from) == false)
            {
                Console.Error.WriteLine("The from account does not exist!");
                return;

            }
            if (from.Balance < amount)
            {
                Console.Error.WriteLine("There is no enough money to transfer!");
                return;
            }
            if (amount <= 0)
            {
                Console.Error.WriteLine("The amount should be larger than zero!");
                return;
            }
            if (transferDate.CompareTo(DateTimeOffset.Now) > 0)
            {
                Console.Error.WriteLine("The transfer date should be earlier than today!");
                return;
            }
            if (to == null || this.accountList.Contains(to) == false)
            {
                Console.Error.WriteLine("The to account does not exist!");
                return;

            }
            Account _from = (Account)from;
            _from.Balance -= amount;
            StatementRow statementRow = new StatementRow(_from, amount, _from.Balance - amount, transferDate, description);
            transactionLog.Add(statementRow);
            Account _to = (Account)to;
            _to.Balance += amount;
            statementRow = new StatementRow(_to, amount, _to.Balance + amount, transferDate, description);
            transactionLog.Add(statementRow);
        }

        public void PerformWithdrawal(IAccount account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            if (account == null || this.accountList.Contains(account) == false)
            {
                Console.Error.WriteLine("The account does not exist!");
                return;

            }
            if (account.Balance < amount)
            {
                Console.Error.WriteLine("There is no enough money to withDrawal!");
                return;
            }
            if (amount <= 0)
            {
                Console.Error.WriteLine("The amount should be larger than zero!");
                return;
            }
            if (withdrawalDate.CompareTo(DateTimeOffset.Now) > 0)
            {
                Console.Error.WriteLine("The transfer date should be earlier than today!");
                return;
            }
            Account _account = (Account)account;
            _account.Balance -= amount;
            StatementRow statementRow = new StatementRow(_account, amount, _account.Balance - amount, withdrawalDate, description);
            transactionLog.Add(statementRow);
        }
    }
}