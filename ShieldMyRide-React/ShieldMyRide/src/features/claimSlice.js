import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { getAllClaims, createClaim, deleteClaim, updateClaim } from "../Services/ClaimService";


export const fetchClaims = createAsyncThunk(
  "claims/fetchClaims",
  async (_, { rejectWithValue }) => {
    try {
      const data = await getAllClaims();
      return data;
    } catch (error) {
      return rejectWithValue(error.message || "Failed to fetch claims");
    }
  }
);


export const createClaimThunk = createAsyncThunk(
  "claims/createClaim",
  async (claimData, { rejectWithValue }) => {
    try {
      const result = await createClaim(claimData);
      return result;
    } catch (error) {
      return rejectWithValue(error.message || "Failed to create claim");
    }
  }
);

export const deleteClaimThunk = createAsyncThunk(
  "claims/deleteClaim",
  async (claimId, { rejectWithValue }) => {
    try {
      await deleteClaim(claimId);
      return claimId; // return id for state removal
    } catch (error) {
      return rejectWithValue(error.message || "Failed to delete claim");
    }
  }
);

export const updateClaimThunk = createAsyncThunk(
  "claims/updateClaim",
  async ({ claimId, updatedData }, { rejectWithValue }) => {
    try {
      const updatedClaim = await updateClaim(claimId, updatedData);
      return updatedClaim;
    } catch (error) {
      return rejectWithValue(error.message || "Failed to update claim");
    }
  }
);

const claimSlice = createSlice({
  name: "claims",
  initialState: {
    list: [],
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch Claims
      .addCase(fetchClaims.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchClaims.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchClaims.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })

      // Create Claim
      .addCase(createClaimThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createClaimThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.list.push(action.payload);
      })
      .addCase(createClaimThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })

      // Delete Claim
      .addCase(deleteClaimThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteClaimThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.list = state.list.filter((c) => c.claimId !== action.payload);
      })
      .addCase(deleteClaimThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })

      // Update Claim
      .addCase(updateClaimThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateClaimThunk.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.list.findIndex((c) => c.claimId === action.payload.claimId);
        if (index !== -1) {
          state.list[index] = action.payload;
        }
      })
      .addCase(updateClaimThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      });
  },
});

export default claimSlice.reducer;
