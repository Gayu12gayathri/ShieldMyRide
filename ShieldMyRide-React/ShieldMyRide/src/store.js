// src/store.js
import { configureStore } from "@reduxjs/toolkit";
import authReducer from "./features/auth/authSlice";
import proposalReducer from "./features/proposalSlice";
import paymentReducer from "./features/paymentSlice";
import quoteReducer from "./features/quoteSlice";
import claimReducer from "./features/claimSlice";
import policyDocumentsReducer from "./features/policydocumentSlice"
import userReducer from "./features/userSlice";

export const store = configureStore({
  reducer: {
    auth: authReducer,
    proposals: proposalReducer,
    payments : paymentReducer,
    quotes : quoteReducer,
    claims : claimReducer,
    policydocuments : policyDocumentsReducer,
    users: userReducer,
  },
});

export default store;
