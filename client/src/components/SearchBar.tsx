// -----------------------------------------------------------------------------
// File: SearchBar.tsx
// Project: OrderSolutions - React Client
// Description: Input component for debounced text search.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

interface Props {
  search: string;
  setSearch: (val: string) => void;
}

const SearchBar = ({ search, setSearch }: Props) => {
  return (
    <div>
      <input
        type="text"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search by Order ID / Customer Name"
        className="border p-2 rounded w-full"
      />
    </div>
  );
};

export default SearchBar;
