import React from "react";

interface Props {
  search: string;
  setSearch: (val: string) => void;
}
const SearchBar = ({ search, setSearch }: Props) => {
  return (
    <div className="mb-4">
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
