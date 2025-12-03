import { http } from './http.ts';
import type { Hero, HeroInput } from '../types/hero';

const BASE_PATH = '/api/HeroManager'; 

export async function getHeroes(): Promise<Hero[]> {
  const { data } = await http.get<Hero[]>(BASE_PATH);
  return data;
}

export async function getHero(id: number): Promise<Hero> {
  const { data } = await http.get<Hero>(`${BASE_PATH}/${id}`);
  return data;
}

export async function createHero(payload: HeroInput): Promise<Hero> {
  const { data } = await http.post<Hero>('/api/HeroManager/cadastrar-heroi', payload);
  return data;
}

export async function updateHero(id: number, payload: HeroInput): Promise<void> {
  await http.put(`${BASE_PATH}/${id}`, payload);
}

export async function deleteHero(id: number): Promise<void> {
  await http.delete(`${BASE_PATH}/${id}`);
}
