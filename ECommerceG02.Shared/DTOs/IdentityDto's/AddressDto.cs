using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerceG02.Shared.DTOs.AddressDtos
{
    public class AddressDto
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

