# hero-manager

Aplicação full-stack para gerenciamento de heróis e seus superpoderes, desenvolvida com API .NET 8 e frontend Vue 3 + TypeScript. Criada para uma avaliação técnica.





* Funcionalidades principais

&nbsp;

Heróis



Listar heróis



Cadastrar novo herói



Editar herói existente



Excluir herói



Associar múltiplos superpoderes



Exibir superpoderes diretamente na tabela



* Superpoderes



(criar, listar)



Usados para compor o cadastro de heróis



Se nenhum superpoder existir, a tela de heróis sugere o cadastro





* Rascunho automático (UX)



Ao cadastrar um herói:



Se o usuário ainda não possui superpoderes e precisa criar um o sistema salva tudo que já foi preenchido, após criar o superpoder, o usuário retorna ao formulário sem perder nada, 

isso evita retrabalho e melhora muito a experiência.



* Estrutura do projeto

hero-manager/

&nbsp; ├── HeroManager.Api/ → API .NET 8

&nbsp; └── HeroManager.Vue/ → Aplicação Vue 3 + TS





* Como rodar o projeto



Backend – API (.NET 8)



Pré-requisitos:

.NET 8 SDK



* Rodando a API



cd HeroManager.Api

dotnet restore

dotnet run



A API abrirá em algo como:



https://localhost:32769



ou conforme sua porta no launchSettings.json tiver configurada



Swagger:



https://localhost:32769/swagger





* Frontend – Vue 3 + TypeScript



Pré-requisitos:

Node.js 18+

npm ou yarn



* Rodando o projeto

cd HeroManager.Vue

npm install

npm run dev





O frontend abrirá em:



http://localhost:5173





* Configurar URL da API



No arquivo:



HeroManager.Vue/src/api/http.ts





Ajuste:



baseURL: "https://localhost:32769" Obs: Tem que ser a que está sendo apresentada quando você executa sua aplicação local







* Detalhes técnicos



Backend (.NET 8)

ASP.NET Core Web API

DTOs para entrada e saída

Repositórios e serviços organizados

AutoMapper para mapeamento entre entidades e DTOs

CORS liberado para o Vue durante o desenvolvimento

O armazenamento está InMemory, mas a estrutura de dados está criada caso queria executar usando conexão com sqlserver.  





* Endpoints principais



Heróis

GET /api/HeroManager

GET /api/HeroManager/{id}

POST /api/HeroManager/cadastrar-heroi

PUT /api/HeroManager/{id}

DELETE /api/HeroManager/{id}



Exemplo de payload:



{

&nbsp; "nome": "Peter Parker",

&nbsp; "nomeHeroi": "Homem-Aranha",

&nbsp; "dataNascimento": "1995-06-10",

&nbsp; "altura": 1.80,

&nbsp; "peso": 70,

&nbsp; "superpoderIds": \[1, 3]

}



Superpoderes



GET /api/superpowers

POST /api/superpowers





* Frontend (Vue 3 + TS)



Composition API (<script setup>)

State reactivo e tipado

Formulários com validation básica

UX aprimorada com localStorage (rascunho)

Mensagens reais de erro vindas da API (fallback seguro)



* Telas



Lista de Heróis

Design moderno

Badges para superpoderes

Ações rápidas (editar/excluir)

Botões: + Novo Herói e + Novo Superpoder



* Formulário de Herói



Campos:

Nome

Nome de Herói

Data de nascimento

Altura

Peso

Seleção de superpoderes (checkboxes)

Quando não existem superpoderes → botão Cadastrar superpoder



* Superpoderes



Formulário simples e direto para auxiliar no cadastro dos heróis 

Listagem com ações





