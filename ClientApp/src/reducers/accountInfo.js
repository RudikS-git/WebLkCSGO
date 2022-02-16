import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { fatFetch } from './../utils/ajaxHelper';
import { credentials, domain } from './../configureFetch';

const initialState = {
  user: null,
  error: '',
  isFetching: true,
  token: null
}

const accountInfo = createSlice({

  name: "accountInfo",
  initialState,
  reducers: {
    accountInfoFetching: (state) => {
      state.isFetching = true;

      return state;
    },
    accountInfoFetched: (state, action) => {
      state.isFetching = false;
      state.user = action.payload

      return state;
    },
    accountInfoFetchingError:(state, action) => {
      state.isFetching = false;
      state.error = action.payload.error

      return state;
    },
    accountLogOut:(state, action) => {
      state.user = initialState;

      return state;
    },
    setToken:(state, action) => {
      state.token = action.payload;

      return state;
    }
  },
 
});

const { actions, reducer } = accountInfo;

export const {
  accountInfoFetched, accountInfoFetching, accountInfoFetchingError, accountLogOut, setToken
} = actions;

export default reducer;

export const GetAccountInfo = () => async (dispatch) => {
  dispatch(accountInfoFetching());

  fatFetch(`/api/account/userinfo`, true)
  .then(data => {

      dispatch(accountInfoFetched(data));
  })
  .catch((error) => {
    dispatch(accountInfoFetchingError(error));
  })
}