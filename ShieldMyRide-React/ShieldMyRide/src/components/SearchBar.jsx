import React, { useState } from "react";
import axios from "axios";
import "./SearchBar.css";

const SearchBar = () => {
  const [searchType, setSearchType] = useState("vehicle");
  const [searchInput, setSearchInput] = useState({});
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const apiBase = "https://localhost:7153/api/Search";

  // ✅ handle input changes for multiple fields
  const handleChange = (e) => {
    const { name, value } = e.target;
    setSearchInput((prev) => ({ ...prev, [name]: value }));
  };

 const handleSearch = async (e) => {
  e.preventDefault();
  setLoading(true);
  setError("");
  setMessage("");
  setResults([]);

  try {
    let url = "";

    switch (searchType) {
      case "vehicle":
        url = `${apiBase}/officer/by-vehicle?regNo=${searchInput.regNo || ""}&vehicleType=${searchInput.vehicleType || "car"}`;
        break;
      case "email":
        url = `${apiBase}/officer/by-email/${searchInput.email}`;
        break;
      case "userId":
        url = `${apiBase}/officer/by-userid/${searchInput.userId}`;
        break;
      case "proposalId":
        url = `${apiBase}/user/proposals/${searchInput.proposalId}`;
        break;
      case "userProposals":
        url = `${apiBase}/user/proposals/${searchInput.userId}`;
        break;
      case "userClaims":
        url = `${apiBase}/user/claims/${searchInput.userId}`;
        break;
      default:
        throw new Error("Invalid search type selected");
    }

    // ✅ Get token from localStorage (adjust if you store it elsewhere)
    const token = localStorage.getItem("token");

    // ✅ Include token in headers
    const response = await axios.get(url, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    if (Array.isArray(response.data)) {
      setResults(response.data);
    } else {
      setResults([response.data]);
    }

    if (response.data.length === 0) {
      setMessage("No results found.");
    }
  } catch (err) {
    console.error(err);

    if (err.response && err.response.status === 401) {
      setError("Unauthorized. Please log in again.");
    } else {
      setError("Something went wrong. Please check your input or API.");
    }
  } finally {
    setLoading(false);
  }
};


  // ✅ render input fields dynamically
  const renderInputs = () => {
    switch (searchType) {
      case "vehicle":
        return (
          <>
            <input
              type="text"
              name="regNo"
              placeholder="Vehicle Reg No"
              onChange={handleChange}
            />
            <input
              type="text"
              name="vehicleType"
              placeholder="Vehicle Type (e.g., car)"
              onChange={handleChange}
            />
          </>
        );
      case "email":
        return (
          <input
            type="email"
            name="email"
            placeholder="Enter Officer Email"
            onChange={handleChange}
          />
        );
      case "userId":
      case "userProposals":
      case "userClaims":
        return (
          <input
            type="number"
            name="userId"
            placeholder="Enter User ID"
            onChange={handleChange}
          />
        );
      case "proposalId":
        return (
          <input
            type="number"
            name="proposalId"
            placeholder="Enter Proposal ID"
            onChange={handleChange}
          />
        );
      default:
        return null;
    }
  };

  return (
    <div className="search-container">
      {/* <h2>Search Dashboard</h2> */}

      <form className="search-bar" onSubmit={handleSearch}>
        <select
          value={searchType}
          onChange={(e) => {
            setSearchType(e.target.value);
            setSearchInput({});
            setResults([]);
            setMessage("");
          }}
        >
          <option value="vehicle">Search by Vehicle</option>
          <option value="email">Search by Email</option>
          <option value="userId">Search by User ID</option>
          <option value="proposalId">Search by Proposal ID</option>
          <option value="userProposals">User’s Proposals</option>
          <option value="userClaims">User’s Claims</option>
        </select>

        {renderInputs()}

        <button type="submit" disabled={loading}>
          {loading ? "Searching..." : "Search"}
        </button>
      </form>

      {error && <p className="error">{error}</p>}
      {message && <p className="message">{message}</p>}

      {results.length > 0 && (
        <div className="results-section">
          <h3>Search Results</h3>
          <table className="search-table">
            <thead>
              <tr>
                {Object.keys(results[0]).map((key) => (
                  <th key={key}>{key}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {results.map((row, index) => (
                <tr key={index}>
                  {Object.values(row).map((val, i) => (
                    <td key={i}>{JSON.stringify(val)}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default SearchBar;
