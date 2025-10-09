import { useState, useEffect } from "react";
import "./App.css";
import NavBar from "./components/NavBar";
import { BrowserRouter, Route, Routes, Navigate, useNavigate } from "react-router-dom";
import Home from "./components/HomePage";
import Login from "./components/Login";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Register from "./components/Register";
import AdminDashboard from "./components/Dashboards/AdminDashboard";
import OfficerDashboard from "./components/Dashboards/OfficerDashboard";
import UserDashboard from "./components/Dashboards/UserDashboard";
import QuotePage from "./components/QuotePage";
import CarInsurance from "./components/motorinsurance/CarInsurance";
import TwoWheeler from "./components/motorinsurance/TwoWheeler";
import ThirdParty from "./components/motorinsurance/ThirdParty";
import Truck from "./components/motorinsurance/Truck";
import Proposal from "./components/Proposal";
import ScrollToTop from "./ScrollTop";
import UserProposals from "./components/UserDashboard/UserProposal";
import UserPayments from "./components/UserDashboard/UserPayments";
import OfficerPolicyDocument from "./components/OfficerDashboard/OfficerPolicyDocument";
import OfficerProposals from "./components/OfficerDashboard/OfficerProposal";
import Renewal from "./components/Renewal";
import Claim from "./components/Claim";
import UserClaims from "./components/UserDashboard/UserClaims";
// import Navbar from "./components/NavBar";
import Footer from "./components/Footer";
import InsurancePolicy from "./components/motorinsurance/InsurancePolicy"
// import Dashboard from "./components/Dashboards/Dashboard";
import Queries from "./components/Queries"
import PolicyDownloadLink from "./components/PolicyDownload";


function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [roles, setRoles] = useState([]);
  useEffect(() => {
    const storedRoles = localStorage.getItem("roles");
    if (storedRoles) {
      setRoles(JSON.parse(storedRoles));
    }
  }, [token]);

  const handleLogout = () => {
    localStorage.clear();
    setToken(null);
    setRoles([]);
  };

  // const navigate = useNavigate();

  return (
    <BrowserRouter>
      <ScrollToTop />
      <NavBar />
      {/* <Navbar isAuthenticated={!!token} onLogoutClick={handleLogout} /> */}
      <Routes>
        <Route
          path="/"
          element={<Home/>}
          // element={token ?<Home /> : <Navigate to="/login" />}
        />
        <Route
          path="/home"
          element={<Home />}
        />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        {/* <Route path="/dashboard" element={<Dashboard />} /> */}
         <Route path="/motor-insurance/car" element={<CarInsurance />} />
        <Route path="/motor-insurance/bike" element={<TwoWheeler />} />
        <Route path="/motor-insurance/third-party" element={<ThirdParty />} />
        <Route path="/motor-insurance/truck" element={<Truck />} />
        <Route path="/proposal" element={<Proposal />} />
        <Route path="/renewals" element={<Renewal/>}/>
        <Route path="/claims" element={<Claim/>}/>
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/officer-dashboard" element={<OfficerDashboard />} />
        <Route path="/user-dashboard" element={<UserDashboard />} />
        <Route path="/quote" element={<QuotePage />} />
        <Route path="/insurancepolicies" element={<InsurancePolicy />} />
        <Route path="/user-claim" element={<UserClaims />} />
        <Route path="/user/proposals" element={<UserProposals />} />
        <Route path="/user/payments" element={<UserPayments />} />
        <Route path="/officer/proposals" element={<OfficerProposals/>}/>
        <Route path="/officer/policydocument/:proposalId" element={<OfficerPolicyDocument />} />
        <Route path="/queries" element={<Queries/>}/>
        <Route path="/download-policy" element={<PolicyDownloadLink />} />
        {/* <Route path="/crud" element={<ProposalCRUDTest/>}/> */}
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
      <Footer />
      <ToastContainer position="top-right" autoClose={3000} />
    </BrowserRouter>
  );
}

export default App;
