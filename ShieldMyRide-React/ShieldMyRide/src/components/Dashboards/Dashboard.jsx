// // src/components/Dashboard.jsx
// import { useEffect, useState } from "react";
// import { getUserDetails } from "../../Services/UserInfo";

// export default function Dashboard() {
//   const [userData, setUserData] = useState(null);
//   const [role, setRole] = useState("");

//   useEffect(() => {
//     const userId = localStorage.getItem("userId");
//     const roles = JSON.parse(localStorage.getItem("roles")) || [];

//     if (!userId || roles.length === 0) return;

//     let currentRole = "";
//     if (roles.includes("Admin")) currentRole = "admin";
//     else if (roles.includes("Officer")) currentRole = "officer";
//     else currentRole = "customer";

//     setRole(currentRole);

//     getCustomer(userId)
//       .then(setUserData)
//       .catch(console.error);
//   }, []);

//   if (!userData) return <p>Loading dashboard...</p>;

//   return (
//     <div className="p-6">
//       <h1 className="text-2xl font-bold mb-4">
//         {role === "admin" && "Admin Dashboard"}
//         {role === "officer" && "Officer Dashboard"}
//         {role === "customer" && "User Dashboard"}
//       </h1>

//       <div className="bg-white shadow-md rounded-lg p-4">
//         <p><b>Name:</b> {userData.firstName} {userData.lastName}</p>
//         <p><b>Email:</b> {userData.email}</p>
//         <p><b>Phone:</b> {userData.phoneNumber}</p>
//       </div>

//       {role === "admin" && (
//         <div className="mt-6">
//           <h2 className="text-xl font-semibold mb-2">Admin Features</h2>
//           <p>Manage officers, customers, and system settings.</p>
//         </div>
//       )}

//       {role === "officer" && (
//         <div className="mt-6">
//           <h2 className="text-xl font-semibold mb-2">Officer Features</h2>
//           <p>View assigned proposals, approve/reject claims, and monitor cases.</p>
//         </div>
//       )}

//       {role === "customer" && (
//         <div className="mt-6">
//           <h2 className="text-xl font-semibold mb-2">Customer Features</h2>
//           <p>Check your proposals, claims, and balance details.</p>
//         </div>
//       )}
//     </div>
//   );
// }
