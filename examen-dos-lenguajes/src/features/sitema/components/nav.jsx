export const Nav = () => {
    
    return (
      <div>
        <header className="bg-gray-800 text-white py-4">
          <div className="container mx-auto text-center">
            <h1 className="text-2xl font-semibold">Examen Sistema de Partidas</h1>
          </div>
          <nav className="mt-4">
            <div className="flex justify-center space-x-4">
              <a
                href="/home"
                className="text-white px-4 py-2 hover:bg-gray-600 rounded"
              >
                Inicio
              </a>
              <a
                href="/login"
                className="text-white px-4 py-2 hover:bg-gray-600 rounded"
              >
                Login
              </a>
              <a
                href="/partidas"
                className="text-white px-4 py-2 hover:bg-gray-600 rounded"
              >
                Partidas
              </a>
              <a
                href="/logs"
                className="text-white px-4 py-2 hover:bg-gray-600 rounded"
              >
                Logs
              </a>
            </div>
          </nav>
        </header>
      </div>
    );
  };
  