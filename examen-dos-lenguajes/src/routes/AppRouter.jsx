import { Routes, Route } from "react-router-dom";
import { SistemaRouters } from "../features/sitema/routes";
import { SecurityRouter } from "../features/security/routes"

export const AppRouter = () => {
  return (
    <Routes>
      <Route path="/security/*" element={<SecurityRouter />} />
      <Route path="/*" element={<SistemaRouters />} />
    </Routes>
  );
};
