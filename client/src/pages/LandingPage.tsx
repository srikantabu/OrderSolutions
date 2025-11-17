import AppHeader from "../components/AppHeader";

// -----------------------------------------------------------------------------
// File: LandingPage.tsx
// Project: OrderSolutions - React Client
// Description: Application landing screen with navigation entry point.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

const LandingPage = () => {
  return (
    <div className="min-h-screen bg-gray-100">
      <AppHeader />

      <main className="max-w-4xl mx-auto px-4 py-10">
        <h2 className="text-3xl font-bold text-gray-800 mb-4">Welcome</h2>

        <p className="text-gray-700 text-lg mb-4">
          This application lets you search, filter, sort and manage customer
          orders.
        </p>

        <p className="text-gray-600">
          Use the navigation in the header to open the Orders Dashboard.
        </p>
      </main>
    </div>
  );
};

export default LandingPage;
