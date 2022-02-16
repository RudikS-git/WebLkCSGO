import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { lockScroll, unlockScroll } from "../libs/scrollLock.js";

const initialState = {
    modalType: null,
    modalProps: {}
}

const modal = createSlice({

  name: "modal",
  initialState,
  reducers: {
    modalShow: (state, action) => {
        lockScroll();

        state.modalType = action.payload.modalType;
        state.modalProps = action.payload.modalProps;

        return state;
    },
    modalHide: (state, action) => {
        unlockScroll();
        return initialState;
    }
  },
});

const { actions, reducer } = modal;

export const {
    modalShow, modalHide
} = actions;

export default reducer;