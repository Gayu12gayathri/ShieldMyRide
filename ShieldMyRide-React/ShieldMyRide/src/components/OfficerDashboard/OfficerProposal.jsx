import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchAllProposals, reviewProposalThunk } from "../../features/proposalSlice";
import { useNavigate } from "react-router-dom";
import "./OfficerProposal.css";
import { logOfficerAction } from "../../Services/loggerOfficerAction";

export default function OfficerProposals() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const { list: proposals, loading, error } = useSelector((state) => state.proposals);
  const [reviewStatus, setReviewStatus] = useState({});
  const [reviewComments, setReviewComments] = useState({});
  const [verifiedProposals, setVerifiedProposals] = useState({});


  // Navigate to policy document
  const handlePolicyDocClick = (proposalId) => {
    navigate(`/officer/policydocument/${proposalId}`);
  };

  // Fetch proposals on mount
  useEffect(() => {
    dispatch(fetchAllProposals())
      .unwrap()
      .then((data) => console.log("Proposals fetched:", data))
      .catch((err) => console.error("Error fetching proposals:", err));
  }, [dispatch]);

 // Review handler
const handleReview = async (proposalId) => {
  try {
    const status = reviewStatus[proposalId] || "Pending";
    const comments = reviewComments[proposalId] || "";

    const reviewData = { proposalStatus: status, remarks: comments };
    const result = await dispatch(reviewProposalThunk({ proposalId, reviewData })).unwrap();

    setReviewStatus(prev => ({ ...prev, [proposalId]: result.proposalStatus || result.newStatus }));
    setReviewComments(prev => ({ ...prev, [proposalId]: result.remarks || result.officerRemarks }));

    alert(`Proposal ${proposalId} updated to ${result.proposalStatus || result.newStatus}`);

    // ===== Log the action centrally =====
    logOfficerAction(
      "ProposalReviewed",
      proposalId,
      `Status updated to ${result.proposalStatus || result.newStatus}, Remarks: ${comments}`
    );

  } catch (err) {
    console.error("Review failed:", err);
    alert("Failed to review proposal. Check console.");
  }
};
  // Determine display status
 const getDisplayStatus = (proposal) => {
  if (proposal.proposalStatus === "Approved") {
    return proposal.paymentDone ? "Active" : "QuoteGenerated";
  }
  if (proposal.proposalStatus === "Settled") return "Settled";
  return proposal.proposalStatus || "Pending"; // default to "Pending"
};

  // Render loading/error/no data states
  if (loading) return <p>Loading proposals...</p>;
  if (error) return <p className="text-red-500">{error}</p>;
  if (!proposals || proposals.length === 0) return <p>No proposals to review.</p>;

  return (
    <div className="officer-proposals">
      <h2>Officer Proposal Review</h2>
      <table className="proposal-table">
        <thead>
          <tr>
            <th>Policy Name</th>
            <th>User ID</th>
            <th>Vehicle Type</th>
            <th>Vehicle Reg No</th>
            <th>Vehicle Age</th>
            <th>Premium (₹)</th>
            <th>Review Documents</th>
            <th>Status</th>
            <th>Remarks</th>
            <th>Review</th>
            <th>Actions</th>
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
                <td>
                  <button onClick={() => handlePolicyDocClick(proposal.proposalId)}>
                    Policy Document
                  </button>
                </td>
                <td className={`status-${status.toLowerCase()}`}>{status}</td>
                <td>
                  <input
                    type="text"
                    placeholder="Remarks"
                    value={reviewComments[proposal.proposalId] || proposal.officerRemarks || ""}
                    onChange={(e) =>
                      setReviewComments({
                        ...reviewComments,
                        [proposal.proposalId]: e.target.value,
                      })
                    }
                  />
                </td>
                <td>
                  <select
                    value={reviewStatus[proposal.proposalId] || proposal.proposalStatus || "Pending"}
                    onChange={(e) =>
                      setReviewStatus({
                        ...reviewStatus,
                        [proposal.proposalId]: e.target.value,
                      })
                    }
                  >
                    <option value="Pending">Pending</option>
                    <option value="Approved">Approve</option>
                    <option value="Rejected">Reject</option>
                  </select>
                </td>
                <td>
                  <button
                    className="btn btn-sm btn-success"
                    onClick={() => handleReview(proposal.proposalId)}
                  >
                    📝 Review
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}