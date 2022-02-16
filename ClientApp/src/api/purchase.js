import { credentials, domain } from './../configureFetch';
import { fatFetch } from './../utils/ajaxHelper';
    
export const GetPrivileges = () => {

    return fatFetch(`/api/purchase/privilege`, true);
}

export const Purchase = (id, steamId) => {
    //const formData = new FormData();
    //formData.append("id", id);
    //formData.append("steamId", steamId);

    return fatFetch(`/api/purchase/qiwi`, true, "POST", { 'Content-Type': 'application/json' }, JSON.stringify({id, steamId}))
}