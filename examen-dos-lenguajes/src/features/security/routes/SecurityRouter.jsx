import { Navigate, Route, Routes } from "react-router-dom";
import { Footer, Nav } from "../../sitema/components"
import { LoginPage } from "../pages";

export const SecurityRouter = () => {
  return (
    <div className="bg-gray-100 min-h-screen flex flex-col w-full">
      <Nav />
      <div className="px-6 py-8">
        <div className="container flex justify-between mx-auto">
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/*" element={<Navigate to={"/security/login"} />} />
          </Routes>
        </div>
      </div>
      <Footer />
    </div>
  );
};
