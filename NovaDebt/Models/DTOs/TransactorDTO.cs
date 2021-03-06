﻿using System.Xml.Serialization;

namespace NovaDebt.Models.DTOs
{
    [XmlType("Transactor")]
    public class TransactorDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("no")]
        public int No { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Since")]
        public string Since { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("Facebook")]
        public string Facebook { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("CurrencyAbbreviation")]
        public string CurrencyAbbreviation { get; set; }

        [XmlElement("TransactorType")]
        public string TransactorType { get; set; }
    }
}
