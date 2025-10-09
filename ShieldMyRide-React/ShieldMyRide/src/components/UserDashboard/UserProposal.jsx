import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { fetchAllProposals, removeProposal } from "../../features/proposalSlice";
import { payProposal } from "../../features/paymentSlice";
import '../UserDashboard/UserProposal.css';

export default function UserProposals({ userId }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { list: proposals, loading, error } = useSelector(state => state.proposals);
  const paymentState = useSelector(state => state.payments);

 useEffect(() => {
    dispatch(fetchAllProposals());
  }, [dispatch]);

  const handleDelete = async (proposalId) => {
    if (!window.confirm("Are you sure you want to delete this proposal?")) return;
    try {
      await dispatch(removeProposal(proposalId)).unwrap();
      alert("Proposal deleted successfully");
    } catch (err) {
      alert("Failed to delete proposal: " + JSON.stringify(err));
    }
  };

  const handlePay = (proposal) => {
    navigate("/user/payments", { state: { proposalId: proposal.proposalId } });
  };

  const handlePayment = async (proposalId, amount) => {
    console.log("Payment request:", { proposalId, amount, userId });
    if (!window.confirm(`Pay ₹${amount} for this proposal?`)) return;
    try {
      const resultAction = await dispatch(payProposal({ proposalId, amount, userId })).unwrap();
      alert(`Payment successful! Remaining Balance: ₹${balance}, Status: ${status}`);
    } catch (err) {
      console.error("Payment failed:", err);
      alert(`Payment failed: ${err?.message || err}`);
    }
  };

  const getDisplayStatus = (proposal) => {
  if (proposal.proposalStatus === "Approved") {
    return proposal.paymentDone ? "Active" : "QuoteGenerated";
  }
  return proposal.proposalStatus;
};

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!proposals || proposals.length === 0) return <p>No proposals found.</p>;

  return (
    <div className="user-proposals">
      <table className="proposal-table">
        <thead>
          <tr>
            <th>Policy Name</th>
            <th>Vehicle Type</th>
            <th>Vehicle Reg No</th>
            <th>Vehicle Age</th>
            <th>Premium (₹)</th>
            <th>NCB (%)</th>
            <th>Roadside Assist</th>
            <th>Zero Dep</th>
            <th>Status</th>
            <th>Created At</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {proposals.map((proposal) => (
            <tr key={proposal.proposalId}>
              <td>{proposal.policyName}</td>
              <td>{proposal.vehicleType}</td>
              <td>{proposal.vehicleRegNo}</td>
              <td>{proposal.vehicleAge}</td>
              <td>{proposal.premium}</td>
              <td>{proposal.ncbPercent}</td>
              <td>{proposal.roadsideAssist ? "Yes" : "No"}</td>
              <td>{proposal.zeroDep ? "Yes" : "No"}</td>
              <td className={`status-${getDisplayStatus(proposal).toLowerCase()}`}>
                {getDisplayStatus(proposal)}
              </td>
              <td>{new Date(proposal.createdAt).toLocaleString()}</td>
              <td>
                <button
                  onClick={() => handleDelete(proposal.proposalId)}
                  className="btn btn-sm btn-danger"
                >
                  🚮 Delete
                </button>{" "}
                {["QuoteGenerated", "Approved"].includes(getDisplayStatus(proposal)) && (
                  <button
                  onClick={() => handlePayment(proposal.proposalId, proposal.premium)}
                  className="btn btn-sm btn-success"
                >
                  💸 Pay
                </button>
              )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {paymentState.loading && <p>Processing payment...</p>}
      {paymentState.error && <p className="error">{paymentState.error}</p>}
    </div>
  );
}
