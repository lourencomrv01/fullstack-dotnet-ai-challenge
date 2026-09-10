# Guia de avaliação

> Documento público de propósito: você deve saber exatamente o que está sendo medido.
> O detalhamento específico da feature fica com o avaliador — o que está aqui vale para qualquer uma.

## O que interessa

**Não esperamos tudo pronto.** O sinal que buscamos é a **ordem de trabalho** (spec → IA → verificação)
e a **capacidade de julgar o output da IA** — não a quantidade de código.

Uma spec excelente, parte dos testes verdes e um `AI-LOG.md` honesto vale mais que todos os testes
verdes obtidos colando um bloco gerado, sem spec e sem log.

## Rubrica

### 1. Qualidade da spec — 35%

| Nível | Sinal |
|---|---|
| **Forte** | Contratos explícitos (status HTTP por situação); regras numeradas e testáveis; seção *Fora de escopo* com decisões reais; **identificou ambiguidades do enunciado e as resolveu por escrito** |
| **Médio** | Regras corretas, mas parafraseando o enunciado; casos de borda genéricos; *Fora de escopo* vazio ou trivial |
| **Fraco** | Spec preenchida depois do código, ou copiada do enunciado, ou em branco |

> 💡 **O enunciado que você receber terá ambiguidades deliberadas** — situações que os testes não
> cobrem e que o texto não decide. Encontrá-las e **documentar a sua decisão** é o que separa
> o nível forte do médio. Qualquer decisão razoável é aceita; o que não é aceito é não perceber.

### 2. Condução da IA — 30%

| Nível | Sinal |
|---|---|
| **Forte** | Prompts que **referenciam a spec e os testes como fonte de verdade**; iteração visível; o log mostra um erro da IA que você **pegou** |
| **Médio** | Prompts razoáveis, mas descritivos ("crie um endpoint que faz X"); log superficial |
| **Fraco** | Log vazio, um único prompt genérico, ou não ter usado IA |

> ⚠️ **Log que só mostra acerto é sinal de alerta.** Trabalhando com IA, algo sempre sai errado.
> Um log sem nenhum atrito costuma ser log escrito no fim, de memória.

### 3. Implementação — 25%

- Testes do contrato verdes. **Parcial pontua proporcionalmente.**
- Os testes recebidos **não** foram alterados.
- Código idiomático de Minimal API: retorno via `Results.*`, `ProblemDetails` para erros de negócio, sem `try/catch` genérico engolindo exceção.
- Regras de negócio concentradas e legíveis, não espalhadas em `if`s aninhados.
- **A ordem das validações foi pensada** (qual erro ganha quando duas condições falham ao mesmo tempo?).

### 4. Fatia vertical — 10%

- O front exercita o que você implementou no backend.
- Erro devolvido pela API aparece para o usuário, não morre no console.

## Sinais fora da rubrica

**Positivos**
- Commits separando `spec:` de `feat:` — mostra que a ordem de trabalho foi real, não encenada.
- Dizer o que ficou de fora e por quê.
- Perguntar algo sobre o enunciado antes de começar.

**Negativos**
- Insistir em "terminar tudo" em vez de priorizar.
- Reescrever partes do scaffold que não precisavam mudar.
- Adicionar banco de dados ou trocar a stack apesar da regra explícita.

## As perguntas da conversa

Publicadas de propósito. Pense nelas enquanto trabalha:

1. Me mostre um prompt que **não** funcionou. O que você mudou?
2. A IA gerou algo que você não teria escrito à mão? Manteve ou reescreveu? Por quê?
3. **Que ambiguidade do enunciado você encontrou?**
4. Se isso fosse para produção com muitos usuários simultâneos, o que quebra primeiro?
5. Onde você **não** deixaria a IA decidir sozinha neste código?
