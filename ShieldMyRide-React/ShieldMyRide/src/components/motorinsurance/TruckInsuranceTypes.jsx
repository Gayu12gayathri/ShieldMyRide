import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../motorinsurance/CarInsuranceTypes.css";
import { useNavigate } from "react-router-dom";

// Images/icons for truck insurance types
import compTruck from "../../assets/comp-car-policy.png";
import thirdPartyTruck from "../../assets/third-party-motor.png";
import goodsCarry from "../../assets/third-party-motor.png";
import ownDamageTruck from "../../assets/third-party-motor.png";

const truckInsuranceData = [
  {
    id: 1,
    title: "Comprehensive Truck Insurance",
    image: compTruck,
    description: `Complete protection for your truck including accident, theft, fire, natural calamities, and third-party liabilities.`,
    basePremium: 10000,
  },
  {
    id: 2,
    title: "Third-Party Truck Insurance",
    image: thirdPartyTruck,
    description: `Covers damages or injuries caused to third parties. Mandatory by law.`,
    basePremium: 5000,
  },
  {
    id: 3,
    title: "Goods Carrying Vehicle Insurance",
    image: goodsCarry,
    description: `For trucks used to carry goods commercially. Protects your cargo and vehicle against loss or damage.`,
    basePremium: 12000,
  },
  {
    id: 4,
    title: "Standalone Own Damage Cover",
    image: ownDamageTruck,
    description: `Covers damage to your truck due to accidents, fire, or natural disasters, excluding third-party liabilities.`,
    basePremium: 9000,
  },
];

export default function TruckTypes() {
  const [index, setIndex] = useState(0);
  const navigate = useNavigate();

  const prevSlide = () => setIndex((prev) => (prev > 0 ? prev - 1 : prev));
  const nextSlide = () =>
    setIndex((prev) =>
      prev < truckInsuranceData.length - 1 ? prev + 1 : prev
    );

  const handleBuyPolicy = (policy) => {
    navigate(
      `/proposal?policyId=${policy.id}&policyName=${encodeURIComponent(
        policy.title
      )}&coverageAmount=${policy.basePremium}`
    );
  };

  const currentPolicy = truckInsuranceData[index];

  return (
    <section className="car-insurance-section py-5">
      <div className="container text-center">
        <h2 className="section-title mb-5">
          Types of Commercial Vehicle Insurance Policies
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
          {index < truckInsuranceData.length - 1 && (
            <button className="arrow-btn right" onClick={nextSlide}>
              &#10095;
            </button>
          )}
        </div>
      </div>
    </section>
  );
}
