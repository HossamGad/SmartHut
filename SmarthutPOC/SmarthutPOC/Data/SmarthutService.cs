using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SmarthutPOC.Data
{
    public class SmarthutService : ISmarthutService
    {
        private readonly HttpClient _client; // skapar client
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor; // hämtar instans från minnet för att göra ett anrop
        private readonly IMemoryCache _memoryCache; // sparar anropet i minnet 
        private string _token { get; set; }

        public SmarthutService(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _token = httpContextAccessor.HttpContext.GetTokenAsync("id_token").Result;

            client.BaseAddress = configuration.GetValue<Uri>("SmartHutApi:BaseUri");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            _client = client;
        }

        public async Task<Building> GetBuilding()
        {
            var response = await _client.GetStringAsync("/buildingInfo/getMyBuilding");

            Building building = JsonConvert.DeserializeObject<Building>(response);

            return building;
        }

        public async Task<IEnumerable<Device>> GetDevices()
        {
            var building = await GetBuilding();

            var response = await _client.GetStringAsync($"/BuildingInfo/{building.Id}/true");

            Building buildingWithDevices = JsonConvert.DeserializeObject<Building>(response);
            var devices = buildingWithDevices.Devices;

            //TODO #36 task no5
            foreach (var device in devices)
            {
                foreach (var unit in await GetUnits())
                {
                    if (device.UnitId == unit.Id)
                    {
                        device.Units = unit;
                    }
                }
            }

            return devices;
        }

        public async Task<IEnumerable<Units>> GetUnits()
        {
            var response = await _client.GetStringAsync("/unit");

            var units = JsonConvert.DeserializeObject<IEnumerable<Units>>(response);

            return units;
        }

        public async Task<NegotiationResult> NegotiateSignalR()
        {
            var negotiationClient = new HttpClient();
            negotiationClient.DefaultRequestHeaders.Add("X-MS-SIGNALR-USERID", _httpContextAccessor.HttpContext.User.Identity.Name);

            var response = await negotiationClient.GetStringAsync(_configuration.GetValue<Uri>("SmartHutApi:NegotiateUri"));
            var negotiationResult = JsonConvert.DeserializeObject<NegotiationResult>(response);

            return negotiationResult;
        }

        public async Task<HttpResponseMessage> RestoreAlarm(Guid deviceId)
        {

            try
            {
                var client = new HttpClient();

                var jsonObject = new RestoreAlarm()
                {
                    deviceId = deviceId,
                    userName = _httpContextAccessor.HttpContext.User.Identity.Name
                };

                var json = JsonConvert.SerializeObject(jsonObject);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_configuration.GetValue<Uri>("SmartHutApi:RestoreAlarmUri"),
                    content);
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"Something went wrong when trying to restore the alarm \n Message: {e.Message}");
            }
        }

    }
}
