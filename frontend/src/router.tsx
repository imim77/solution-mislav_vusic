import App from "./App";
import { createBrowserRouter } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import FavoritesPage from "./pages/FavoritesPage";
import ProtectedRoute from "./components/ProtectedRoute";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />
  },
  {
    path: "/products/:id",
    element: <ProductDetailPage />
  },
  {
    path: "/favorites",
    element: (
      <ProtectedRoute>
        <FavoritesPage />
      </ProtectedRoute>
    )
  },
  {
    path: "/login",
    element: <LoginPage/>
  }
]);

export default router;
