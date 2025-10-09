import React from "react";
import QuotePage from "../QuotePage.jsx";
import { useNavigate } from "react-router-dom";
import { FaClock, FaTruck, FaWarehouse } from "react-icons/fa";
import InsurancePolicies from "./InsurancePolicy.jsx";
import zero from "../../assets/carisnurance/zero.png"
import consumable from "../../assets/carisnurance/consumable.png"
import invoice from "../../assets/carisnurance/invoice.png"
import road from "../../assets/carisnurance/towing-vehicle.png"
import fire from "../../assets/icons/fire.png"
import accident from "../../assets/icons/accident.png"
import personal from "../../assets/icons/personal.png"
import theft from "../../assets/icons/theft.png"
import natural from "../../assets/icons/flood.png"
import loan from "../../assets/icons/third.png"; 
import carins from "../../assets/carisnurance/carins.png"
import  addons from "../../assets/carisnurance/addons.png"
import age from "../../assets/carisnurance/age.png"
import model from "../../assets/carisnurance/model.png"
import ncb from "../../assets/carisnurance/ncb.png"
import coverage from "../../assets/third-party-motor.png"
import safety from "../../assets/carisnurance/safety.png"
import carMain from "../../assets/carisnurance/carmain.png"
import truckins from "../../assets/truckins.png"

import TruckTypes from "./TruckInsuranceTypes.jsx"

export default function Truck() {
     const navigate = useNavigate();
  return (
    <div className="insurance-page">
    <section className="car-insurance-section">
      <div className="car-left">
        <img src={truckins} alt="Car Illustration" className="car-image" />

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
          <h1>Truck Insurance</h1>
          <p className="subtitle">Doorstep Cashless Repairs</p>
          <p className="subtitle">₹15 lakh Personal Accident Cover</p>
          <p className="subtitle">Upto 50% off with NCB</p>
            {/* Navigate to QuotePage on click */}
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
    <img src={truckins} alt="Truck Insurance" className="brand-logo" />
  </div>
  <div className="about-car-text">
    <h2>What is Truck Insurance?</h2>
    <p>
      Truck insurance is a legal contract between a policyholder (you) and an insurance company that protects you against financial loss in case of accidents, theft, or damages involving your truck. In India, having at least a third-party liability policy is mandatory under the Motor Vehicles Act, 1988. Comprehensive truck insurance provides broader coverage, including damage due to fire, natural disasters, and personal accident cover for the driver.
    </p>
  </div>
</div>

<section className="content-section">
  <h2>Why Choose Our Truck Insurance?</h2>
  <div className="ul-list-content">
    <ul>Instant policy issuance for your truck in minutes</ul>
    <ul>Cashless claim settlements at 4300+ network garages</ul>
    <ul>Completely digital process with zero paperwork</ul>
    <ul>24/7 roadside assistance and customer support</ul>
  </div>
</section>

<TruckTypes />

<section className="insurance-section">
  <h2><strong>Inclusions: Coverage Offered by Our Truck Insurance Policy</strong></h2>
  <p>Protect your truck and business operations with our insurance policy, offering the following benefits:</p>
  
  <div className="card-grid">
    <div className="card">
      <div className="card-header">
        <img src={theft} alt="Theft"/>
        <h3>Truck Theft</h3>
      </div>
      <p>Coverage against losses if your truck is stolen.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={accident} alt="Accident"/>
        <h3>Accidents or Collisions</h3>
      </div>
      <p>Covers repair or replacement costs for your truck in case of an accident.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={fire} alt="Fire"/>
        <h3>Fire Damage</h3>
      </div>
      <p>Damages caused due to accidental fire or engine-related incidents.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={personal} alt="Accident Cover"/>
        <h3>Driver Personal Accident Cover</h3>
      </div>
      <p>Compensation in case of injury or death of the truck driver.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={natural} alt="Natural Disaster"/>
        <h3>Natural Disaster Damage</h3>
      </div>
      <p>Protection against damages from floods, cyclones, earthquakes, and other natural calamities.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={loan} alt="Third Party"/>
        <h3>Third-Party Liabilities</h3>
      </div>
      <p>Covers property damages or injuries caused to third parties by your truck.</p>
    </div>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>Add-ons: Boost Your Truck Insurance Coverage</strong></h2>
  <p>Enhance your truck insurance with optional add-ons for extra protection.</p>
  
  <div className="card-grid">
    <div className="card">
      <div className="card-header">
        <img src={zero} alt="Zero depreciation"/>
        <h3>Zero Depreciation Cover</h3>
      </div>
      <p>Full claim amount without deduction for depreciation of truck parts.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={road} alt="Roadside Assistance"/>
        <h3>Roadside Assistance</h3>
      </div>
      <p>Immediate help in case of breakdowns or accidents during your truck journeys.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={consumable} alt="Consumables cover"/>
        <h3>Consumables Cover</h3>
      </div>
      <p>Covers repair or replacement of consumables like engine oil, filters, nuts, and screws.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={invoice} alt="Return invoice"/>
        <h3>Return to Invoice Cover</h3>
      </div>
      <p>Ensures you receive the original purchase price if your truck is stolen or totaled.</p>
    </div>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>What’s Not Covered Under Truck Insurance?</strong></h2>
  <div className="ul-list">
    <ul>❌ <strong>Substance use:</strong> Accidents caused due to alcohol or drugs are not covered.</ul>
    <ul>❌ <strong>Driving outside India:</strong> Damages occurring outside India are not covered.</ul>
    <ul>❌ <strong>Illegal Driving:</strong> Driving without a valid license or reckless driving is excluded.</ul>
    <ul>❌ <strong>Wear and tear:</strong> Normal wear and tear of truck parts is not covered.</ul>
    <ul>❌ <strong>War:</strong> Damage due to war, nuclear risk, or civil war situations is not covered.</ul>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>Factors Affecting Truck Insurance Premium</strong></h2>
  <div className="card-grid">
    <div className="card">
      <div className="card-header">
        <img src={model} alt="Truck Make & Model"/>
        <h3>Truck Make & Model</h3>
      </div>
      <p>The brand and model of your truck influence premium rates.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={age} alt="Truck Age"/>
        <h3>Truck Age</h3>
      </div>
      <p>Older trucks may have lower market value but higher repair costs, affecting premiums.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={coverage} alt="Types of Coverage"/>
        <h3>Types of Coverage</h3>
      </div>
      <p>Comprehensive coverage for accidents, fire, theft, and natural calamities costs more than basic third-party insurance.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={addons} alt="Add-ons"/>
        <h3>Add-ons</h3>
      </div>
      <p>Add-ons like zero depreciation, roadside assistance, or invoice cover affect premiums.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={ncb} alt="No Claim Bonus"/>
        <h3>No Claim Bonus</h3>
      </div>
      <p>Discounts are provided if no claims are made during the policy period.</p>
    </div>
    <div className="card">
      <div className="card-header">
        <img src={safety} alt="Safety Features"/>
        <h3>Safety Features</h3>
      </div>
      <p>Anti-theft devices, GPS trackers, and reinforced safety features can reduce your premium.</p>
    </div>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>How To Choose The Best Truck Insurance Policy Online</strong></h2>
  <div className="card-grid">
    <div className="card">
      <h3>Check Coverage Options</h3>
      <p>Ensure coverage includes third-party liability, own damage, theft, fire, and natural disasters. Consider add-ons for extra protection.</p>
    </div>
    <div className="card">
      <h3>Check Insurer's Reputation</h3>
      <p>Look for insurers with high claim settlement ratios and positive reviews.</p>
    </div>
    <div className="card">
      <h3>Analyse Discounts and Benefits</h3>
      <p>Check for discounts for GPS tracking, safe driving history, or higher deductibles to reduce premiums.</p>
    </div>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>How To Download Your Truck Insurance Policy Copy</strong></h2>
  <div className="ul-list">
    <ul>1️⃣ Log in to your insurer’s website or app</ul>
    <ul>2️⃣ Navigate to 'My Policies' or 'Download Policy'</ul>
    <ul>3️⃣ Enter truck details or OTP if required</ul>
    <ul>4️⃣ Download and save the PDF copy of your policy</ul>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>FAQs About Truck Insurance Policy</strong></h2>
  <div className="ul-list">
    <ul><strong>Q:</strong> Does the truck's region affect the premium?<br/><strong>A:</strong> Yes, trucks operating in high-risk areas may have higher premiums.</ul>
    <ul><strong>Q:</strong> Can I retain No Claim Bonus if I switch insurers?<br/><strong>A:</strong> Yes, with proof from your previous insurer.</ul>
    <ul><strong>Q:</strong> Is vehicle inspection always required?<br/><strong>A:</strong> Depends on truck age and policy type.</ul>
    <ul><strong>Q:</strong> Does insurance cover natural disasters?<br/><strong>A:</strong> Yes, if you have comprehensive coverage.</ul>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>Conclusion</strong></h2>
  <p>
    Choosing the right truck insurance policy requires understanding coverage, evaluating premiums, and considering add-ons. Our truck insurance policies provide comprehensive protection for your vehicle and business.
  </p>
</section>

     
    </div>
  );
}
