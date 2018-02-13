
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace SignalRChatSample
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("broadcastMessage", name, message);
        }
        public void addGroup(string name) {
            var GroupName = name;
            Groups.AddAsync(Context.ConnectionId, name);
            //Clients.Group(GroupName).InvokeAsync("broadcastMessage", "SYSTEM", "Joined: " + GroupName);
        }
        public string Add2Group(string connectionId)
        {

            var GroupName = groupName();
            Groups.AddAsync(connectionId, GroupName);
            Groups.AddAsync(Context.ConnectionId, GroupName);
            Clients.Group(GroupName).InvokeAsync("broadcastMessage", "SYSTEM", "Joined: " + GroupName);
            return GroupName;

        }
        public void getMyId(string name)
        {
            Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", name, Context.ConnectionId);
        }
        public string getId() => Context.ConnectionId.ToString();

        private string groupName()
        {
            var randy = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz1234567890";
            var chars = Enumerable.Range(0, 9).Select(x => pool[randy.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
        public void SendDM(string name, string groupName, string message)
        {
            
            string toSend = "[PRIVATE] " + message;
            Clients.Group(groupName).InvokeAsync("broadcastMessage", name, toSend);
            //Clients.Group(groupName).InvokeAsync("broadcastMessage", name, message);

        }
        public void Dm(string name, string target, string message) {
            string toSend = "[PRIVATE] " + message;
            Clients.Group(target).InvokeAsync("broadcastMessage", name, toSend);
            Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", name + " to " + target, toSend);
        }
    }
}