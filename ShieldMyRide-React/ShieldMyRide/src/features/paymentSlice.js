// src/features/paymentSlice.js
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { processProposalPayment, getAllPayments,deletePayment,updatePayment  } from "../Services/PaymentService";
import { fetchProposals } from "./proposalSlice"; // refresh proposals after payment

// Pay for a proposal
export const payProposal = createAsyncThunk(
  "payments/payProposal",
  async ({ proposalId, amount, userId }, { rejectWithValue, dispatch }) => {
    try {
      const result = await processProposalPayment(proposalId, amount, userId);
      console.log("payProposal payload:", { proposalId, amount, userId });
      dispatch(fetchProposals(userId)); // Refresh proposals
      return result;
    } catch (err) {
      return rejectWithValue(err);
    }
  }
);

export const fetchPayments = createAsyncThunk(
  "payments/fetchPayments",
  async (_, { rejectWithValue }) => {
    try {
      const payments = await getAllPayments();
      return payments;
    } catch (err) {
      return rejectWithValue(err);
    }
  }
);
export const removePayment = createAsyncThunk(
  "payments/removePayment",
  async (paymentId, { rejectWithValue }) => {
    try {
      await deletePayment(paymentId);
      return paymentId; // return the deleted ID to remove from state
    } catch (err) {
      return rejectWithValue(err);
    }
  }
);
export const modifyPayment = createAsyncThunk(
  "payments/modifyPayment",
  async ({ paymentId, updatedData }, { rejectWithValue, dispatch }) => {
    try {
      const result = await updatePayment(paymentId, updatedData);
      // Optionally refresh proposals to update claim/payment balance
      const { Payment } = result;
      dispatch(fetchProposals(Payment.userID));
      return result;
    } catch (err) {
      return rejectWithValue(err);
    }
  }
);
const paymentSlice = createSlice({
  name: "payments",
  initialState: { list: [], loading: false, error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Pay Proposal
      .addCase(payProposal.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(payProposal.fulfilled, (state, action) => {
  state.loading = false;
  const { Payment } = action.payload || {};
  if (Payment) {
    const index = state.list.findIndex(p => p.paymentId === Payment.paymentId);
    if (index !== -1) state.list[index] = Payment;
    else state.list.push(Payment);
  }
})
      .addCase(payProposal.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Payment failed";
      })
      // Fetch Payments
      .addCase(fetchPayments.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchPayments.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchPayments.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to fetch payments";
      })
      .addCase(removePayment.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(removePayment.fulfilled, (state, action) => {
        state.loading = false;
        state.list = state.list.filter((p) => p.paymentId !== action.payload);
      })
      .addCase(removePayment.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to delete payment";
      })

      .addCase(modifyPayment.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(modifyPayment.fulfilled, (state, action) => {
        state.loading = false;
        const { Payment, BalanceAmount, Status } = action.payload;
        const index = state.list.findIndex(
          (p) => p.paymentId === Payment.paymentId
        );
        if (index !== -1) state.list[index] = Payment;
        state.balanceAmount = BalanceAmount;
        state.status = Status;
      })
      .addCase(modifyPayment.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to update payment";
      });
  },
});

export default paymentSlice.reducer;