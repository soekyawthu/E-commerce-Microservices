import axios, {AxiosError, AxiosResponse} from "axios";
import {toast} from "react-toastify";
import {router} from "../router/Routes.tsx";
import {Basket} from "../models/basket";
const sleep = () => new Promise(resolve => setTimeout(resolve, 500))

axios.defaults.baseURL = "https://localhost:5011";
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response.data

axios.interceptors.response.use(async response => {
    await sleep()
    return response
}, (error: AxiosError) => {
    const {data, status} = error.response as AxiosResponse
    
    switch (status) {
        case 400:
            if (data.errors){
                const modelStateErrors: string[] = [];
                for (const key in data.errors){
                    if (data.errors[key]){
                        modelStateErrors.push(data.errors[key])
                    }
                }
                throw modelStateErrors.flat()
            }
            toast.error(data.title)
            break;
            
        case 401:
            //window.location.href = "https://localhost:8011/bff/login"
            break;
        case 403:
            toast.error(data.title)
            break;
        case 500:
            router.navigate('/server-error', {state: {error: data}})
            break;
        default:
            break;
    }
    
    return Promise.reject(error.response)
})

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, {
        params, 
        withCredentials: true, 
        headers: {
            "X-CSRF": '1',
        }}).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody)
}

const catalog = {
    list: (params?: URLSearchParams) => requests.get('catalog', params),
    details: (id: string) => requests.get(`catalog/${id}`)
}

const basket = {
    get: (params?: URLSearchParams) => requests.get('basket/soekyawthu', params),
    addItem: (body: Basket) => requests.post(`basket`, body),
    remove: () => requests.delete(`basket/soekyawthu`),
    checkout: (body: never) => requests.post('basket/checkout', body)
}

const order = {
    list: () => fetchOrder(),
    fetch: (id: string) => requests.get(`order/checkorder/${id}`),
    create: (values: never) => requests.post('order', values)
}

async function fetchOrder() {
    const response = await fetch('https://localhost:8011/order/soekyawthu', {
        method: 'GET',
        credentials: 'include',
        headers: {
            "X-CSRF": '1',
        },
    });
    console.log("Response Status : ", response.status);
    if(response.status === 401){
        //window.location.href = "https://localhost:8011/bff/login"
    }
    else if(response.status === 200){
        const data = await response.json();
        console.log("Response data : ", data);
        //setForecasts(data);
    }
}

async function fetchToken() {
    const response = await fetch('https://localhost:5011/api/Account', {
        method: 'GET',
        credentials: 'include',
        headers: {
            "X-CSRF": '1',
        },
    });
    console.log("Response Status : ", response.status);
    if(response.status === 200){
        console.log(await response.json())
    }
}

const agent = {
    catalog,
    basket,
    order
}

export default agent