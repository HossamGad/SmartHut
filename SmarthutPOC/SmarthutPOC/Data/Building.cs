using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public class Building
    {
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Guid AzureAdGroupId { get; set; }
        public IEnumerable<Device> Devices { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}