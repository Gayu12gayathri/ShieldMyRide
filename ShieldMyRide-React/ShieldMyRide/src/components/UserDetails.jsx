import React, { useEffect, useState } from "react";
import { getCustomer, getOfficer, getAdmin } from "../Services/UserInfo";
import "./UserDetails.css";

const UserDetails = ({ userId, role }) => {
  const [userDetails, setUserDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        setLoading(true);
        let data;

        let apiRole = "";
        if (role?.toLowerCase() === "user") apiRole = "customer";
        else if (role?.toLowerCase() === "officer") apiRole = "officer";
        else if (role?.toLowerCase() === "admin") apiRole = "admin";

        if (!apiRole) throw new Error("Invalid user role");

        if (apiRole === "customer") data = await getCustomer(userId);
        else if (apiRole === "officer") data = await getOfficer(userId);
        else if (apiRole === "admin") data = await getAdmin(userId);

        setUserDetails(data);
        setError("");
      } catch (err) {
        console.error(err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (userId && role) fetchUserDetails();
  }, [userId, role]);

  if (loading) return <div className="message">Loading user details...</div>;
  if (error) return <div className="message error">⚠️ {error}</div>;
  if (!userDetails) return <div className="message">No user details found.</div>;

  return (
    <div >
      <h2 className="card-title">{role} Details</h2>
      <div className="details-list">
        {Object.entries(userDetails).map(([key, value]) => (
          <div key={key} className="details-item">
            <span className="details-key">{key.replace(/_/g, " ")}:</span>
            <span className="details-value">{value !== null ? value.toString() : "N/A"}</span>
          </div>
        ))}
      </div>
    </div>
  );
};

export default UserDetails;
