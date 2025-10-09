import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../motorinsurance/CarInsuranceTypes.css";
import compBike from "../../assets/comp-car-policy.png";
import thirdPartyBike from "../../assets/third-party-motor.png";
import ownDamageBike from "../../assets/alone.png";
import { useNavigate } from "react-router-dom";

const bikeInsuranceData = [
  {
    id: 1,
    title: "Comprehensive Bike Insurance",
    image: compBike,
    description: `Provides complete protection for your bike including damages from accidents, theft, natural disasters, and third-party liabilities.`,
    basePremium: 3000,
  },
  {
    id: 2,
    title: "Third-Party Bike Insurance",
    image: thirdPartyBike,
    description: `Covers damages or injuries you cause to another person or their property. Mandatory by law.`,
    basePremium: 1500,
  },
  {
    id: 3,
    title: "Standalone Own Damage Cover",
    image: ownDamageBike,
    description: `Covers damages to your own bike due to accidents, theft, fire, or natural disasters, excluding third-party liabilities.`,
    basePremium: 2500,
  },
];

export default function TwoWheelerTypes() {
  const [index, setIndex] = useState(0);
  const navigate = useNavigate();

  const prevSlide = () => setIndex((prev) => (prev > 0 ? prev - 1 : prev));
  const nextSlide = () =>
    setIndex((prev) =>
      prev < bikeInsuranceData.length - 1 ? prev + 1 : prev
    );

  const handleBuyPolicy = (policy) => {
    navigate(
      `/proposal?policyId=${policy.id}&policyName=${encodeURIComponent(
        policy.title
      )}&coverageAmount=${policy.basePremium}`
    );
  };

  const currentPolicy = bikeInsuranceData[index];

  return (
    <section className="car-insurance-section py-5">
      <div className="container text-center">
        <h2 className="section-title mb-5">
          Types of Bike Insurance Plans Available in India
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
          {index < bikeInsuranceData.length - 1 && (
            <button className="arrow-btn right" onClick={nextSlide}>
              &#10095;
            </button>
          )}
        </div>
      </div>
    </section>
  );
}
