using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomersApi.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Column("ContactId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ContactType { get; set; }

        public long CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual List<Address> Addresses { get; set; }
    }
}
