import { combineReducers } from 'redux'
import accountInfo from './accountInfo'
import modal from './modal'
import signalR from './signalR';

export const rootReducer = combineReducers({
  accountInfo: accountInfo,
  modal: modal,
  signalR: signalR
})