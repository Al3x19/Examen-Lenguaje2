import React from "react";

export const CuentasPage = () => {
  return (
    <div className="bg-gray-100 min-h-screen flex flex-col w-full text-center">
      <div className="bg-gray-100 p-4 min-h-screen items-center">
        <div className="bg-gray-200 p-2 rounded-lg">
          <div className="font-bold">Activos</div>
          <ul className="ml-4">
            <li>
              <div className="font-bold">11 - ACTIVO CORRIENTE</div>
              <ul className="ml-4">
                <li>
                  <div className="font-bold">1101 - CAJAS</div>
                  <ul className="ml-4">
                    <li>110101 - CAJA GENERAL</li>
                  </ul>
                </li>
                <li>
                  <div className="font-bold">1102 - BANCOS</div>
                  <ul className="ml-4">
                    <li>
                      <div className="font-bold">110201 - BANCOS</div>
                      <ul className="ml-4">
                        <li>
                          11020102-00 - Banco de Occidente DLS $22100108706 $
                        </li>
                        <li>
                          11020101-00 - Banco Atlantida Cta. 11100022395 Lps.
                        </li>
                        <li>
                          11020102-00 - Banco del País Cta. 01070500124895 Lps.
                        </li>
                        <li>11020102-00 - Banco de Occidente 11100043227</li>
                        <li>11020102-00 - Bac Honduras 72928021</li>
                        <li>11020102-00 - Bac Honduras $72928631</li>
                        <li>11020102-00 - Banco Ficohsa 20000904814</li>
                        <li>11020102-00 - Banco del País DLS $220770004307</li>
                        <li>11020102-00 - Banco Ficohsa $200016611432</li>
                        <li>11020102-00 - BanRural 301930000002</li>
                        <li>11020159-00 - Depositos en Garantia</li>
                        <li>11020159-00 - Cheques Recibidos no Depositados</li>
                        <li>11020102-00 - Inactiva</li>
                        <li>11020102-00 - Inactiva</li>
                        <li>11020102-00 - Inactiva</li>
                        <li>11020102-00 - Inactiva</li>
                      </ul>
                    </li>
                  </ul>
                </li>
                <li>
                  <div className="font-bold">1103 - CUENTAS POR COBRAR</div>
                  <ul className="ml-4">
                    <li>
                      <div className="font-bold">
                        110301 - CUENTAS POR COBRAR
                      </div>
                      <ul className="ml-4">
                        <li>11030101-00 - Clientes</li>
                        <li>
                          11030102-00 - Amortización de Cuentas Incobrables
                        </li>
                        <li>11030103-00 - Empleados</li>
                        <li>11030104-00 - Socios</li>
                        <li>11030105-00 - Alquileres Pagados Por Anticipado</li>
                        <li>11030105-00 - Anticipos de Proveedores</li>
                      </ul>
                    </li>
                  </ul>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
};
