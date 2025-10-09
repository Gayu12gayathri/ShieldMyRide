import React from "react";
import { useNavigate } from "react-router-dom";
import QuotePage from "../QuotePage.jsx";
import { FaClock, FaMotorcycle, FaWarehouse } from "react-icons/fa";

import bikeMain from "../../assets/bikeins.jpg";
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
import addons from "../../assets/carisnurance/addons.png";
import age from "../../assets/carisnurance/age.png";
import model from "../../assets/carisnurance/model.png";
import ncb from "../../assets/carisnurance/ncb.png";
import scooter from "../../assets/bike.svg"
import coverage from "../../assets/third-party-motor.png";
import safety from "../../assets/carisnurance/safety.png";
import TwoWheelerTypes from "./TwoWheelerTypes.jsx";

export default function TwoWheeler() {
  const navigate = useNavigate();

  return (
    <div className="insurance-page">
      {/* Hero Section */}
      <section className="car-insurance-section">
        <div className="car-left">
          <img src={bikeMain} alt="Bike Illustration" className="car-image" />

          <div className="car-features">
            <div className="feature-item">
              <FaMotorcycle className="icon" />
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
          <h1>Two-Wheeler Insurance</h1>
          <p className="subtitle">Doorstep Cashless Repairs</p>
          <p className="subtitle">₹15 Lakh Personal Accident Cover</p>
          <p className="subtitle">Up to 50% off with NCB</p>
          <button
            className="get-quote-btn"
            onClick={() => navigate("/quote")}
          >
            GET QUOTE
          </button>
        </div>
      </section>

      {/* About Section */}
      <div className="about-car-section">
         <div className="about-car-image">
                 <img src={scooter} alt="Vehicle Insurance" className="brand-logo" />
             </div>
        <div className="about-car-text">
          <h2>What is Two-Wheeler Insurance?</h2>
          <p>
            Two-wheeler insurance is a policy that protects you and your bike against financial losses caused by accidents, theft, or natural calamities. 
            It is mandatory to have at least third-party liability cover under the Motor Vehicles Act. 
            Comprehensive policies provide broader protection, including personal accident cover, damage to your bike, and add-on benefits like zero depreciation and roadside assistance.
          </p>
        </div>
      </div>

      {/* Why Choose Section */}
      <section className="content-section">
        <h2>Why Choose Our Two-Wheeler Insurance?</h2>
        <div className="ul-list-content">
          <ul>Instant policy issuance and renewals</ul>
          <ul>Cashless claim settlements at 11,800+ garages</ul>
          <ul>Zero paperwork with complete digital process</ul>
          <ul>24/7 roadside assistance and customer support</ul>
        </div>
      </section>

      {/* Types Section */}
      <TwoWheelerTypes />{/* Similar to CarInsuranceTypes.jsx but for bikes */}

      {/* Inclusions Section */}
      <section className="insurance-section">
        <h2><strong>Inclusions: Coverage Offered by Our Two-Wheeler Insurance Policy</strong></h2>
        <p>Protect your bike and yourself with the following benefits:</p>
        <div className="card-grid">
          <div className="card">
            <div className="card-header">
              <img src={theft} alt="Theft" />
              <h3>Bike Theft</h3>
            </div>
            <p>Covers losses incurred when your bike is stolen.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={accident} alt="Accident" />
              <h3>Accidents</h3>
            </div>
            <p>Covers repair or replacement costs after an accident.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={fire} alt="Fire" />
              <h3>Fire Damage</h3>
            </div>
            <p>Protection against accidental fire damage.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={personal} alt="Personal Accident" />
              <h3>Personal Accident Cover</h3>
            </div>
            <p>Compensation in case of injury or death of the rider.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={natural} alt="Natural Disaster" />
              <h3>Natural Disaster Damage</h3>
            </div>
            <p>Protection against damages due to floods, storms, and other natural disasters.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={loan} alt="Third Party" />
              <h3>Third-Party Liabilities</h3>
            </div>
            <p>Covers property damage and injury caused to others.</p>
          </div>
        </div>
      </section>

      {/* Add-ons Section */}
      <section className="insurance-section">
        <h2><strong>Add-ons: Boost Your Two-Wheeler Insurance Coverage</strong></h2>
        <p>Enhance protection with optional add-on covers:</p>
        <div className="card-grid">
          <div className="card">
            <div className="card-header">
              <img src={zero} alt="Zero Depreciation" />
              <h3>Zero Depreciation Cover</h3>
            </div>
            <p>Full claim amount without depreciation deductions.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={road} alt="Roadside Assistance" />
              <h3>Roadside Assistance</h3>
            </div>
            <p>Immediate help during breakdowns or emergencies.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={consumable} alt="Consumables Cover" />
              <h3>Consumables Cover</h3>
            </div>
            <p>Covers replacement of consumables like nuts, screws, oils, etc.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={invoice} alt="Invoice Cover" />
              <h3>Return to Invoice</h3>
            </div>
            <p>Receive the original invoice value if your bike is stolen or totaled.</p>
          </div>
        </div>
      </section>

      <section className="insurance-section">
          <h2><strong>What’s Not Covered Under Bike Insurance?</strong></h2><br/>
          <div className="ul-list">
          <ul>❌ <strong>Substance use:</strong>  If you get into an accident because of alcoholism or drugs, such events will not be covered under your car insurance plan.</ul>

          <ul>❌ <strong>Driving outside India: </strong> If you are driving outside the Indian region and any mishap occurs, that loss or damage will not be covered.</ul>

          <ul>❌ <strong>Illegal Driving:</strong>  Driving without a valid Driving License or indulging in speed racing, crash testing, etc., is not covered.</ul>

          <ul>❌ <strong>Wear and tear:</strong>  Normal wear and tear that occurs with time is not covered.</ul>

          <ul>❌ <strong>War: </strong> Any damage caused to your car due to war, nuclear risk, or civil war-like situations will not be covered under your car insurance plan.</ul>
          </div>
        </section>

      {/* Factors Affecting Premium */}
      <section className="insurance-section">
        <h2><strong>Factors Affecting Two-Wheeler Insurance Premium</strong></h2>
        <div className="card-grid">
          <div className="card">
            <div className="card-header">
              <img src={model} alt="Bike Model" />
              <h3>Bike Make & Model</h3>
            </div>
            <p>Premium varies depending on the bike’s brand, model, and market value.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={age} alt="Bike Age" />
              <h3>Bike Age</h3>
            </div>
            <p>Older bikes generally have lower premiums compared to newer models.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={coverage} alt="Coverage Type" />
              <h3>Coverage Type</h3>
            </div>
            <p>Comprehensive policies cost more than third-party liability plans.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={addons} alt="Add-ons" />
              <h3>Add-ons</h3>
            </div>
            <p>Optional covers enhance protection and may affect premium.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={ncb} alt="No Claim Bonus" />
              <h3>No Claim Bonus</h3>
            </div>
            <p>Discount for claim-free policy periods.</p>
          </div>
          <div className="card">
            <div className="card-header">
              <img src={safety} alt="Safety Features" />
              <h3>Safety Features</h3>
            </div>
            <p>Anti-theft and safety devices reduce premiums.</p>
          </div>
        </div>
      </section>
       <section className="insurance-section">
          <h2><strong>How To Choose The Best Two-Wheeler Insurance Policy Online</strong></h2>
          <div className="card-grid">
          <div className="card">
              <h3>Check Coverage Options</h3>
              <p>Always look for a policy that offers extensive coverage, including third-party liability, own damage, theft, and natural calamities. A good policy should also cover add-ons like zero depreciation, engine protection, and roadside assistance to provide additional protection.</p>
            </div>
            <div className="card">
              <h3>Check Insurer's Reputation</h3>
              <p>Always check to see if the insurer has a good claim settlement ratio and good customer reviews. A high claim settlement ratio indicates reliability and efficiency in processing claims, which is very important in times of need.</p>
            </div>
            <div className="card">
              <h3>Analyse Discounts and Benefits</h3>
              <p>Many insurers offer discounts for installing anti-theft devices, maintaining a good driving record, or going for a higher deductible. Take advantage of these benefits to reduce your premium.</p>
            </div>
            </div>
        </section>
        <section className="insurance-section">
          <h2><strong>How To Download Your Bike Insurance Policy Copy</strong></h2>
          <div className="ul-list">
            <ul>1️⃣ Log in to your insurer’s website or app</ul>
            <ul>2️⃣ Navigate to 'My Policies' or 'Download Policy'</ul>
            <ul>3️⃣ Enter vehicle details or OTP if required</ul>
            <ul>4️⃣ Download and save the PDF copy of your policy</ul>
          </div>
        </section>

      {/* FAQs */}
      <section className="insurance-section">
        <h2><strong>FAQs About Two-Wheeler Insurance</strong></h2>
        <div className="ul-list">
          <ul><strong>Q:</strong> Does bike model affect the premium?<br/><strong>A:</strong> Yes, high-end models have higher premiums.</ul>
          <ul><strong>Q:</strong> Can I retain NCB if I switch insurer?<br/><strong>A:</strong> Yes, proof from previous insurer is needed.</ul>
          <ul><strong>Q:</strong> Is vehicle inspection always required?<br/><strong>A:</strong> Depends on the bike’s age and renewal history.</ul>
          <ul><strong>Q:</strong> Does insurance cover natural disasters?<br/><strong>A:</strong> Yes, if you have comprehensive coverage.</ul>
        </div>
      </section>

      {/* Conclusion */}
      <section className="insurance-section">
        <h2><strong>Conclusion</strong></h2>
        <p>
          Choosing the right two-wheeler insurance policy requires understanding coverage, evaluating premiums, 
          and considering add-ons. Our blue-themed two-wheeler insurance policies provide comprehensive 
          protection and peace of mind.
        </p>
      </section>
    </div>
  );
}
