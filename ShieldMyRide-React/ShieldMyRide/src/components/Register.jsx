import { useState } from "react"; 
import axios from "axios";
import { useNavigate, Link } from "react-router-dom";

export default function Register() {
  const [formData, setFormData] = useState({
    username: "",
    firstName: "",
    lastName: "",
    email: "",
    dateOfBirth: "",
    phoneNumber: "",
    aadhaarMasked: "",
    panMasked: "",
    password: "",
    confirmPassword: "",
    role: "",
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
    setErrors({ ...errors, [e.target.name]: "" });
    setErrors(prev => ({ ...prev, api: "" }));
  };

  const validate = () => {
    const newErrors = {};

    if (!formData.username.trim()) newErrors.username = "Username is required.";
    if (!formData.firstName.trim()) newErrors.firstName = "First Name is required.";
    if (!formData.lastName.trim()) newErrors.lastName = "Last Name is required.";

    if (!formData.email.trim()) {
      newErrors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Invalid email format.";
    }

    if (!formData.password.trim()) newErrors.password = "Password is required.";
    if (!formData.confirmPassword.trim()) newErrors.confirmPassword = "Please confirm your password.";
    if (formData.password && formData.confirmPassword && formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match.";
    }

    if (!formData.dateOfBirth) newErrors.dateOfBirth = "Date of Birth is required.";
    if (!formData.role) newErrors.role = "Role selection is required.";

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
      await axios.post("https://localhost:7153/api/Authentication/register", formData);
      alert("Registration successful! Please login.");
      navigate("/login");
    } catch (err) {
      let message = "Registration failed. Please try again.";
      if (err.response?.data?.message) {
        message = err.response.data.message;
      }
      setErrors(prev => ({ ...prev, api: message }));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-page">
      <div className="register-card">
        <h1 className="register-title">Create Account</h1>
        <p className="register-subtitle">Fill in the details below to register</p>

        <form onSubmit={handleSubmit} className="register-form">
          <div className="form-row">
            <div className="form-field">
              <label className="form-label">Username</label>
              <input
                name="username"
                placeholder="Username"
                value={formData.username}
                onChange={handleChange}
                className={`input ${errors.username ? "input-error" : ""}`}
              />
              {errors.username && <p className="error-text">{errors.username}</p>}
            </div>

            <div className="form-field">
              <label className="form-label">First Name</label>
              <input
                name="firstName"
                placeholder="First Name"
                value={formData.firstName}
                onChange={handleChange}
                className={`input ${errors.firstName ? "input-error" : ""}`}
              />
              {errors.firstName && <p className="error-text">{errors.firstName}</p>}
            </div>

            <div className="form-field">
              <label className="form-label">Last Name</label>
              <input
                name="lastName"
                placeholder="Last Name"
                value={formData.lastName}
                onChange={handleChange}
                className={`input ${errors.lastName ? "input-error" : ""}`}
              />
              {errors.lastName && <p className="error-text">{errors.lastName}</p>}
            </div>
          </div>

          <div className="form-row">
            <div className="form-field">
              <label className="form-label">Email</label>
              <input
                type="email"
                name="email"
                placeholder="Email"
                value={formData.email}
                onChange={handleChange}
                className={`input ${errors.email ? "input-error" : ""}`}
              />
              {errors.email && <p className="error-text">{errors.email}</p>}
            </div>

            <div className="form-field">
              <label className="form-label">DOB</label>
              <input
                type="date"
                name="dateOfBirth"
                value={formData.dateOfBirth}
                onChange={handleChange}
                className={`input ${errors.dateOfBirth ? "input-error" : ""}`}
              />
              {errors.dateOfBirth && <p className="error-text">{errors.dateOfBirth}</p>}
            </div>

            <div className="form-field">
              <label className="form-label">Phone No</label>
              <input
                name="phoneNumber"
                placeholder="Phone Number"
                value={formData.phoneNumber}
                onChange={handleChange}
                className="input"
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-field">
              <label className="form-label">Aadhaar</label>
              <input
                name="aadhaarMasked"
                placeholder="Aadhaar Number"
                value={formData.aadhaarMasked}
                onChange={handleChange}
                className="input"
              />
            </div>
            <div className="form-field">
              <label className="form-label">PAN</label>
              <input
                name="panMasked"
                placeholder="PAN Number"
                value={formData.panMasked}
                onChange={handleChange}
                className="input"
              />
            </div>
            <div className="form-field">
              <label className="form-label">Password</label>
              <input
                type="password"
                name="password"
                placeholder="Password"
                value={formData.password}
                onChange={handleChange}
                className={`input ${errors.password ? "input-error" : ""}`}
              />
              {errors.password && <p className="error-text">{errors.password}</p>}
            </div>
          </div>

          <div className="form-row">
            <div className="form-field">
              <label className="form-label">Confirm Password</label>
              <input
                type="password"
                name="confirmPassword"
                placeholder="Confirm Password"
                value={formData.confirmPassword}
                onChange={handleChange}
                className={`input ${errors.confirmPassword ? "input-error" : ""}`}
              />
              {errors.confirmPassword && <p className="error-text">{errors.confirmPassword}</p>}
            </div>

            <div className="form-field">
              <label className="form-label">Role</label>
              <select
                name="role"
                value={formData.role}
                onChange={handleChange}
                className={`input ${errors.role ? "input-error" : ""}`}
              >
                <option value="">Select Role</option>
                <option value="user">User</option>
                <option value="officer">Officer</option>
                <option value="admin">Admin</option>
              </select>
              {errors.role && <p className="error-text">{errors.role}</p>}
            </div>
          </div>

          {errors.api && <p className="error-text">{errors.api}</p>}

          <button type="submit" className="register-button" disabled={loading}>
            {loading ? "Registering..." : "Register"}
          </button>
        </form>

        <p className="register-footer">
          Already have an account? <Link to="/login" className="register-link">Login</Link>
        </p>
      </div>
    </div>
  );
}
