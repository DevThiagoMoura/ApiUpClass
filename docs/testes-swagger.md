# Roteiro de testes pelo Swagger

Use este roteiro depois de executar o script `docs/database-upclass.sql` e iniciar a API.

Swagger:

```text
https://localhost:7224/swagger
```

## 1. Status da API

```http
GET /
```

Resposta esperada:

```json
{
  "api": "ApiUpClass",
  "status": "up"
}
```

## 2. Criar usuario instrutor

```http
POST /usuarios
```

Body:

```json
{
  "nome": "Instrutor UpClass",
  "email": "instrutor@upclass.com",
  "senha": "123456",
  "papel": "instrutor"
}
```

## 3. Criar usuario aluno

```http
POST /usuarios
```

Body:

```json
{
  "nome": "Aluno UpClass",
  "email": "aluno@upclass.com",
  "senha": "123456",
  "papel": "aluno"
}
```

## 4. Login como instrutor

```http
POST /auth/login
```

Body:

```json
{
  "email": "instrutor@upclass.com",
  "senha": "123456"
}
```

Copie o token e clique em `Authorize` no Swagger.

Formato:

```text
Bearer SEU_TOKEN
```

## 5. Criar categoria

```http
POST /categorias
```

Body:

```json
{
  "nome": "DevOps",
  "descricao": "Cursos sobre automacao e entrega de software"
}
```

## 6. Criar curso

Use um `categoriaId` existente.

```http
POST /cursos
```

Body:

```json
{
  "titulo": "APIs REST com .NET",
  "descricao": "Curso pratico de criacao de APIs REST",
  "preco": 129.90,
  "categoriaId": 1
}
```

## 7. Criar modulo

Use um `cursoId` existente.

```http
POST /modulos
```

Body:

```json
{
  "titulo": "Primeiros passos com API",
  "ordem": 1,
  "cursoId": 1
}
```

## 8. Criar aula

Use um `moduloId` existente.

```http
POST /aulas
```

Body:

```json
{
  "titulo": "Criando o primeiro controller",
  "moduloId": 1,
  "duracao": 30,
  "urlVideo": "https://video.local/primeiro-controller"
}
```

## 9. Criar tags

```http
POST /tags
```

Body:

```json
{
  "nome": "dotnet"
}
```

Repita para outra tag:

```json
{
  "nome": "api"
}
```

## 10. Associar tags ao curso em lote

```http
POST /cursos/1/tags
```

Body:

```json
{
  "ids": [1, 2]
}
```

Resposta esperada: curso retornando a lista `tags`.

## 11. Consultas publicas de curso

```http
GET /cursos
GET /cursos/ativos
GET /cursos/categoria/1
GET /cursos/tag/1
GET /cursos/1/modulos
```

## 12. Login como aluno

```http
POST /auth/login
```

Body:

```json
{
  "email": "aluno@upclass.com",
  "senha": "123456"
}
```

Atualize o token no Swagger com o token do aluno.

## 13. Testar pagamento sem matricula

```http
POST /pagamentos
```

Body:

```json
{
  "usuarioId": 2,
  "cursoId": 1,
  "valor": 99.90
}
```

Resposta esperada:

```json
{
  "message": "O usuario precisa estar matriculado no curso para realizar pagamento"
}
```

Status esperado: `409`.

## 14. Criar matricula

```http
POST /matriculas
```

Body:

```json
{
  "usuarioId": 2,
  "cursoId": 1
}
```

Resposta esperada: matricula com status `ativo`.

## 15. Criar pagamento

```http
POST /pagamentos
```

Body:

```json
{
  "usuarioId": 2,
  "cursoId": 1,
  "valor": 99.90
}
```

Resposta esperada: pagamento com status `pendente`.

## 16. Criar avaliacao

```http
POST /avaliacoes
```

Body:

```json
{
  "usuarioId": 2,
  "cursoId": 1,
  "nota": 5,
  "comentario": "Curso muito bom para aprender APIs."
}
```

Resposta esperada: avaliacao cadastrada.

## 17. Testar duplicidades

Tente repetir:

```http
POST /matriculas
POST /avaliacoes
```

Resposta esperada: erro `409`, pois o usuario ja esta matriculado ou ja avaliou o curso.

## Fluxo resumido para demonstracao

1. `GET /`
2. `POST /usuarios` instrutor
3. `POST /usuarios` aluno
4. `POST /auth/login` instrutor
5. `POST /cursos`
6. `POST /modulos`
7. `POST /aulas`
8. `POST /tags`
9. `POST /cursos/{id}/tags`
10. `GET /cursos`
11. `POST /auth/login` aluno
12. `POST /matriculas`
13. `POST /pagamentos`
14. `POST /avaliacoes`
