import AuctionCard from "@/components/auctions/AuctionCard";
import { Auction } from "@/models/Auction";

const fetchAuctions = async () => {
  const res = await fetch("http://localhost:6001/search", {cache: "force-cache"});

  if (!res.ok) {
    throw new Error("Failed to fetch auctions");
  }

  return await res.json() as Auction[];
};

export default async function Page() {
  const auctions = await fetchAuctions();

  return (
    <div className="grid grid-cols-4 gap-6">
      {auctions.map(auction => (
        <AuctionCard key={auction.id} auction={auction} />
      ))}
    </div>
  );
}
