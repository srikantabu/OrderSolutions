// -----------------------------------------------------------------------------
// File: Order.ts
// Project: OrderSolutions - React Client
// Description: Type definitions for Order model, sorting types, query parameters, and responses.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

export interface Order {
  id: number;
  customerName: string;
  amount: number;
  status: "Pending" | "Completed" | "Cancelled";
  createdDate: string;
}

export type SortBy = "amount" | "createddate" | "";
export type SortOrder = "asc" | "desc";

export interface OrdersQueryParams {
  page: number;
  limit?: number;
  search?: string;
  status?: string;
  sortBy?: SortBy;
  order?: SortOrder;
}

export interface OrdersResponse {
  data: Order[];
  page: number;
  totalPages: number;
  totalRecords: number;
}
