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
import CarInsuranceTypes from "./CarInsurancetypes.jsx";

export default function CarInsurance() {
     const navigate = useNavigate();
  return (
    <div className="insurance-page">
    <section className="car-insurance-section">
      <div className="car-left">
        <img src={carMain} alt="Car Illustration" className="car-image" />

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
          <h1>Car Insurance</h1>
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
      {/* <div className="car-right">
        <h1 className="main-heading">Car Insurance</h1>
        <p>Doorstep Cashless Repairs<sup>H</sup></p>
        <p>₹15 lakh Personal Accident Cover</p>
        <p>Upto 50% off with NCB<sup>F</sup></p>

        <div className="quote-box">
          <QuotePage />
        </div>
      </div> */}
    </section>
      <div className="about-car-section">
        <div className="about-car-image">
                 <img src={carins} alt="Vehicle Insurance" className="brand-logo" />
             </div>
             <div className="about-car-text">
                 <h2>What is car insurance?</h2>
                 <p>
                 Car insurance is a legal contract between a policyholder (you) and an insurance company that protects you against financial loss in the event of an accident or theft. In India, having at least a third-party liability policy is mandatory under the Motor Vehicles Act, 1988. Driving without valid insurance can lead to significant legal penalties. While third-party insurance is mandatory, comprehensive car insurance policy offers broader protection against accidents, theft, natural disasters, and even personal injury.
                 </p>
             </div>
             
         </div>

      <section className="content-section">
        <h2>Why Choose Our Car Insurance?</h2>
        <div className="ul-list-content">
          <ul>Instant policy issuance in minutes</ul>
          <ul>Cashless claim settlements at 4300+ network garages</ul>
          <ul>Zero paper hassle with complete digital process</ul>
          <ul>24/7 roadside assistance and customer support</ul>
        </div>
      </section>

      <CarInsuranceTypes/>
      {/* <InsurancePolicies/> */}
      {/* <!-- ========== Inclusions Section ========== --> */}
          <section className="insurance-section">
          <h2><strong>Inclusions: Coverage Offered by Our Car Insurance Policy</strong></h2>
          <p>Get your vehicle covered with our Motor Vehicle Insurance policy and secure your peace of mind with the following benefits -</p>
          
          <div className="card-grid">
              <div className="card">
                <div className="card-header">
              <img src={theft} alt="Theft"/>
              <h3>Vehicle Theft</h3>
              </div>
              <p>Coverage against losses incurred when your car is stolen.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={accident} alt="Accident"/>
              <h3>Accidents or Collisions</h3>
              </div>
              <p>Covers repair or replacement costs for your car in case of an accident.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={fire} alt="Fire"/>
              <h3>Fire Damage</h3>
              </div>
              <p>Damages and losses caused due to an accidental fire.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={personal} alt="Accident Cover"/>
              <h3>Personal Accident Cover</h3>
              </div>
              <p>Compensation of up to Rs. 15 lakhs is given in case of injury or death of the owner of the insured vehicle.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={natural} alt="Natural Disaster"/>
              <h3>Natural Disaster Damage</h3>
              </div>
              <p>Damages and losses due to acts of nature such as floods, cyclones, etc.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={loan} alt="Third Party"/>
              <h3>Third-Party Losses</h3>
              </div>
              <p>Pays for property damages and injuries caused by your car.</p>
              </div>
          </div>
          </section>
      
      {/* <!-- ========== Add-ons Section ========== --> */}
          <section className="insurance-section">
          <h2><strong>Add-ons: Boost Your Car Insurance Coverage</strong></h2>
          <p>Choose from a range of add-on covers to increase the protection of your plan.</p>
          
          <div className="card-grid">
              <div className="card">
                <div className="card-header">
              <img src={zero} alt="Zero depreciation"/>
              <h3>Zero Depreciation Cover</h3>
              </div>
              <p>Compensates for the claim amount getting deducted due to depreciation because of the car's age.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={road} alt="Roadside Assistance"/>
              <h3>Roadside Assistance</h3>
              </div>
              <p>Get immediate help in case of a breakdown or accident,ensuring you are never stranded.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={consumable} alt="Consumables cover"/>
              <h3>Consumables cover</h3>
              </div>
              <p>Covers expenses for repairing or replacement of consumables like nuts, screws, engine oil, oil filter, washer etc.</p>
              </div>
              <div className="card">
                <div className="card-header">
              <img src={invoice} alt="Return invoice"/>
              <h3>Return to Invoice Cover</h3>
              </div>
              <p>In case your vehicle is stolen or declared a total loss, this add-on cover ensures that you get the price you paid for the vehicle according to the original invoice rather than its current market value.</p>
              </div>
          </div>
          </section>

           <section className="insurance-section">
          <h2><strong>What’s Not Covered Under Car Insurance?</strong></h2><br/>
          <div className="ul-list">
          <ul>❌ <strong>Substance use:</strong>  If you get into an accident because of alcoholism or drugs, such events will not be covered under your car insurance plan.</ul>

          <ul>❌ <strong>Driving outside India: </strong> If you are driving outside the Indian region and any mishap occurs, that loss or damage will not be covered.</ul>

          <ul>❌ <strong>Illegal Driving:</strong>  Driving without a valid Driving License or indulging in speed racing, crash testing, etc., is not covered.</ul>

          <ul>❌ <strong>Wear and tear:</strong>  Normal wear and tear that occurs with time is not covered.</ul>

          <ul>❌ <strong>War: </strong> Any damage caused to your car due to war, nuclear risk, or civil war-like situations will not be covered under your car insurance plan.</ul>
          </div>
        </section>

        {/* <!-- ========== factors affecting Section ========== --> */}
        <section className="insurance-section">
          <h2><strong>Factors Affecting Car Insurance Premium</strong></h2>
          <div className="card-grid">
            <div className="card">
              <div className="card-header">
              <img src={model} alt="Vehicle Make & Model"/>
              <h3>Vehicle Make & Model</h3>
              </div>
              <p>The sort of car you have has a big impact on your premium. Luxury and high-end vehicles often have higher rates due to increased repair and replacement expenses.</p>
            </div>
            <div className="card">
              <div className="card-header">
              <img src={age} alt="Vehicle Age"/>
              <h3>Vehicle Age</h3>
              </div>
              <p>Newer cars usually have higher premiums compared to older cars. This is because newer vehicles have a higher market value and are more expensive to repair or replace.</p>
            </div>
             <div className="card">
              <div className="card-header">
              <img src={coverage} alt="Types of Coverage"/>
              <h3>Types of  Coverage</h3>
              </div>
              <p>Comprehensive policies offering extensive protection against various risks will cost more than basic third-party liability coverage.</p>
            </div>
            <div className="card">
              <div className="card-header">
              <img src={addons} alt="Add-ons"/>
              <h3>Add-ons</h3>
              </div>
              <p>Comprehensive policies offering extensive protection against various risks will cost more than basic third-party liability coverage.</p>
            </div>
            <div className="card">
              <div className="card-header">
              <img src={ncb} alt="No Claim Bonus"/>
              <h3>No Claim Bonus</h3>
              </div>
              <p>If you haven’t made any claims during the policy period, you can benefit from a No Claim Bonus, which provides a discount on your renewal premium.</p>
            </div>
            <div className="card">
              <div className="card-header">
              <img src={safety} alt="Safety Features"/>
              <h3>Safety Features</h3>
              </div>
              <p>Cars equipped with anti-theft devices, airbags, and other safety features often have lower premiums as they are less likely to be stolen or cause injury.</p>
            </div>
          </div>
        </section>

        {/* <!-- ========== Choosing Best Policy Section ========== --> */}
        <section className="insurance-section">
          <h2><strong>How To Choose The Best Four-Wheeler Insurance Policy Online</strong></h2>
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
  <h2><strong>How To Download Your Car Insurance Policy Copy</strong></h2>
  <div className="ul-list">
    <ul>1️⃣ Log in to your insurer’s website or app</ul>
    <ul>2️⃣ Navigate to 'My Policies' or 'Download Policy'</ul>
    <ul>3️⃣ Enter vehicle details or OTP if required</ul>
    <ul>4️⃣ Download and save the PDF copy of your policy</ul>
  </div>
</section>
<section className="insurance-section">
  <h2><strong>FAQs About Car Insurance Policy</strong></h2>
  <div className="ul-list">
    <ul><strong>Q:</strong> Does the city of registration affect the premium?<br/><strong>A:</strong> Yes, high-risk areas lead to higher premiums.</ul>
    <ul><strong>Q:</strong> Can I retain No Claim Bonus if I switch insurers?<br/><strong>A:</strong> Yes, you must provide proof from your previous insurer.</ul>
    <ul><strong>Q:</strong> Is vehicle inspection always required?<br/><strong>A:</strong> Depends on the car’s age and renewal history.</ul>
    <ul><strong>Q:</strong> Does insurance cover natural disasters?<br/><strong>A:</strong> Yes, if you have comprehensive coverage including natural calamities.</ul>
  </div>
</section>

<section className="insurance-section">
  <h2><strong>Conclusion</strong></h2>
  <p>
    Choosing the right car insurance policy requires understanding coverage, 
    evaluating premiums, and considering additional add-ons. Our blue-themed car 
    insurance policies provide comprehensive protection and peace of mind.
  </p>
</section>

     
    </div>
  );
}
