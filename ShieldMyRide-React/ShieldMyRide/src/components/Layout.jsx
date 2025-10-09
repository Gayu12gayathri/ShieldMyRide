import React from "react";
import Dashboard from "./Dashboard";
import { Outlet, useNavigate } from "react-router-dom";

export default function Layout({ onLogout }) {
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const handleLogout = () => {
    if (onLogout) onLogout();
    navigate("/login");
  };

  const handleLogin = () => {
    navigate("/login");
  };

  return (
    <div className="container mt-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        {/* <Dashboard /> */}
        {token ? (
          <button onClick={handleLogout} className="btn btn-outline-danger">
            🚪 Logout
          </button>
        ) : (
          <button onClick={handleLogin} className="btn btn-outline-success">
            🔑 Login
          </button>
        )}
      </div>
      <Outlet />
    </div>
  );
}