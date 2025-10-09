import React from "react";
import QuotePage from "../QuotePage.jsx";
import { useNavigate } from "react-router-dom";
import { FaClock, FaTruck, FaWarehouse } from "react-icons/fa";
import InsurancePolicies from "./InsurancePolicy.jsx";
import zero from "../../assets/carisnurance/zero.png";
import consumable from "../../assets/carisnurance/consumable.png";
import invoice from "../../assets/carisnurance/invoice.png";
import road from "../../assets/carisnurance/towing-vehicle.png";
import fire from "../../assets/icons/fire.png";
import accident from "../../assets/icons/accident.png";
import personal from "../../assets/icons/personal.png";
import theft from "../../assets/icons/theft.png";
import natural from "../../assets/icons/flood.png";
import loan from "../../assets/icons/third.png";
import carins from "../../assets/thirdabout.webp";
import addons from "../../assets/carisnurance/addons.png";
import age from "../../assets/carisnurance/age.png";
import model from "../../assets/carisnurance/model.png";
import ncb from "../../assets/carisnurance/ncb.png";
import coverage from "../../assets/third-party-motor.png";
import safety from "../../assets/carisnurance/safety.png";
import carMain from "../../assets/carisnurance/carmain.png";
import third from "../../assets/Thirdparty.jpg";

import ThirdPartyInsurance from "./ThirdTypes.jsx";

export default function ThirdParty() {
  const navigate = useNavigate();
  return (
    <div className="insurance-page">
      <section className="car-insurance-section">
        <div className="car-left">
          <img src={third} alt="Car Illustration" className="car-image" />

          <div className="car-features">
            <div className="feature-item">
              <FaTruck className="icon" />
              <div>
                <h4>Roadside Assistance</h4>
                <p>Included</p>
              </div>
            </div>
            <div className="feature-item">
              <FaWarehouse className="icon" />
              <div>
                <h4>11,800+</h4>
                <p>Cashless Garages</p>
              </div>
            </div>
            <div className="feature-item">
              <FaClock className="icon" />
              <div>
                <h4>4 Hour</h4>
                <p>Quick Claim Settlement</p>
              </div>
            </div>
          </div>
        </div>
        <div className="hero-text">
          <h1>Third-Party Motor Insurance</h1>
          <p className="subtitle">Legal liability coverage for third parties</p>
          <p className="subtitle">Covers death, injury or property damage caused to third parties</p>
          <p className="subtitle">Mandatory as per Motor Vehicles Act, 1988</p>
          <button
            className="get-quote-btn"
            onClick={() => navigate("/quote")}
          >
            GET QUOTE
          </button>
        </div>
      </section>

      <div className="about-car-section">
        <div className="about-car-image">
          <img src={carins} alt="Vehicle Insurance" className="brand-logo" />
        </div>
        <div className="about-car-text">
          <h2>What is Third-Party Motor Insurance?</h2>
          <p>
            Third-party motor insurance protects you from legal and financial liabilities
            towards third parties (other people or their property) arising from an accident
            caused by your vehicle. It covers costs such as bodily injury, death, or property
            damage inflicted on third parties. This is the minimum mandatory insurance required
            under the Motor Vehicles Act, 1988 in India.
          </p>
        </div>
      </div>

      <section className="content-section">
        <h2>Why Choose Our Third-Party Insurance?</h2>
        <div className="ul-list-content">
          <ul>Instant issuance of policy online</ul>
          <ul>No cashless repair (since it's liability cover only)</ul>
          <ul>Transparent pricing and no hidden charges</ul>
          <ul>24/7 support for third-party claims</ul>
        </div>
      </section>

      <ThirdPartyInsurance />

      <section className="insurance-section">
        <h2><strong>Coverage Under Third-Party Insurance</strong></h2>
        <p>
          Our third-party cover provides you protection for legal liabilities arising
          from an accident. It includes:
        </p>
        <div className="card-grid">
          <div className="card">
            <div className="card-header">
              <img src={personal} alt="Bodily Injury / Death" />
              <h3>Third-Party Bodily Injury / Death</h3>
            </div>
            <p>
              Compensation or legal liability arising from injury or death caused to
              a third party. 
            </p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={theft} alt="Property Damage" />
              <h3>Third-Party Property Damage</h3>
            </div>
            <p>
              Liability for damage to third-party property (e.g. vehicle, structure) caused
              by your vehicle. 
            </p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={natural} alt="Other Losses" />
              <h3>Additional Legal Costs</h3>
            </div>
            <p>
              Court fees, legal expenses, or litigation costs incurred while defending
              third-party claims.
            </p>
          </div>
        </div>
      </section>

      <section className="insurance-section">
        <h2><strong>What’s Not Covered Under Third-Party Insurance?</strong></h2><br />
        <div className="ul-list">
          <ul>❌ <strong>Damage to your own vehicle:</strong> Losses incurred by your own vehicle are not covered.</ul>
          <ul>❌ <strong>Theft or total loss of your vehicle:</strong> This liability-only policy does not provide coverage for theft or total loss.</ul>
          <ul>❌ <strong>Personal injury to the driver:</strong> Injuries sustained by the driver at fault are not covered under third-party cover.</ul>
          <ul>❌ <strong>Natural calamities to your own vehicle:</strong> Damage to your own vehicle due to natural disasters is excluded.</ul>
        </div>
      </section>

      <section className="insurance-section">
        <h2><strong>Factors Affecting Premium of Third-Party Insurance</strong></h2>
        <div className="card-grid">
          <div className="card">
            <div className="card-header">
              <img src={model} alt="Vehicle Make & Model" />
              <h3>Vehicle Make & Model</h3>
            </div>
            <p>The make, model, and specification of your vehicle influence the risk and hence the premium.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={age} alt="Vehicle Age" />
              <h3>Vehicle Age</h3>
            </div>
            <p>Older vehicles generally have lower third-party premiums compared to new or high-end ones.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={coverage} alt="Coverage Limit" />
              <h3>Sum Insured / Coverage Limit</h3>
            </div>
            <p>The extent of liability coverage (limits) chosen will influence the premium.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={ncb} alt="No Claim Bonus" />
              <h3>No Claim Bonus</h3>
            </div>
            <p>If no claims are made in the previous term, you may be eligible for a discount on renewal premiums.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={safety} alt="Safety Features" />
              <h3>Safety & Anti-theft Features</h3>
            </div>
            <p>Vehicles equipped with safety devices or anti-theft mechanisms can attract lower premiums.</p>
          </div>
        </div>
      </section>

      <section className="insurance-section">
        <h2><strong>How to Claim Under Third-Party Insurance</strong></h2>
        <div className="ul-list">
          <ul>1️⃣ File an FIR at the nearest police station immediately after the accident.</ul>
          <ul>2️⃣ Inform the insurer and lodge a third-party claim with required documents.</ul>
          <ul>3️⃣ Cooperate with investigation, provide evidence, and follow the legal process.</ul>
          <ul>4️⃣ The insurer will settle the claim up to the legal liability amount once validated.</ul>
        </div>
      </section>

      <section className="insurance-section">
        <h2><strong>FAQs About Third-Party Motor Insurance</strong></h2>
        <div className="ul-list">
          <ul>
            <strong>Q:</strong> Is third-party insurance mandatory?<br />
            <strong>A:</strong> Yes, every motor vehicle on road must have a valid third-party liability insurance as per the Motor Vehicles Act, 1988.
          </ul>
          <ul>
            <strong>Q:</strong> Does this policy cover damage to my car?<br />
            <strong>A:</strong> No — it only covers liabilities arising to third parties, not damage to your own vehicle.
          </ul>
          <ul>
            <strong>Q:</strong> Can I get a “cashless repair” benefit under this policy?<br />
            <strong>A:</strong> No, cashless repairs are not applicable — this is a liability cover only.
          </ul>
        </div>
      </section>

      <section className="insurance-section">
        <h2><strong>Conclusion</strong></h2>
        <p>
          A third-party motor insurance policy is a mandatory, liability-only cover to protect you
          from legal risks arising from harm caused to others by your vehicle. To safeguard yourself and
          comply with the law, choose a reliable insurer with transparent claim handling and solid reputation.
        </p>
      </section>
    </div>
  );
}
