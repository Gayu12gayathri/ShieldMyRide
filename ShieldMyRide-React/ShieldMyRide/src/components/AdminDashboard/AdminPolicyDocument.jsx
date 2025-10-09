import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchAllPolicyDocuments } from "../../features/policydocumentSlice";
import { fetchAllProposals } from "../../features/proposalSlice";
import "../OfficerDashboard/OfficerPolicyDocument.css";

export default function AdminPolicyDocument() {
  const dispatch = useDispatch();
  const { list: documents, loading, error } = useSelector(
    (state) => state.policydocuments
  );
  const { list: proposals } = useSelector((state) => state.proposals);

  useEffect(() => {
    dispatch(fetchAllPolicyDocuments());
    dispatch(fetchAllProposals());
  }, [dispatch]);



  return (
    <div className="admin-policy-dashboard">
      <h2>All Policy Documents</h2>

      {loading && <p>Loading documents...</p>}
      {error && <p className="msg">❌ {error}</p>}
      {!loading && documents.length === 0 && <p>No documents found.</p>}

      <table className="doc-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Policy Name</th>
            <th>Document Type</th>
            <th>Uploaded Date</th>
            <th>View</th>
          </tr>
        </thead>
        <tbody>
          {documents.map((doc) => (
            <tr key={doc.documentId}>
              <td>{doc.documentId}</td>
              <td>{doc.policyId}</td>
              <td>{doc.documentType}</td>
              <td>{new Date(doc.uploadedAt).toLocaleDateString()}</td>
              <td>
                <a
                  href={`https://localhost:7153/uploads/${doc.documentPath}`}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  View
                </a>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
