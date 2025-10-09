import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import { submitProposalWithDocuments } from "../Services/ProposalService";
import "./ProposalForm.css";

export default function ProposalForm() {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);

  const policyId = queryParams.get("policyId");
  const defaultPolicyName = queryParams.get("policyName")
    ? decodeURIComponent(queryParams.get("policyName"))
    : "Unknown Policy";
  const defaultCoverageAmount = queryParams.get("coverageAmount") || "";

  // const userInfo = {
  //   firstName: "Aruna",
  //   lastName: "Devi",
  //   email: "aruna@gmail.com",
  //   phoneNumber: "7895674325",
  //   aadhaarMasked: "**********3456",
  //   panMasked: "****1234",
  // };

  const [formData, setFormData] = useState({
    vehicleType: "",
    vehicleRegNo: "",
    vehicleAge: "",
    coverageAmount: defaultCoverageAmount,
  });

  const [files, setFiles] = useState({
    drivingLicense: null,
    previousInsurance: null,
    incomeProof: null,
    passportPhoto: null,
    addressProof: null,
    signature: null,
  });

  const [errors, setErrors] = useState({});
  const [message, setMessage] = useState("");

  // Max file sizes
  const MAX_FILE_SIZE = {
    pdf: 10 * 1024 * 1024, // 10 MB
    image: 1 * 1024 * 1024, // 1 MB
  };

  const ALLOWED_FILE_TYPES = ["image/jpeg", "image/png", "application/pdf"];

  const handleChange = (e) =>
    setFormData({ ...formData, [e.target.name]: e.target.value });

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    const name = e.target.name;

    if (!file) return;

    // Validate file type
    if (!ALLOWED_FILE_TYPES.includes(file.type)) {
      setErrors((prev) => ({
        ...prev,
        [name]: "Invalid file format. Allowed: JPG, PNG, PDF.",
      }));
      setFiles((prev) => ({ ...prev, [name]: null }));
      return;
    }

    // Validate file size based on type
    if (file.type === "application/pdf" && file.size > MAX_FILE_SIZE.pdf) {
      setErrors((prev) => ({
        ...prev,
        [name]: "PDF file size exceeds 10 MB.",
      }));
      setFiles((prev) => ({ ...prev, [name]: null }));
      return;
    } else if (
      (file.type === "image/jpeg" || file.type === "image/png") &&
      file.size > MAX_FILE_SIZE.image
    ) {
      setErrors((prev) => ({
        ...prev,
        [name]: "Image file size exceeds 1 MB.",
      }));
      setFiles((prev) => ({ ...prev, [name]: null }));
      return;
    }

    // Clear previous errors and set file
    setErrors((prev) => ({ ...prev, [name]: "" }));
    setFiles((prev) => ({ ...prev, [name]: file }));
  };

  const validate = () => {
    const newErrors = {};

    if (!formData.vehicleType) newErrors.vehicleType = "Please select a vehicle type.";

    if (!formData.vehicleRegNo.trim()) {
      newErrors.vehicleRegNo = "Vehicle registration number is required.";
    } else if (!/^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{4}$/i.test(formData.vehicleRegNo)) {
      newErrors.vehicleRegNo = "Invalid vehicle registration number format.";
    }

    if (!formData.vehicleAge) {
      newErrors.vehicleAge = "Vehicle age is required.";
    } else if (formData.vehicleAge < 0 || formData.vehicleAge > 50) {
      newErrors.vehicleAge = "Vehicle age must be between 0 and 50 years.";
    }

    if (!formData.coverageAmount) {
      newErrors.coverageAmount = "Coverage amount is required.";
    } else if (formData.coverageAmount <= 0) {
      newErrors.coverageAmount = "Coverage amount must be greater than 0.";
    }

    // Required files
    if (!files.drivingLicense) newErrors.drivingLicense = "Driving License is required.";
    if (!files.passportPhoto) newErrors.passportPhoto = "Passport photo is required.";
    if (!files.signature) newErrors.signature = "Signature is required.";

    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validate();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      setMessage("");
      return;
    }

    setMessage("");
    try {
      const data = new FormData();
      data.append("PolicyId", policyId);
      data.append("PolicyName", defaultPolicyName);
      data.append("VehicleType", formData.vehicleType);
      data.append("VehicleRegNo", formData.vehicleRegNo);
      data.append("VehicleAge", formData.vehicleAge);
      data.append("CoverageAmount", formData.coverageAmount);
      Object.entries(files).forEach(([key, value]) => {
        if (value) data.append(key, value);
      });

      await submitProposalWithDocuments(data);
      setMessage("Proposal submitted successfully!");
      setErrors({});
    } catch (error) {
      console.error(error);
      setMessage("Error submitting proposal. Please try again.");
    }
  };

  return (
    <div className="proposals">
      <div className="proposal-form">
        <h2>Proposal Form</h2>

        {/* <div className="user-details">
          <p><strong>Name:</strong> {userInfo.firstName} {userInfo.lastName}</p>
          <p><strong>Email:</strong> {userInfo.email}</p>
          <p><strong>Phone:</strong> {userInfo.phoneNumber}</p>
          <p><strong>Aadhaar:</strong> {userInfo.aadhaarMasked}</p>
          <p><strong>PAN:</strong> {userInfo.panMasked}</p>
        </div> */}

        {message && <p style={{ color: message.includes("successfully") ? "green" : "red", textAlign: "center" }}>{message}</p>}

        <form onSubmit={handleSubmit} encType="multipart/form-data">
          <label>
            Vehicle Type
            <select
              name="vehicleType"
              value={formData.vehicleType}
              onChange={handleChange}
              className={errors.vehicleType ? "input-error" : ""}
            >
              <option value="">Select Vehicle Type</option>
              <option value="Car">Car</option>
              <option value="Bike">Bike</option>
              <option value="Truck">Truck</option>
            </select>
            {errors.vehicleType && <p className="error-text">{errors.vehicleType}</p>}
          </label>

          <label>
            Vehicle Reg. No
            <input
              type="text"
              name="vehicleRegNo"
              value={formData.vehicleRegNo}
              onChange={handleChange}
              className={errors.vehicleRegNo ? "input-error" : ""}
            />
            {errors.vehicleRegNo && <p className="error-text">{errors.vehicleRegNo}</p>}
          </label>

          <label>
            Vehicle Age
            <input
              type="number"
              name="vehicleAge"
              value={formData.vehicleAge}
              onChange={handleChange}
              className={errors.vehicleAge ? "input-error" : ""}
            />
            {errors.vehicleAge && <p className="error-text">{errors.vehicleAge}</p>}
          </label>

          <label>
            Coverage Amount
            <input
              type="number"
              name="coverageAmount"
              value={formData.coverageAmount}
              onChange={handleChange}
              className={errors.coverageAmount ? "input-error" : ""}
            />
            {errors.coverageAmount && <p className="error-text">{errors.coverageAmount}</p>}
          </label>

          <label>
            Passport Photo (JPG/PNG, max 1MB)
            <input type="file" name="passportPhoto" onChange={handleFileChange} />
            {errors.passportPhoto && <p className="error-text">{errors.passportPhoto}</p>}
          </label>

          <label>
            Signature (JPG/PNG, max 1MB)
            <input type="file" name="signature" onChange={handleFileChange} />
            {errors.signature && <p className="error-text">{errors.signature}</p>}
          </label>

          <label>
            Driving License (PDF, max 10MB)
            <input type="file" name="drivingLicense" onChange={handleFileChange} />
            {errors.drivingLicense && <p className="error-text">{errors.drivingLicense}</p>}
          </label>

          <label>
            Income Proof (PDF, max 10MB)
            <input type="file" name="incomeProof" onChange={handleFileChange} />
            {errors.incomeProof && <p className="error-text">{errors.incomeProof}</p>}
          </label>

          <label>
            Previous Insurance (PDF, max 10MB)
            <input type="file" name="previousInsurance" onChange={handleFileChange} />
            {errors.previousInsurance && <p className="error-text">{errors.previousInsurance}</p>}
          </label>

          <label>
            Address Proof (PDF, max 10MB)
            <input type="file" name="addressProof" onChange={handleFileChange} />
            {errors.addressProof && <p className="error-text">{errors.addressProof}</p>}
          </label>

          <button type="submit">Submit Proposal</button>
        </form>
      </div>
    </div>
  );
}
