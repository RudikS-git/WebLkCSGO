import { setToken } from '../reducers/accountInfo';
import { credentials, domain } from './../configureFetch';
import { getCookie } from './../libs/cookie';

export const UpdateToken = async () => {
    console.log("Истек срок токена. Попытка обновить...");

    return fetch(`${domain}/api/token/refresh?token=${getCookie("token")}`, {
        credentials: credentials,
    })
    .then(response => {
      return response;
    })
    .catch((error) => {
      console.log(error);
    })
  }