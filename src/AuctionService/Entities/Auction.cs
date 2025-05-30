namespace AuctionService.Entities;

public class Auction : BaseEntity
{
    public int ReservePrice { get; set; }
    public string Seller { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }
    public DateTime AuctionEnd { get; set; }
    public AuctionStatus Status { get; set; }

    public Item Item { get; set; } = null!;
}
