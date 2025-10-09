import React, { useState,useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { generateQuote, fetchAllQuotes } from "../../features/quoteSlice"; 
// import "./OfficerQuotes.css";

export default function OfficerQuotes() {
  const dispatch = useDispatch();
  const [proposalId, setProposalId] = useState("");
  const { list: quotes, loading,error  } = useSelector((state) => state.quotes);

  
    useEffect(() => {
      dispatch(fetchAllQuotes());
    }, [dispatch]);
  
    

  const handleGenerateQuote = async () => {
    if (!proposalId) {
      alert("Please enter a Proposal ID");
      return;
    }
    try {
      await dispatch(generateQuote( proposalId )).unwrap();
        dispatch(fetchAllQuotes());
      alert("Quote generated successfully!");
    } catch (error) {
  if (error.response?.status === 401) {
    alert("Unauthorized! Please login as Officer or Admin.");
  } else if (error.response?.status === 404) {
    alert(`Proposal not found: ${proposalId}`);
  } else {
    alert(`Failed to generate quote: ${error.message}`);
  }
}
  };
  if (loading) return <p>Loading quotes...</p>;
    if (error) return <p className="error">Error: {error}</p>;
    // if (!quotes || quotes.length === 0)
    //   return <p>No quotes available for this user.</p>;

  return (
    <div className="quote-container">
      <div className="quote-form">
        <input
          type="number"
          placeholder="Enter Proposal ID"
          value={proposalId}
          onChange={(e) => setProposalId(e.target.value)}
        />
        <button onClick={handleGenerateQuote} disabled={loading}>
          {loading ? "Generating..." : "Generate Quote"}
        </button>
      </div>

      <table className="proposal-table">
        <thead>
          <tr>
           {/* <th>Quote ID</th> */}
            <th>Proposal ID</th>
            <th>Premium</th>
            <th>Coverage</th>
            <th>Date Issued</th>
            <th>Valid Till</th>
          </tr>
        </thead>
        <tbody>
          {quotes.map((quote) => (
            <tr key={quote.quoteId}>
              {/* <td>{quote.quoteId}</td> */}
              <td>{quote.proposalId}</td>
              <td>₹{quote.premiumAmount}</td>
              <td style={{ textAlign: "left",  whiteSpace: "pre-line" }}>{quote.coverageDetails.split(", ").map((line, index) => (
                  <div key={index}>{line}</div>
                ))}</td>
              <td>{new Date(quote.dateIssued).toLocaleDateString()}</td>
              <td>{new Date(quote.validTill).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
      
    </div>
    
  );
}
