import { useCallback, useEffect, useState } from "react";
import type { Order, SortBy, SortOrder } from "../types/Order";
import { getOrders, updateStatus } from "../services/ordersApi";
import SearchBar from "../components/SearchBar";
import Filters from "../components/Filters";
import OrdersTable from "../components/OrdersTable";
import Pagination from "../components/Pagination";
import AppHeader from "../components/AppHeader";
import Loading from "../components/Loading";
import ErrorBox from "../components/ErrorBox";

// -----------------------------------------------------------------------------
// File: OrdersDashboard.tsx
// Project: OrderSolutions - React Client
// Description: Main dashboard page displaying search, filters, table, and pagination.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

const OrdersDashboard = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState("all");
  const [sortBy, setSortBy] = useState<SortBy>("");
  const [order, setOrder] = useState<SortOrder>("asc");
  const [debSearch, setDebSearch] = useState("");
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const fetchOrders = useCallback(async () => {
    setLoading(true);
    try {
      const start = Date.now();
      const data = await getOrders({
        page,
        search: debSearch,
        status,
        sortBy,
        order,
      });

      const MIN_LOADING_MS = 600;
      const elapsed = Date.now() - start;
      if (elapsed < MIN_LOADING_MS) {
        await new Promise((resolve) =>
          setTimeout(resolve, MIN_LOADING_MS - elapsed)
        );
      }

      setOrders(data.data);
      setTotalPages(data.totalPages);
      setError("");
    } catch {
      setError("Failed to load orders.");
    } finally {
      setLoading(false);
    }
  }, [page, debSearch, status, sortBy, order]);

  const toggleSort = (column: SortBy) => {
    if (sortBy === column) {
      setOrder(order === "asc" ? "desc" : "asc");
    } else {
      setSortBy(column);
      setOrder("asc");
    }
  };

  const handleStatusUpdate = async (id: number, newStatus: string) => {
    await updateStatus(id, newStatus);

    const data = await getOrders({ page, search, status, sortBy, order });
    setOrders(data.data);
  };

  useEffect(() => {
    const timer = setTimeout(() => setDebSearch(search), 300);
    return () => clearTimeout(timer);
  }, [search]);

  useEffect(() => {
    fetchOrders();
  }, [fetchOrders]);

  return (
    <div className="min-h-screen bg-gray-100">
      <AppHeader />

      <main className="relative max-w-4xl mx-auto px-4 py-10">
        <Loading loading={loading} />

        <div className="flex items-center gap-4 mb-6">
          <div className="w-4/5">
            <SearchBar search={search} setSearch={setSearch} />
          </div>

          <div className="w-1/5">
            <Filters status={status} setStatus={setStatus} />
          </div>
        </div>

        {error && <ErrorBox message={error} onRetry={fetchOrders} />}

        <OrdersTable
          orders={orders}
          toggleSort={toggleSort}
          onUpdateStatus={handleStatusUpdate}
          sortBy={sortBy}
          order={order}
        />
        <Pagination page={page} setPage={setPage} totalPages={totalPages} />
      </main>
    </div>
  );
};

export default OrdersDashboard;
