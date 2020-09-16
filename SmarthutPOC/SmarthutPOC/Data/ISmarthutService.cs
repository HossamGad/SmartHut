using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarthutPOC.Data
{
    public interface ISmarthutService
    {
        public Task<IEnumerable<Units>> GetUnits();
        public Task<Building> GetBuilding();
        public Task<IEnumerable<Device>> GetDevices();
        public Task<NegotiationResult> NegotiateSignalR();
    }
}
