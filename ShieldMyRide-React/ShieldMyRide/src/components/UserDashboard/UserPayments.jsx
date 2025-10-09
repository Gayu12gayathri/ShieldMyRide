import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useLocation } from "react-router-dom";
import {
  fetchPayments,
  removePayment,
  payProposal,
  modifyPayment,
} from "../../features/paymentSlice";
import PaymentModal from "../UserDashboard/PaymentModel";
import "../UserDashboard/UserProposal.css";

export default function UserPayments({ userId }) {
  const dispatch = useDispatch();
  const location = useLocation();
  const { proposalId: fromProposal } = location.state || {};
  const { list: payments, loading, error } = useSelector(
    (state) => state.payments
  );

  const [showPaymentModal, setShowPaymentModal] = useState(false);
  const [selectedProposal, setSelectedProposal] = useState(fromProposal || null);
  const [selectedPayment, setSelectedPayment] = useState(null);
  const [isUpdateMode, setIsUpdateMode] = useState(false);

  useEffect(() => {
    dispatch(fetchPayments());
  }, [dispatch]);

  // open modal if user navigates from proposal
  useEffect(() => {
    if (fromProposal) setShowPaymentModal(true);
  }, [fromProposal]);

  const handleDelete = (paymentId) => {
    if (window.confirm("Are you sure you want to delete this payment?")) {
      dispatch(removePayment(paymentId));
    }
  };

  const handleMakePayment = (proposalId) => {
    setSelectedProposal(proposalId);
    setSelectedPayment(null);
    setIsUpdateMode(false);
    setShowPaymentModal(true);
  };

  const handleUpdatePayment = (payment) => {
    setSelectedProposal(payment.proposalID);
    setSelectedPayment(payment);
    setIsUpdateMode(true);
    setShowPaymentModal(true);
  };

  const handlePaymentSubmit = (paymentData) => {
    if (isUpdateMode && selectedPayment) {
      // Update existing payment
      dispatch(
        modifyPayment({
          paymentId: selectedPayment.paymentId,
          updatedData: {
            userID: userId,
            proposalID: selectedPayment.proposalID,
            transactionId:
              paymentData.transactionId || "TXN-" + Date.now(),
            amountPaid: paymentData.amountPaid,
            paymentStatus: paymentData.paymentStatus || "Paid",
            paymentDate: new Date().toISOString().split("T")[0],
            forClaim: paymentData.forClaim || false,
          },
        })
      );
    } else {
      // Create new payment
      dispatch(
        payProposal({
          proposalId: paymentData.proposalID,
          amount: paymentData.amountPaid,
          userId: paymentData.userID,
        })
      );
    }

    setShowPaymentModal(false);
    setIsUpdateMode(false);
    setSelectedPayment(null);
  };

  if (loading) return <p>Loading payments...</p>;
  if (error) return <p className="error">Error: {error}</p>;

  return (
    <div className="user-proposals">
      <h3>User Payments</h3>
        <button
          className="make-payment-btn"
          onClick={() => handleMakePayment(selectedProposal || fromProposal)}
        >
          💳 Make New Payment
        </button>
      {(!payments || payments.length === 0) ? (
        <p>No payments found.</p>
      ) : (
        <table className="proposal-table">
          <thead>
            <tr>
              <th>Proposal ID</th>
              <th>Amount Paid</th>
              <th>Status</th>
              <th>Payment Date</th>
              <th>Balance</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((pay) => (
              <tr key={pay.paymentId}>
                <td>{pay.proposalID}</td>
                <td>₹{pay.amountPaid}</td>
                <td>{pay.paymentStatus}</td>
                <td>{new Date(pay.paymentDate).toLocaleDateString()}</td>
                <td>{pay.balance}</td>
                <td>
                  {/* <button
                    className="make-payment-btn"
                    onClick={() => handleMakePayment(pay.proposalID)}
                  >
                    💹 Make Payment
                  </button> */}
                  <button
                    className="update-btn"
                    onClick={() => handleUpdatePayment(pay)}
                  >
                    📝 Update
                  </button>
                  <button
                    className="delete-btn"
                    onClick={() => handleDelete(pay.paymentId)}
                  >
                    🚮 Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {showPaymentModal && (
        <PaymentModal
          userId={userId}
          proposalId={selectedProposal}
          payment={selectedPayment}
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