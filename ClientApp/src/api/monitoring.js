import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';

export const GetServers = () => {

  return fatFetch(`/api/servermonitoring/getserversinfo`, false);
}
