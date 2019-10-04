using System.Collections.Generic;

namespace CustomersApi.Models
{
    public class BaseFilter
    {
        public List<string> Include { get; set; } = new List<string>();
    }
}
