import { Auction } from "@/models/Auction";

import Image from "next/image";

type Props = {
  auction: Auction
};

export default function AuctionCard({ auction }: Props) {
  return (
    <a>
      <div className="w-full bg-gray-200 aspect-video rounded-lg overflow-hidden">
        <Image 
          src={auction.description} 
          alt="Image of a car" 
          fill 
          objectFit="cover" 
          priority
          sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 25vw"
        />
      </div>
      <div className="flex justify-between items-center mt-4">
        <h3 className="text-gray-700">{auction.make}</h3>
      </div>
    </a>
  )
}
