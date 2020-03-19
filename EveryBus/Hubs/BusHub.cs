using EveryBus.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EveryBus.Hubs
{
    public class BusHub : Hub
    {
        public async Task SendMessage(string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }
    }
}