import React, { useState } from "react";
import "./Queries.css";

const Queries = () => {
  const [activeTab, setActiveTab] = useState("Complaint");

  const renderForm = () => {
    return (
      <form className="form-container">
        <div className="form-row">
          <input type="text" placeholder="First Name" required />
          <input type="text" placeholder="Last Name" required />
        </div>

        <div className="form-row">
          <input type="email" placeholder="Email Address" required />
          <input type="tel" placeholder="Enter Mobile No." required />
        </div>

        <div className="form-row">
          <textarea
            placeholder="Remarks"
            rows="4"
            required
          ></textarea>
        </div>

        <div className="form-row captcha-row">
          <span>Enter the Text shown in the image</span>
          <div className="captcha-group">
            <span className="captcha-text">9 + 8 =</span>
            <input type="text" placeholder="Type here" />
          </div>
        </div>

        <button type="submit" className="submit-btn">Submit</button>
      </form>
    );
  };

  return (
    <div className="queries-page">
      <div className="queries-left">
        <h3>Hello Guest</h3>
        <p>How can we help you?</p>

        <div className="tabs">
          {["Complaint", "Feedback"].map((tab) => (
            <button
              key={tab}
              className={activeTab === tab ? "tab active" : "tab"}
              onClick={() => setActiveTab(tab)}
            >
              {tab}
            </button>
          ))}
        </div>

        {renderForm()}
      </div>

      <div className="queries-right">
        <h4>Corporate Office and Registered Office</h4>
        <p><strong>ShieldMyRide Insurance Company Ltd.</strong></p>
        <p>9th Floor, Tesla Wing, Fulcrum Building, Sahar Road, Andheri East, Mumbai – 400099</p>
        <p><strong>Office Timings:</strong> 9:30 a.m. to 5:30 p.m. (Monday to Friday)</p>
      </div>
    </div>
  );
};

export default Queries;
