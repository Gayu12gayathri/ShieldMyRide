import React, { useState } from "react";
import { downloadPolicyPDF } from "../Services/PolicyDownloadService";
import "./PolicyDownloadLink.css";

export default function PolicyDownloadLink() {
  const [proposalId, setProposalId] = useState("");
  const [vehicleRegNo, setVehicleRegNo] = useState("");
  const [loading, setLoading] = useState(false);

  const handleDownload = async () => {
    if (!proposalId || !vehicleRegNo) {
      alert("Please enter both Proposal ID and Vehicle Registration Number.");
      return;
    }

    try {
      setLoading(true);
      await downloadPolicyPDF(proposalId, vehicleRegNo);
    } catch (error) {
      console.error("Error downloading policy:", error);
      alert("Failed to download policy. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="download-policy-container">
      <h2>Download Your Policy</h2>
      <p>Enter your Vehicle Registration Number and Proposal ID below to get your policy document.</p>
      
      <div className="download-policy-form">
        <input
          type="text"
          placeholder="Vehicle Registration Number"
          value={vehicleRegNo}
          onChange={(e) => setVehicleRegNo(e.target.value)}
        />
        <input
          type="number"
          placeholder="Proposal ID"
          value={proposalId}
          onChange={(e) => setProposalId(e.target.value)}
        />
        <button onClick={handleDownload} disabled={loading}>
          {loading ? "Downloading..." : "Download Policy PDF"}
        </button>
      </div>
    </div>
  );
}
