import axios from "axios";


const backendUrl = "https://localhost:7153/api/Claims";


// Get all claims (Officer/User)
export const getAllClaims = async () => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.get(backendUrl, config);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Get claim by ID
export const getClaimById = async (id) => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.get(`${backendUrl}/${id}`, config);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

// Get claim by proposal ID
export const getClaimByProposal = async (proposalId) => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.get(
      `${backendUrl}/by-proposal/${proposalId}`,
      config
    );
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

//  Create a new claim (User only)
export const createClaim = async (claimData) => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.post(`${backendUrl}`, claimData, config);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

//  Update a claim (Officer/User)
export const updateClaim = async (id, updatedClaim) => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.put(
      `${backendUrl}/claims/${id}`,
      updatedClaim,
      config
    );
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};

//  Delete a claim (Officer/User)
export const deleteClaim = async (id) => {
  const token = localStorage.getItem("token");
  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };

  try {
    const response = await axios.delete(`${backendUrl}/${id}`, config);
    return response.status === 204;
  } catch (error) {
    throw error.response ? error.response.data : error;
  }
};
