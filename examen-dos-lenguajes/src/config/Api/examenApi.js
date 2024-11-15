import axios from "axios";
import { useAuthStore } from "../../features/security/store";

const API_URL = "https://localhost:7191/api";
axios.defaults.baseURL = API_URL;

// funcion para frontend
const setAuthToken = () => {
  const auth = getAuth();
  if (auth) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${auth.token}`;
  } else {
    delete axios.defaults.headers.common["Authorization"];
  }
};

const getAuth = () => {
  const lsToken = localStorage.getItem("token");

  if (lsToken && lsRefreshToken) {
    return { token: lsToken};
  }

  return null;
};

setAuthToken();