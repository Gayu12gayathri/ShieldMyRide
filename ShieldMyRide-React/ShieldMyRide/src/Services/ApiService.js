import axios from "axios";
import {toast} from "react-toastify"

const api = axios.create({
    baseURL: "https://localhost:7153/api"
});

//Attach access token to request
api.interceptors.request.use((config)=>{
    const token = localStorage.getItem("token");
    if(token){
        config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
});

//handle expired token
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        if(error.response?.status ===401){
            const refreshToken = localStorage.getItem("refreshToken");
            const accessToken = localStorage.getItem("token");

            try {
                const res = await axios.post(
                    "https://localhost:7153/api/Authentication/refresh",
                    {
                        accessToken,
                        refreshToken,
                    }
                );
                console.log("✅ Token refreshed successfully");
                toast.success("🔄 Token refreshed automatically!");
                // Save new tokens
                localStorage.setItem("token", res.data.token);
                localStorage.setItem("refreshToken", res.data.refreshToken);

                // Retry original request
                error.config.headers["Authorization"] = `Bearer ${res.data.token}`;
                return api.request(error.config);
            } catch (refreshError){
                // Refresh failed → logout
                console.log("❌ Refresh token expired/invalid, logging out");
                toast.error("⚠️ Session expired, please login again");
                localStorage.clear();
                window.location.href = "/login";
            }
        }
        return Promise.reject(error);
    }
);

export default ApiService;