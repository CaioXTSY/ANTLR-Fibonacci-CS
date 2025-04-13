# ANTLR-Fibonacci-CS

Este é um projeto desenvolvido como parte da disciplina de Compiladores, ministrada pelo Professor Alexandre Paes na Universidade Federal de Alagoas - Campus Arapiraca.

O projeto utiliza [ANTLR4](https://www.antlr.org/) para criar um analisador (lexer + parser) que reconhece e processa expressões do tipo `fib(n)` em C#, incluindo um visitor para calcular o número de Fibonacci correspondente.

### Equipe
- Caio Teixeira da Silva
- Noemy Torres Pereira
- Gustavo Henrique dos Santos Malaquias

## Estrutura do Projeto

```
ANTLR-Fibonacci-CS/
│   ANTLR-Fibonacci-CS.csproj   # Arquivo de projeto (.NET 9.0)
│   Program.cs                  # Ponto de entrada do console (exemplo de uso)
│
├── .vscode/
│   └── settings.json           # Configurações para a extensão ANTLR4 (auto-geração etc.)
│
├── .antlr/                     # Pasta onde a extensão salva arquivos auxiliares (.tokens, .interp)
│
├── Grammar/                    # Pasta contendo a(s) gramática(s) do ANTLR
│   ├── Fibonacci.g4            # Gramática definindo "fib(n)" e tokens (NUMBER, WS, etc.)
│   ├── FibonacciBaseVisitor.cs # (Opcional) Pode ser gerado se a extensão estiver configurada
│   ├── FibonacciParser.cs      # Arquivo gerado (parser)
│   ├── FibonacciLexer.cs       # Arquivo gerado (lexer)
│   └── Fibonacci.tokens        # Tokens do ANTLR
│
├── Generated/                  # Pasta onde podem ser movidos os arquivos gerados pelo ANTLR
│   ├── FibonacciLexer.cs
│   ├── FibonacciParser.cs
│   ├── FibonacciBaseVisitor.cs
│   ├── FibonacciVisitors.cs
│   └── ...
│
├── bin/                        # Binários gerados (pasta de build)
├── obj/                        # Arquivos temporários do build
└── ANTLR-Fibonacci-Sln.sln     # Solução para Visual Studio
```

### Observações Importantes

1. **Pasta Grammar**:  
   - Aqui fica o arquivo `Fibonacci.g4`, que define a gramática do ANTLR para reconhecer `fib(n)`.  
   - A extensão do VSCode pode gerar automaticamente o lexer (FibonacciLexer.cs), parser (FibonacciParser.cs) e outras classes (por exemplo, FibonacciBaseVisitor.cs) nessa mesma pasta ou na `.antlr`.

2. **Pasta Generated** (Opcional):  
   - Dependendo da configuração, a extensão do VSCode ou o comando do ANTLR pode gerar os arquivos diretamente nessa pasta.

3. **Program.cs**:  
   - Exemplo simples de como usar o parser e o visitor para calcular Fibonacci.  
   - Cria o lexer e parser a partir de uma string fixa (`fib(10)`), depois instancia o visitor personalizado.

4. **ANTLR-Fibonacci-CS.csproj**:  
   - Arquivo de projeto que define as configurações de build, incluindo a versão do .NET (9.0) e as dependências (por exemplo, o pacote `Antlr4.Runtime.Standard`).

5. **.vscode/settings.json**:  
   - Configurações específicas do VSCode para que a extensão ANTLR4 gere o código C# ao salvar a gramática.  
   - Exemplos de configurações: `"mode": "external"`, `"language": "CSharp"`, `"visitors": true`, etc.

6. **.antlr**:  
   - Pasta em que a extensão ANTLR do VSCode normalmente salva arquivos auxiliares, como `.tokens`, `.interp`, entre outros, usados para análise e visualização da gramática.

7. **bin/** e **obj/**:  
   - Pastas padrão de compilação do .NET, onde ficam os artefatos de build (executáveis, assemblies, etc.).  
   - Geralmente são ignoradas no controle de versão via `.gitignore`.

---

## Funcionamento do Projeto

1. **Gramática (Fibonacci.g4)**  
   - Define duas regras principais: `prog` (que termina em EOF) e `expr` (reconhecendo `fib(n)`).  
   - Cria tokens: `NUMBER` (para inteiros), e `WS` (para espaços e quebras de linha).  

2. **Lexer e Parser**  
   - O lexer (FibonacciLexer.cs) converte a string de entrada em tokens.  
   - O parser (FibonacciParser.cs) cria a árvore de sintaxe (parse tree) usando as regras definidas na gramática.

3. **Visitor para Cálculo de Fibonacci**  
   - A classe `FibonacciVisitor` (seja em `Generated/` ou em outro local) herda de `FibonacciBaseVisitor<int>` e sobrescreve os métodos de visita às regras (por exemplo, `VisitExpr`).  
   - Quando encontra `fib(n)`, converte `n` para inteiro e faz o cálculo usando um método `Fib(n)`, retornando o resultado.

4. **Program.cs**  
   - Pega a string de entrada, instancia `FibonacciLexer` e `FibonacciParser`, e chama `parser.prog()`.  
   - Em seguida, cria o `FibonacciVisitor` e executa `visitor.Visit(tree)`.  
   - Exibe no console o resultado de `fib(n)`.

---

## Como Usar

1. **Abra o projeto no Visual Studio Code**.  
2. **Verifique as configurações do ANTLR** em `.vscode/settings.json`. Se quiser que a geração seja automática, mantenha `"autoGenerate": true`.  
3. **Edite** o arquivo `Fibonacci.g4` conforme necessário para expandir a gramática.  
4. **Compile e Execute** via terminal:
   ```bash
   dotnet build
   dotnet run
   ```
   Por padrão, o projeto executa `Program.cs`, que calcula `fib(10)` e exibe o resultado.

---