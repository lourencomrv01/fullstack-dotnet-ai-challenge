# TaskFlow — Desafio Fullstack .NET · SDD + AI-First

> ⏱️ **Timebox: 20 minutos.** Não é um teste de digitação. É um teste de **como você conduz a IA a partir de uma especificação**.

Você recebe uma aplicação que **já roda**: API .NET 8 (Minimal API) + um front simples servido pela própria API. Falta **uma fatia vertical**: a transição de status de uma tarefa.

O que estamos avaliando não é se você sabe escrever um `switch`. É se você consegue, em 20 minutos:

1. **Especificar antes de codar** (Spec-Driven Development);
2. **Delegar a implementação à IA** (GitHub Copilot, Claude Code, Cursor…) a partir dessa spec;
3. **Verificar** o que a IA produziu contra um contrato executável.

---

## Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- Uma ferramenta de IA de sua preferência: GitHub Copilot, Claude Code, Cursor, Windsurf, Gemini Code Assist…

Verifique que o ambiente está OK **antes** de iniciar o cronômetro:

```bash
dotnet build && dotnet test
```

Esperado: **1 teste passa, 5 falham.** É esse o seu ponto de partida.

Para ver a aplicação:

```bash
dotnet run --project src/TaskFlow.Api
```

Abra a URL exibida no console (algo como `http://localhost:5xxx`).

---

## A tarefa

### O que falta

**Backend** — o endpoint não existe:

```
PATCH /api/tasks/{id}/status
Content-Type: application/json

{ "status": "Doing" }
```

**Frontend** — em `src/TaskFlow.Api/wwwroot/index.html`, o botão de cada tarefa está desligado (`alert('Não implementado')`).

### As regras de negócio

O contrato executável está em [`tests/TaskFlow.Api.Tests/StatusTransitionTests.cs`](tests/TaskFlow.Api.Tests/StatusTransitionTests.cs). **Não altere as asserções** — ajuste a implementação.

| # | Regra | Resposta |
|---|-------|----------|
| R1 | `Todo → Doing` é válido | `200` com o item atualizado |
| R2 | `Todo → Done` é proibido (não se pula etapa) | `409` |
| R3 | Concluir tarefa sem responsável é proibido | `422` |
| R4 | Concluir preenche `completedAt`; reabrir (`Done → Doing`) limpa | `200` |
| R5 | Status desconhecido (ex.: `"Arquivado"`) | `400` |
| R6 | Id inexistente | `404` |

Transições válidas: `Todo → Doing`, `Doing → Done`, `Doing → Todo`, `Done → Doing`.

---

## O fluxo que queremos ver (nesta ordem)

### 1. Spec primeiro — ~5 min

Preencha [`specs/002-status-transition.spec.md`](specs/002-status-transition.spec.md).

Use [`specs/001-create-task.spec.md`](specs/001-create-task.spec.md) como exemplo — é a spec da feature que **já está implementada** no repositório, escrita no mesmo formato que esperamos de você.

> A spec é o artefato de maior peso na avaliação. Uma spec boa é ambígua em nada: contratos, códigos de status, casos de borda e o que está **fora** de escopo.

### 2. IA depois — ~10 min

Com a spec pronta, conduza sua ferramenta de IA para gerar a implementação **a partir dela** — não a partir de uma descrição improvisada no chat.

O repositório já vem configurado para isso:

- [`.github/copilot-instructions.md`](.github/copilot-instructions.md) — contexto automático para o GitHub Copilot
- [`CLAUDE.md`](CLAUDE.md) — contexto automático para o Claude Code

Registre **os prompts que você usou** em [`AI-LOG.md`](AI-LOG.md), incluindo o que a IA errou e como você corrigiu. Um log honesto vale mais que um log bonito.

### 3. Verificação — ~5 min

```bash
dotnet test
```

Os 6 testes verdes. Depois ligue o botão do front e confira no navegador que a mensagem de erro da API aparece quando a transição é recusada.

---

## Entrega

1. Faça um **fork** deste repositório;
2. Commits pequenos e descritivos — o histórico conta a sua história (ex.: `spec: transição de status`, depois `feat: PATCH /status`);
3. Abra um **Pull Request** para este repositório com o título `Desafio — <seu nome>`;
4. Na descrição do PR, responda em 3 linhas: **onde a IA te ajudou mais, onde ela te atrapalhou, e o que você teria feito diferente com mais tempo.**

---

## Regras

- ✅ **Use IA à vontade.** É o ponto do exercício. Não usar é o erro.
- ✅ Vale pesquisar, consultar docs, usar qualquer editor.
- ❌ Não altere os testes em `tests/`.
- ❌ Não instale banco de dados nem troque a stack. O armazenamento em memória é proposital.
- ℹ️ O CI deste repositório está **vermelho de propósito** — os 5 testes falhando são o seu ponto de partida. No seu PR ele deve ficar verde.
- ⏱️ Se os 20 minutos acabarem com algo incompleto: **entregue mesmo assim** e escreva no PR o que faltou e por quê. Entregar parcial com diagnóstico honesto pontua melhor que estourar o tempo.

---

## Como você será avaliado

O detalhamento está em [`EVALUATION.md`](EVALUATION.md). Resumo:

| Critério | Peso |
|---|---|
| Qualidade da spec (clareza, contratos, casos de borda) | 35% |
| Condução da IA (prompts, iteração, log) | 30% |
| Implementação (testes verdes, código idiomático) | 25% |
| Fatia vertical completa (front ligado ao back) | 10% |

Boa sorte. 🚀
