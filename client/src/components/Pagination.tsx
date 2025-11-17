// -----------------------------------------------------------------------------
// File: Pagination.tsx
// Project: OrderSolutions - React Client
// Description: Pagination controls for navigating between pages of orders.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  page: number;
  setPage: (p: number) => void;
  totalPages: number;
}
const Pagination = ({ page, setPage, totalPages }: Props) => {
  return (
    <div className="flex justify-center mt-4 gap-4">
      <button
        onClick={() => page > 1 && setPage(page - 1)}
        disabled={page === 1}
        className="px-4 py-2 border rounded disabled:opacity-50"
      >
        Previous
      </button>

      <span className="self-center">
        Page {page} of {totalPages}
      </span>

      <button
        onClick={() => page < totalPages && setPage(page + 1)}
        disabled={page === totalPages}
        className="px-4 py-2 border rounded disabled:opacity-50"
      >
        Next
      </button>
    </div>
  );
};

export default Pagination;
