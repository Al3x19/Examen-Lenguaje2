import { Navigate, Route, Routes } from "react-router-dom";
import { HomePage, PartidasPage, AuditLogs, CuentasPage } from "../pages";
import { Nav } from "../components/nav";
import { Footer } from "../components";

export const SistemaRouters = () => {
  return (
    <div className="bg-gray-100 min-h-screen flex flex-col w-full">
      <Nav/>
      <div className="px-6 py-8">
        <div className="container flex justify-between mx-auto">
          <Routes>
            <Route path="/home" element={<HomePage />} />
            <Route path="/partidas" element={<PartidasPage />} />
            <Route path="/logs" element={<AuditLogs />} />
            <Route path="/cuentas" element={<CuentasPage />}/>
            <Route path='/*' element={<Navigate to={"/security/login"} />} />
          </Routes>
        </div>
      </div>
      <Footer/>
    </div>
  );
};
