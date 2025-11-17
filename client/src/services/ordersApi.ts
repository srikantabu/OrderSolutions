import axios from "axios";
import type { OrdersQueryParams, OrdersResponse } from "../types/Order";

// -----------------------------------------------------------------------------
// File: orderApi.ts
// Project: OrderSolutions - React Client
// Description: Axios API layer for fetching orders and updating order status from backend.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

const api = axios.create({
  baseURL: "http://localhost:5179/v1",
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
