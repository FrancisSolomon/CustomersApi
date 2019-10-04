using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomersApi.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Column("CustomerId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public decimal? TotalOrderValue { get; set; }

        public long NumberOfOrders { get; set; }

        public virtual List<Contact> Contacts { get; set; }
    }
}
