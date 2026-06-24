# ApiUpClass

API REST para uma plataforma de cursos online, desenvolvida em ASP.NET Core com acesso a banco de dados MySQL.

## Desenvolvedores

- Thiago Moura de Carvalho
- Daniélly Bernardino Batista

## Objetivo

O projeto permite gerenciar usuarios, categorias, cursos, modulos, aulas, matriculas, pagamentos, tags e avaliacoes.

O padrao principal segue a estrutura usada em aula:

- `Controllers`: recebem as requisicoes HTTP.
- `Services`: concentram regras de negocio e acesso ao banco.
- `Models`: representam as tabelas do banco.
- `Dtos`: representam dados de entrada.
- `Dtos/Responses`: representam dados de saida da API.
- `Profiles`: configuram os mapeamentos do AutoMapper.
- `DataContexts`: configuram o Entity Framework Core.

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Pomelo EntityFrameworkCore MySql
- MySQL
- AutoMapper
- JWT Bearer
- Swagger

## Como utilizar em outro ambiente

Para executar o projeto em outro computador, garanta que o ambiente tenha:

- .NET 8 SDK
- Visual Studio 2022 ou VS Code
- MySQL Server em execucao
- MySQL Workbench ou outro cliente SQL

Depois siga os passos abaixo.

### 1. Clonar o repositorio

```bash
git clone URL_DO_REPOSITORIO
cd ApiUpClass
```

Abra a solucao no Visual Studio:

```text
ApiUpClass.sln
```

### 2. Restaurar pacotes

Na pasta da solucao, execute:

```bash
dotnet restore
```

Tambem e possivel restaurar os pacotes pelo Visual Studio.

### 3. Criar o banco de dados

Crie o banco usando o script:

```sql
docs/database-upclass.sql
```

Esse script cria o banco `upclass`, as tabelas e os dados iniciais usados nos testes.

### 4. Configurar a conexao local

Crie ou ajuste o arquivo:

```text
ApiUpClass/appsettings.Development.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "mysql": "server=127.0.0.1;port=3306;database=upclass;user=root;password=SUA_SENHA;SslMode=None"
  },
  "Jwt": {
    "Key": "chave-local-de-desenvolvimento-com-tamanho-suficiente",
    "Issuer": "ApiUpClass",
    "Audience": "ApiUpClassUsers"
  }
}
```

Troque `SUA_SENHA` pela senha do MySQL da maquina local.

Importante: o arquivo `appsettings.Development.json` contem configuracoes locais e nao deve ser enviado ao Git. O arquivo `appsettings.json` deve ficar sem senha real.

### 5. Executar o projeto

Pelo Visual Studio:

- selecione o perfil `https`;
- execute o projeto.

Pelo terminal:

```bash
cd ApiUpClass/ApiUpClass
dotnet run
```

### 6. Acessar o Swagger

```text
https://localhost:7224/swagger
```

ou, se estiver usando HTTP:

```text
http://localhost:5108/swagger
```

### 7. Fluxo basico de teste

1. Criar um usuario em `POST /usuarios`.
2. Fazer login em `POST /auth/login`.
3. Copiar o token JWT retornado.
4. Clicar em `Authorize` no Swagger.
5. Informar o token no formato `Bearer SEU_TOKEN`.
6. Testar as rotas protegidas conforme o perfil do usuario.

### Execucao rapida por terminal

Caso o banco ja esteja criado e o `appsettings.Development.json` ja esteja configurado:

```bash
dotnet restore
dotnet build
cd ApiUpClass/ApiUpClass
dotnet run
```

## Autenticacao

A API usa JWT.

Crie primeiro um usuario em:

```http
POST /usuarios
```

Depois faca login em:

```http
POST /auth/login
```

No Swagger, clique em `Authorize` e informe:

```text
Bearer SEU_TOKEN
```

## Perfis

Perfis usados no campo `papel` do usuario:

- `aluno`
- `instrutor`
- `administrador`

Rotas de criacao de curso, modulo, aula e associacao de tags a curso exigem perfil `instrutor`.

Rotas de matricula, pagamento e avaliacao exigem perfil `aluno`.

## Rotas principais

Status:

- `GET /`

Autenticacao:

- `POST /auth/login`

Usuarios:

- `GET /usuarios`
- `GET /usuarios/{id}`
- `POST /usuarios`
- `PUT /usuarios/{id}`
- `DELETE /usuarios/{id}`

Categorias:

- `GET /categorias`
- `GET /categorias/{id}`
- `POST /categorias`
- `PUT /categorias/{id}`
- `DELETE /categorias/{id}`

Cursos:

- `GET /cursos`
- `GET /cursos/{id}`
- `GET /cursos/ativos`
- `GET /cursos/categoria/{categoriaId}`
- `GET /cursos/tag/{tagId}`
- `GET /cursos/{id}/modulos`
- `POST /cursos`
- `POST /cursos/{id}/tags`
- `PUT /cursos/{id}`
- `DELETE /cursos/{id}`

Modulos:

- `GET /modulos`
- `GET /modulos/{id}`
- `POST /modulos`
- `PUT /modulos/{id}`
- `DELETE /modulos/{id}`

Aulas:

- `GET /aulas`
- `GET /aulas/{id}`
- `POST /aulas`
- `PUT /aulas/{id}`
- `DELETE /aulas/{id}`

Matriculas:

- `GET /matriculas`
- `GET /matriculas/{id}`
- `POST /matriculas`
- `PUT /matriculas/{id}`
- `DELETE /matriculas/{id}`

Pagamentos:

- `GET /pagamentos`
- `GET /pagamentos/{id}`
- `POST /pagamentos`
- `PUT /pagamentos/{id}`
- `DELETE /pagamentos/{id}`

Avaliacoes:

- `GET /avaliacoes`
- `GET /avaliacoes/{id}`
- `POST /avaliacoes`
- `PUT /avaliacoes/{id}`
- `DELETE /avaliacoes/{id}`

Tags:

- `GET /tags`
- `GET /tags/{id}`
- `POST /tags`
- `PUT /tags/{id}`
- `DELETE /tags/{id}`

## Regras de negocio implementadas

- Usuario tem senha protegida com `PasswordHasher`.
- Login retorna token JWT.
- Curso recebe status ativo na criacao.
- Matricula recebe status ativo na criacao.
- Pagamento recebe status pendente na criacao.
- Avaliacao recebe data de criacao automaticamente.
- Nao permite matricula duplicada para o mesmo usuario e curso.
- Nao permite avaliacao duplicada para o mesmo usuario e curso.
- Avaliacao so e permitida se o usuario estiver matriculado no curso.
- Pagamento so e permitido se o usuario estiver matriculado no curso.
- Associacao de tags ao curso evita duplicidade.

## Arquivos de apoio

- `docs/database-upclass.sql`: script para criar banco, tabelas e dados iniciais.
- `docs/testes-swagger.md`: roteiro de testes manual pelo Swagger.

## Observacoes para Git

O arquivo `appsettings.Development.json` deve ficar fora do Git, pois contem configuracao local do banco.

Antes de enviar:

```powershell
git status
git add .
git commit -m "Atualiza documentacao e fluxo de testes"
git push
```
