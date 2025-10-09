import React, { useState,useEffect } from "react";
import "./UserDashboard.css";
import UserProposals from "../UserDashboard/UserProposal";
import { fetchProposals, removeProposal } from "../../features/proposalSlice";
import UserClaims from "../UserDashboard/UserClaims";
import UserQuotes from "../UserDashboard/UserQuotes";
import UserPayments from "../UserDashboard/UserPayments";
import {getUserStats} from "../../Services/UserStats";
import SearchBar from "../SearchBar";


export default function UserDashboard() {
  const [isOpen, setIsOpen] = useState(true);
  const [activeTab, setActiveTab] = useState("Dashboard");

  const userId = localStorage.getItem("userId");
  const roles = JSON.parse(localStorage.getItem("roles"));
  const role = roles?.[0];
  const username = localStorage.getItem("username") || "User";

  const [stats, setStats] = useState({
    proposalsSubmitted: 0,
    policiesActive: 0,
    totalClaimed: 0,
    balance: 0,
  });

  useEffect(() => {
    const fetchStats = async () => {
      const data = await getUserStats(userId);
      setStats(data);
    };

    fetchStats();
  }, [userId]);

  if (!userId || !role) {
    return (
      <div className="text-center p-6 text-red-500">
        ⚠️ Error: User not logged in. Please login first.
      </div>
    );
  }


  return (
    <div className="dashboard">
       {/* <SearchBar/> */}
      {/* Sidebar */}
      <div className={`sidebar ${isOpen ? "open" : "collapsed"}`}>
        <div className="sidebar-header">
          <h2 className={`menu-title ${!isOpen ? "hidden" : ""}`}>My Menu</h2>
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
              className={activeTab === "Claims" ? "active" : ""}
              onClick={() => setActiveTab("Claims")}
            >
              Claims
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
          </ul>
        )}
      </div>

      {/* Main Content */}
      <div className="main-content">
            
        <h1 className="dashboard-title">User Dashboard</h1>
        <p className="welcome-text">
          Welcome, <span className="username">{username}</span>
        </p>

        {/* === Conditional Rendering === */}
        {activeTab === "Dashboard" && (
          <>
            <div className="cards-container">
            <div className="card blue">
              <h3>Proposals Submitted</h3>
              <p>{stats.proposalsSubmitted}</p>
            </div>
            <div className="card green">
              <h3>Policies Active</h3>
              <p>{stats.policiesActive}</p>
            </div>
            <div className="card red">
              <h3>Total Claimed</h3>
              <p>₹{stats.totalClaimed.toLocaleString()}</p>
            </div>
            <div className="card purple">
              <h3>Balance</h3>
              <p>₹{stats.balance.toLocaleString()}</p>
            </div>
          </div>
      <div className="search-bar-container">
        <SearchBar />
      </div>
            <div className="recent-section">
              <h2>Recent Proposals</h2>
              <div className="recent-box"><UserProposals userId={userId} /></div>
            </div>
          </>
        )}

        {activeTab === "Proposals" && (
          <div className="tab-content">
            <h2 className="section-title">Your Proposals</h2>
            <UserProposals userId={userId} />
          </div>
        )}

        {activeTab === "Claims" && (
          <div className="tab-content">
            <h2 className="section-title">Claims Section</h2>
            <p>Claim details will be displayed here.</p>
            <UserClaims userId={userId} />

          </div>
        )}

        {activeTab === "Quotes" && (
          <div className="tab-content">
            <h2 className="section-title">Quotes Section</h2>
            <p>Your available quotes will be shown here.</p>
             <UserQuotes userId={userId} />
          </div>
        )}

        {activeTab === "Payments" && (
          <div className="tab-content">
            <h2 className="section-title">Payment Summary</h2>
            <p>Payment records and balances will be shown here.</p>
            <UserPayments userId={userId} />
          </div>
        )}
      </div>
      {/* <div className="right-panel">
        <SearchBar />
      </div> */}
    </div>
  );
}

// import React from "react";
// import UserDetails from "../UserDetails";
// import UserProposals from "../UserProposal";

// export default function UserDashboard() {
//   const userId = localStorage.getItem("userId");
//   const roles = JSON.parse(localStorage.getItem("roles"));
//   const role = roles?.[0];
//   const username = localStorage.getItem("username");

//   console.log("Logged-in User ID:", userId);
//   console.log("Role:", role);
//   console.log("Username:", username);

//   if (!userId || !role) {
//     return (
//       <div className="text-center p-6 text-red-500">
//         ⚠️ Error: User not logged in. Please login first.
//       </div>
//     );
//   }

//   return (
//     <div className="dashboard-container p-6">
//       <h1 className="text-2xl font-bold text-center mb-6">User Dashboard</h1>
//       <p className="text-center mb-4 text-lg font-medium">
//         Welcome, <span className="text-blue-600 font-semibold">{username}</span>!
//       </p>

//       {/* User personal info */}
//       <UserDetails userId={userId} role={role} />

//       {/* User's proposals */}
//       <UserProposals userId={userId} />
//     </div>
//   );
// }
