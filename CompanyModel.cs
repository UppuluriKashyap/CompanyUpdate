using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyUpdate
{
    class CompanyModel
    {
            public int id { get; set; }

            /// <summary>
            /// The unique ID number of the account this company belongs to.
            /// </summary>
            public int accountId { get; set; }

            public int? parentCompanyId { get; set; }

            public string companyCode { get; set; }

            public string name { get; set; }

            public bool isDefault { get; set; }

            public int? defaultLocationId { get; set; }

            public bool isActive { get; set; }

            public string taxpayerIdNumber { get; set; }

            public bool hasProfile { get; set; }

            public bool isReportingEntity { get; set; }

            public string defaultCountry { get; set; }

            public string baseCurrencyCode { get; set; }

            public string errorMessage { get; set; }
    }
}

