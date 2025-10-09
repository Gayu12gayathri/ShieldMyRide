import React, { useEffect, useState } from "react";
import "./ProposalDeatils.css";

export default function ProposalDetails({ proposal }) {
  const [activeTab, setActiveTab] = useState("details");
  const [userDetails, setUserDetails] = useState(null);
  const [documents, setDocuments] = useState([]);

  useEffect(() => {
    // Fetch user details
    fetch(`https://localhost:7153/api/users/${proposal.userId}`)
      .then((res) => res.json())
      .then(setUserDetails);

    // Fetch submitted documents
    fetch(`https://localhost:7153/api/proposals/${proposal.proposalId}/documents`)
      .then((res) => res.json())
      .then(setDocuments);
  }, [proposal]);

  return (
    <div className="proposal-details">
      <div className="details-header">
        <h3>{proposal.policyName}</h3>
        <span className={`status-tag ${proposal.status.toLowerCase()}`}>
          {proposal.status}
        </span>
      </div>

      <div className="tabs">
        <button
          className={activeTab === "details" ? "active" : ""}
          onClick={() => setActiveTab("details")}
        >
          Borrower Details
        </button>
        <button
          className={activeTab === "documents" ? "active" : ""}
          onClick={() => setActiveTab("documents")}
        >
          Documents
        </button>
        <button
          className={activeTab === "bank" ? "active" : ""}
          onClick={() => setActiveTab("bank")}
        >
          Bank Details
        </button>
      </div>

      {/* Borrower Details */}
      {activeTab === "details" && userDetails && (
        <div className="tab-content">
          <p><b>Name:</b> {userDetails.firstName} {userDetails.lastName}</p>
          <p><b>Email:</b> {userDetails.email}</p>
          <p><b>Phone:</b> {userDetails.phoneNumber}</p>
          <p><b>DOB:</b> {userDetails.dob}</p>
          <p><b>Address:</b> {userDetails.address}</p>
        </div>
      )}

      {/* Documents */}
      {activeTab === "documents" && (
        <div className="tab-content doc-grid">
          {documents.length > 0 ? (
            documents.map((doc, index) => (
              <div key={index} className="doc-card">
                <h4>{doc.documentType}</h4>
                <img src={doc.documentUrl} alt={doc.documentType} />
              </div>
            ))
          ) : (
            <p>No documents uploaded.</p>
          )}
        </div>
      )}

    </div>
  );
}
