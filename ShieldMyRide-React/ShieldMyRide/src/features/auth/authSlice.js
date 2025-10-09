// src/features/auth/authSlice.js
import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  token: localStorage.getItem("token") || null,
  roles: JSON.parse(localStorage.getItem("roles")) || [],
  username: localStorage.getItem("username") || "",
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    login: (state, action) => {
      const { token, roles, username } = action.payload;
      state.token = token;
      state.roles = roles;
      state.username = username;

      // Save to localStorage
      localStorage.setItem("token", token);
      localStorage.setItem("roles", JSON.stringify(roles));
      localStorage.setItem("username", username);
    },
    logout: (state) => {
      state.token = null;
      state.roles = [];
      state.username = "";
      localStorage.clear();
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;
