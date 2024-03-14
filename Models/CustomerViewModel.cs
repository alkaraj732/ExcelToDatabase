﻿namespace ExcelToDatabase.Models
{
    public class CustomerViewModel
    {
        public string? FileName { get; set; }
        public DateTime? ImportedOn { get; set; }
        public int? ImportedBy { get; set; }
        public string? NAME { get; set; }
        public decimal? PHONENUMBER { get; set; }
        public string? TAGS { get; set; }
        public decimal? AGENTPHONENUMBER { get; set; }
        public DateTime? CUSTOMERDATECREATED { get; set; }
        public string? SOURCE { get; set; }
        public string? CUSTOMERBLOCKEDSTATUS { get; set; }
        public DateTime? LASTTEMPLATESENTAT { get; set; }
        public DateTime? FIRSTMESSAGERECEIVEDAT { get; set; }
        public DateTime? FIRSTMESSAGESENTAT { get; set; }
        public string? WHATSAPPNAME { get; set; }
        public string? OPTOUT { get; set; }
        public DateTime? LASTMESSAGESENTAT { get; set; }
        public string? CUSTOMERNAME { get; set; }
        public string? EMAIL { get; set; }
        public string? CITY { get; set; }
        public string? COI { get; set; }
        public string? RTI { get; set; }
        public string? LINKEDIN { get; set; }
    }
}
