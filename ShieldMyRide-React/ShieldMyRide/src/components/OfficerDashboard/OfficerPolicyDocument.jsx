import React, { useEffect, useState } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchDocumentsByProposalThunk,
  removePolicyDocumentThunk,
  fetchAllPolicyDocuments,
} from "../../features/policydocumentSlice";
import "./OfficerPolicyDocument.css";

export default function OfficerPolicyDocument() {
  const { proposalId } = useParams();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const location = useLocation();
  const setVerifiedProposals = location.state?.setVerifiedProposals; // get setter from navigation

  const { list: documents, loading, error } = useSelector(
    (state) => state.policydocuments
  );

  const [verifiedDocs, setVerifiedDocs] = useState({});
  const [remarks, setRemarks] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (proposalId) {
      dispatch(fetchDocumentsByProposalThunk(proposalId));
    } else {
      dispatch(fetchAllPolicyDocuments());
    }
  }, [dispatch, proposalId]);

  const handleVerifyToggle = (id) => {
    setVerifiedDocs((prev) => ({
      ...prev,
      [id]: !prev[id],
    }));
  };

  const handleDeleteDocument = (id) => {
    if (window.confirm("Are you sure you want to delete this document?")) {
      dispatch(removePolicyDocumentThunk(id));
    }
  };

  const handleSaveVerification = () => {
    const allVerified = Object.values(verifiedDocs).every(v => v === true);

    if (!allVerified) {
      alert("Please verify all documents before proceeding.");
      return;
    }

    // Update verifiedProposals in OfficerProposals
    if (setVerifiedProposals) {
      setVerifiedProposals(prev => ({ ...prev, [proposalId]: true }));
    }

    setMessage("✅ Documents verified successfully!");
    setTimeout(() => navigate("/officer/proposals"), 1000);
  };

  return (
    <div className="officer-doc-review">
      <h2>Document Verification for Proposal #{proposalId}</h2>

      {loading && <p>Loading documents...</p>}
      {error && <p className="msg">❌ {error}</p>}
      {message && <p className="msg">{message}</p>}

      <table className="doc-table">
        <thead>
          <tr>
            <th>Document Type</th>
            <th>View</th>
            <th>Verified</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {documents.length > 0 ? (
            documents.map((doc) => (
              <tr key={doc.documentId}>
                <td>{doc.documentType}</td>
                <td>
                  <a
                    href={`https://localhost:7153/uploads/${doc.documentPath}`}
                    target="_blank"
                    rel="noreferrer"
                  >
                    View
                  </a>
                </td>
                <td>
                  <input
                    type="checkbox"
                    checked={verifiedDocs[doc.documentId] || false}
                    onChange={() => handleVerifyToggle(doc.documentId)}
                  />
                </td>
                <td>
                  <button
                    onClick={() => handleDeleteDocument(doc.documentId)}
                    className="delete-btn"
                  >
                    🗑 Delete
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="4">No documents found.</td>
            </tr>
          )}
        </tbody>
      </table>

      <label>
        Officer Remarks:
        <textarea
          value={remarks}
          onChange={(e) => setRemarks(e.target.value)}
          rows={3}
        />
      </label>

      <button onClick={handleSaveVerification} className="save-btn">
        💾 Save Verification
      </button>
    </div>
  );
}
