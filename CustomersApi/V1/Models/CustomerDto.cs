using System;
using System.Collections.Generic;

namespace CustomersApi.V1.Models
{
    public class CustomerDto
    {
        public long CustomerId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastOrderDate { get; set; }

        public decimal TotalOrderValue { get; set; }

        public long NumberOfOrders { get; set; }

        public List<ContactDto> Contacts { get; set; }
    }
}
