import { http } from './http.ts';
import type { Superpower } from '../types/superpower';

const BASE_PATH = '/api/superpoderes';

export async function getSuperpowers(): Promise<Superpower[]> {
  const { data } = await http.get<Superpower[]>(BASE_PATH);
  return data;
}

export async function createSuperpower(payload: Omit<Superpower, 'id'>): Promise<Superpower> {
  const { data } = await http.post<Superpower>(BASE_PATH, payload);
  return data;
}
