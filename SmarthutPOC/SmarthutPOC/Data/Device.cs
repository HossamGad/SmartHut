using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public class Device
    { 
        public Guid BuildingId { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public Guid UnitId { get; set; }
        public int MetricType { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Units Units { get; set; }
    }
}
