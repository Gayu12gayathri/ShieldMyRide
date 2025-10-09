// src/Services/policyService.js
import axios from "axios";

const backendUrl = "https://localhost:7153/api/PolicyDocuments";


const getAuthHeaders = () => {
  const token = localStorage.getItem("token");
  if (!token) throw new Error("User not authenticated");
  return {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json",
    Accept: "application/json",
  };
};


export const getAllPolicyDocuments = async () => {
  try {
    const response = await axios.get(backendUrl, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching all policy documents:", error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};


export const getDocumentsByProposal = async (proposalId) => {
  try {
    const response = await axios.get(`${backendUrl}/proposal/${proposalId}`, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error(`Error fetching documents for proposal ${proposalId}:`, error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};


export const createPolicyDocument = async (documentData) => {
  try {
    const response = await axios.post(backendUrl, documentData, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error("Error creating policy document:", error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};


export const updatePolicyDocument = async (documentId, documentData) => {
  try {
    const response = await axios.put(`${backendUrl}/${documentId}`, documentData, {
      headers: getAuthHeaders(),
    });
    return response.data;
  } catch (error) {
    console.error(`Error updating document ${documentId}:`, error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};


export const deletePolicyDocument = async (documentId) => {
  try {
    const response = await axios.delete(`${backendUrl}/${documentId}`, {
      headers: getAuthHeaders(),
    });
    return response.status === 204;
  } catch (error) {
    console.error(`Error deleting document ${documentId}:`, error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};
