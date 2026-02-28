using Microsoft.AspNetCore.SignalR;

namespace MadlenArtGallery.Hubs
{
    public class BidHub : Hub
    {
        public async Task SendNewBid(int artworkId, string userName, int bidAmount)
        {
            await Clients.All.SendAsync("ReceiveNewBid", artworkId, userName, bidAmount);
        }

        public async Task JoinArtworkRoom(int artworkId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"artwork_{artworkId}");
        }

        public async Task LeaveArtworkRoom(int artworkId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"artwork_{artworkId}");
        }
    }
}
