import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';

export const AddFeature = (id, name) => {
    const data = new FormData();
    data.append("typePrivilegeId", id);
    data.append("name", name);

    return fatFetch(`${domain}/api/admin/privilege/addfeature`, true, "Post", null, data,)
}
