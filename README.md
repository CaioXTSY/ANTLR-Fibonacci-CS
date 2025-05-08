# ANTLR-Fibonacci-CS

Este é um projeto desenvolvido como parte da disciplina de Compiladores, ministrada pelo Professor Alexandre Paes na Universidade Federal de Alagoas - Campus Arapiraca.

O projeto utiliza [ANTLR4](https://www.antlr.org/) para criar um analisador (lexer + parser) que reconhece e processa expressões do tipo `fib(n)` em C#, incluindo um visitor para calcular o número de Fibonacci correspondente.

### Equipe
- Caio Teixeira da Silva
- Noemy Torres Pereira
- Gustavo Henrique dos Santos Malaquias

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

## Gramática ANTLR4

```antlr
grammar Fibonacci;

options {
    language = CSharp;
}

// Regra inicial – a expressão seguida do fim da entrada
prog: expr EOF ;

// Regra para reconhecer "fib(n)", onde n é um número
expr: 'fib' '(' NUMBER ')' ;

// Token para números inteiros
NUMBER: [0-9]+ ;

// Ignora espaços, quebras de linha e tabulações
WS: [ \t\r\n]+ -> skip ;
```

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

### Observações

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
    - Configurações específicas do VSCode para que a extensão ANTLR4 gere o código C# ao salvar a gramática:
      ```json
      {
            "antlr4.generation": {
                 "mode": "external",
                 "language": "CSharp",
                 "listeners": false,
                 "visitors": true
            },
            "antlr4.generation.autoGenerate": true
      }
      ```
    - `mode: "external"`: Gera arquivos fora da pasta .antlr
    - `visitors: true`: Habilita geração de visitors
    - `autoGenerate: true`: Gera código automaticamente ao salvar

6. **.antlr**:  
   - Pasta em que a extensão ANTLR do VSCode normalmente salva arquivos auxiliares, como `.tokens`, `.interp`, entre outros, usados para análise e visualização da gramática.

7. **bin/** e **obj/**:  
   - Pastas padrão de compilação do .NET, onde ficam os artefatos de build (executáveis, assemblies, etc.).  
   - Geralmente são ignoradas no controle de versão via `.gitignore`.


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

## Principais etapas em [Program.cs](http://_vscodecontentref_/10)
```csharp
var input = args.Length > 0 ? args[0] : "fib(10)";
var inputStream = new AntlrInputStream(input);
var lexer       = new FibonacciLexer(inputStream);
var tokens      = new CommonTokenStream(lexer);
tokens.Fill();

// 1) Exibe tokens
Console.WriteLine("Tokens:");
foreach (var tk in tokens.GetTokens())
    Console.WriteLine($"  '{{ {tk.Text} }}' -> {tk.Type}");

// 2) Análise sintática
var parser = new FibonacciParser(tokens);
IParseTree tree = parser.prog();

// 3) Impressão da árvore
Console.WriteLine("ToStringTree:");
Console.WriteLine(tree.ToStringTree(parser));

// 4) Árvore “desenhada”
PrintTree(tree, parser);

// 5) Cálculo via visitor
var visitor = new FibonacciVisitor();
int result = visitor.Visit(tree);
Console.WriteLine($"Resultado: {result}");
```