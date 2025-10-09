import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../motorinsurance/CarInsuranceTypes.css";
import thirdparty from "../../assets/third-party-motor.png";
import commercial from "../../assets/comp-car-policy.png";
import twoWheeler from "../../assets/scooter.png";
import { useNavigate } from "react-router-dom";

const thirdPartyPlans = [
  {
    id: 1,
    title: "Private Car Third-Party Liability Policy",
    image: thirdparty,
    description: `This policy covers any legal liability arising due to injury or damage caused to a third party 
    by your private car. It’s mandatory under the Motor Vehicles Act, 1988.`,
    basePremium: 2800,
  },
  {
    id: 2,
    title: "Two-Wheeler Third-Party Liability Policy",
    image: twoWheeler,
    description: `Provides coverage for bodily injury or death to a third person and property damage caused 
    by your two-wheeler. It’s a legal requirement for all bike owners.`,
    basePremium: 1200,
  },
  {
    id: 3,
    title: "Commercial Vehicle Third-Party Insurance",
    image: commercial,
    description: `Ideal for taxis, trucks, or delivery vans. Covers third-party liabilities such as injury, 
    death, or property damage caused by a commercial vehicle.`,
    basePremium: 5000,
  },
];

export default function ThirdPartyInsurance() {
  const [index, setIndex] = useState(0);
  const navigate = useNavigate();

  const prevSlide = () => setIndex((prev) => (prev > 0 ? prev - 1 : prev));
  const nextSlide = () =>
    setIndex((prev) =>
      prev < thirdPartyPlans.length - 1 ? prev + 1 : prev
    );

  const handleBuyPolicy = (policy) => {
    navigate(
      `/proposal?policyId=${policy.id}&policyName=${encodeURIComponent(
        policy.title
      )}&coverageAmount=${policy.basePremium}`
    );
  };

  const currentPolicy = thirdPartyPlans[index];

  return (
    <section className="car-insurance-section py-5">
      <div className="container text-center">
        <h2 className="section-title mb-5">
          Real-Time Third-Party Motor Insurance Plans
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
                Buy Policy
              </button>
            </div>
          </div>

          {/* Right Arrow */}
          {index < thirdPartyPlans.length - 1 && (
            <button className="arrow-btn right" onClick={nextSlide}>
              &#10095;
            </button>
          )}
        </div>
      </div>
    </section>
  );
}
