import { Route, Routes } from "react-router-dom";
import OrdersDashboard from "./pages/OrdersDashboard";
import LandingPage from "./pages/LandingPage";

function App() {
  return (
    <Routes>
      <Route path="/" element={<LandingPage />} />
      <Route path="/dashboard" element={<OrdersDashboard />} />
    </Routes>
  );
}

export default App;
