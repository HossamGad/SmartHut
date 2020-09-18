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
            foreach(var device in devices)
            {
                foreach (var unit in await GetUnits())
                {
                    if(device.UnitId == unit.Id)
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

        public async Task RestoreAlarm(Guid deviceId)
        {
            
            var client = new HttpClient();
           
            var jsonObject = new RestoreAlarm()
            {
                DeviceId = deviceId,
                UserName = _httpContextAccessor.HttpContext.User.Identity.Name
            };
            
            var json = JsonConvert.SerializeObject(jsonObject);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(_configuration.GetValue<Uri>("SmartHutApi:RestoreAlarmUri"), content);
            
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

            // var person = new Person();
            // person.Name = "John Doe";
            // person.Occupation = "gardener";

            // var json = JsonConvert.SerializeObject(person);
            // var data = new StringContent(json, Encoding.UTF8, "application/json");

            // var url = "https://httpbin.org/post";
            // using var client = new HttpClient();

            // var response = await client.PostAsync(url, data);

            // string result = response.Content.ReadAsStringAsync().Result;
            // Console.WriteLine(result);

        //private static JsonSerializerOptions JsonOptions()
        //{
        //    return new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault, PropertyNameCaseInsensitive = true };
        //}

        //public static MemoryCacheEntryOptions MemoryCacheEntryOpt()
        //{
        //    return new MemoryCacheEntryOptions()
        //    {
        //        AbsoluteExpiration = DateTime.Now.AddHours(6),
        //        Priority = CacheItemPriority.Normal,
        //        SlidingExpiration = TimeSpan.FromMinutes(5)
        //    };
        //}


    }  
}
