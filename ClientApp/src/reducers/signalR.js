import { domain, credentials } from '../configureFetch';
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { UpdateToken } from './../utils/updateToken';
import { CheckValidateToken } from './../utils/ajaxHelper';

const signalR = require("@microsoft/signalr");

const initialState = {
    connection: null,
}

const signalRSocket = createSlice({

    name: "signalRSocket",
    initialState,
    reducers: {
        setConnection: (state, action) => {

            state.connection = action.payload;

            return state;
        }
    },
});

const { actions, reducer } = signalRSocket;

export const {
    setConnection
} = actions;

export default reducer;