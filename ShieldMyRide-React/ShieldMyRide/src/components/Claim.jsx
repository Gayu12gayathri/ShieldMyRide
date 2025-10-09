import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Claim.css";
import claim from "../assets/bikeins.jpg"

export default function Claim() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [category, setCategory] = useState("");
  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    mobileNumber: "",
    policyNumber: "",
    vehicleNumber: "",
    engineNumber: "",
    captcha: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleContinue = () => {
    if (step === 3) {
      navigate("/user-claim", { state: formData });
    } else {
      setStep(step + 1);
    }
  };

  return (
    <div className="claim-container">

      {step === 1 && (
        <div className="claim-section">
  <div className="claim-image">
    {/* <img src="/claim.png" alt="Claim Process" /> */}
    <img src={claim} alt="ClaimProcess" />
  </div>

  <div className="claim-content">
    <h2>Make a Claim</h2>
    <p>
      At ShieldMyRide, we have a dedicated and experienced claims team aimed to
      deliver you a customer service experience that’s fast, fair, convenient,
      and transparent. Our expert assessors are here to support you every step
      of the way to ensure you receive the assistance you need.
    </p>
    <button className="primary-btn" onClick={handleContinue}>
      Register A Claim
    </button>
  </div>
</div>

      )}


      {step === 2 && (
        <div className="claim-category">
          <h3>Intimate Claim & Track Claim Status</h3>
          <p>Please select any one category and continue</p>

          <div className="category-options">
            {["Motor", "Others"].map((cat) => (
              <div
                key={cat}
                className={`option-card ${category === cat ? "selected" : ""}`}
                onClick={() => setCategory(cat)}
              >
                <span className="icon">🚗</span>
                <p>{cat}</p>
              </div>
            ))}
          </div>

          <button
            className="primary-btn"
            disabled={!category}
            onClick={handleContinue}
          >
            Continue
          </button>
        </div>
      )}


      {step === 3 && (
        <div className="policy-validation">
          <h3>Policy Validation</h3>
          <form
            onSubmit={(e) => {
              e.preventDefault();
              handleContinue();
            }}
          >
            <input
              type="text"
              name="fullName"
              placeholder="Full Name *"
              required
              value={formData.fullName}
              onChange={handleChange}
            />
            <input
              type="email"
              name="email"
              placeholder="Email *"
              required
              value={formData.email}
              onChange={handleChange}
            />
            <input
              type="tel"
              name="mobileNumber"
              placeholder="Mobile Number *"
              required
              value={formData.mobileNumber}
              onChange={handleChange}
            />
            <input
              type="text"
              name="policyNumber"
              placeholder="Policy Number *"
              required
              value={formData.policyNumber}
              onChange={handleChange}
            />
            <input
              type="text"
              name="vehicleNumber"
              placeholder="Vehicle Registration Number"
              value={formData.vehicleNumber}
              onChange={handleChange}
            />
            {/* <p className="or-divider">OR</p>
            <input
              type="text"
              name="engineNumber"
              placeholder="Engine No (Last 5 digits)"
              value={formData.engineNumber}
              onChange={handleChange}
            /> */}
            <div className="captcha-section">
              <label>Enter Captcha: 8 + 16 =</label>
              <input
                type="text"
                name="captcha"
                required
                value={formData.captcha}
                onChange={handleChange}
              />
            </div>

            <button type="submit" className="primary-btn">
              Continue
            </button>
          </form>
        </div>
      )}
    </div>
  );
}
