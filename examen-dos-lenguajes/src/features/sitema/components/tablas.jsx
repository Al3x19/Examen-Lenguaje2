import React from 'react';

export const TablaContable = () => {
  const rows = Array.from({ length: 10 }, (_, index) => ({
    isGray: index % 2 === 1,
  }));

  return (
    <div className="overflow-x-auto w-full">
      <table className="min-w-full border-collapse mt-4 border border-gray-400">
        <thead>
          <tr className="bg-blue-600 text-white">
            <th className="border border-gray-400 px-4 py-2">Código</th>
            <th className="border border-gray-400 px-4 py-2">Nombre de Cuenta</th>
            <th className="border border-gray-400 px-4 py-2">Descripción</th>
            <th className="border border-gray-400 px-4 py-2">Debe</th>
            <th className="border border-gray-400 px-4 py-2">Haber</th>
            <th className="border border-gray-400 px-4 py-2">Referencia</th>
          </tr>
        </thead>
        <tbody>
          {rows.map((row, index) => (
            <tr key={index} className={row.isGray ? 'bg-gray-100' : 'bg-white'}>
              <td className="border border-gray-400 px-4 py-2"></td>
              <td className="border border-gray-400 px-4 py-2"></td>
              <td className="border border-gray-400 px-4 py-2"></td>
              <td className="border border-gray-400 px-4 py-2"></td>
              <td className="border border-gray-400 px-4 py-2"></td>
              <td className="border border-gray-400 px-4 py-2"></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
