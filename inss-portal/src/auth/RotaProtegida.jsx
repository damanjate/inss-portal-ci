import { Navigate, useLocation} from "react-router-dom";
import{ useAuth} from "./AuthContext";

export default function RotaProtegida({children}){
    const {sessao} = useAuth();
    const local = useLocation();

    if(!sessao){
        return(
            < Navigate to ="/login" state={{de: local.pathname}}/>
        );
    }

    return children;
}