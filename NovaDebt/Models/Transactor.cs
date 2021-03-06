﻿using NovaDebt.Models.Contracts;
using System;

namespace NovaDebt.Models
{
    public class Transactor : ITransactor
    {
        private int id;
        private int no;
        private string name;
        private string since;
        private string dueDate;
        private string phoneNumber;
        private string email;
        private string facebook;
        private decimal amount;
        private string currencyAbbreviation;
        private string transactorType;

        public Transactor()
        {
        }

        public Transactor(string name, string since, string dueDate, string phoneNumber, string email, string facebook, decimal amount, string currencyAbbreviation, string transactorType)
        {
            this.name = name;
            this.since = since;
            this.dueDate = dueDate;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.facebook = facebook;
            this.amount = amount;
            this.currencyAbbreviation = currencyAbbreviation;
            this.transactorType = transactorType;
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int No
        {
            get { return this.no; }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException($"{nameof(this.No)} cannot be less or equal to zero.");
                }

                this.no = value;
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException($"{nameof(this.Name)} cannot be null.");
                }

                this.name = value;
            }
        }

        public string Since
        {
            get { return this.since; }
            set { this.since = value; }
        }

        public string DueDate
        {
            get { return this.dueDate; }
            set { this.dueDate = value; }
        }

        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set { this.phoneNumber = value; }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public string Facebook
        {
            get { return this.facebook; }
            set { this.facebook = value; }
        }

        public decimal Amount
        {
            get { return this.amount; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException($"{nameof(this.Amount)} cannot be less than zero.");
                }

                this.amount = value;
            }
        }

        public string CurrencyAbbreviation
        {
            get { return this.currencyAbbreviation; }
            set { this.currencyAbbreviation = value; }
        }

        public string TransactorType
        {
            get { return this.transactorType; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException($"{nameof(this.TransactorType)} cannot be null.");
                }
                else if (value != "Debtor" || value != "Creditor")
                {
                    throw new InvalidOperationException($"{nameof(this.TransactorType)} should be either Debtor or Creditor.");
                }

                this.transactorType = value;
            }
        }
    }
}
