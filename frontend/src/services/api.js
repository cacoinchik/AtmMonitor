import axios from "axios";

const API_BASE_URL = "http://localhost:7166/api"

export const atmApi = {
    async getATMs() {
        const response = await axios.get('${API_BASE_URL}/ATMs');
        return response.data;
    },

    async getTransactions(filters) {
        const params = new URLSearchParams();

        if(filters.atmIds && filters.atmIds.length > 0) {
            filters.atmIds.forEach(id => params.append('ATMIds', id));
        }

        if(filters.dateFrom) {
            params.append('DateFrom', filters.dateFrom);
        }

        if(filters.dateTo){
            params.append('DateTo', dateTo);
        }

        const response = await axios.get('${API_BASE_URL}/Transactions?${params}');
    }
};