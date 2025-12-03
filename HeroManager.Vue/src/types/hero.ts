export interface Hero {
  id: number;
  nome: string;
  nomeHeroi: string;
  dataNascimento: string;
  altura: number;
  peso: number;
  superpoderIds?: number[]; 
}

export interface HeroInput {
  nome: string;
  nomeHeroi: string;
  dataNascimento: string;
  altura: number;
  peso: number;
  superpoderIds: number[]; 
}
