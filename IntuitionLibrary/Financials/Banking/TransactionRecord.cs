using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Financials.Banking
{
    /// <summary>
    /// Details of a transaction recorded in a bank system.
    /// <para/>Based on mBank implementation.
    /// </summary>
    public class TransactionRecord : AccountTransaction
    {
        /// <summary>
        /// Example: Data operacji		11-03-2009
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Example: Data księgowania		11-03-2009
        /// </summary>
        public DateTime AccountingDate { get; set; }
        /// <summary>
        /// Example: Numer operacji		4547
        /// </summary>
        public int TransactionNumber { get; set; }
        /// <summary>
        /// Example: Rodzaj operacji		PRZELEW WEWNĘTRZNY WYCHODZĄCY
        /// </summary>
        public string TransactionKind { get; set; }
        /// <summary>
        /// Example: Tytuł operacji		RESZTA 
        /// </summary>
        public string TransactionTitle { get; set; }
        /// <summary>
        /// Example: Rachunek odbiorcy		77 1140 2004 0000 3102 1092 2324
        /// </summary>
        public string ReceivingAccount { get; set; }
        /// <summary>
        /// Example: Nazwa/imię i nazwisko odbiorcy		ARTUR PERWENIS
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// Example: Adres odbiorcy (ulica)		POLSKICH MARYNARZY 8/10
        /// </summary>
        public string ReceiverAddressLine1 { get; set; }
        /// <summary>
        /// Example: Kod pocztowy, miejscowość odbiorcy		71-050 SZCZECIN
        /// </summary>
        public string ReceiverAddressLine2 { get; set; }
        /// <summary>
        /// Example: Rachunek nadawcy		37 1140 2004 0000 3802 0646 7394
        /// </summary>
        public string SenderAccount { get; set; }
        /// <summary>
        /// Example: Nazwa/imię i nazwisko nadawcy		ARTUR GRZEGORZ PERWENIS
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Example: Adres nadawcy (ulica)		UL POLSKICH MARYNARZY 8 M. 10
        /// </summary>
        public string SenderAddressLine1 { get; set; }
        /// <summary>
        /// Example: Kod pocztowy, miejscowość nadawcy		71-050 SZCZECIN
        /// </summary>
        public string SenderAddressLine2 { get; set; }
        /// <summary>
        /// Example: Kwota operacji		188,43 PLN
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// Example: Saldo po operacji		0,00 PLN
        /// <para/>TODO: Do we need this as part of transaction record?
        /// </summary>
        public decimal AccountBalanceAfterTransaction { get; set; }
    }
}
