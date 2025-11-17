import { Link, useLocation } from "react-router-dom";

// -----------------------------------------------------------------------------
// File: AppHeader.tsx
// Project: OrderSolutions - React Client
// Description: Top navigation header component shared across pages.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

const AppHeader = () => {
  const location = useLocation();
  const isDashboard = location.pathname === "/dashboard";

  const title = isDashboard ? "Orders Dashboard" : "Order Management";
  const subtitle = isDashboard
    ? "Review, filter and update order statuses."
    : "Manage orders, update statuses, and explore analytics.";
  const linkText = isDashboard ? "Home" : "Go to Dashboard";
  const linkTo = isDashboard ? "/" : "/dashboard";

  return (
    <div className="w-full bg-gray-900 text-white py-4 shadow-md">
      <div className="max-w-5xl mx-auto px-4 flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-semibold tracking-wide">{title}</h1>
          <p className="text-sm text-gray-300 mt-1">{subtitle}</p>
        </div>

        <nav>
          <Link
            to={linkTo}
            className="text-sm font-medium text-gray-300 hover:text-white transition"
          >
            {linkText}
          </Link>
        </nav>
      </div>
    </div>
  );
};

export default AppHeader;
