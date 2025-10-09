import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { login } from "../features/auth/authSlice";

export default function Login() {
  const [credentials, setCredentials] = useState({ username: "", password: "" });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const handleChange = (e) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
    setErrors({ ...errors, [e.target.name]: "" }); // Clear field error on change
  };

  const validate = () => {
    const newErrors = {};
    if (!credentials.username.trim()) newErrors.username = "Username is required.";
    if (!credentials.password.trim()) newErrors.password = "Password is required.";
    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = validate();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    setLoading(true);
    try {
      const res = await axios.post(
        "https://localhost:7153/api/Authentication/login",
        credentials
      );

      const { token, refreshToken, username, roles, expiration, userId } = res.data;

      // Store in localStorage
      localStorage.setItem("token", token);
      localStorage.setItem("refreshToken", refreshToken);
      localStorage.setItem("username", username);
      localStorage.setItem("roles", JSON.stringify(roles));
      localStorage.setItem("expiration", expiration);
      localStorage.setItem("userId", userId);

      // Update Redux
      dispatch(login({ token, roles, username, userId }));

      // Redirect
      navigate("/home");
    } catch (err) {
      console.error(err);
      let message = "Login failed!";
      if (err.response) {
        if (err.response.status === 401) {
          message = "Invalid username or password.";
        } else if (err.response.data?.message) {
          message = err.response.data.message;
        }
      } else if (err.request) {
        message = "No response from server. Please try again later.";
      }
      alert(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-form">
          <h2 className="auth-title">Welcome Back!</h2>
          <p className="auth-subtitle">Please login to your account</p>

          <form onSubmit={handleSubmit} className="auth-form-fields">
            <label className="form-label">Username</label>
            <input
              name="username"
              placeholder="Username"
              className={`input ${errors.username ? "input-error" : ""}`}
              value={credentials.username}
              onChange={handleChange}
            />
            {errors.username && <p className="error-text">{errors.username}</p>}

            <label className="form-label">Password</label>
            <input
              type="password"
              name="password"
              placeholder="Password"
              className={`input ${errors.password ? "input-error" : ""}`}
              value={credentials.password}
              onChange={handleChange}
            />
            {errors.password && <p className="error-text">{errors.password}</p>}

            <div className="auth-row">
              <label className="checkbox">
                <input type="checkbox" /> Remember me
              </label>
              <button type="button" className="link-button">
                Forgot Password?
              </button>
            </div>

            <button type="submit" className="primary-button" disabled={loading}>
              {loading ? "Logging in..." : "Login"}
            </button>
          </form>

          <div className="auth-footer">
            Don’t have an account yet?{" "}
            <a href="/register" className="link">
              Register here
            </a>
          </div>
        </div>

        <div className="auth-visual"></div>
      </div>
    </div>
  );
}
