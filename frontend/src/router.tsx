import App from "./App";
import { createBrowserRouter } from "react-router-dom";
import LoginInPage from "./pages/LoginInPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import FavoritesPage from "./pages/FavoritesPage";
import ProtectedRoute from "./components/ProtectedRoute";

const router = createBrowserRouter([
  { 
    path: "/", 
    element: <App /> 
  },
  {
    path: "/proizvodi/:id",
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
    element: <LoginInPage/>
  }
]);

export default router;