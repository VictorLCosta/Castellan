namespace SearchService.Entities;

public class Item : Entity
{
    public int ReservedPrice { get; set; }
    public string Seller { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; } = string.Empty;
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string Color { get; set; }
    public int Mileage { get; set; }
    public required string ImageUrl { get; set; }
}
