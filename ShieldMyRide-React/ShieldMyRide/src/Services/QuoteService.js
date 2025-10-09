import axios from "axios";

const API_BASE = "https://localhost:7153/api/Quote";

export const createQuote = async (proposalId) => {
  const token = localStorage.getItem("token");
  try {
    const response = await axios.post(
      `${API_BASE}/generate/${proposalId}`,
      {},
      { headers: { Authorization: `Bearer ${token}` } }
    );
    return response.data;
  } catch (error) {
    console.error("Quote generation failed:", error.response || error);
    throw error;
  }
};

//   Get all quotes (Officer/Admin/User)
export const getAllQuotes = async () => {
  const token = localStorage.getItem("token");
  try {
    const response = await axios.get(API_BASE, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error("Fetching all quotes failed:", error.response || error);
    throw error;
  }
};

//  Get single quote (User/Officer/Admin)
export const getQuoteById = async (quoteId) => {
  const token = localStorage.getItem("token");
  try {
    const response = await axios.get(`${API_BASE}/${quoteId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error("Fetching quote by ID failed:", error.response || error);
    throw error;
  }
};

//  Update quote (Officer/Admin only)
export const updateQuote = async (quoteId, updateData) => {
  const token = localStorage.getItem("token");
  try {
    const response = await axios.put(`${API_BASE}/${quoteId}`, updateData, {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Updating quote failed:", error.response || error);
    throw error;
  }
};

// Calculate quote (public — for frontend estimation)
export const calculateQuote = async (quoteRequest) => {
  try {
    const response = await axios.post(`${API_BASE}/calculate`, quoteRequest, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Quote calculation failed:", error.response || error);
    throw error;
  }
};

//  Delete quote (Admin only)
export const deleteQuote = async (quoteId) => {
  const token = localStorage.getItem("token");
  try {
    await axios.delete(`${API_BASE}/${quoteId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return true;
  } catch (error) {
    console.error("Deleting quote failed:", error.response || error);
    throw error;
  }
};