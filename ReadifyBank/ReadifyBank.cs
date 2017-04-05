using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace ReadifyBank
{
    // This class is the implementation class of IReadifyBank interface
    class ReadifyBank : Interfaces.IReadifyBank
    {
        //This const is the yearly interest rate for home loan account 
        private const decimal INTEREST_RATE_LN = 0.0399M;
        //This const is the yearly interest rate for saving account
        private const decimal INTEREST_RATE_SV = 0.06M * 12M;

        //This variable stores all active account
        private IList<IAccount> accountList;
        //This variable stores all transaction records
        private IList<IStatementRow> transactionLog;
        //This variable stores all close account
        private IList<IAccount> closedAccountList;

        //This variable stores the current available home load count number
        private int LNNum;
        //This variable stores the current available saving account
        private int SVNum;

        /// <summary>
        /// Construct method
        /// </summary>
        public ReadifyBank()
        {
            this.accountList = new List<IAccount>();
            this.transactionLog = new List<IStatementRow>();
            this.closedAccountList = new List<IAccount>();
            this.LNNum = 1;
            this.SVNum = 1;
        }

        /// <summary>
        /// Bank accounts list
        /// </summary>
        public IList<IAccount> AccountList
        {
            get
            {
                return accountList;
            }
        }

        /// <summary>
        /// Transactions log of the bank
        /// </summary>
        public IList<IStatementRow> TransactionLog
        {
            get
            {
                return transactionLog;
            }
        }

        /// <summary>
        /// Calculate interest rate for an account to a specific time
        /// The interest rate for Saving account is 6% monthly
        /// The interest rate for Home loan account is 3.99% annually
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <param name="toDate">Calculate interest to this date</param>
        /// <returns>The added value</returns>
        public decimal CalculateInterestToDate(IAccount account, DateTimeOffset toDate)
        {
            if(!checkAcc(account) || isEarlier(toDate, DateTimeOffset.Now))
            {
                return -1;
            }

            IStatementRow lastStatement = transactionLog.Where(x => x.Account.AccountNumber == account.AccountNumber).Last();
            
            int days = (toDate - lastStatement.Date).Days;
            if (account.AccountNumber.StartsWith("LN"))
            {
                return days * INTEREST_RATE_LN / 365 * account.Balance;
            }
            else
            {
                return days * INTEREST_RATE_SV / 365 * account.Balance;
            }
        }

        /// <summary>
        /// Close an account
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <param name="closeDate">Close Date</param>
        /// <returns>All transactions happened on the closed account</returns>
        public IEnumerable<IStatementRow> CloseAccount(IAccount account, DateTimeOffset closeDate)
        {
            if(!checkAcc(account) || !isEarlier(closeDate, DateTimeOffset.Now))
            {
                return new List<IStatementRow>(); 
            }

            accountList.Remove(account);
            PerformWithdrawal(account, account.Balance,"Withdral all money due to close date", DateTimeOffset.Now);
            Account closeAccount = new Account(account.AccountNumber, account.CustomerName, closeDate);
            closedAccountList.Add(closeAccount);
            return transactionLog.Where(x => x.Account.AccountNumber == account.AccountNumber).OrderBy(x => x.Date).ToList();
        }

        /// <summary>
        /// Return the balance for an account
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <returns></returns>
        public decimal GetBalance(IAccount account)
        {
            if(checkAcc(account) == false)
            {
                return -1;
            }
            return account.Balance;
        }

        /// <summary>
        /// Get mini statement (the last 5 transactions occurred on an account)
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <returns>Last five transactions</returns>
        public IEnumerable<IStatementRow> GetMiniStatement(IAccount account)
        {
            if(checkAcc(account) == false)
            {
                return new List<IStatementRow>();
            }
            return transactionLog.Where(x => x.Account.AccountNumber == account.AccountNumber).OrderByDescending(x=>x.Date).Take(5).ToList();  //bug
        }

        /// <summary>
        /// Open a home loan account
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <param name="openDate">The date of the transaction</param>
        /// <returns>Opened Account</returns>
        public IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            if (!checkCustomerName(customerName) || !isEarlier(openDate, DateTimeOffset.Now) || !isFull("LN"))
            {
                return null;
            }
            
            String accountNumber = "LN-" + LNNum.ToString("D6");
            LNNum++;
            Account newAccount = new Account(accountNumber, customerName, openDate);
            accountList.Add(newAccount);
            return newAccount;
        }

        /// <summary>
        /// Open a saving account
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <param name="openDate">The date of the transaction</param>
        /// <returns>Opened account</returns>
        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            
            if(!checkCustomerName(customerName) || !isEarlier(openDate, DateTimeOffset.Now) || !isFull("SV"))
            {
                return null;
            }
            String accountNumber = "SV-" + SVNum.ToString("D6");
            SVNum++;
            Account newAccount = new Account(accountNumber, customerName, openDate);
            accountList.Add(newAccount);
            return newAccount;
        }

        /// <summary>
        /// Deposit amount in an account
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="depositDate">The date of the transaction</param>
        public void PerformDeposit(IAccount account, decimal amount, string description, DateTimeOffset depositDate)
        {
            if (!checkAcc(account) || !isEarlier(depositDate, DateTimeOffset.Now))
            {
                return;
            }

            if (amount <= 0)
            {
                Console.Error.WriteLine("The amount should be larger than zero!");
                return;
            }
            
            Account _account = (Account)account;
            _account.Balance += amount;
            StatementRow statementRow = new StatementRow(_account, amount, _account.Balance + amount, depositDate, description);
            transactionLog.Add(statementRow);
        }

        /// <summary>
        /// Transfer amount from an account to an account
        /// </summary>
        /// <param name="from">From account</param>
        /// <param name="to">To account</param>
        /// <param name="amount">Transfer amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="transferDate">The date of the transaction</param>
        public void PerformTransfer(IAccount from, IAccount to, decimal amount, string description, DateTimeOffset transferDate)
        {
            if (!checkAcc(from) || !checkMoney(from, amount) || !isEarlier(transferDate, DateTimeOffset.Now) || !checkAcc(to))
            {
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
            statementRow = new StatementRow(_from, -amount, _from.Balance - amount, transferDate, description);
            transactionLog.Add(statementRow);
        }

        /// <summary>
        /// Withdraw amount in an account
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">Withdrawal amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="withdrawalDate">The date of the transaction</param>
        public void PerformWithdrawal(IAccount account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            if(!checkAcc(account) || !checkMoney(account, amount) || !isEarlier(withdrawalDate, DateTimeOffset.Now))
            {
                return;
            }
            
            Account _account = (Account)account;
            _account.Balance -= amount;
            StatementRow statementRow = new StatementRow(_account, amount, _account.Balance - amount, withdrawalDate, description);
            transactionLog.Add(statementRow);
        }


        /// <summary>
        /// Check whether account is valid
        /// </summary>
        /// <param name="account">Account</param>
        /// <returns>Whether account is valid</returns>
        private bool checkAcc(IAccount account)
        {
            if (account == null || this.accountList.Contains(account) == false)
            {
                Console.Error.WriteLine("The account does not exist!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether account have enough money
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">Withdrawal or transfer amount</param>
        /// <returns>Wheter accout have enough money</returns>
        private bool checkMoney(IAccount account, decimal amount)
        {
            if (account.Balance < amount)
            {
                Console.Error.WriteLine("There is no enough money!");
                return false;
            }
            if (amount <= 0)
            {
                Console.Error.WriteLine("The amount should be larger than zero!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether date1 is early than date2
        /// </summary>
        /// <param name="date1">Date1</param>
        /// <param name="date2">Date2</param>
        /// <returns>Whether date1 is early than date2</returns>
        private bool isEarlier(DateTimeOffset date1, DateTimeOffset date2)
        {
            if (date1.CompareTo(date2) > 0)
            {
                Console.Error.WriteLine("The transfer date should be earlier than today!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether customer name is valid
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <returns>Whether customer name is valid</returns>
        private bool checkCustomerName(string customerName)
        {
            if (customerName == null || customerName.Length == 0)
            {
                Console.Error.WriteLine("The customer name should not be empty");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether there is no valid account number
        /// </summary>
        /// <param name="type">Type of account</param>
        /// <returns>Whether there is no valid account number</returns>
        private bool isFull(string type)
        {
            if(type == "SV")
            {
                if (SVNum == 999999)
                {
                    Console.Error.WriteLine("There is no available customer number for SV account");
                    return false;
                }
            }else{
                if (LNNum == 999999)
                {
                    Console.Error.WriteLine("There is no available customer number for LN account");
                    return false;
                }
            }
            return true;
        }
    }
}
