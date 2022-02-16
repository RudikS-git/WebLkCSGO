import { fatFetch } from './../utils/ajaxHelper';

export const GetHistory = (serverId, count, steamId) => {
    return fatFetch(`/api/admin/gamechat/gethistory?serverId=${serverId}&count=${count}&steamId2=${steamId}`, true, "get", null, null)
}