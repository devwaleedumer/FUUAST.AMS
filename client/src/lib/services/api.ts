import axios from 'axios'

import { setupInterceptors } from './interceptor'

export const apiAxios = setupInterceptors(
    axios.create({
        baseURL: process.env.NEXT_PUBLIC_SERVER_URI,
        withCredentials: true as const,
        headers: { 'Content-Type': 'application/json' },

    })
)