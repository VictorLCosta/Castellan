export type Auction = {
  id: string;
  title: string;
  description: string;
  make: string;
  startPrice: number;
  currentPrice: number;
  startDate: Date;
  endDate: Date;
  status: 'active' | 'ended' | 'cancelled';
  ownerId: string;
  highestBidderId?: string;
};