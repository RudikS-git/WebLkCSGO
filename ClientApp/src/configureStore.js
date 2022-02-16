import { applyMiddleware, createStore } from 'redux';

import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';

//import thunk from 'redux-thunk';
//import { composeWithDevTools } from 'redux-devtools-extension';

import { rootReducer } from './reducers/rootReducer';

const middleware = getDefaultMiddleware({
    immutableCheck: false,
    serializableCheck: false,
    thunk: true,
  });
  
  export const store = configureStore({
   reducer: rootReducer,
   middleware,
   devTools: process.env.NODE_ENV !== 'production',
  });

//export const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(thunk)));