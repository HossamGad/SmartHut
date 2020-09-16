using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public class Telemetry
    {
        public Guid BuildingId { get; set; }
        public Guid DeviceId { get; set; }
        public long Time { get; set; }
        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = (float)Math.Round(value, 1); }
        }
    }
}
