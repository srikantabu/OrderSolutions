import { useEffect, useState } from "react";
import type { Order, SortBy, SortOrder } from "../types/Order";
import { getOrders, updateStatus } from "../services/ordersApi";
import SearchBar from "./SearchBar";
import Filters from "./Filters";
import OrdersTable from "./OrdersTable";
import Pagination from "./Pagination";
import Header from "./Header";

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

  useEffect(() => {
    const timer = setTimeout(() => setDebSearch(search), 300);
    return () => clearTimeout(timer);
  }, [search]);

  useEffect(() => {
    const fetchData = async () => {
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
    };

    fetchData();
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

  return (
    <div className="max-w-4xl mx-auto py-10">
      <Header />
      <SearchBar search={search} setSearch={setSearch} />
      <Filters status={status} setStatus={setStatus} />

      {loading && <p className="text-center">Loading...</p>}
      {error && <p className="text-center text-red-500">{error}</p>}

      <OrdersTable
        orders={orders}
        toggleSort={toggleSort}
        onUpdateStatus={handleStatusUpdate}
        sortBy={sortBy}
        order={order}
      />
      <Pagination page={page} setPage={setPage} totalPages={totalPages} />
    </div>
  );
};

export default OrdersDashboard;
