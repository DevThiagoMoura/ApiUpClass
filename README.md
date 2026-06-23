# ApiUpClass

API REST para uma plataforma de cursos online, desenvolvida em ASP.NET Core com acesso a banco de dados MySQL.

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

## Como executar

1. Crie o banco usando o script:

```sql
docs/database-upclass.sql
```

2. Configure a conexao local no arquivo `ApiUpClass/appsettings.Development.json`.

Exemplo:

```json
{
  "ConnectionStrings": {
    "mysql": "server=127.0.0.1;port=3306;database=upclass;user=root;password=SUA_SENHA"
  },
  "Jwt": {
    "Key": "upclass-chave-super-secreta-2026",
    "Issuer": "ApiUpClass",
    "Audience": "ApiUpClassUsers"
  }
}
```

3. Abra a solucao `ApiUpClass.sln` no Visual Studio.

4. Execute o projeto.

5. Acesse o Swagger:

```text
https://localhost:7224/swagger
```

ou, se estiver usando HTTP:

```text
http://localhost:5108/swagger
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
