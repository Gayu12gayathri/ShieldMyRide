// src/components/RequireAuth.jsx
import { Navigate } from "react-router-dom";

export default function RequireAuth({ children, allowedRoles }) {
  const token = localStorage.getItem("token");
  const roles = JSON.parse(localStorage.getItem("roles") || "[]");

  if (!token) {
    // No token = redirect to login
    return <Navigate to="/login" replace />;
  }

  if (allowedRoles && !roles.some((r) => allowedRoles.includes(r))) {
    // Token exists but role not allowed
    return <Navigate to="/" replace />;
  }

  return children;
}