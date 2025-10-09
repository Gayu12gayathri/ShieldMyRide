import React from "react"
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import about from "../assets/about.png"
import license from "../assets/icons/license.png"
import modifications from "../assets/icons/modifications.png"
import mechanical from "../assets/icons/mechanical.png"
import substance from "../assets/icons/substance.png"
import negligence from "../assets/icons/negligence.png"
import getquote from "../assets/getquote.png"
import legal from "../assets/icons/legal.png"
import loan from "../assets/icons/loan.png"
import fire from "../assets/icons/fire.png"
import accident from "../assets/icons/accident.png"
import personal from "../assets/icons/personal.png"
import theft from "../assets/icons/theft.png"
import natural from "../assets/icons/natural.png"
import financial from "../assets/icons/financial.png"
import Reviews from "../components/Review";
import peace from "../assets/icons/peace.png"
// import car_cal from "../assets/car_cal.svg"
import home from "../assets/home.gif"
import "./Home.css";
import InsurancePolicies from "../components/motorinsurance/InsurancePolicy";

export default function Home () {
    const [regNo, setRegNo] = useState("");
    const [captcha, setCaptcha] = useState("");
    const navigate = useNavigate();

  const correctCaptcha = "15"; // 7+8=15

  const handleSubmit = (e) => {
    e.preventDefault();
    if (captcha !== correctCaptcha) {
      alert("Captcha is incorrect!");
      return;
    }
    // Navigate to quote page, pass regNo as state
    navigate("/quote", { state: { regNo } });
  };

    return(
        <>
       <div className="homepage">
      {/* Hero Section */}
      <div className="hero-section">
        {/* Left - GIF */}
        <div className="hero-image">
          <img src={home} alt="Insurance" />
        </div>

        {/* Right - Text */}
        <div className="hero-text">
          <h1>Vehicle Insurance <br /> that is right for you!</h1>
          <p className="subtitle">Affordable Premium • Superfast Claims</p>
            {/* Navigate to QuotePage on click */}
          <button
            className="get-quote-btn"
            onClick={() => navigate("/quote")}
          >
            GET QUOTE
          </button>

          {/* Stats Section */}
          <div className="stats">
            <div>
              <h3>$44M+</h3>
              <p>Total Savings</p>
            </div>
            <div>
              <h3>37.57 Million</h3>
              <p>Policies issued</p>
            </div>
            <div>
              <h3>26M+</h3>
              <p>Real time quotes</p>
            </div>
            <div>
              <h3>4.8 ⭐</h3>
              <p>Based on 3000+ reviews</p>
            </div>
          </div>
        </div>
      </div>
    </div>

        {/* Right Form
        <div className="main-form-card">
          <h2>Vehicle Insurance Calculator</h2>
          <p>Premium Quote Calculator</p>

          <form onSubmit={handleSubmit}>
            <input
              type="text"
              placeholder="Vehicle Registration No."
              value={regNo}
              onChange={(e) => setRegNo(e.target.value)}
              required
            />

            <div className="captcha-box">
              <label>7 + 8 = ?</label>
              <input
                type="text"
                placeholder="Enter Answer"
                value={captcha}
                onChange={(e) => setCaptcha(e.target.value)}
                required
              />
            </div>

            <button type="submit" className="submit-btn">
              View Prices
            </button>
          </form>

       <div className="checkboxes">
        <label>
            <input type="checkbox" />
            <span className="checkbox-text">
            I accept <a href="#">Terms and Conditions</a>
            </span>
        </label>
        <label>
            <input type="checkbox" />
            <span className="checkbox-text">
            Get updates on <b>WhatsApp</b>
            </span>
        </label>
        </div>
        </div> */}
       {/* about section */}
        <div className="about-section">
        <div className="about-text">
            <h2>About vehicle insurance</h2>
            <p>
            Vehicle insurance or motor insurance is meant for cars, two wheelers and
            other road vehicles. A motor package policy protects the insured vehicle
            against the damages caused due to accidents and natural disasters.
            </p>
            <p>
            In addition to the own vehicle damage, motor vehicle insurance also
            provides the mandatory coverage for third-party liabilities.
            </p>
            <p>
            Simply put, a comprehensive vehicle insurance allows for worry-free
            drives by curbing your vehicle repair expenses and helping you adhere to
            the law when on road.
            </p>
        </div>
        <div className="about-image">
            <img src={about} alt="Vehicle Insurance" className="brand-logo" />
        </div>
    </div>
    {/* <!-- ========== Why You Need Motor Insurance Section ========== --> */}
    <section className="insurance-section">
    <h2><strong>Why Do You Need Motor Insurance in India?</strong></h2>
    <p>Vehicle insurance is required in India for the following reasons:</p>
    
    <div className="card-grid">
      <div className="card">
        <div className="card-header">
          <img src={legal} alt="Legal" />
          <h3>Legal obligation</h3>
        </div>
        <p>As per the Indian Motor Vehicle Act, it is mandatory to have a valid motor vehicle insurance plan to avoid legal penalties.</p>
      </div>

      <div className="card">
        <div className="card-header">
          <img src={financial} alt="Protection" />
          <h3>Financial protection</h3>
        </div>
        <p>Get covered for potential risks and unforeseen circumstances and secure yourself from getting a financial jolt.</p>
      </div>

      <div className="card">
        <div className="card-header">
          <img src={loan} alt="Coverage" />
          <h3>Personal Accident Coverage</h3>
        </div>
        <p>Motor vehicle insurance policy provides coverage for personal accident, offering financial support in case of injury or death of the vehicle owner.</p>
      </div>

      <div className="card">
        <div className="card-header">
          <img src={peace} alt="Peace of Mind" />
          <h3>Peace of mind</h3>
        </div>
        <p>Knowing you are protected against the unprecedented risks, you can rest assured without any worries.</p>
      </div>
    </div>
  </section>

    {/* <!-- ========== Inclusions Section ========== --> */}
    <section className="insurance-section">
    <h2><strong>Inclusions: Coverage Offered by Our Motor Insurance Policy</strong></h2>
    <p>Get your vehicle covered with our Motor Vehicle Insurance policy and secure your peace of mind with the following benefits -</p>
    
    <div className="card-grid">
        <div className="card">
          <div className="card-header">
        <img src={theft} alt="Theft"/>
        <h3>Vehicle Theft</h3>
        </div>
        <p>Lowers the financial burden in case of theft</p>
        </div>
        <div className="card">
          <div className="card-header">
        <img src={accident} alt="Accident"/>
        <h3>Accidental Damage</h3>
        </div>
        <p>Coverage for loss or damage arising due to accidents.</p>
        </div>
        <div className="card">
          <div className="card-header">
        <img src={fire} alt="Fire"/>
        <h3>Fire Damage</h3>
        </div>
        <p>Fire-related loss and damage is covered.</p>
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
        <p>Coverage for disasters like earthquakes, floods, tsunamis, hurricanes, etc.</p>
        </div>
        <div className="card">
          <div className="card-header">
        <img src={loan} alt="Third Party"/>
        <h3>Third-Party Losses</h3>
        </div>
        <p>Any loss or damage caused by your vehicle to third-party persons or property is covered.</p>
        </div>
    </div>
    </section>

    
    {/* <!-- ========== Exclusions Section ========== --> */}
        <section className="insurance-section">
        <h2><strong>Exclusions: What's Not Covered By Our Motor Insurance Policy</strong></h2>
        <p>We aim to provide maximum coverage for your vehicle, but under certain circumstances your vehicle might not get covered.</p>
        
        <div className="card-grid">
            <div className="card">
              <div className="card-header">
            <img src={license} alt="License"/>
            <h3>Driving without license</h3>
            </div>
            <p>If you get into an accident and are found driving without a valid driving license, then you cannot claim for damages.</p>
            </div>
            <div className="card">
              <div className="card-header">
            <img src={negligence} alt="Negligence"/>
            <h3>Driver's negligence</h3>
            </div>
            <p>If the driver is not being vigilant while driving and that leads to an accident, then such losses are not covered.</p>
            </div>
            <div className="card">
             <div className="card-header">
            <img src={substance} alt="Substance"/>
            <h3>Substance abuse</h3>
            </div>
            <p>Accidents caused by the consumption of alcohol or drugs will not be covered.</p>
            </div>
            <div className="card">
              <div className="card-header">
            <img src={mechanical} alt="Mechanical"/>
            <h3>Mechanical breakdown</h3>
            </div>
            <p>Any electric, non-electric, or mechanical breakdown will not be covered.</p>
            </div>
            <div className="card">
              <div className="card-header">
            <img src={modifications}  alt="Modifications"/>
            <h3>Unauthorized modifications</h3>
            </div>
            <p>If damage is caused due to unauthorized modifications done to the vehicle, then such damage will not be covered.</p>
            </div>
        </div>
        </section>
        <InsurancePolicies/>
    <Reviews/>
    </>

    )
    
}