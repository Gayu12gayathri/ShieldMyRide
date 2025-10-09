import { useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import "./Home.css"; // reuse same styling if needed
import "./QuotePage.css"

export default function QuotePage() {
  const location = useLocation();
  const regNoFromHome = location.state?.regNo || "";

  // Step 1: State for first form (registration + captcha)
  const [regNo, setRegNo] = useState(regNoFromHome);
  const [captcha, setCaptcha] = useState("");
  const [showCalculator, setShowCalculator] = useState(false);

  const correctCaptcha = "15"; // 7 + 8 = 15

  // Step 2: State for calculator form
  const [form, setForm] = useState({
    vehicleType: "Car",
    vehicleAge: 1,
    coverageAmount: 100000,
    zeroDep: false,
    roadsideAssist: false,
    ncbPercent: 0,
  });

  const [quote, setQuote] = useState(null);

  // Handle input changes for calculator
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({
      ...form,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  // Step 3: Handle first form submission
  const handleCaptchaSubmit = (e) => {
    e.preventDefault();
    if (captcha !== correctCaptcha) {
      alert("Captcha is incorrect!");
      return;
    }
    if (!regNo) {
      alert("Please enter your vehicle registration number!");
      return;
    }
    setShowCalculator(true);
  };

  // Step 4: Handle quote calculation
  const handleQuoteSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7153/api/Quote/calculate", form);
      console.log("Response data:", res.data);
      setQuote(res.data);
    } catch (err) {
      if (err.response) {
        alert(`Server Error: ${err.response.status}\n${JSON.stringify(err.response.data)}`);
      } else if (err.request) {
        alert("No response from server. Check if backend is running and CORS is enabled.");
      } else {
        alert("Error: " + err.message);
      }
    }
  };

  return (
    <div className="quote-page">
      {!showCalculator ? (
        // 🧾 STEP 1: Registration Form
        <div className="main-section">
        <div className="main-form-card">
          <h2>Vehicle Insurance Calculator</h2>
          <p>Premium Quote Calculator</p>

          <form onSubmit={handleCaptchaSubmit}>
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
        </div>
        </div>
      ) : (
        // 💡 STEP 2: Show Calculation Form after captcha success
        <div className="quote-form">
          <h2>Calculate Insurance Quote</h2>
          <p>
            For Vehicle Reg No: <b>{regNo}</b>
          </p>

          <form onSubmit={handleQuoteSubmit}>
            <label>
              Vehicle Type:
              <select name="vehicleType" value={form.vehicleType} onChange={handleChange}>
                <option value="Car">Car</option>
                <option value="Bike">Bike</option>
                <option value="Truck">Truck</option>
              </select>
            </label>

            <label>
              Vehicle Age (years):
              <input
                type="number"
                name="vehicleAge"
                value={form.vehicleAge}
                onChange={handleChange}
                min="0"
              />
            </label>

            <label>
              Coverage Amount:
              <input
                type="number"
                name="coverageAmount"
                value={form.coverageAmount}
                onChange={handleChange}
              />
            </label>

            <label className="checkbox-label">
              <input
                type="checkbox"
                name="zeroDep"
                checked={form.zeroDep}
                onChange={handleChange}
              />
              Zero Depreciation
            </label>

            <label className="checkbox-label">
              <input
                type="checkbox"
                name="roadsideAssist"
                checked={form.roadsideAssist}
                onChange={handleChange}
              />
              Roadside Assistance
            </label>

            <label>
              NCB Percent:
              <select name="ncbPercent" value={form.ncbPercent} onChange={handleChange}>
                <option value={0}>0%</option>
                <option value={20}>20%</option>
                <option value={50}>50%</option>
              </select>
            </label>

            <button type="submit" className="submit-btn">
              Calculate Quote
            </button>
          </form>

          {quote && (
            <div className="quote-result">
              <h3>Quote Result</h3>
              <p><b>Premium Amount:</b> ₹{quote.premiumAmount}</p>
              <p><b>Coverage Details:</b> {quote.coverageDetails}</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
