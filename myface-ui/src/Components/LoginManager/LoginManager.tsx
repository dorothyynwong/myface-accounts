import React, {createContext, ReactNode, useState} from "react";
import {login} from "./../../Api/apiClient";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: (username: string, password: string) => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

interface LoginResponse {
    id: number,
    firstName: string,
    lastName: string,
    displayName: string,
    userName: string
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    
    function logIn(username: string, password: string) {
        let loginResponse:Promise<LoginResponse> = login(username, password);
        if (loginResponse != null)
        {
            setLoggedIn(true);
            
        }
        // if(loggedIn) return <>Login Successful</>
            
    }
    
    function logOut() {
        setLoggedIn(false);
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}