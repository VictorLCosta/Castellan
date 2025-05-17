using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Persistence;

public class AuctionDbContext(DbContextOptions<AuctionDbContext> options)
    : DbContext(options)
{
    public DbSet<Auction> Auctions { get; set; }
}
