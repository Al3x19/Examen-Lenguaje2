import React from "react";

export const AuditLogs = () => {
  return (
    <div className="bg-gray-100 font-sans flex flex-col items-center content-center w-full mt-5">
      <div className="container mx-auto p-4">
        <div className="flex justify-between items-center mb-4">
          <div className="flex items-center">
            <i className="fas fa-calendar-alt text-teal-600 mr-2"></i>
            <span className="text-teal-600">Today: 2020-09-21</span>
          </div>
          <div className="flex items-center">
            <i className="fas fa-file-csv text-teal-600 mr-4"></i>
            <i className="fas fa-sync-alt text-teal-600 mr-4"></i>
            <div className="relative">
              <input
                type="text"
                placeholder="Search"
                className="border border-gray-300 rounded pl-8 pr-4 py-2"
              />
              <i className="fas fa-search absolute left-2 top-2 text-gray-400"></i>
            </div>
          </div>
        </div>
        <div className="bg-white shadow-md rounded">
          <div className="bg-gray-700 text-white p-4 rounded-t">
            <h2 className="text-lg">Audit Logs</h2>
          </div>
          <table className="min-w-full bg-white">
            <thead>
              <tr className="w-full bg-gray-200 text-gray-600 uppercase text-sm leading-normal">
                <th className="py-3 px-6 text-left">Date/Time</th>
                <th className="py-3 px-6 text-left">User</th>
                <th className="py-3 px-6 text-left">IP Address</th>
                <th className="py-3 px-6 text-left">Source</th>
                <th className="py-3 px-6 text-left">Event</th>
              </tr>
            </thead>
            <tbody className="text-gray-600 text-sm font-light">
              <tr className="border-b border-gray-200 hover:bg-gray-100">
                <td className="py-3 px-6 text-left">2020-09-21 09:26:33</td>
                <td className="py-3 px-6 text-left">ptp600</td>
                <td className="py-3 px-6 text-left">99.247.57.72</td>
                <td className="py-3 px-6 text-left">WGC</td>
                <td className="py-3 px-6 text-left">Login</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};
