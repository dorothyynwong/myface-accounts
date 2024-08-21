// just a reference

import {useContext} from "react";
import { LoginContext } from "../LoginManager/LoginManager";

interface ApiWrapperProps{
    url: string,
    options: RequestInit
}

export function ApiWrapper() {
    const loginContext = useContext(LoginContext);
    let base64 = require("base-64");
    const credientals: string =  base64.encode(`${loginContext.username}:${loginContext.password}`);

    async function fetchWithAuth(props: ApiWrapperProps):Promise<any> {
        const headers = {
            ...props.options.headers,
            ...({ 'Authorization': `${credientals}` })
        };

        const response = await fetch(props.url, { ...props.options, headers });
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    }

    return fetchWithAuth;
}