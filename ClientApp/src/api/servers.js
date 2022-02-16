import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';

export const GetServers = () => {
  return fatFetch(`/api/admin/monitoring/getservers`, true)
}

export const DeleteServer = (id) => {
    return fatFetch(`/api/admin/monitoring/deleteserver?id=${id}`, true)
}

export const AddServer = (ip, port) => {
    const data = new FormData();
    data.append("ip", ip);
    data.append("port", port);

    return fatFetch(`/api/admin/monitoring/addserver`,
                              true, 
                              "Post",
                              null,
                              data,
    )
}

export const ChangeServer = (server) => {
  const data = new FormData();
  data.append("id", server.id)
  data.append("ip", server.ip);
  data.append("port", server.port);

  return fatFetch(`/api/admin/monitoring/changeserverinfo`, true, "Post", null, data)    
}