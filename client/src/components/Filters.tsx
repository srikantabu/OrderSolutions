import React from "react";

interface Props {
  status: string;
  setStatus: (s: string) => void;
}
const Filters = ({ status, setStatus }: Props) => {
  return (
    <div className="mb-4">
      <select
        className="border p-2 rounded"
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
