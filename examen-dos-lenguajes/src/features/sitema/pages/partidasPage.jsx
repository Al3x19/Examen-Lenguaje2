// PartidasPage.js
import React from "react";
import { TablaContable } from "../components/tablas";

export const PartidasPage = () => {
  return (
      <div className="flex flex-col items-center content-center w-full mt-5">
        <div className="bg-white w-full border-2 border-solid border-red-500 p-4">
          <div className="w-full flex justify-end space-x-2 mb-4">
            <button
              type="submit"
              className="w-20 h-10 bg-gray-400 text-white py-2 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <a href="/cuentas">Cuentas</a>
            </button>
            <button
              type="submit"
              className="w-40 h-10 bg-gray-400 text-white py-2 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <a href="#">Agregar Cuentas</a>
            </button>
            <button
              type="submit"
              className="w-20 h-10 bg-gray-400 text-white py-2 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              guardar
            </button>
            <button
              type="submit"
              className="w-20 h-10 bg-gray-400 text-white py-2 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              editar
            </button>
            <button
              type="submit"
              className="w-20 h-10 bg-gray-400 text-white py-2 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              Inhabilitar
            </button>
          </div>
          <div className="w-full">
            <h1 className="text-black font-bold border-b-2 border-black pb-2">
              Partidas
            </h1>
          </div>
          <div className="flex items-center justify-center mt-2 bg-gray-100">
            <form
              action=""
              className="flex flex-wrap md:flex-row md:items-center space-y-4 md:space-y-0 md:space-x-4 w-3/4 border-2 border-solid border-blue-500 p-4"
            >
              <div className="flex flex-wrap items-center">
                <p>#Numero de partida </p>
                <input
                  className="w-40 px-2 py-1 mx-2 border border-gray-300 rounded-md text-center"
                  type="text"
                  placeholder="#Numero de partida"
                />
              </div>
              <div className="flex flex-wrap items-center">
                <p>Usuario </p>
                <input
                  className="w-40 px-2 mx-2 py-1 border border-gray-300 rounded-md text-center"
                  type="text"
                  placeholder="Usuario"
                />
              </div>
              <div className="flex flex-wrap items-center">
                <p>Fecha </p>
                <input
                  className="w-40 px-2 py-1 mx-2 border border-gray-300 rounded-md text-center"
                  type="date"
                  placeholder="Fecha"
                />
              </div>
              <div className="flex flex-wrap mt-10 items-center w-9/12">
                <p>Descripcion </p>
                <input
                  className="w-9/12 px-2 py-1 mx-2 mt-2 border border-gray-300 text-center"
                  type="text"
                  placeholder="Descripcion"
                />
              </div>
              <TablaContable />
            </form>
          </div>
        </div>
      </div>
  );
};
