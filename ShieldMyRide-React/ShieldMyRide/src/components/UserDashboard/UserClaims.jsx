import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchClaims, createClaimThunk, deleteClaimThunk } from "../../features/claimSlice";
import { fetchPayments } from "../../features/paymentSlice";
import "../UserDashboard/UserProposal.css";

export default function UserClaims({ userId }) {
  const dispatch = useDispatch();
  const { list: claims, loading, error } = useSelector((state) => state.claims);
  const { list: payments } = useSelector((state) => state.payments);

  const [claimData, setClaimData] = useState({
    proposalId: "",
    claimDescription: "",
    claimAmount: "",
  });
  const [creating, setCreating] = useState(false);
  const [message, setMessage] = useState("");
  const [showForm, setShowForm] = useState(false); // ✅ show/hide form

  // Fetching both claims and payments
  useEffect(() => {
    dispatch(fetchClaims());
    dispatch(fetchPayments(userId));
  }, [dispatch, userId]);

  // Auto-fill claimAmount when proposalId changes
  useEffect(() => {
    if (claimData.proposalId && payments?.length > 0) {
      const proposalPayments = payments.filter(
        (p) => p.proposalID === Number(claimData.proposalId)
      );
      if (proposalPayments.length > 0) {
        const latest = proposalPayments[proposalPayments.length - 1];
        setClaimData((prev) => ({
          ...prev,
          claimAmount: latest.balance || 0,
        }));
      }
    }
  }, [claimData.proposalId, payments]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setClaimData({ ...claimData, [name]: value });
  };

  const handleCreateClaim = async () => {
  setCreating(true);
  setMessage("");
  try {
    const settlement = getClaimSettlementAmount(Number(claimData.proposalId));

    const newClaim = {
      claimId: 0,
      proposalId: Number(claimData.proposalId),
      userId: userId,
      claimDate: new Date().toISOString(),
      claimDescription: claimData.claimDescription || "",
      claimAmount: Number(claimData.claimAmount) || 0,
      settlementAmount: settlement,
      claimStatus: settlement === 0 ? "Submitted" : settlement < claimData.claimAmount ? "PartiallyPaid" : "Settled",
    };

    await dispatch(createClaimThunk(newClaim)).unwrap();

    setClaimData({
      proposalId: "",
      claimDescription: "",
      claimAmount: "",
      settlementAmount: 0,
      claimStatus: "",
    });
    setShowForm(false);
    setMessage("✅ Claim created successfully!");
  } catch (err) {
    console.error(err);
    setMessage("❌ Failed to create claim. " + (err?.message || ""));
  } finally {
    setCreating(false);
  }
};



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
  
// // Calculate settlement dynamically
// const getClaimSettlementAmount = (proposalId) => {
//   const relatedPayments = payments.filter(
//     (p) => p.proposalID === proposalId && p.forClaim
//   );
//   return relatedPayments.reduce((sum, p) => sum + (p.amountPaid || 0), 0);
// };

// Calculate remaining balance
// Calculate remaining balance (based on payments)

const getClaimBalance = (claim) => {
  const balance = (claim.settlementAmount-claim.claimAmount);
  return balance  ;
};

// Determine display status without touching settlementAmount
const getDisplayStatus = (claim) => {
  const balance = getClaimBalance(claim);
  if (balance === 0) return "Settled";
  if (balance > claim.claimAmount) return "PartiallyPaid";
  return "Pending";
};



  return (
    <div className="user-claims">
      <h3>User Claims</h3>
      <button
        className="create-claim-btn"
        onClick={() => setShowForm((prev) => !prev)}
      >
       {showForm ? "Cancel" : "➕ Create Claim"}
      </button>
      {showForm && (
        <div className="claim-form">
          <input
            type="number"
            name="proposalId"
            placeholder="Proposal ID"
            value={claimData.proposalId}
            onChange={handleInputChange}
          />
          <input
            type="text"
            name="claimDescription"
            placeholder="Claim Description"
            value={claimData.claimDescription}
            onChange={handleInputChange}
          />
          <input
            type="number"
            name="claimAmount"
            placeholder="Claim Amount (auto-filled)"
            value={claimData.claimAmount}
            readOnly
          />
          <button className="submit-claim-btn" onClick={handleCreateClaim} disabled={creating}>
            {creating ? "Creating..." : "Submit Claim"}
          </button>
        </div>
      )}

      {message && <p>{message}</p>}

      {/* Claims table */}
      {loading && <p>Loading claims...</p>}
      {error && <p className="error">Error: {error}</p>}

      <table className="proposal-table">
        <thead>
          <tr>
            {/* <th>Claim ID</th> */}
            <th>Proposal ID</th>
            <th>Claim Amount</th>
            <th>Claim Status</th>
            <th>Filed Date</th>
            <th>Payment Balance</th>
            <th>Settlement Amount</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {claims.map((claim) => (
            <tr key={claim.claimId}>
              {/* <td>{claim.claimId}</td> */}
              <td>{claim.proposalId}</td>
              <td>₹{claim.claimAmount}</td>
              <td className={`status-${getDisplayStatus(claim).toLowerCase()}`}>
                {getDisplayStatus(claim)}
              </td>
              <td>{new Date(claim.claimDate).toLocaleDateString()}</td>
              <td>₹{getClaimBalance(claim)}</td>
              <td>{claim.settlementAmount}</td>
              <td>
                <button
                  className="delete-btn"
                  onClick={() => handleDeleteClaim(claim.claimId)}
                >
                 🚮 Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
