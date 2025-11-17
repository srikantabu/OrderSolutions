// -----------------------------------------------------------------------------
// File: Filters.tsx
// Project: OrderSolutions - React Client
// Description: Dropdown selector for filtering by order status.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  status: string;
  setStatus: (s: string) => void;
}

const Filters = ({ status, setStatus }: Props) => {
  return (
    <div>
      <select
        className="border p-2 rounded w-full"
        value={status}
        onChange={(e) => setStatus(e.target.value)}
      >
        <option value="all">All</option>
        <option value="Pending">Pending</option>
        <option value="Completed">Completed</option>
        <option value="Cancelled">Cancelled</option>
      </select>
    </div>
  );
};

export default Filters;
