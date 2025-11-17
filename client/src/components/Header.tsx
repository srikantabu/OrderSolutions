const Header = () => {
  return (
    <div className="w-full bg-gray-900 text-white py-4 shadow-md mb-4">
      <div className="max-w-5xl mx-auto px-4">
        <h1 className="text-2xl font-semibold tracking-wide">
          Orders Dashboard
        </h1>
        <p className="text-sm text-gray-300 mt-1">
          Manage orders, update statuses, and explore analytics.
        </p>
      </div>
    </div>
  );
};

export default Header;
