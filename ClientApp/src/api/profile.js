import { fatFetch } from './../utils/ajaxHelper';

export const GetProfile = async (id) => {
    
    try {
        const data = await fatFetch(`/api/players/getuserstat?id=${id}`)

        return data;
    }
    catch(error) {
        return error;
    } 
}