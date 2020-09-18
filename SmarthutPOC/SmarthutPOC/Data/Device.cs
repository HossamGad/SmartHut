using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public class Device
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public Units Units { get; set; }
        public int MetricType { get; set; }
        public Guid BuildingId { get; set; }

        //Below properties are used for signalR
        public Guid DeviceId { get; set; }
        public DateTime Time { get; set; }


        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = (float)Math.Round(value, 1); }
        }

        public bool IsAlarm { get; set; }

    }
}