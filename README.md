# BetSecrets ⚽

> Uma API pessoal para organizar campeonatos de futebol amador: times, atletas, rodadas, partidas, eventos e classificação.

## Sobre o projeto

O **BetSecrets** é um projeto pessoal desenvolvido para praticar e consolidar conhecimentos em desenvolvimento back-end com .NET. A proposta é centralizar a gestão de um campeonato, desde o cadastro das entidades até o acompanhamento de jogos, artilharia e tabela de classificação.

O projeto está em evolução contínua e serve como laboratório para aplicar boas práticas de API, organização em camadas, autenticação e persistência de dados.

## Funcionalidades

- Cadastro e gerenciamento de bairros, times e jogadores;
- Vínculo de jogadores aos times e consulta de elenco/histórico;
- Criação de campeonatos, inscrições de times e definição de grupos;
- Organização de rodadas e agendamento de partidas;
- Registro de placares, reagendamentos e eventos de partida;
- Consulta de súmula e artilharia do campeonato;
- Geração e consulta de classificação;
- Cadastro/login de usuários com autenticação JWT;
- Verificação de saúde da API e da conexão com o banco.

## Tecnologias

- **.NET 8 / ASP.NET Core Web API**
- **PostgreSQL**
- **Dapper** e **Npgsql**
- **JWT Bearer Authentication**
- **BCrypt** para proteção de senhas
- **Swagger / OpenAPI** para documentação da API em desenvolvimento
- **Docker** e configuração preparada para deploy no Render

## Organização do código

```text
Controllers/   Endpoints HTTP da API
Services/      Regras e orquestração da aplicação
Repositories/  Queries e acesso ao PostgreSQL
Interfaces/    Contratos de serviços e repositórios
Modelos/       Modelos de domínio, requests e responses
ORM/           Contexto de conexão com o banco de dados
```

## Pré-requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL

## Como executar localmente

1. Clone o repositório e entre na pasta do projeto.

2. Crie o arquivo de configuração local a partir do exemplo:

```bash
copy appsettings.example.json appsettings.json
```

3. Preencha em `appsettings.json` a conexão PostgreSQL e uma chave JWT segura.

4. Restaure as dependências e inicie a API:

```bash
dotnet restore
dotnet run
```

5. Em ambiente de desenvolvimento, acesse a documentação interativa em:

```text
http://localhost:5027/swagger
```

> A porta pode variar conforme a configuração do ambiente.

## Configurações necessárias

| Configuração | Finalidade |
| --- | --- |
| `ConnectionStrings:Postgres` | String de conexão com o banco PostgreSQL. |
| `Jwt:Key` | Chave usada para assinar os tokens JWT. Deve ter ao menos 16 bytes. |
| `Jwt:Issuer` e `Jwt:Audience` | Emissor e público esperado do token. |
| `Cors:AllowedOrigins` | Endereços do front-end autorizados a consumir a API. |
| `ApiFutebol:ApiKey` | Chave opcional para integração com a API Futebol. |

Para publicação, também podem ser utilizadas variáveis de ambiente, por exemplo:

```text
ConnectionStrings__Postgres=...
Jwt__Key=...
CORS_ALLOWED_ORIGINS=https://seu-front.com
PORT=8080
```

## Principais endpoints

Os endpoints protegidos exigem um token JWT no cabeçalho `Authorization: Bearer <token>`.

| Recurso | Exemplos de rotas |
| --- | --- |
| Saúde | `GET /health`, `GET /health/db` |
| Bairros | `POST /api/Bairro/cadastrar-bairro`, `GET /api/Bairro/listar-bairros` |
| Times | `POST /api/Time/cadastrar-time`, `GET /api/Time/listar-times` |
| Jogadores | `POST /api/Jogador/cadastra-jogador`, `GET /api/Jogador/buscar-jogadores` |
| Campeonatos | `POST /api/Campeonato/cadastrar-campeonato`, `GET /api/Campeonato/listar-campeonatos` |
| Rodadas | `POST /api/Rodada/cadastrar-rodada`, `GET /api/Rodada/listar-rodadas-campeonato/{campeonatoId}` |
| Partidas | `POST /api/Partida/cadastrar-partida`, `PUT /api/Partida/registrar-resultado/{id}` |
| Eventos | `POST /api/PartidaEvento/cadastrar-partida-evento`, `GET /api/PartidaEvento/listar-artilharia/{campeonatoId}` |
| Classificação | `GET /api/Classificacao/listar-classificacao/{campeonatoId}` |

Consulte o Swagger para a relação completa de rotas, parâmetros e modelos.

## Status

Este é um **projeto pessoal em desenvolvimento**. Novas funcionalidades, melhorias de validação, testes automatizados e documentação serão incorporados conforme a evolução do projeto.

---

Feito com dedicação para aprender, construir e acompanhar o futebol amador. ⚽
