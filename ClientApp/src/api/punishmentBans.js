import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';

  //state.sourcebans.sourcebans.map(sb=>{ sb.num = num; num++; sb.collapse = false })

export const GetPage = (page) =>  {
    return fatFetch(`/api/Punished/BanList/page?id=${page}`, false);
}

export const Search = (searchEntity, page) => {
    
    let request;
    if(searchEntity == "")
    {
        request = `/api/punished/banlist/page?id=0`;
    }
    else
    {
        request = `/api/punished/searchbans?input=${searchEntity}&id=${page}`;
    }

    return fatFetch(request, false);

    // searchEntity
    // page
}

export const UnbanUser = (id) => {

    return fatFetch(`/api/admin/punished/unbanuser?id=${id}`, true);
}

export const BanUser = (id) => {
  
    return fatFetch(`/api/admin/punished/banuser?id=${id}`, true);
}