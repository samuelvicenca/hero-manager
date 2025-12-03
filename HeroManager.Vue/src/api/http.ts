import axios from 'axios';

// Ajuste essa URL pro que sua API .NET local estiver apresentando
// Ex: https://localhost:5001 ou http://localhost:5195
const API_BASE_URL = 'https://localhost:32769';

export const http = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
});
