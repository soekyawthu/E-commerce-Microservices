import './App.css'
import {Outlet} from "react-router-dom";
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import Header from "./Header.tsx";
import {useAppDispatch} from "../store/configureStore.ts";
import {useEffect} from "react";
import {fetchBasketAsync} from "../../features/basket/basketSlice.ts";

function App() {

    const dispatch = useAppDispatch()

    useEffect(() => {
        dispatch(fetchBasketAsync())
    }, [dispatch]);

    const theme = createTheme({
        palette: {
            
        }
    })
    
    return(
        <ThemeProvider theme={theme}>
            <CssBaseline/>
            <Header/>
            <Container>
                <Outlet/>
            </Container>
        </ThemeProvider>
    )
}

export default App
