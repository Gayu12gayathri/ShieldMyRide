import axios from "axios";

const API_URL = "https://localhost:7153/api/OfficerAssignment";

export const fetchAssignmentsByOfficer = async (officerId) => {
  const response = await axios.get(`${API_URL}/officer/${officerId}`);
  return response.data;
};

export const fetchAllAssignments = async () => {
  try {
    const res = await axios.get(`${API_URL}/all`);
    return res.data;
  } catch (err) {
    console.error(err);
    throw err;
  }
};