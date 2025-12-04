import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5201/api"

export const atmApi = {
    async getATMs() {
        const response = await axios.get(`${API_BASE_URL}/ATMs`);
        return response.data;
    },

    async getTransactions(filters) {
        const params = new URLSearchParams();

        if(filters.atmIds && filters.atmIds.length > 0) {
            filters.atmIds.forEach(id => params.append("ATMIds", id));
        }

        if(filters.dateFrom) {
            params.append("DateFrom", filters.dateFrom);
        }

        if(filters.dateTo){
            params.append("DateTo", filters.dateTo);
        }

        const response = await axios.get(`${API_BASE_URL}/Transactions?${params}`);

        return response.data;
    }
};