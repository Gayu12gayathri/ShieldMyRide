import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { createQuote,getAllQuotes,deleteQuote as deleteQuoteService } from "../Services/QuoteService";

export const generateQuote = createAsyncThunk(
  "quotes/generateQuote",
  async (proposalId) => {
    return await createQuote(proposalId);
  }
);

export const fetchAllQuotes = createAsyncThunk(
  "quotes/fetchAllQuotes",
  async (_, { rejectWithValue }) => {
    try {
      const data = await getAllQuotes();
      return data;
    } catch (error) {
      return rejectWithValue(error.response?.data || error.message);
    }
  }
);
export const removeQuote = createAsyncThunk(
  "quotes/removeQuote",
  async (quoteId, { rejectWithValue }) => {
    try {
      await deleteQuoteService(quoteId);
      return quoteId; // return deleted quote ID
    } catch (error) {
      return rejectWithValue(error.response?.data || error.message);
    }
  }
);
const quoteSlice = createSlice({
  name: "quotes",
  initialState: { list: [], loading: false },
  reducers: {},
  extraReducers: (builder) => {
   builder
      .addCase(generateQuote.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(generateQuote.fulfilled, (state, action) => {
        state.loading = false;
        state.list.push(action.payload);
      })
      .addCase(generateQuote.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(fetchAllQuotes.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAllQuotes.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })
      .addCase(fetchAllQuotes.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(removeQuote.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(removeQuote.fulfilled, (state, action) => {
        state.loading = false;
        state.list = state.list.filter((q) => q.quoteId !== action.payload);
      })
      .addCase(removeQuote.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      });
  },
});

export default quoteSlice.reducer;
