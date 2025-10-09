import React, { useState, useEffect } from "react";
import "./UserDashboard.css"; 
import AdminUserDetails from "../AdminDashboard/AdminUserDetails";
import AdminClaim from "../AdminDashboard/AdminClaim";
import AdminProposals from "../AdminDashboard/AdminProposal";
import AdminPolicyDocument from "../AdminDashboard/AdminPolicyDocument";
import AdminQuotes from "../AdminDashboard/AdminQuote";
import AdminPayments from "../AdminDashboard/AdminPayments";
import { useDispatch, useSelector } from "react-redux";
import { fetchPayments } from "../../features/paymentSlice";
import { fetchProposals, fetchAllProposals } from "../../features/proposalSlice";
import { fetchAllQuotes } from "../../features/quoteSlice";
import { fetchClaims } from "../../features/claimSlice";
import OfficerAssignments from "../OfficerDashboard/OfficerAssignment";
import SearchBar from "../SearchBar";

export default function AdminDashboard() {
  const [isOpen, setIsOpen] = useState(true);
  const [activeTab, setActiveTab] = useState("Dashboard");
  const dispatch = useDispatch();

  // Redux state
  const { list: payments, loading: loadingPayments } = useSelector((state) => state.payments);
  const { list: proposals, loading: loadingProposals } = useSelector((state) => state.proposals);
  const { list: quotes, loading: loadingQuotes } = useSelector((state) => state.quotes);
  const { list: claims, loading: loadingClaims } = useSelector((state) => state.claims);

  // Fetch data on mount
  useEffect(() => {
    dispatch(fetchPayments());
    dispatch(fetchAllProposals());
    dispatch(fetchAllQuotes());
    dispatch(fetchClaims());
  }, [dispatch]);

  const userId = localStorage.getItem("userId");
  const roles = JSON.parse(localStorage.getItem("roles"));
  const role = roles?.[0]; 
  const username = localStorage.getItem("username");

  if (!userId || !role) {
    return (
      <div className="text-center p-6 text-red-500">
        ⚠️ Error: User not logged in. Please login first.
      </div>
    );
  }

  return (
    <div className="dashboard">
      {/* Sidebar */}
      <div className={`sidebar ${isOpen ? "open" : "collapsed"}`}>
        <div className="sidebar-header">
          <h2 className={`menu-title ${!isOpen ? "hidden" : ""}`}>Admin Menu</h2>
          <button className="toggle-btn" onClick={() => setIsOpen(!isOpen)}>
            {isOpen ? "✖" : "☰"}
          </button>
        </div>

        {isOpen && (
          <ul className="menu">
            {["Dashboard","Proposals","PolicyDocument","Quotes","Payments","Claims","OfficerAssignments","AdminDetails"].map(tab => (
              <li
                key={tab}
                className={activeTab === tab ? "active" : ""}
                onClick={() => setActiveTab(tab)}
              >
                {tab === "AdminDetails" ? "User Details" : tab === "OfficerAssignments" ? "Officer Assignments" : tab}
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Main Content */}
      <div className="main-content">
        <h1 className="dashboard-title">Admin Dashboard</h1>
        <p className="welcome-text">
          Welcome, <span className="username">{username}</span>
        </p>

        {/* Dashboard Cards */}
        {activeTab === "Dashboard" && (
          <>
            <div className="cards-container">
              <div className="card blue">
                <h3>Proposals Reviewed</h3>
                {loadingProposals ? <p>Loading...</p> : <p>{proposals?.length || 0}</p>}
              </div>

              <div className="card green">
                <h3>Quotes Generated</h3>
                {loadingQuotes ? <p>Loading...</p> : <p>{quotes?.length || 0}</p>}
              </div>

              <div className="card red">
                <h3>Claims Verified</h3>
                {loadingClaims ? <p>Loading...</p> : <p>{claims?.length || 0}</p>}
              </div>

              <div className="card purple">
                <h3>Pending Payments</h3>
                {loadingPayments ? <p>Loading...</p> : <p>{payments?.length || 0}</p>}
              </div>
            </div>
 <div className="search-bar-container">
        <SearchBar />
      </div>
            <div className="recent-section">
              <h2>Recent Proposals</h2>
              <div className="recent-box">
                <AdminProposals userId={userId} />
              </div>
            </div>
          </>
        )}

        {/* Conditional Tabs */}
        {activeTab === "Proposals" && (
          <div className="tab-content">
            <h2 className="section-title">Proposals for Review</h2>
            <AdminProposals userId={userId} />
          </div>
        )}

        {activeTab === "PolicyDocument" && (
          <div className="tab-content">
            <h2 className="section-title">Policy Document</h2>
            <AdminPolicyDocument userId={userId} />
          </div>
        )}

        {activeTab === "Quotes" && (
          <div className="tab-content">
            <h2 className="section-title">All Quotes Generated</h2>
            <AdminQuotes userId={userId} />
          </div>
        )}

        {activeTab === "Payments" && (
          <div className="tab-content">
            <h2 className="section-title">Payment Verification</h2>
            <AdminPayments />
          </div>
        )}

        {activeTab === "Claims" && (
          <div className="tab-content">
            <h2 className="section-title">Claims Verification</h2>
            <AdminClaim />
          </div>
        )}

        {activeTab === "AdminDetails" && (
          <div className="tab-content">
            <AdminUserDetails />
          </div>
        )}
        {activeTab === "OfficerAssignments" && (
        <div className="tab-content">
          <h2 className="section-title">Officer Assignments</h2>
          <OfficerAssignments />
        </div>
      )}

      </div>
      {/* <div className="right-panel">
        <SearchBar />
      </div> */}
    </div>
  );
}
