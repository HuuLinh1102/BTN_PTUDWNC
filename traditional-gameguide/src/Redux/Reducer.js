import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    keyword: ''
};

const postFilterReducer = createSlice({
    name: 'postFilter',
    initialState,
    reducers: {
        reset: (state, action) => {
            return initialState;
        },
        updateKeyword: (state, action) => {
            return {
                ...state,
                keyword: action.payload.keyword
            };
        },
    },
});

export const {
    reset,
    updateKeyword} = postFilterReducer.actions;

export const reducer = postFilterReducer.reducer;