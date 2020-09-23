using System;
using System.Collections.Generic;
using SmarthutPOC.Components.Toast.Configuration;
using SmarthutPOC.Components.Toast.Services;
using SmarthutPOC.Data;

namespace SmarthutPOC.Helpers
{
    public class VerifyTelemetryValue
    {
        private static IToastService _toastService;

        public VerifyTelemetryValue(IToastService toastService)
        {
            _toastService = toastService;
        }

        public static List<Device> SetDeviceWithTelemetry(List<Device> devices,
            List<Telemetry> telemetrys)
        {
            foreach (var telemetry in telemetrys)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    if (telemetry.DeviceId == devices[i].Id)
                    {
                        devices[i] = CheckValue(devices[i]);

                        devices[i].Time = UnixTimeToUtc.UnixTimeToDateTime(telemetry.Time);
                        devices[i].Value = telemetry.Value;
                    }
                }
            }

            return devices;
        }

        private static Device CheckValue(Device device)
        {
            //This is needed for the initial load, otherwise the value returned is true for  each responce from the hub during initial load
            if (device.Value > 0)
            {
                if (device.Value < device.MinValue)
                {

                    device.UnitTypeHighLow = device.Units.Unit switch
                    {
                        "°C" => UnitTypeHighLow.CelsiusBelow,
                        "%" => UnitTypeHighLow.HumidityBelow,
                        _ => device.UnitTypeHighLow
                    };

                    device.IsAlarm = true;
                    return device;
                }
                else if (device.Value > device.MaxValue)
                {
                    device.UnitTypeHighLow = device.Units.Unit switch
                    {
                        "°C" => UnitTypeHighLow.CelsiusAbove,
                        "%" => UnitTypeHighLow.HumidityAbove,
                        _ => device.UnitTypeHighLow
                    };

                    device.IsAlarm = true;
                    return device;
                }
            }

            return device;
        }
    }
}