using ADEntities.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ADSystem.Hubs
{
	/// <summary>
	/// NotificationsHub
	/// </summary>
	/// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
	public class NotificationsHub : Hub
	{
		/// <summary>
		/// Sends the ticket notification.
		/// </summary>
		/// <param name="ticket">The ticket.</param>
		public void SendTicketNotification(string ticket)
		{
			Clients.All.addNewMessageToPage("ClosedTicket", "Entró");
		}
	}
}