import { AiOutlineCar } from "react-icons/ai";

export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 flex justify-between items-center p-5 text-gray-800 shadow-md bg-white">
      <div className="flex items-center gap-2 text-2xl font-semibold text-red-500">
        <AiOutlineCar size={34} />
        <span>Castellan</span>
      </div>
      <div>Center</div>
      <div>Right</div>
    </header>
  )
}