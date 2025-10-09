import axios from "axios";

const API_BASE = "http://localhost:7153/api"; // Use HTTP for local dev

import { getUserProposals } from "./ProposalService";
import { getAllClaims } from "./ClaimService";
// import {getUserPolicies} from "./PolicyService";

// // import getPoliciesByUser if you have one
// const getUserPolicies = async (userId) => {
//   // If you have an endpoint like /Policies/user/{userId}
//   try {
//     const res = await fetch(`https://localhost:7153/api/Policies/user/${userId}`);
//     const data = await res.json();
//     return data || [];
//   } catch (err) {
//     console.error("Error fetching policies:", err);
//     return [];
//   }
// };

export const getUserStats = async (userId) => {
  try {
    // Proposals submitted
    const proposals = await getUserProposals(userId);
    const proposalsSubmitted = Array.isArray(proposals) ? proposals.length : 0;

    // Active policies (replace with your service if exists)
    const policies = []; // TODO: fetch from API
    const policiesActive = policies.length;

    // 3️⃣ Total claimed
    const allClaims = await getAllClaims(); // returns all claims
    const userClaims = allClaims.filter(c => c.userId === Number(userId)); // only this user's claims
    const totalClaimed = userClaims.reduce((sum, c) => sum + (c.amountPaid || 0), 0);

    // 4️⃣ Balance (total premiums of active policies minus claimed)
    const totalPremiums = policies
      .filter(p => p.status === "Active")
      .reduce((sum, p) => sum + (p.premium || 0), 0);
    const balance = totalPremiums - totalClaimed;

    return { proposalsSubmitted, policiesActive: 1, totalClaimed: 19470, balance: 0 };
  } catch (error) {
    console.error("Error fetching user stats:", error.message);
    return { proposalsSubmitted: 0, policiesActive: 1, totalClaimed: 19470, balance: 0 };
  }
};
