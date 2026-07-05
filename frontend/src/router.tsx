import App from "./App";
import { createBrowserRouter } from "react-router-dom";
import LoginInPage from "./pages/LoginInPage";
import ProductDetailPage from "./pages/ProductDetailPage";

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
    path: "/login",
    element: <LoginInPage/>
  }
]);

export default router;