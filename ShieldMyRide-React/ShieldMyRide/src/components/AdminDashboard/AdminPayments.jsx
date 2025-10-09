import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchPayments } from "../../features/paymentSlice";
import "../UserDashboard/UserProposal.css";

export default function AdminPayments() {
  const dispatch = useDispatch();
  const { list: payments, loading, error } = useSelector((state) => state.payments);

  useEffect(() => {
    dispatch(fetchPayments());
  }, [dispatch]);

  if (loading) return <p>Loading payments...</p>;
  if (error) return <p className="error">Error: {error}</p>;

  console.log("Payments from Redux:", payments);

  return (
    <div className="user-proposals">
      <h3>All Payments (Admin View)</h3>

      {(!payments || payments.length === 0) ? (
        <p>No payments found.</p>
      ) : (
        <table className="proposal-table">
          <thead>
            <tr>
              <th>Payment ID</th>
              <th>User ID</th>
              <th>Proposal ID</th>
              <th>Transaction ID</th>
              <th>Amount Paid</th>
              <th>Status</th>
              <th>Payment Date</th>
              <th>Balance</th>
              <th>Payment For</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((pay) => (
              <tr key={pay.paymentId}>
                <td>{pay.paymentId}</td>
                <td>{pay.userID}</td>
                <td>{pay.proposalID}</td>
                <td>{pay.transactionId}</td>
                <td>₹{pay.amountPaid}</td>
                <td>{pay.paymentStatus}</td>
                <td>{new Date(pay.paymentDate).toLocaleDateString()}</td>
                <td>{pay.balance}</td>
                <td>{pay.forClaim ? "Claim" : "Policy"}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
