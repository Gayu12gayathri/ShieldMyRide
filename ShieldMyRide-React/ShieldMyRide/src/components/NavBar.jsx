import { Link, useNavigate } from "react-router-dom";
import logo from "../assets/logo.png";
import userIcon from "../assets/user.png";
import "../index.css";
import { useState, useRef, useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../features/auth/authSlice";

export default function NavBar() {
  // Separate dropdown states
  const [isInsuranceDropdownOpen, setInsuranceDropdownOpen] = useState(false);
  const [isUserDropdownOpen, setUserDropdownOpen] = useState(false);
  const [isSupportDropdownOpen, setSupportDropdownOpen] = useState(false);
  const [menuOpen, setMenuOpen] = useState(false);

  // Separate refs
  const insuranceRef = useRef(null);
  const userRef = useRef(null);
 const supportRef = useRef(null);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const { token, username, roles } = useSelector((state) => state.auth);
  const isLoggedIn = Boolean(token);

  // Toggle handlers
  const toggleInsuranceDropdown = () => {
    setInsuranceDropdownOpen(!isInsuranceDropdownOpen);
    setUserDropdownOpen(false); // close user dropdown if open
  };

  const toggleUserDropdown = () => {
    setUserDropdownOpen(!isUserDropdownOpen);
    setInsuranceDropdownOpen(false); // close insurance dropdown if open
  };


const toggleSupportDropdown = () => {
  setSupportDropdownOpen(prev => !prev);
  setInsuranceDropdownOpen(false);
  setUserDropdownOpen(false);
};


  // Click outside handler
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (
        (insuranceRef.current && !insuranceRef.current.contains(event.target)) &&
        (userRef.current && !userRef.current.contains(event.target)) &&
        (supportRef.current && !supportRef.current.contains(event.target))
      ) {
        setInsuranceDropdownOpen(false);
        setUserDropdownOpen(false);
        setSupportDropdownOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleLogout = () => {
    dispatch(logout());
    navigate("/home");
  };

  const handleViewProfile = () => {
    if (roles?.includes("Admin")) navigate("/admin-dashboard");
    else if (roles?.includes("Officer")) navigate("/officer-dashboard");
    else navigate("/user-dashboard");
  };

  return (
    <nav className="navbar-wrapper">
      <div className="navbar">
        <div className="navbar-content">
          {/* Brand */}
          <div className="brand">
            <img src={logo} alt="ShieldMyRide logo" className="brand-logo" />
            <span className="brand-name">ShieldMyRide</span>
          </div>

          {/* Hamburger for small screens */}
          <div className="hamburger" onClick={() => setMenuOpen(!menuOpen)}>
            ☰
          </div>

          {/* Links */}
          <div className={`nav-links ${menuOpen ? "open" : ""}`}>
            <Link to="/home">Home</Link>

            {/* Motor Insurance Dropdown */}
            <div
              className={`dropdown ${isInsuranceDropdownOpen ? "open" : ""}`}
              ref={insuranceRef}
            >
              <button className="dropdown-toggle" onClick={toggleInsuranceDropdown}>
                Motor Insurance
              </button>
              {isInsuranceDropdownOpen && (
                <div className="dropdown-menu">
                  <Link to="/motor-insurance/car">Car Insurance</Link>
                  <Link to="/motor-insurance/bike">Bike Insurance</Link>
                  <Link to="/motor-insurance/truck">Truck Insurance</Link>
                  <Link to="/motor-insurance/third-party">
                    Third-Party Insurance
                  </Link>
                </div>
              )}
            </div>

            <Link to="/renewals">Renewals</Link>
            <Link to="/claims">Claims</Link>
            <Link to="/proposal">Service</Link>

            {/* Support Dropdown */}
            <div
              className={`dropdown ${isSupportDropdownOpen ? "open" : ""}`}
              ref={supportRef}
            >
              <button className="dropdown-toggle" onClick={toggleSupportDropdown}>
                Support
              </button>

              {isSupportDropdownOpen && (
                <div className="support-dropdown">
                  <div className="support-section">
                    <h4>Our Customers</h4>
                    <p>📞 1800 102 1111</p>
                    <p>Customer Service: Available 24/7</p>
                    <p>Renewals: Mon–Sun (8:00AM – 8:00PM)</p>
                    <p>Purchase New Policy: Mon–Sat (9:30AM – 6:30PM)</p>
                  </div>

                  <div className="support-section">
                    <h4>Our Agents & Intermediaries</h4>
                    <p>📞 1800 22 1111 (Available 24/7)</p>
                    <p>Health Insurance Helpline: 1800 210 3366 / 1800 210 6366</p>
                  </div>

                  <div className="support-section">
                    <h4>Email Queries & Complaints</h4>
                    <p>✉️ Customer.care@sbigeneral.in</p>
                    <p>✉️ Grievance Redressal: gro@sbigeneral.in</p>
                  </div>
                </div>
              )}
            </div>

          </div>

          {/* User icon & dropdown */}
          <div className="nav-actions" ref={userRef}>
            {isLoggedIn ? (
              <div className="user-menu">
                <img
                  src={userIcon}
                  alt="User"
                  className="user-icon"
                  onClick={toggleUserDropdown}
                />
                {isUserDropdownOpen && (
                  <div className="user-dropdown">
                    <button onClick={handleViewProfile}>View Profile</button>
                    <button  onClick={() => navigate("/download-policy")} >DownloadPolicy</button>
                    <button onClick={handleLogout}>Logout</button>
                    
                  </div>
                )}
              </div>
            ) : (
              <Link to="/login" className="login-btn">
                Login
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
