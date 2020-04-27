using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Qards.Hubs
{
    public class CardsTrafic : Hub
    {
        public async Task SendCard(string from, string to, string family, string name)
        {
            await Clients.All.SendAsync("ReceiveCard", from, to, family, name);
        }
    }
}
