
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';
    
export const GetPage = (page) => {

    return fatFetch(`/api/punished/commsList/page?id=${page}`, false)  
}

export const Search = (searchEntity, page) => {

    let request;
    if(searchEntity == "")
    {
        request = `/api/punished/commsList/page?id=0`;
    }
    else
    {
        request = `/api/punished/searchComms?input=${searchEntity}&id=${page}`;
    }

    return fatFetch(request, false)
}

export const UnmuteUser = (user) => {

    return fatFetch(`/api/admin/punished/muteuser?id=${user.Id}`, true)
}

export const MuteUser = (user) => {

    return fatFetch(`/api/admin/punished/muteuser?id=${user.Id}`, true)    
}