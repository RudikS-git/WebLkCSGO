import { fatFetch } from './../utils/ajaxHelper';

export const GetTopPlayers = async () => {
    
    try {
        const data = await fatFetch(`/api/players/gettopbypoints?count=10`)

        return data;
    }
    catch(error) {
        return error;
    } 
}