import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchClaims, updateClaimThunk, deleteClaimThunk } from "../../features/claimSlice";
import "../UserDashboard/UserProposal.css";

export default function AdminClaim() {
  const dispatch = useDispatch();
  const { list: claims, loading, error } = useSelector((state) => state.claims);

  const [updatingClaimId, setUpdatingClaimId] = useState(null);
  const [updatedStatus, setUpdatedStatus] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    dispatch(fetchClaims());
  }, [dispatch]);

  const handleDeleteClaim = async (claimId) => {
    if (window.confirm("Are you sure you want to delete this claim?")) {
      try {
        await dispatch(deleteClaimThunk(claimId)).unwrap();
        setMessage(`✅ Claim #${claimId} deleted successfully!`);
      } catch (err) {
        console.error(err);
        setMessage("❌ Failed to delete claim. " + (err?.message || ""));
      }
    }
  };

  const handleStartUpdate = (claim) => {
    setUpdatingClaimId(claim.claimId);
    setUpdatedStatus(claim.claimStatus);
  };

  const handleUpdateClaim = async () => {
    try {
      await dispatch(updateClaimThunk({ 
        claimId: updatingClaimId, 
        updatedData: { claimStatus: updatedStatus } 
      })).unwrap();
      setMessage(`✅ Claim #${updatingClaimId} updated successfully!`);
      setUpdatingClaimId(null);
      setUpdatedStatus("");
    } catch (err) {
      console.error(err);
      setMessage("❌ Failed to update claim. " + (err?.message || ""));
    }
  };
  const getClaimBalance = (claim) => {
  const balance = (claim.settlementAmount-claim.claimAmount);
  return balance  ;
};

    const getDisplayStatus = (claim) => {
    if (claim.settlementAmount >= claim.claimAmount) return "Settled";
    if (claim.settlementAmount > 0 && claim.settlementAmount < claim.claimAmount)
      return "PartiallyPaid";
    if (claim.claimStatus === "Rejected") return "Rejected";
    if (claim.claimStatus === "Approved" && claim.settlementAmount === 0)
      return "Approved";
    if (claim.claimStatus === "UnderReview") return "UnderReview";
    if (claim.claimStatus === "Submitted") return "Submitted";
    if (claim.claimStatus === "Pending") return "Pending";
    return claim.claimStatus; // fallback
  };


  return (
    <div className="officer-claims">
      <h3>Admin Claims Dashboard</h3>
      {message && <p>{message}</p>}
      {loading && <p>Loading claims...</p>}
      {error && <p className="error">Error: {error}</p>}

      <table className="proposal-table">
        <thead>
          <tr>
            <th>Claim ID</th>
            <th>Proposal ID</th>
            <th>ClaimAmount</th>
            <th>ClaimStatus</th>
            <th>Balance</th>
            <th>Filed Date</th>
            <th>Settlement Amount</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {claims.map((claim) => (
            <tr key={claim.claimId}>
              <td>{claim.claimId}</td>
              <td>{claim.proposalId}</td>
              <td>₹{claim.claimAmount}</td>
              <td className={`status-${getDisplayStatus(claim).toLowerCase()}`}>
                {updatingClaimId === claim.claimId ? (
                  <select
                    value={updatedStatus}
                    onChange={(e) => setUpdatedStatus(e.target.value)}
                  >
                    <option value="Submitted">Submitted</option>
                    <option value="Pending">Pending</option>
                    <option value="UnderReview">UnderReview</option>
                    <option value="Approved">Approved</option>
                    <option value="Rejected">Rejected</option>
                    <option value="PartiallyPaid">PartiallyPaid</option>
                    <option value="Settled">Settled</option>
                  </select>
                ) : (
                  getDisplayStatus(claim)
                )}
              </td>
              <td>₹{getClaimBalance(claim)}</td>
              <td>{new Date(claim.claimDate).toLocaleDateString()}</td>
              <td>₹{claim.settlementAmount}</td>
              <td>
                {updatingClaimId === claim.claimId ? (
                  <>
                    <button className="submit-btn" onClick={handleUpdateClaim}>
                      💾 Save
                    </button>
                    <button className="cancel-btn" onClick={() => setUpdatingClaimId(null)}>
                      ❌ Cancel
                    </button>
                  </>
                ) : (
                  <>
                    <button className="update-btn" onClick={() => handleStartUpdate(claim)}>
                      ✏️ Update
                    </button>
                    <button className="delete-btn" onClick={() => handleDeleteClaim(claim.claimId)}>
                      🚮 Delete
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
