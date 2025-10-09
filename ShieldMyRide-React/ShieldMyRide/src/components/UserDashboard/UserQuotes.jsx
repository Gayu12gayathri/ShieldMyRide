import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchAllQuotes } from "../../features/quoteSlice";
import "../UserDashboard/UserProposal.css";

export default function UserQuotes({ userId }) {
  const dispatch = useDispatch();
  const { list: quotes, loading, error } = useSelector((state) => state.quotes);

  useEffect(() => {
    dispatch(fetchAllQuotes());
  }, [dispatch]);

  if (loading) return <p>Loading quotes...</p>;
  if (error) return <p className="error">Error: {error}</p>;
  if (!quotes || quotes.length === 0)
    return <p>No quotes available for this user.</p>;

  return (
   <div className="user-proposals">
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
