import { credentials, domain } from './../configureFetch';
import { UpdateToken } from './updateToken';
import { accountLogOut } from './../reducers/accountInfo';
import { getCookie } from './../libs/cookie';
import { NotificationManager} from 'react-notifications';

let refreshTokenRequest = null; // чтобы избежать гонки

export const fatFetch = async (path, isAuth = true, method, headers, body) => {

    try
    {
        if(isAuth)
        {
            const isOk = await CheckValidateToken();

            const token = getCookie("token");

            if(token) {
                headers = { "Authorization": `Bearer ${token}`, ...headers };
            }

            if(isOk) {
                return await Request(path, method, headers, body);
            }
        }
        else
        {
            return await Request(path, method, headers, body);
        }
    }
    catch(error) {

        NotificationManager.error(error, 500);
        throw error;
    }
}

const Request = async (path, method, headers, body) => {

    const response = await fetch(`${domain}${path}`, {
        method: method,
        credentials: credentials,
        body: body,
        headers: {
            ...headers
        },
    })

    if(response.status == 401) { // if unauth then try check validate token and do this
        const newIsOk = await CheckValidateToken();

        if(newIsOk) {
            const repeatResponse = await fetch(`${domain}${path}`, {
                method: method,
                credentials: credentials,
                body: body,
                headers: {
                    ...headers
                },
            })
            
            return repeatResponse;
        }
    }

    const json = await response.json();

    if(json.isError)
    {
        throw json.responseException.exceptionMessage;
    }

    return json.result
}
 
export const CheckValidateToken = async () => {

    const date = localStorage.getItem("getting-refresh");

    if(!date) {
        return true;
    }

    const now = new Date();
    const currentUtcTime = (Date.UTC(now.getFullYear(),now.getMonth(), now.getDate() , 
                            now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds()) / 1000)
                            .toFixed();

    if(currentUtcTime - date >= 500) {

        if(!refreshTokenRequest) {
            console.log("Создаем запрос на update refresh token");
            refreshTokenRequest = UpdateToken();
        }
        else {
            console.log("Запрос на refresh token уже идет");
        }

        refreshTokenRequest = await refreshTokenRequest;

        if(!refreshTokenRequest.ok) {
            accountLogOut();
            localStorage.removeItem("getting-refresh");
            refreshTokenRequest = null;
            console.log("Не удалось обновить refresh token");
            
            return false;
        }

        localStorage.setItem("getting-refresh", currentUtcTime);
        refreshTokenRequest = null;
        console.log("Успешное обновление refresh token'a");
    }

    return true;
}

