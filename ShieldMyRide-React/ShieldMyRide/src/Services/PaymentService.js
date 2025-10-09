// src/Services/PaymentService.js
import axios from "axios";

const backendUrlPayments = "https://localhost:7153/api/Payments";

// Process a payment for a proposal
export const processProposalPayment = async (proposalId, amount, userId,forClaim) => {
  try {
    const token = localStorage.getItem("token");
    if (!token) throw new Error("User not authenticated");

    const payload = {
        UserID: userId,
        ProposalID: proposalId,
        TransactionId: "TXN-" + Date.now(),
        AmountPaid: parseFloat(amount), // ✅ matches backend
        PaymentDate: new Date().toISOString(),
        PaymentStatus: "Paid",
        ForClaim: forClaim
      };

    const response = await axios.post(backendUrlPayments, payload, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json"
      }
    });

    return response.data;
  } catch (error) {
    console.error("Payment error:", error.response || error);
    throw error.response?.data || error.message || "Payment failed";
  }
};

// Fetch all payments
export const getAllPayments = async () => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(backendUrlPayments, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
  } catch (err) {
    throw err.response?.data || err.message || "Failed to fetch payments";
  }
};
export const updatePayment = async (paymentId, updatedData) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.put(
      `${backendUrlPayments}/${paymentId}`,
      updatedData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    return response.data; // contains Payment, BalanceAmount, and Status
  } catch (error) {
    console.error("Update payment failed:", error.response || error);
    throw error.response?.data || error.message || "Failed to update payment";
  }
};
export const deletePayment = async (paymentId) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.delete(`${backendUrlPayments}/${paymentId}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.status === 204 || response.data;
  } catch (error) {
    console.error("Delete payment failed:", error.response || error);
    throw error.response?.data || error.message || "Delete failed";
  }
};