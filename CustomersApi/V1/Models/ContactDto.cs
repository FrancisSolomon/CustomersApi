using System.Collections.Generic;

namespace CustomersApi.V1.Models
{
    public class ContactDto
    {
        public long ContactId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactType { get; set; }

        public List<AddressDto> Addresses { get; set; }
    }
}
