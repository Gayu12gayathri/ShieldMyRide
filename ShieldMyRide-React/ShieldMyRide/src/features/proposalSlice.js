import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { deleteProposal, getUserProposals, getAllProposals, reviewProposal } from "../Services/ProposalService";

// Fetch proposals for the logged-in user (no need to pass userId)
export const fetchProposals = createAsyncThunk(
  "proposals/fetchProposals",
  async () => {
    return await getUserProposals(); // token-based fetching
  }
);

// Fetch all proposals
export const fetchAllProposals = createAsyncThunk(
  "proposals/fetchAllProposals",
  async () => {
    return await getAllProposals();
  }
);

// Delete a proposal
export const removeProposal = createAsyncThunk(
  "proposals/removeProposal",
  async (proposalId) => {
    await deleteProposal(proposalId);
    return proposalId;
  }
);

// Review a proposal
export const reviewProposalThunk = createAsyncThunk(
  "proposals/reviewProposal",
  async ({ proposalId, reviewData }) => {
    return await reviewProposal(proposalId, reviewData);
  }
);

const proposalSlice = createSlice({
  name: "proposals",
  initialState: { list: [], loading: false, error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // User proposals
      .addCase(fetchProposals.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchProposals.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchProposals.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Failed to load proposals";
      })

      // All proposals
      .addCase(fetchAllProposals.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAllProposals.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchAllProposals.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Failed to fetch all proposals";
      })

      // Remove proposal
      .addCase(removeProposal.fulfilled, (state, action) => {
        state.list = state.list.filter((p) => p.proposalId !== action.payload);
      })
      .addCase(removeProposal.rejected, (state, action) => {
        state.error = action.error.message || "Failed to remove proposal";
      })

      // Review proposal
      .addCase(reviewProposalThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(reviewProposalThunk.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.list.findIndex(p => p.proposalId === action.payload.proposalId);
        if (index !== -1) {
          state.list[index] = action.payload;
        }
      })
      .addCase(reviewProposalThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Failed to review proposal";
      });
  },
});

export default proposalSlice.reducer;
