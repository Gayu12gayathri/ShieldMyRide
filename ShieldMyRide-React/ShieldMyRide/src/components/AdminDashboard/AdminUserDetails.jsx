import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  fetchUsers,
  createUser,
  updateUser,
  deleteUser,
} from "../../features/userSlice";
import "../UserDashboard/UserProposal.css";

export default function AdminUserDetails() {
  const dispatch = useDispatch();
  const { users, loading, error } = useSelector((state) => state.users);

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    aadhaarNumber: "",
    panNumber: "",
    role: "Officer",
  });
  const [editingUserId, setEditingUserId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [message, setMessage] = useState("");

  useEffect(() => {
    dispatch(fetchUsers());
  }, [dispatch]);

  // Filter Admin & Officer only
  const filteredUsers = users.filter(
    (u) => (u.role || u.Role) === "User" || (u.role || u.Role) === "Officer"
  );

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingUserId) {
        await dispatch(updateUser({ id: editingUserId, userData: formData })).unwrap();
        setMessage(`✅ User updated successfully!`);
        setEditingUserId(null);
      } else {
        await dispatch(createUser(formData)).unwrap();
        setMessage(`✅ User created successfully!`);
      }
      setFormData({
        firstName: "",
        lastName: "",
        email: "",
        phoneNumber: "",
        aadhaarNumber: "",
        panNumber: "",
        role: "Officer",
      });
      setShowForm(false);
    } catch (err) {
      console.error(err);
      setMessage(`❌ Failed: ${err?.message || ""}`);
    }
  };

  const handleEdit = (user) => {
    setEditingUserId(user.userId || user.UserId);
    setFormData({
      firstName: user.firstName || user.FirstName,
      lastName: user.lastName || user.LastName,
      email: user.email || user.Email,
      phoneNumber: user.phoneNumber || user.PhoneNumber,
      aadhaarNumber: user.aadhaarNumber || user.AadhaarNumber,
      panNumber: user.panNumber || user.PanNumber,
      role: user.role || user.Role,
    });
    setShowForm(true);
  };

  const handleDelete = async (userId) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        await dispatch(deleteUser(userId)).unwrap();
        setMessage(`✅ User deleted successfully!`);
      } catch (err) {
        console.error(err);
        setMessage(`❌ Failed to delete user: ${err?.message || ""}`);
      }
    }
  };

  return (
    <div className="user-claims">
      <h3>Admin & Officer Users</h3>
      <button className="create-claim-btn" onClick={() => setShowForm((prev) => !prev)}>
        {showForm ? "Cancel" : "➕ Add User"}
      </button>

      {showForm && (
        <div className="claim-form">
          <input
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            placeholder="First Name"
            required
          />
          <input
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            placeholder="Last Name"
            required
          />
          <input
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="Email"
            type="email"
            required
          />
          <input
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            placeholder="Phone Number"
          />
          <input
            name="aadhaarNumber"
            value={formData.aadhaarNumber}
            onChange={handleChange}
            placeholder="Aadhaar Number"
          />
          <input
            name="panNumber"
            value={formData.panNumber}
            onChange={handleChange}
            placeholder="PAN Number"
          />
          <select name="role" value={formData.role} onChange={handleChange}>
            <option value="Officer">Officer</option>
            <option value="User">User</option>
            <option value="Admin">Admin</option>
          </select>
          <button className="submit-claim-btn" onClick={handleSubmit}>
            {editingUserId ? "Update User" : "Add User"}
          </button>
        </div>
      )}

      {message && <p>{message}</p>}

      {loading && <p>Loading users...</p>}
      {error && <p className="error">Error: {error}</p>}

      <table className="proposal-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Aadhaar</th>
            <th>PAN</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredUsers.map((user) => (
            <tr key={user.UserId || user.userId}>
              <td>{user.UserId || user.userId}</td>
              <td>{user.FirstName || user.firstName} {user.LastName || user.lastName}</td>
              <td>{user.Email || user.email}</td>
              <td>{user.PhoneNumber || user.phoneNumber}</td>
              <td>{user.AadhaarNumber || user.aadhaarNumber}</td>
              <td>{user.PanNumber || user.panNumber}</td>
              <td>{user.Role || user.role}</td>
              <td>
                <button className="update-btn" onClick={() => handleEdit(user)}>✏️ Edit</button>
                <button className="delete-btn" onClick={() => handleDelete(user.UserId || user.userId)}>🚮 Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
