import type { Order, SortBy, SortOrder } from "../types/Order";

// -----------------------------------------------------------------------------
// File: OrdersTable.tsx
// Project: OrderSolutions - React Client
// Description: Sortable and interactive table displaying order rows and actions.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  orders: Order[];
  toggleSort: (column: SortBy) => void;
  onUpdateStatus: (id: number, status: string) => void;
  sortBy: SortBy;
  order: SortOrder;
}

const OrdersTable = ({
  orders,
  toggleSort,
  onUpdateStatus,
  sortBy,
  order,
}: Props) => {
  return (
    <table className="w-full border rounded shadow bg-white">
      <thead>
        <tr className="bg-gray-100">
          <th className="p-2 border">Order ID</th>
          <th className="p-2 border">Customer</th>
          <th
            className="p-2 border cursor-pointer"
            onClick={() => toggleSort("amount")}
          >
            <span className="flex items-center gap-1">
              Amount
              {sortBy === "amount" && (
                <span className="text-xs">{order === "asc" ? "▲" : "▼"}</span>
              )}
            </span>
          </th>
          <th className="p-2 border">Status</th>
          <th
            className="p-2 border cursor-pointer"
            onClick={() => toggleSort("createddate")}
          >
            <span className="flex items-center gap-1">
              Date
              {sortBy === "createddate" && (
                <span className="text-xs">{order === "asc" ? "▲" : "▼"}</span>
              )}
            </span>
          </th>
          <th className="p-2 border">Actions</th>
        </tr>
      </thead>

      <tbody>
        {orders.length === 0 ? (
          <tr>
            <td colSpan={6} className="p-6 text-center text-gray-500 text-sm">
              No orders found. Try changing your search or filters.
            </td>
          </tr>
        ) : (
          orders.map((o) => (
            <tr key={o.id}>
              <td className="p-2 border">{o.id}</td>
              <td className="p-2 border">{o.customerName}</td>
              <td className="p-2 border">{o.amount.toFixed(2)}</td>
              <td className="p-2 border">{o.status}</td>
              <td className="p-2 border">
                {new Date(o.createdDate).toLocaleDateString()}
              </td>
              <td className="p-2 border">
                <select
                  className="border p-1 rounded"
                  value={o.status}
                  onChange={(e) => onUpdateStatus(o.id, e.target.value)}
                >
                  <option>Pending</option>
                  <option>Completed</option>
                  <option>Cancelled</option>
                </select>
              </td>
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
};

export default OrdersTable;
