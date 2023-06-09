# DapperDemo

Este projeto tem como objetivo demonstrar como realizar operações de leitura e escrita em um banco de dados SQL Server usando ASP.NET Core 6 e o micro ORM Dapper.

## Estrutura do Projeto

O projeto é composto por duas partes principais:

### Runner

Aplicação console em ASP.NET Core que executa consultas no console.

### DataLayer

Camada de dados que contém as entidades que representam as tabelas do banco e as classes repository responsáveis por executar as consultas.

## Requisitos

- ASP.NET Core 6 SDK
- Visual Studio 2022 (ou outra versão compatível)
- Docker

## Configuração do Ambiente

Antes de executar o projeto, é necessário configurar o ambiente:

1. Abra a solução `DapperDemo.sln` no Visual Studio 2022.

2. Execute o arquivo `docker-compose.yml` usando o seguinte comando no terminal: "docker-compose down -v"


Esse comando iniciará uma instância do SQL Server usando Docker.

3. Todos os scripts para criação do banco de dados, tabelas e procedures estão na pasta `sql-scripts`. Execute esses scripts no banco de dados para criar a estrutura necessária.

## Executando o Projeto

1. Abra a solução `DapperDemo.sln` no Visual Studio 2022.

2. Certifique-se de que o projeto de inicialização seja o projeto `Runner`.

3. Execute o projeto pressionando F5 ou clique em "Iniciar" no Visual Studio.

4. O console do projeto `Runner` será exibido e você poderá interagir com as operações de leitura e escrita no banco de dados.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir um problema ou enviar uma solicitação de pull.

## Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
