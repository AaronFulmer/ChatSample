
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
        //  Add all new users to a group named their username
        public void addGroup(string name) => Groups.AddAsync(Context.ConnectionId, name);

        public void Dm(string name, string target, string message) {

            string toSend = "[PRIVATE] " + message;                              //  Add a [PRIVATE] marker to private messages
            Clients.Group(target).InvokeAsync("broadcastMessage", name, toSend); //  Send the private message
            Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", //  Send a message to the sender with a copy of the message
                name + " to " + target, toSend);
        }
    }
}