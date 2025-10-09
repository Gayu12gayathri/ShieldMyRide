import React from "react";
import { useNavigate } from "react-router-dom";
import "./InsurancePolicies.css";
import car_ins from "../../assets/car_ins.png";
import two_wheeler from "../../assets/two_wheeler.png";
import truck from "../../assets/truck.png";
import thirdparty from "../../assets/third-party-motor.png";

const InsurancePolicies = () => {
  const navigate = useNavigate();

  const policies = [
    {
      id: 1,
      title: "Car Insurance",
      image: car_ins,
      description:
        "A car insurance plan provides financial protection and peace of mind for vehicle owners against potential accidents, damages, and liabilities.",
      highlights: [
        "Coverage for own damage",
        "Coverage for third-party liability",
        "Choice of add-ons",
        "Natural & manmade calamity cover",
      ],
      link: "/motor-insurance/car",
    },
    {
      id: 2,
      title: "Two-Wheeler Insurance",
      image: two_wheeler,
      description:
        "A comprehensive bike insurance plan ensures the security of the bike owners and the two-wheeler in case of loss or damage due to accidents, thefts, or third-party liabilities.",
      highlights: [
        "Coverage for own damage",
        "Coverage for third-party liability",
        "Coverage for natural & manmade risks",
        "Personalized plans",
      ],
      link: "/motor-insurance/bike",
    },
    {
      id: 3,
      title: "Commercial Vehicle Insurance",
      image: truck,
      description:
        "Get coverage for your commercial vehicles like trucks, taxis, vans, buses, etc., against various external damages such as theft, accidents, natural disasters, and liabilities.",
      highlights: [
        "Coverage for own damage & third-party liability",
        "Coverage for natural & manmade risks",
        "Personal accident coverage",
        "Choice of add-ons",
      ],
      link: "/motor-insurance/truck",
    },
    {
      id: 4,
      title: "Third-Party Motor Insurance",
      image: thirdparty,
      description:
        "Third-party motor insurance is mandatory in India and covers legal liabilities if your vehicle causes injury, death, or property damage to others. It protects you against financial losses arising from such accidents.",
      highlights: [
        "Covers injury or death to third party",
        "Covers third-party property damage",
        "Legal liability protection",
        "Does not cover own vehicle damage",
      ],
      link: "/motor-insurance/third-party",
    },
  ];

  // === Navigation Handlers ===
  const handleKnowMore = (path) => {
    navigate(path);
  };

const handleBuyPolicy = (policy) => {
  navigate(
    `/proposal?policyId=${policy.id}&policyName=${encodeURIComponent(policy.title)}&coverageAmount=${policy.basePremium || ""}`
  );
};



  return (
    <div className="insurance-policies">
      <h1 className="main-title">Types of Motor Insurance Policies</h1>

      <div className="policy-grid">
        {policies.map((policy) => (
          <div key={policy.id} className="policy-card">
            <div className="policy-header">
              <img src={policy.image} alt={policy.title} className="policy-icon" />
              <div className="policy-title-section">
                <h2 className="policy-title">{policy.title}</h2>
                <div className="policy-line"></div>
              </div>
            </div>

            <p className="policy-description">{policy.description}</p>

            <div className="policy-highlights">
              <h3>Policy Highlights</h3>
              <ul className="highlights-list">
                {policy.highlights.map((highlight, index) => (
                  <li key={index}>{highlight}</li>
                ))}
              </ul>
            </div>

            <div className="policy-actions">
              <button
                className="know-more-btn"
                onClick={() => handleKnowMore(policy.link)}
              >
                Know More
              </button>
              <button
                className="buy-policy-btn"
                onClick={() => handleBuyPolicy(policy)}
              >
                Buy a Policy
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default InsurancePolicies;
