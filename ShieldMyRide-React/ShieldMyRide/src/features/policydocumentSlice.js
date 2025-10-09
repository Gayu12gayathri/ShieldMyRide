import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { getAllPolicyDocuments,getDocumentsByProposal, deletePolicyDocument } from "../Services/PolicyDocumentService";


export const fetchAllPolicyDocuments = createAsyncThunk(
  "policyDocuments/fetchAll",
  async (_, { rejectWithValue }) => {
    try {
      const data = await getAllPolicyDocuments();
      return data;
    } catch (error) {
      return rejectWithValue(error);
    }
  }
);


export const fetchDocumentsByProposalThunk = createAsyncThunk(
  "policyDocuments/fetchByProposal",
  async (proposalId) => {
    return await getDocumentsByProposal(proposalId);
  }
);

export const removePolicyDocumentThunk = createAsyncThunk(
  "policyDocuments/removeDocument",
  async (documentId) => {
    await deletePolicyDocument(documentId);
    return documentId; 
  }
);


const policyDocumentSlice = createSlice({
  name: "policyDocuments",
  initialState: {
    list: [],
    loading: false,
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch documents by proposal
      .addCase(fetchDocumentsByProposalThunk.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchDocumentsByProposalThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchDocumentsByProposalThunk.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Failed to load documents";
      })
      .addCase(fetchAllPolicyDocuments.pending, (state) => {
      state.loading = true;
      state.error = null;
    })
    .addCase(fetchAllPolicyDocuments.fulfilled, (state, action) => {
      state.loading = false;
      state.list = action.payload;
    })
    .addCase(fetchAllPolicyDocuments.rejected, (state, action) => {
      state.loading = false;
      state.error = action.payload?.message || action.error?.message || "Failed to fetch documents";
    })

      // Delete document
      .addCase(removePolicyDocumentThunk.fulfilled, (state, action) => {
        state.list = state.list.filter((doc) => doc.DocumentId !== action.payload);
      })
      .addCase(removePolicyDocumentThunk.rejected, (state, action) => {
        state.error = action.error.message || "Failed to remove document";
      });
  },
});

export default policyDocumentSlice.reducer;
