# 001 — Criar tarefa

**Status:** Implementada
**Autor:** Time TaskFlow
**Data:** 2026-01-15

> Esta é a spec da feature que **já existe** no repositório (`POST /api/tasks`).
> Use-a como referência de formato e de nível de detalhe para escrever a sua.

## 1. Problema

Um usuário precisa registrar um trabalho a fazer para que ele apareça no quadro e possa ser acompanhado. Hoje o quadro só é populado por dados de exemplo no startup.

## 2. Fora de escopo

- Persistência durável (o armazenamento é em memória, por decisão de arquitetura desta versão).
- Autenticação e autorização — qualquer chamador pode criar tarefas.
- Edição do título após a criação.
- Validação de que o `assignee` corresponde a um usuário existente (não há cadastro de usuários).

## 3. Contrato

### Requisição

```http
POST /api/tasks
Content-Type: application/json

{ "title": "Escrever a spec", "assignee": "ana" }
```

`assignee` é opcional e aceita `null`.

### Respostas

| Situação | Status | Corpo |
|---|---|---|
| Criada com sucesso | `201 Created` + header `Location: /api/tasks/{id}` | O `WorkItem` criado |
| `title` ausente, vazio ou só espaços | `400 Bad Request` | `ValidationProblemDetails` com a chave `title` |

Formato do `WorkItem`:

```json
{
  "id": "8f1c…",
  "title": "Escrever a spec",
  "status": "Todo",
  "assignee": "ana",
  "createdAt": "2026-01-15T12:00:00+00:00",
  "completedAt": null
}
```

`status` é serializado como **string**, não como número.

## 4. Regras de negócio

- **R1** — Toda tarefa nasce com `status = "Todo"`, independentemente do que o cliente enviar.
- **R2** — `title` é obrigatório; espaços à esquerda e à direita são removidos antes de persistir.
- **R3** — `id` é gerado pelo servidor (GUID v4); um `id` enviado pelo cliente é ignorado.
- **R4** — `createdAt` é registrado pelo servidor em UTC.
- **R5** — `completedAt` nasce `null`.

## 5. Casos de borda

- Título com apenas espaços (`"   "`) → tratado como ausente → `400`.
- Título duplicado → **permitido**; não há restrição de unicidade.
- `assignee` como string vazia → tratado como ausente.
- Corpo JSON malformado → `400`, tratado pelo pipeline do ASP.NET Core.

## 6. Critérios de aceite

- [x] `POST` válido devolve `201` com header `Location` apontando para o recurso criado.
- [x] `GET /api/tasks/{id}` na URL do `Location` devolve a mesma tarefa.
- [x] `POST` sem título devolve `400`.
- [x] A tarefa criada aparece no `GET /api/tasks`, ordenada por `createdAt`.
