import React, {createContext, ReactNode, useState, useContext} from "react";
import {login, jwtlogin} from "./../../Api/apiClient";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    username: "",
    password: "",
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
    userName: string,
    token: string
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    function logIn(username: string, password: string) {
        let loginResponse:Promise<LoginResponse> = jwtlogin(username, password);
        loginResponse.then(
            resposne => {
                setLoggedIn(true);
                setUsername(username);
                setPassword(password);
                localStorage.setItem('authToken', resposne.token);
            }
        );
        // if(loggedIn) return <>Login Successful</>
            
    }
    
    function logOut() {
        setLoggedIn(false);
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        username: username,
        password: password,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}