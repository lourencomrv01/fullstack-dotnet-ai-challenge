# Instruções para o GitHub Copilot — TaskFlow

Este repositório trabalha em **Spec-Driven Development**. A ordem é sempre:

> **spec → teste → implementação**, nunca o inverso.

## Fonte de verdade

Antes de propor código para qualquer feature, leia nesta ordem:

1. `specs/<id>-<nome>.spec.md` — a especificação da feature. É a autoridade sobre comportamento.
2. `tests/TaskFlow.Api.Tests/` — o contrato executável. Os testes traduzem a spec 1:1.
3. O código em `src/`.

Se a spec e o código divergirem, **a spec está certa e o código está errado**.
Se a spec estiver incompleta, **diga o que falta em vez de inventar comportamento.**

## Regras deste repositório

- **Nunca altere arquivos em `tests/`.** Eles são o contrato do desafio.
- Não adicione dependências novas, banco de dados ou camadas de abstração. O armazenamento em memória (`InMemoryStore`) é uma decisão deliberada.
- Não troque Minimal API por Controllers.

## Convenções de código

- .NET 8, Minimal API, C# com nullable habilitado.
- Endpoints retornam `Results.*` — `Results.Ok`, `Results.NotFound`, `Results.Problem(statusCode: …)`, `Results.ValidationProblem`.
- Erros de negócio viram `ProblemDetails` com o status HTTP correto. Nada de `try/catch` genérico devolvendo `500`.
- Enums são serializados como **string** (`JsonStringEnumConverter` já está configurado).
- Máquinas de estado se expressam em um único `switch` de expressão sobre a tupla `(estadoAtual, estadoAlvo)` — legível de cima a baixo, não espalhado em `if`s aninhados.

## Ao gerar uma implementação

Diga sempre **qual regra da spec** cada trecho atende (ex.: `// R2 — não se pula etapa`).
Se você precisou assumir algo que a spec não cobre, sinalize explicitamente na resposta.
