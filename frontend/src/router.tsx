import App from "./App";
import { createBrowserRouter } from "react-router-dom";
import LoginInPage from "./pages/LoginInPage";

const router = createBrowserRouter([
  { 
    path: "/", 
    element: <App /> 
  },
  {
    path: "/login",
    element: <LoginInPage/>
  }
]);

export default router;
