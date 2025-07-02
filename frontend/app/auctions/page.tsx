const fetchAuctions = async () => {
  const res = await fetch("http://localhost:6001/search", {cache: "force-cache"})

  if (!res.ok) {
    throw new Error("Failed to fetch auctions");
  }

  return res.json();
};

export default async function Page() {
  const auctions = await fetchAuctions();

  return (
    <div>{JSON.stringify(auctions)}</div>
  );
}
