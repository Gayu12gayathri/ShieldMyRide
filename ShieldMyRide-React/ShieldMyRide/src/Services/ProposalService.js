import axios from "axios";

const backendUrl = "https://localhost:7153/api/Proposals";

// Submit proposal with documents
export const submitProposalWithDocuments = async (formData) => {
  try {
    const token = localStorage.getItem("token");
    const config = {
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${token}`,
      },
    };
    const response = await axios.post(
      `${backendUrl}/create-with-documents`,
      formData,
      config
    );
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Get proposal by ID
export const getProposalById = async (proposalId) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(`${backendUrl}/${proposalId}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Fetch all proposals
export const getAllProposals = async () => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(backendUrl, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Fetch proposals for logged-in user
export const getUserProposals = async () => {
  try {
    const token = localStorage.getItem("token");
    if (!token) throw new Error("User not authenticated");

    const config = {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    };

    const response = await axios.get(`${backendUrl}/user`, config);
    return response.data;
  } catch (error) {
    console.error("Error fetching user proposals:", error.response || error);
    throw error.response ? error.response.data : error.message;
  }
};

// Update a proposal
export const updateProposal = async (proposalId, updatedProposal) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.put(`${backendUrl}/${proposalId}`, updatedProposal, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Review a proposal
export const reviewProposal = async (proposalId, reviewData) => {
  try {
    const token = localStorage.getItem("token");
    const payload = {
      proposalStatus: reviewData.proposalStatus,
      remarks: reviewData.remarks || "",
    };
    const response = await axios.put(`${backendUrl}/${proposalId}/review`, payload, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Process payment
export const processProposalPayment = async (proposalId, amount) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(`${backendUrl}/${proposalId}/payment`, { amount }, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Delete proposal
export const deleteProposal = async (proposalId) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.delete(`${backendUrl}/${proposalId}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.status === 204;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};
