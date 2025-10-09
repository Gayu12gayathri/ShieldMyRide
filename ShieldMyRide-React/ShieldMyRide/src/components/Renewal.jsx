import { useState } from "react";
import axios from "axios";
import "./Home.css";
import "./QuotePage.css";
import "./Renewal.css"
// import "./UserDashboard/UserProposal.css"

export default function Renewal() {
  const [regNo, setRegNo] = useState("");
  const [captcha, setCaptcha] = useState("");
  const [showProposals, setShowProposals] = useState(false);
  const [proposals, setProposals] = useState([]);
  const [loading, setLoading] = useState(false);

  const correctCaptcha = "15"; // 7 + 8 = 15

  // Step 1: Handle Registration form + captcha
 const handleCaptchaSubmit = async (e) => {
  e.preventDefault();

  const token = localStorage.getItem("token");
  if (!token) {
    alert("You must be logged in to view proposals!");
    return;
  }

  if (captcha !== correctCaptcha) {
    alert("Captcha is incorrect!");
    return;
  }
  if (!regNo) {
    alert("Please enter your vehicle registration number!");
    return;
  }

  try {
    setLoading(true);
    const res = await axios.get(
      `https://localhost:7153/api/Proposals/by-regno/${regNo}`,
      { headers: { Authorization: `Bearer ${token}` } } // Add token here
    );
    if (res.data && res.data.length > 0) {
      setProposals(res.data);
      setShowProposals(true);
    } else {
      alert("No proposals found for this registration number.");
    }
  } catch (err) {
    console.error(err);
    const message =
      err.response?.data?.message || err.message || "Error fetching proposals.";
    alert(message);
  } finally {
    setLoading(false);
  }
};


  // Step 2: Handle Renewal
  const handleRenew = async (proposalId) => {
    if (!window.confirm("Are you sure you want to renew this policy?")) return;

    try {
      setLoading(true);

      const token = localStorage.getItem("token");
      if (!token) {
        alert("You must be logged in to renew a policy!");
        return;
      }

      const res = await axios.post(
        `https://localhost:7153/api/Proposals/renew/${proposalId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );

      console.log("Renewal API response:", res.data);

      const newProposal = res.data.renewalProposal;
      if (!newProposal) {
        alert("Backend did not return the renewal proposal correctly.");
        return;
      }

      alert(
        `✅ Renewal successful!\nNew Proposal ID: ${newProposal.proposalId}\nPremium: ₹${newProposal.premium}`
      );

      // Add renewed proposal to the list
      setProposals((prev) => [...prev, newProposal]);
    } catch (err) {
      console.error(err);
      const message =
        err.response?.data?.message || err.message || "Error renewing policy.";
      alert(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="renewal-container">
      {!showProposals ? (
        <div className="main-section">
          <div className="main-form-card">
            <h2>Renew Your Policy</h2>
            <p>Enter your registration number to find existing policies</p>

            <form onSubmit={handleCaptchaSubmit}>
              <input
                type="text"
                placeholder="Vehicle Registration No."
                value={regNo}
                onChange={(e) => setRegNo(e.target.value)}
                required
              />

              <div className="captcha-box">
                <label>7 + 8 = ?</label>
                <input
                  type="text"
                  placeholder="Enter Answer"
                  value={captcha}
                  onChange={(e) => setCaptcha(e.target.value)}
                  required
                />
              </div>

              <button type="submit" className="submit-btn" disabled={loading}>
                {loading ? "Fetching..." : "View Proposals"}
              </button>
            </form>

            <div className="checkboxes">
              <label>
                <input type="checkbox" />
                <span className="checkbox-text">
                  I accept <a href="#">Terms and Conditions</a>
                </span>
              </label>
              <label>
                <input type="checkbox" />
                <span className="checkbox-text">
                  Get updates on <b>WhatsApp</b>
                </span>
              </label>
            </div>
          </div>
        </div>
      ) : (
        <div className="renewal-table">
        <div className="user-proposal">
          <h2>Available Proposals for Renewal</h2>
          <p>
            For Vehicle Reg No: <b>{regNo}</b>
          </p>

          {loading ? (
            <p>Loading...</p>
          ) : (
            <table className="proposals-table">
              <thead>
                <tr>
                  <th>Proposal ID</th>
                  <th>Policy Name</th>
                  <th>Status</th>
                  <th>Start Date</th>
                  <th>End Date</th>
                  <th>Vehicle Reg No</th>
                  <th>Vehicle Type</th>
                  <th>Vehicle Age</th>
                  <th>Premium (₹)</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {proposals.map((proposal) => (
                  <tr key={proposal.proposalId}>
                    <td>{proposal.proposalId}</td>
                    <td>{proposal.policyName || "N/A"}</td>
                    <td>{proposal.proposalStatus}</td>
                    <td>{proposal.policyStartDate?.split("T")[0]}</td>
                    <td>{proposal.policyEndDate?.split("T")[0]}</td>
                    <td>{proposal.vehicleRegNo}</td>
                    <td>{proposal.vehicleType}</td>
                    <td>{proposal.vehicleAge}</td>
                    <td>{proposal.premium}</td>
                    <td>
                      <button
                        className="submit-btn"
                        onClick={() => handleRenew(proposal.proposalId)}
                        disabled={loading}
                      >
                        Renew Policy
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
        </div>
      )}
    </div>
  );
}
