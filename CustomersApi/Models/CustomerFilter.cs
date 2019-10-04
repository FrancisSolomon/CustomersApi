namespace CustomersApi.Models
{
    public class CustomerFilter : BaseFilter
    {
        public string Name { get; set; }

        public bool? IncludeInactive { get; set; }

        public decimal? MinimumSpend { get; set; }
    }
}
