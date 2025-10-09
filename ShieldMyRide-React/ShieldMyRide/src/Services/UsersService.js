// UsersService.js
import axios from "axios";

const API_URL = "https://localhost:7153/api/Users"; // adjust base URL as needed

const BackendURL = "https://localhost:7153/api/Users/Register";

class UsersService {
  // Get all users
  static async getAllUsers() {
    try {
      const response = await axios.get(API_URL);
      return response.data;
    } catch (error) {
      console.error("Error fetching users:", error);
      throw error;
    }
  }

  // Get user by ID
  static async getUserById(id) {
    try {
      const response = await axios.get(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching user with ID ${id}:`, error);
      throw error;
    }
  }

  // Get user by email
  static async getUserByEmail(email) {
    try {
      const response = await axios.get(`${API_URL}/email/${email}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching user with email ${email}:`, error);
      throw error;
    }
  }

  // Create a new user
 static async createUser(userData) {
    try {
      // Map frontend formData to backend RegisterModel
      const registerModel = {
        Username: userData.email,         // or generate a username
        Email: userData.email,
        Password: userData.password,      // make sure your form collects this
        PhoneNumber: userData.phoneNumber,
        AadhaarMasked: userData.aadhaarNumber,
        PanMasked: userData.panNumber,
        FirstName: userData.firstName,
        LastName: userData.lastName,
        Role: userData.role,
        DateOfBirth: userData.dateOfBirth,
      };

      const response = await axios.post(BackendURL, registerModel);
      return response.data;
    } catch (error) {
      console.error("Error creating user:", error.response?.data || error.message);
      throw error;
    }
  }

  // Update an existing user
  static async updateUser(id, userData) {
    try {
      await axios.put(`${API_URL}/${id}`, userData);
    } catch (error) {
      console.error(`Error updating user with ID ${id}:`, error);
      throw error;
    }
  }

  // Delete a user
  static async deleteUser(id) {
    try {
      await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
      console.error(`Error deleting user with ID ${id}:`, error);
      throw error;
    }
  }
}

export default UsersService;
