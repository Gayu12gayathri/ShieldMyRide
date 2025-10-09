import React, { useEffect, useState } from "react";
import { fetchAllAssignments  } from "../../Services/AssignOfficerService";

export default function OfficerAssignments() {
  const [assignments, setAssignments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const getAssignments = async () => {
      setLoading(true);
      try {
        const data = await fetchAllAssignments();
        setAssignments(data);
      } catch (err) {
        console.error(err);
        setError("Failed to fetch officer assignments");
      } finally {
        setLoading(false);
      }
    };

    getAssignments();
  }, []);

  if (loading) return <p>Loading assignments...</p>;
  if (error) return <p>{error}</p>;
  if (!assignments.length) return <p>No assignments found.</p>;

  return (
    <table className="proposal-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Officer ID</th>
          <th>Action Type</th>
          <th>Target ID</th>
          <th>Remarks</th>
          <th>Status</th>
          <th>Action Time</th>
        </tr>
      </thead>
      <tbody>
        {assignments.map((a) => (
          <tr key={a.id}>
            <td>{a.id}</td>
            <td>{a.officerId}</td>
            <td>{a.actionType}</td>
            <td>{a.targetId}</td>
            <td>{a.remarks}</td>
            <td>{a.status}</td>
            <td>{new Date(a.actionTime).toLocaleString()}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
