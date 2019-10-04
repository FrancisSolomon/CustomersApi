using System.Collections.Generic;

namespace CustomersApi.V1.Models
{
    public class DtoContact
    {
        public long ContactId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactType { get; set; }

        public List<DtoAddress> Addresses { get; set; }
    }
}
