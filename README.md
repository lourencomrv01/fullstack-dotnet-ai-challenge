# TaskFlow — Desafio Fullstack .NET · SDD + AI-First

> **Não é um teste de digitação.** É um teste de como você conduz a IA a partir de uma especificação.

Este repositório é a **preparação** para a sessão. Ele não contém o desafio — contém o ambiente, o método e os critérios de avaliação. **A feature que você vai implementar é entregue no início da sessão.**

Leia tudo com antecedência e deixe a máquina pronta. Nada aqui é surpresa: quanto melhor você chegar preparado, mais a sessão fala sobre o seu raciocínio e menos sobre instalação de SDK.

---

## 1. Prepare o ambiente (faça isto antes)

Você vai precisar de:

- [.NET SDK 8.0 ou superior](https://dotnet.microsoft.com/download)
- Uma ferramenta de IA de sua preferência, **já autenticada e funcionando**: GitHub Copilot, Claude Code, Cursor, Windsurf, Gemini Code Assist… a que você usa no dia a dia

Clone e valide:

```bash
dotnet build && dotnet test
```

Esperado: **3 testes verdes.** São testes de fumaça — existem só para provar que o ambiente está de pé.

Suba a aplicação:

```bash
dotnet run --project src/TaskFlow.Api
```

Abra a URL do console (algo como `http://localhost:5xxx`). Você verá um quadro de tarefas simples: lista o que existe e cria tarefas novas.

> Se algo falhar aqui, resolva **antes** da sessão. Se não conseguir, nos avise — a gente ajuda.

## 2. Entenda como trabalhamos

Nossos times usam **Spec-Driven Development** e desenvolvimento **AI-first**. Na prática, a ordem é sempre:

> **spec → contrato executável → implementação**, nunca o inverso.

E, sobre a IA: **use à vontade. É o ponto do exercício — não usar é o erro.** O que avaliamos é a sua capacidade de conduzir e auditar a ferramenta, não a sua memória de sintaxe.

O repositório já vem configurado para isso:

- [`.github/copilot-instructions.md`](.github/copilot-instructions.md) — contexto automático para o GitHub Copilot
- [`CLAUDE.md`](CLAUDE.md) — contexto automático para o Claude Code

Vale abrir sua ferramenta neste repositório antes da sessão só para confirmar que ela está lendo esses arquivos.

## 3. Estude o formato de spec

Leia [`specs/001-create-task.spec.md`](specs/001-create-task.spec.md). É a spec da feature que **já está implementada** aqui (`POST /api/tasks`), escrita exatamente no formato que esperamos de você.

O template em branco está em [`specs/000-template.spec.md`](specs/000-template.spec.md).

> A spec é o artefato de maior peso na avaliação. Uma spec boa não é longa — é **inequívoca**: contratos, códigos de status, casos de borda e o que está deliberadamente **fora** de escopo.

## 4. Conheça o log de IA

Durante a sessão você vai registrar os prompts que usou em [`AI-LOG.md`](AI-LOG.md). Dê uma olhada no formato agora.

Uma coisa importante: **queremos um log honesto, não um log bonito.** Quando a IA errar, mostre o erro e como você percebeu. Saber desconfiar do output é exatamente a habilidade que estamos medindo.

## 5. Saiba como será avaliado

Está tudo aberto em [`EVALUATION.md`](EVALUATION.md) — inclusive as perguntas que faremos depois do exercício. Não há pegadinha.

| Critério | Peso |
|---|---|
| Qualidade da spec | 35% |
| Condução da IA | 30% |
| Implementação | 25% |
| Fatia vertical completa (front ligado ao back) | 10% |

---

## Como será a sessão

Uma conversa remota com a tela compartilhada. Você recebe o enunciado da feature — um arquivo de testes que é o contrato executável e um stub de spec — e trabalha nela normalmente, com sua IA, do seu jeito. Depois conversamos sobre o que você fez.

**Não esperamos que tudo fique pronto.** Uma entrega parcial com um diagnóstico honesto do que faltou vale mais do que uma entrega completa sem spec e sem log. Priorizar faz parte.

## Regras

- ✅ Use IA à vontade, consulte documentação, use o editor que quiser.
- ❌ Não altere os arquivos de teste que você receber — eles são o contrato.
- ❌ Não adicione banco de dados nem troque a stack. O armazenamento em memória é proposital.

## O que NÃO avaliamos

- Algoritmo, estrutura de dados, whiteboard.
- Memória de sintaxe — consulte o que precisar.
- Volume de código produzido.

---

## Estrutura

```
src/TaskFlow.Api/
  Program.cs              # endpoints (Minimal API)
  Domain/WorkItem.cs      # modelo e enum de status
  Storage/InMemoryStore.cs
  wwwroot/index.html      # front em HTML + JS puro, sem build step
specs/                    # especificações
tests/                    # contrato executável
```

Até lá. 🚀
