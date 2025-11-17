import axios from "axios";
import type { OrdersQueryParams, OrdersResponse } from "../types/Order";

const api = axios.create({
  baseURL: "http://localhost:5179",
});

export const getOrders = async (
  params: OrdersQueryParams
): Promise<OrdersResponse> => {
  const {
    page,
    limit = 10,
    search = "",
    status = "all",
    sortBy,
    order = "asc",
  } = params;

  const res = await api.get<OrdersResponse>("/orders", {
    params: {
      page,
      limit,
      search,
      status,
      sortBy,
      order,
    },
  });
  return res.data;
};

export const updateStatus = async (id: number, status: string) => {
  return api.post(`/orders/${id}/status`, { status: status });
};
