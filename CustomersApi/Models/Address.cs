using System.ComponentModel.DataAnnotations.Schema;

namespace CustomersApi.Models
{
    [Table("Address")]
    public class Address
    {
        [Column("AddressId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string StreetAddress { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string State { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PostalCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        public long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }
    }
}
