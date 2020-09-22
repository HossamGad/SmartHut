using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public enum UnitTypeHighLow
    {
        HumidityAbove,
        HumidityBelow,
        CelsiusAbove,
        CelsiusBelow,
        DecibelAbove,
        DecibelBelow,
        FahrenheitAbove,
        FahrenheitBelow,
        RpmAbove,
        RpmBelow,
        Ok
    }

    public class Device
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        private float _minValue;
        public float MinValue
        {
            get => _minValue;
            set => _minValue = (float)Math.Round(value, 1);
        }

        private float _maxValue;
        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = (float)Math.Round(value, 1);
        }

        public Units Units { get; set; }
        public int MetricType { get; set; }
        public Guid BuildingId { get; set; }
        public UnitTypeHighLow UnitTypeHighLow { get; set; } = UnitTypeHighLow.Ok;

        //Below properties are used for signalR
        public Guid DeviceId { get; set; }
        public DateTime Time { get; set; }


        private float _value;
        public float Value
        {
            get => _value;
            set => _value = (float)Math.Round(value, 1);
        }

        public bool IsAlarm { get; set; }
    }
}