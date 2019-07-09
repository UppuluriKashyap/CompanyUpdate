using System;
using System.Collections.Generic;
using System.Text;
using Ganss.Excel;

namespace CompanyTINUpdate
{
    public class CompanyUpdateModel
    {
        [Column("CompanyId")]
        public int companyId { get; set; }

        [Column("CompanyName")]
        public string name { get; set; }

        [Column("CompanyCode")]
        public string companyCode { get; set; }

        [Column("TIN")]
        public string taxPayerIdNumber { get; set; }

        [Column("DefaultCountry")]
        public string defaultCountry { get; set; }

        [Column("AccountId")]
        public int accountId { get; set; }
    }
}
