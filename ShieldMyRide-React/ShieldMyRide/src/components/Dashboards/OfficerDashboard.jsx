import React, { useState } from "react";
import "./UserDashboard.css"; 
import UserDetails from "../UserDetails";
import OfficerProposals from "../../components/OfficerDashboard/OfficerProposal";
import OfficerQuotes from "../OfficerDashboard/OfficerQuote";
import OfficerPayments from "../OfficerDashboard/OfficerPayments";
import OfficerPolicyDocument from "../OfficerDashboard/OfficerPolicyDocument";
import OfficerClaims from "../OfficerDashboard/OfficerClaims";
import SearchBar from "../SearchBar";

export default function OfficerDashboard() {
  const [isOpen, setIsOpen] = useState(true);
  const [activeTab, setActiveTab] = useState("Dashboard");

    const userId = localStorage.getItem("userId");
  const roles = JSON.parse(localStorage.getItem("roles"));
  const role = roles?.[0]; // take the first role if multiple
  const username = localStorage.getItem("username");

  console.log("Logged-in User ID:", userId);
  console.log("Role:", role);
   console.log("Username:", username);

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
          <h2 className={`menu-title ${!isOpen ? "hidden" : ""}`}>
            Officer Menu
          </h2>
          <button className="toggle-btn" onClick={() => setIsOpen(!isOpen)}>
            {isOpen ? "✖" : "☰"}
          </button>
        </div>

        {isOpen && (
          <ul className="menu">
            <li
              className={activeTab === "Dashboard" ? "active" : ""}
              onClick={() => setActiveTab("Dashboard")}
            >
              Dashboard
            </li>
            <li
              className={activeTab === "Proposals" ? "active" : ""}
              onClick={() => setActiveTab("Proposals")}
            >
              Proposals
            </li>
            <li
              className={activeTab === "PolicyDocument" ? "active" : ""}
              onClick={() => setActiveTab("PolicyDocument")}
            >
              Policy Document
            </li>
            <li
              className={activeTab === "Quotes" ? "active" : ""}
              onClick={() => setActiveTab("Quotes")}
            >
              Quotes
            </li>
            <li
              className={activeTab === "Payments" ? "active" : ""}
              onClick={() => setActiveTab("Payments")}
            >
              Payments
            </li>
            <li
              className={activeTab === "Claims" ? "active" : ""}
              onClick={() => setActiveTab("Claims")}
            >
              Claims
            </li>
            <li
              className={activeTab === "OfficerDetails" ? "active" : ""}
              onClick={() => setActiveTab("OfficerDetails")}
            >
              Officer Details
            </li>
          </ul>
        )}
      </div>

      {/* Main Content */}
      <div className="main-content">
        <h1 className="dashboard-title">Officer Dashboard</h1>
        <p className="welcome-text">
          Welcome, <span className="username">{username}</span>
        </p>

        {/* === Conditional Rendering === */}
        {activeTab === "Dashboard" && (
          <>
            <div className="cards-container">
              <div className="card blue">
                <h3>Proposals Reviewed</h3>
                <p>25</p>
              </div>
              <div className="card green">
                <h3>Quotes Generated</h3>
                <p>12</p>
              </div>
              <div className="card red">
                <h3>Claims Verified</h3>
                <p>5</p>
              </div>
              <div className="card purple">
                <h3>Pending Payments</h3>
                <p>₹35,000</p>
              </div>
            </div>
             <div className="search-bar-container">
        <SearchBar />
      </div>
            <div className="recent-section">
              <h2>Recent Proposals</h2>
              <div className="recent-box">
                <OfficerProposals userId={userId} />
              </div>
            </div>
          </>
        )}

        {activeTab === "Proposals" && (
          <div className="tab-content">
            <h2 className="section-title">Proposals for Review</h2>
            <OfficerProposals userId={userId} />
          </div>
        )}
          {activeTab === "PolicyDocument" && (
          <div className="tab-content">
            <h2 className="section-title">Policy Document</h2>
            {/* <OfficerProposals userId={userId} /> */}
            <OfficerPolicyDocument userId = {userId}/>
            {/* <OfficerPolicyDocument proposalId={selectedProposalId} /> */}

          </div>
        )}
        {activeTab === "Quotes" && (
          <div className="tab-content">
            <h2 className="section-title">Quote Generation</h2>
            <OfficerQuotes userId={userId} />
          </div>
        )}

        {activeTab === "Payments" && (
          <div className="tab-content">
            <h2 className="section-title">Payment Verification</h2>
            <OfficerPayments userId={userId} />
          </div>
        )}

        {activeTab === "Claims" && (
          <div className="tab-content">
            <h2 className="section-title">Claims Verification</h2>
            <OfficerClaims userId={userId} />
          </div>
        )}

        {activeTab === "OfficerDetails" && (
          <div className="tab-content">
            <h2 className="section-title">Officer Profile</h2>
            <UserDetails userId={userId} role={role} />
          </div>
        )}
      </div>
      {/* <div className="right-panel">
        <SearchBar />
      </div> */}
    </div>
  );
}


