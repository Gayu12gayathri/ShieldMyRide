import React, { useEffect, useState } from "react";
import "./PaymentModel.css";

export default function PaymentModal({ userId, proposalId, payment, onClose, onSubmit }) {
  const [form, setForm] = useState({
    cardholderName: "",
    cardNumber: "",
    expiryDate: "",
    cvv: "",
    transactionId: "",
    amountPaid: "",
    forClaim: false,
  });

  // Prefill when updating
  useEffect(() => {
    if (payment) {
      setForm({
        ...form,
        amountPaid: payment.amountPaid || "",
        transactionId: payment.transactionId || "",
        forClaim: payment.forClaim || false,
      });
    }
  }, [payment]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === "checkbox" ? checked : value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const paymentData = {
      userID: userId,
      proposalID: proposalId,
      transactionId: form.transactionId || "TXN-" + Date.now(),
      amountPaid: parseFloat(form.amountPaid),
      paymentDate: new Date().toISOString(),
      paymentStatus: "Paid",
      forClaim: form.forClaim,
    };
    onSubmit(paymentData);
  };

  return (
    <div className="paymodal-overlay">
      <div className="paymodal-container">
        <span className="paymodal-close" onClick={onClose}>
          ✖
        </span>

        <h3 className="paymodal-title">
          {payment ? "Update Payment" : "New Payment"}
        </h3>

        <form onSubmit={handleSubmit} className="paymodal-form">
          <input
            type="text"
            name="cardholderName"
            placeholder="Cardholder Name"
            value={form.cardholderName}
            onChange={handleChange}
            required
            className="paymodal-input"
          />
          <input
            type="text"
            name="cardNumber"
            placeholder="Card Number"
            value={form.cardNumber}
            onChange={handleChange}
            required
            className="paymodal-input"
          />
          <div className="paymodal-row">
            <input
              type="text"
              name="expiryDate"
              placeholder="MM/YY"
              value={form.expiryDate}
              onChange={handleChange}
              required
              className="paymodal-input paymodal-half"
            />
            <input
              type="text"
              name="cvv"
              placeholder="CVV"
              value={form.cvv}
              onChange={handleChange}
              required
              className="paymodal-input paymodal-half"
            />
          </div>
          <input
            type="text"
            name="transactionId"
            placeholder="Transaction ID"
            value={form.transactionId}
            onChange={handleChange}
            required
            className="paymodal-input"
          />
          <input
            type="number"
            name="amountPaid"
            placeholder="Amount Paid"
            value={form.amountPaid}
            onChange={handleChange}
            required
            className="paymodal-input"
          />

          <div className="paymodal-checkbox-container">
            <input
              type="checkbox"
              name="forClaim"
              checked={form.forClaim}
              onChange={handleChange}
              className="paymodal-checkbox"
            />
            <label>For claim</label>
          </div>

          <button type="submit" className="paymodal-submit">
            {payment ? "Update Payment" : "Add Card & Pay"}
          </button>
        </form>
      </div>
    </div>
  );
}