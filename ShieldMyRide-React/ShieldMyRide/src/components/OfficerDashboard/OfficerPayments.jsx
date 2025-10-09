import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useLocation } from "react-router-dom";
import { fetchPayments, removePayment, payProposal, modifyPayment } from "../../features/paymentSlice";
import PaymentModal from "../UserDashboard/PaymentModel";
import { logOfficerAction } from "../../Services/loggerOfficerAction"; // import logger
import "../UserDashboard/UserProposal.css";

export default function OfficerPayments({ userId }) {
  const dispatch = useDispatch();
  const location = useLocation();
  const { proposalId: fromProposal } = location.state || {}; 
  const { list: payments, loading, error } = useSelector((state) => state.payments);

  const [showPaymentModal, setShowPaymentModal] = useState(false);
  const [selectedProposal, setSelectedProposal] = useState(fromProposal || null);
  const [selectedPayment, setSelectedPayment] = useState(null);
  const [isUpdateMode, setIsUpdateMode] = useState(false);

  useEffect(() => {
    dispatch(fetchPayments(userId));
  }, [dispatch, userId]);

  useEffect(() => {
    if (fromProposal) setShowPaymentModal(true);
  }, [fromProposal]);

  const handleDelete = async (paymentId) => {
    if (window.confirm("Are you sure you want to delete this payment?")) {
      try {
        await dispatch(removePayment(paymentId)).unwrap();
        // Log deletion
        await logOfficerAction(
          "PaymentDeleted",
          paymentId.toString(),
          `Payment #${paymentId} deleted`,
          "Completed"
        );
      } catch (err) {
        console.error("Failed to delete payment:", err);
      }
    }
  };

  const handleUpdatePayment = (payment) => {
    setSelectedProposal(payment.proposalID);
    setSelectedPayment(payment);
    setIsUpdateMode(true);
    setShowPaymentModal(true);
  };

  // Example: handle submission from modal
  const handlePaymentSubmit = async (paymentData) => {
    try {
      if (isUpdateMode) {
        await dispatch(modifyPayment(paymentData)).unwrap();
        await logOfficerAction(
          "PaymentUpdated",
          paymentData.paymentId.toString(),
          `Payment updated: ₹${paymentData.amountPaid}`,
          "Completed"
        );
      } else {
        await dispatch(payProposal(paymentData)).unwrap();
        await logOfficerAction(
          "PaymentCreated",
          paymentData.proposalId.toString(),
          `Payment created: ₹${paymentData.amountPaid}`,
          "Completed"
        );
      }
    } catch (err) {
      console.error("Payment submission failed:", err);
    } finally {
      setShowPaymentModal(false);
      setIsUpdateMode(false);
      setSelectedPayment(null);
    }
  };

  if (loading) return <p>Loading payments...</p>;
  if (error) return <p className="error">Error: {error}</p>;

  return (
    <div className="user-proposals">
      {(!payments || payments.length === 0) ? (
        <p>No payments found for this user.</p>
      ) : (
        <table className="proposal-table">
          <thead>
            <tr>
              <th>Proposal ID</th>
              <th>Transaction ID</th>
              <th>Amount Paid</th>
              <th>Status</th>
              <th>Payment Date</th>
              <th>Balance</th>
              <th>Payment For</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((pay) => (
              <tr key={pay.paymentId}>
                <td>{pay.proposalID}</td>
                <td>{pay.transactionId}</td>
                <td>₹{pay.amountPaid}</td>
                <td>{pay.paymentStatus}</td>
                <td>{new Date(pay.paymentDate).toLocaleDateString()}</td>
                <td>{pay.balance}</td>
                <td>{pay.forClaim ? "Claim" : "Policy"}</td>
                <td>
                  <button className="update-btn" onClick={() => handleUpdatePayment(pay)}>📝 Update</button>
                  <button className="delete-btn" onClick={() => handleDelete(pay.paymentId)}>🚮 Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {showPaymentModal && (
        <PaymentModal
          userId={userId}
          proposalId={isUpdateMode ? selectedPayment.proposalID : selectedProposal}
          payment={isUpdateMode ? selectedPayment : null}
          onClose={() => {
            setShowPaymentModal(false);
            setIsUpdateMode(false);
            setSelectedPayment(null);
          }}
          onSubmit={handlePaymentSubmit}
        />
      )}
    </div>
  );
}
