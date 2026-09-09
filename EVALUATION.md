# Guia de avaliação (para o avaliador)

> Documento público de propósito: o candidato deve saber exatamente o que está sendo medido.

## Contexto

Timebox de 20 minutos. **Não espere tudo pronto.** O sinal que interessa é a **ordem de trabalho**
(spec → IA → verificação) e a **capacidade de julgar o output da IA** — não a quantidade de código.

Um candidato que entrega uma spec excelente, 4 de 6 testes verdes e um AI-LOG honesto vale mais
que um que entrega 6 testes verdes colando um bloco gerado sem spec e sem log.

## Rubrica

### 1. Qualidade da spec — 35%

| Nível | Sinal |
|---|---|
| **Forte** | Contratos explícitos (status HTTP por situação); regras numeradas e testáveis; seção *Fora de escopo* preenchida com decisões reais; **identificou ambiguidades do enunciado e as resolveu por escrito** |
| **Médio** | Regras corretas mas parafraseando o README; casos de borda genéricos; *Fora de escopo* vazio ou trivial |
| **Fraco** | Spec preenchida depois do código, ou copiada da tabela do README, ou em branco |

**Ambiguidades plantadas no enunciado** (o candidato forte deve notar ao menos uma):

1. **Transição para o mesmo status** (`Doing → Doing`) não está especificada. É `200` idempotente ou `409`? Qualquer decisão é aceita **se estiver documentada**. Nenhum teste cobre isso — de propósito.
2. **`assignee` vazio vs. `null`.** A regra R3 diz "sem responsável". Uma string `"  "` conta como sem responsável? A spec 001 dá a pista, mas exige que o candidato conecte os pontos.
3. **`Doing → Todo`** é listada como válida, mas o enunciado não diz o que acontece com `completedAt` — a tarefa nunca foi concluída, então não há o que limpar. Trivial, mas quem escreve a matriz completa de transições nota.

### 2. Condução da IA — 30%

| Nível | Sinal |
|---|---|
| **Forte** | Prompts que **referenciam a spec e os testes como fonte de verdade** ("implemente conforme `specs/002…`, os testes em `StatusTransitionTests.cs` são o contrato"); iteração visível; log mostra um erro da IA que o candidato **pegou** |
| **Médio** | Prompts razoáveis mas descritivos ("crie um endpoint PATCH que muda status"); log preenchido de forma superficial |
| **Fraco** | Log vazio, ou um único prompt genérico, ou negação de ter usado IA |

⚠️ **Sinal de alerta:** log que só mostra sucesso. Em 20 minutos com IA, algo sempre sai errado. Um log sem nenhum atrito geralmente é log escrito no fim, de memória.

### 3. Implementação — 25%

- `dotnet test` verde (6/6). **Parcial pontua proporcionalmente.**
- Testes **não** foram alterados (verifique o diff em `tests/`).
- Código idiomático de Minimal API: retorno via `Results.*`, `Problem`/`ProblemDetails` para erros, sem `try/catch` genérico engolindo exceção.
- A matriz de transições está em um só lugar, legível — não espalhada em `if`s aninhados.
- **Ordem das validações importa:** `404` (não existe) deve vir antes de `409` (transição inválida)? Ou o `400` de status inválido vem antes de tudo? Não há resposta única; observe se o candidato pensou nisso.

### 4. Fatia vertical — 10%

- Botão do front chamando o `PATCH` e recarregando a lista.
- Erro da API (`409`/`422`) exibido ao usuário em `#error`, não engolido no console.

## Sinais fora da rubrica (anotar, não pontuar diretamente)

**Positivos**
- Commits separando `spec:` de `feat:` — mostra que a ordem de trabalho foi real, não encenada.
- PR descrevendo o que ficou de fora e por quê.
- Perguntou algo sobre o enunciado antes de começar.

**Negativos**
- Estourou muito o timebox para "terminar tudo" — em produção, esse é o candidato que não negocia escopo.
- Reescreveu partes do scaffold que não precisavam mudar.
- Instalou banco de dados ou trocou a stack apesar da regra explícita.

## Roteiro sugerido de conversa (15 min pós-teste)

1. "Me mostre um prompt que **não** funcionou. O que você mudou?"
2. "A IA gerou algo que você não teria escrito à mão? Manteve ou reescreveu? Por quê?"
3. "Que ambiguidade do enunciado você encontrou?" *(a melhor pergunta do roteiro)*
4. "Se isso fosse para produção com 10 mil tarefas e múltiplos usuários, o que quebra primeiro?" *(esperado: armazenamento em memória, concorrência no `WorkItem`, ausência de otimistic locking)*
5. "Onde você **não** deixaria a IA decidir sozinha?"
