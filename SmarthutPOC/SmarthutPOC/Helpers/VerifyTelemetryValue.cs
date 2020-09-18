using System;
using System.Collections.Generic;
using SmarthutPOC.Data;

namespace SmarthutPOC.Helpers
{
    public class VerifyTelemetryValue
    {


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
                if (device.Value - 20 < device.MinValue)
                {
                    device.IsAlarm = true;
                    return device;
                }
                else if (device.Value > device.MaxValue)
                {
                    device.IsAlarm = true;
                    return device;
                }
            }

            return device;
        }
    }
}