import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchAllProposals } from "../../features/proposalSlice";
import "../OfficerDashboard/OfficerProposal.css";

export default function AdminProposals() {
  const dispatch = useDispatch();
  const { list: proposals, loading, error } = useSelector((state) => state.proposals);

  useEffect(() => {
    dispatch(fetchAllProposals());
  }, [dispatch]);

  const getDisplayStatus = (proposal) => {
    if (proposal.proposalStatus === "Approved") {
      return proposal.paymentDone ? "Active" : "QuoteGenerated";
    }
    if (proposal.proposalStatus === "Settled") return "Settled";
    return proposal.proposalStatus;
  };

  if (loading) return <p>Loading proposals...</p>;
  if (error) return <p className="text-red-500">{error}</p>;
  if (!proposals.length) return <p>No proposals available.</p>;

  return (
    <div className="admin-proposals">
      <h2>Admin Proposal View</h2>
      <table className="proposal-table">
        <thead>
          <tr>
            <th>Policy Name</th>
            <th>User ID</th>
            <th>Vehicle Type</th>
            <th>Vehicle Reg No</th>
            <th>Vehicle Age</th>
            <th>Premium (₹)</th>
            <th>Documents Verified</th>
            <th>Status</th>
            <th>Remarks</th>
          </tr>
        </thead>
        <tbody>
          {proposals.map((proposal) => {
            const status = getDisplayStatus(proposal);
            return (
              <tr key={proposal.proposalId}>
                <td>{proposal.policyName}</td>
                <td>{proposal.userId}</td>
                <td>{proposal.vehicleType}</td>
                <td>{proposal.vehicleRegNo}</td>
                <td>{proposal.vehicleAge}</td>
                <td>{proposal.premium}</td>
                <td>{proposal.documentsVerified ? "✅ Verified" : "✅ Verified"}</td>
                <td className={`status-${status.toLowerCase()}`}>{status}</td>
                <td>{proposal.officerRemarks || "-"}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
