import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../motorinsurance/CarInsuranceTypes.css";
import compcarpolicy from "../../assets/comp-car-policy.png";
import thirdparty from "../../assets/third-party-motor.png";
import zero from "../../assets/carisnurance/zero.png";
import alone from "../../assets/alone.png";
import { useNavigate } from "react-router-dom";

const carInsuranceData = [
  {
    id: 1,
    title: "Comprehensive Car Insurance Policy",
    image: compcarpolicy,
    description: `Comprehensive car insurance provides complete protection for your vehicle. 
    It covers damages caused by accidents, theft, natural disasters, and third-party liabilities.`,
    basePremium: 5000,
  },
  {
    id: 2,
    title: "Third-Party Car Insurance Policy",
    image: thirdparty,
    description: `Third-party car insurance covers damages or injuries you cause to another person 
    or their property.`,
    basePremium: 3000,
  },
  {
    id: 3,
    title: "Own Damage Car Insurance Policy",
    image: alone,
    description: `Standalone Own Damage insurance covers your vehicle against damages 
    from accidents, natural disasters, or theft.`,
    basePremium: 4000,
  },
  {
    id: 4,
    title: "Pay As You Drive Policy",
    image: zero,
    description: `This policy allows you to pay premiums based on the kilometers you drive.`,
    basePremium: 2500,
  },
];

export default function CarInsuranceTypes() {
  const [index, setIndex] = useState(0);
  const navigate = useNavigate();

  const prevSlide = () => setIndex((prev) => (prev > 0 ? prev - 1 : prev));
  const nextSlide = () =>
    setIndex((prev) =>
      prev < carInsuranceData.length - 1 ? prev + 1 : prev
    );

  const handleBuyPolicy = (policy) => {
    navigate(
      `/proposal?policyId=${policy.id}&policyName=${encodeURIComponent(
        policy.title
      )}&coverageAmount=${policy.basePremium}`
    );
  };

  const currentPolicy = carInsuranceData[index];

  return (
    <section className="car-insurance-section py-5">
      <div className="container text-center">
        <h2 className="section-title mb-5">
          What are the different types of car insurance policies?
        </h2>

        <div className="insurance-wrapper position-relative d-flex justify-content-center align-items-center">
          {/* Left Arrow */}
          {index > 0 && (
            <button className="arrow-btn left" onClick={prevSlide}>
              &#10094;
            </button>
          )}

          {/* Insurance Card */}
          <div className="insurance-card d-flex align-items-center shadow rounded-4 p-4 bg-white">
            <div className="insurance-image me-4">
              <img
                src={currentPolicy.image}
                alt={currentPolicy.title}
                className="img-fluid rounded-3"
                width="200"
              />
            </div>

            <div className="insurance-content text-start">
              <h4 className="fw-bold">{currentPolicy.title}</h4>
              <p>{currentPolicy.description}</p>
              <p className="fw-semibold">
                Estimated Premium: ₹{currentPolicy.basePremium.toLocaleString()}
              </p>
              <button
                className="buy-policy-btn"
                onClick={() => handleBuyPolicy(currentPolicy)}
              >
                Buy a Policy
              </button>
            </div>
          </div>

          {/* Right Arrow */}
          {index < carInsuranceData.length - 1 && (
            <button className="arrow-btn right" onClick={nextSlide}>
              &#10095;
            </button>
          )}
        </div>
      </div>
    </section>
  );
}
