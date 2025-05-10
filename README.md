# ANTLR-Fibonacci-CS

Este é um projeto desenvolvido como parte da disciplina de Compiladores, ministrada pelo Professor Alexandre Paes na Universidade Federal de Alagoas - Campus Arapiraca.

O projeto utiliza [ANTLR4](https://www.antlr.org/) para criar um analisador (lexer + parser) que reconhece e processa expressões do tipo `fib(n)` em C#, incluindo um visitor para calcular o número de Fibonacci correspondente.

## Sumário

## Sumário

1. [Equipe](#equipe)
2. [O que é ANTLR?](#o-que-é-antlr)
3. [Conceitos Fundamentais de Compiladores](#conceitos-fundamentais-de-compiladores)
   - [Definições Regulares](#definições-regulares)
4. [Análise Léxica (Lexer)](#análise-léxica-lexer)
   - [O Processo de Tokenização](#o-processo-de-tokenização)
   - [Tokens no Projeto](#tokens-no-projeto)
   - [Implementação do Lexer](#implementação-do-lexer)
   - [Exemplo de Código Tokenizado](#exemplo-de-código-tokenizado)
5. [Análise Sintática (Parser)](#análise-sintática-parser)
   - [Gramática ANTLR4](#gramática-antlr4)
   - [Regras da Gramática](#regras-da-gramática)
   - [Árvore de Análise Sintática](#árvore-de-análise-sintática)
   - [Implementação do Parser](#implementação-do-parser)
6. [Estrutura do Projeto](#estrutura-do-projeto)
   - [Arquivos Gerados pelo ANTLR](#arquivos-gerados-pelo-antlr)
   - [Principais etapas em Program.cs](#principais-etapas-em-programcs)
7. [Como Usar](#como-usar)
8. [Referências](#referências)

### Equipe
- Caio Teixeira da Silva
- Noemy Torres Pereira
- Gustavo Henrique dos Santos Malaquias

---

## O que é ANTLR?

ANTLR (ANother Tool for Language Recognition) é uma ferramenta de geração de parser que utiliza gramáticas LL(*) para criar reconhecedores de linguagem. Desenvolvida por Terence Parr, o ANTLR permite:

- Definir gramáticas em um formato de alto nível
- Gerar automaticamente lexers e parsers em várias linguagens (Java, C#, Python, etc.)
- Criar árvores de análise sintática (parse trees)
- Implementar visitors e listeners para percorrer e processar essas árvores

O ANTLR é amplamente utilizado em:
- Compiladores e interpretadores
- Processadores de linguagens específicas de domínio (DSLs)
- Ferramentas de análise de código
- Processadores de dados estruturados

## Conceitos Fundamentais de Compiladores

### Definições Regulares

Definições regulares são formalismos matemáticos usados para descrever padrões de texto. No contexto de compiladores, elas são utilizadas para definir os tokens (unidades léxicas) de uma linguagem. As definições regulares são a base da análise léxica e incluem:

- **Alfabeto**: Conjunto finito de símbolos utilizados na linguagem (por exemplo, letras, dígitos, símbolos especiais)
- **Expressões Regulares**: Notação formal para descrever padrões de texto, como `[0-9]+` para representar números
- **Autômatos Finitos**: Modelos teóricos que implementam o reconhecimento de padrões definidos por expressões regulares

No contexto do nosso projeto:
- O alfabeto inclui dígitos (`0-9`), a palavra-chave `fib`, parênteses e espaços em branco
- As expressões regulares são usadas na gramática para definir tokens como `NUMBER: [0-9]+`
- O ANTLR gera internamente autômatos finitos determinísticos (DFAs) a partir dessas expressões para reconhecer tokens de forma eficiente

A principal característica das definições regulares é que elas possuem limitações expressivas. Por exemplo, elas não podem reconhecer linguagens que exigem um número arbitrário de correspondências balanceadas (como parênteses balanceados em expressões aritméticas complexas). Para essas tarefas, precisamos da análise sintática.

## Análise Léxica (Lexer)

A análise léxica é a primeira fase de um compilador, responsável por converter uma sequência de caracteres (o código-fonte) em uma sequência de tokens. Um token é uma unidade léxica que representa uma categoria específica de elementos da linguagem, como:

- Palavras-chave (ex: `if`, `while`, `fib`)
- Identificadores (nomes de variáveis)
- Literais (números, strings)
- Operadores e símbolos especiais (ex: `+`, `-`, `(`, `)`)

### O Processo de Tokenização

A tokenização segue estas etapas:
1. **Leitura da entrada**: O lexer lê a sequência de caracteres da entrada (geralmente um arquivo de código-fonte)
2. **Identificação de padrões**: O lexer identifica padrões que correspondem às definições de tokens
3. **Criação de tokens**: Para cada padrão reconhecido, o lexer cria um token com:
   - Tipo (categoria do token)
   - Valor (texto correspondente)
   - Posição (linha e coluna na entrada)
4. **Tratamento especial**: Alguns tokens, como comentários e espaços em branco, podem ser descartados
5. **Detecção de erros**: O lexer pode detectar caracteres inválidos ou tokens mal formados

### Tokens no Projeto

Em nosso projeto, definimos os seguintes tokens na gramática ANTLR:

1. **Tokens Literais**:
   - `'fib'`: A palavra-chave para a função de Fibonacci
   - `'('`: Parêntese esquerdo
   - `')'`: Parêntese direito

2. **Tokens Regulares**:
   - `NUMBER: [0-9]+`: Sequência de um ou mais dígitos
   - `WS: [ \t\r\n]+ -> skip`: Espaços em branco (ignorados pelo parser)

3. **Token Implícito**:
   - `EOF`: Fim do arquivo (gerado automaticamente pelo ANTLR)

### Implementação do Lexer

O lexer no ANTLR é gerado automaticamente a partir das definições na gramática. O arquivo `FibonacciLexer.cs` contém o código gerado que implementa o reconhecimento dos tokens. Internamente, o lexer usa autômatos finitos para reconhecer os tokens de forma eficiente.

O lexer gerado pelo ANTLR:
- Define constantes para cada tipo de token (`T_FIB`, `T_LPAREN`, `T_RPAREN`, `NUMBER`, etc.)
- Implementa métodos para reconhecer cada padrão de token
- Gerencia estados internos para lidar com diferentes modos de tokenização (se necessário)
- Trata erros léxicos e recuperação de erros

### Exemplo de Código Tokenizado

Para a entrada `fib(10)`, o lexer produz a seguinte sequência de tokens:

```
TOKEN[tipo=1, texto='fib']
TOKEN[tipo=2, texto='(']
TOKEN[tipo=4, texto='10']
TOKEN[tipo=3, texto=')']
TOKEN[tipo=-1, texto='<EOF>']
```

O `Program.cs` exibe esses tokens durante a execução:
```csharp
Console.WriteLine("Tokens:");
foreach (var tk in tokens.GetTokens())
    Console.WriteLine($"  '{{ {tk.Text} }}' -> {tk.Type}");
```

Este processo de tokenização converte a entrada bruta em uma representação estruturada que será usada pelo analisador sintático (parser).

## Análise Sintática (Parser)

A análise sintática é a segunda fase de um compilador, que determina se a sequência de tokens produzida pelo lexer segue as regras gramaticais da linguagem. O parser:

- Verifica a estrutura gramatical da entrada
- Constrói uma árvore de análise sintática (parse tree)
- Detecta e reporta erros sintáticos

### Gramática ANTLR4

Em ANTLR, a gramática define tanto os tokens (regras lexicais) quanto as regras sintáticas. Nossa gramática para o reconhecedor de Fibonacci é:

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

### Regras da Gramática

As regras da gramática podem ser divididas em dois tipos:

1. **Regras de parser** (começam com letra minúscula):
   - `prog`: Define um programa como uma expressão seguida do fim do arquivo
   - `expr`: Define uma expressão como a palavra "fib" seguida de um número entre parênteses

2. **Regras de lexer** (começam com letra maiúscula):
   - `NUMBER`: Define o token NUMBER como uma sequência de um ou mais dígitos
   - `WS`: Define o token WS (whitespace) como espaços, tabs ou quebras de linha, que serão ignorados

### Árvore de Análise Sintática

A árvore de análise sintática (parse tree) representa a estrutura hierárquica da entrada conforme as regras gramaticais. Para `fib(10)`, a árvore seria:

```
prog
  └── expr
       ├── 'fib'
       ├── '('
       ├── NUMBER (10)
       └── ')'
  └── EOF
```

Esta árvore mostra como os tokens se combinam para formar estruturas sintáticas mais complexas, seguindo as regras da gramática.

### Implementação do Parser

O parser no ANTLR é gerado automaticamente a partir das regras da gramática. O arquivo `FibonacciParser.cs` contém o código gerado que implementa a análise sintática.

O parser gerado pelo ANTLR:
- Define classes para cada regra da gramática (por exemplo, `ProgContext` e `ExprContext`)
- Implementa métodos para reconhecer cada regra
- Constrói a árvore de análise sintática durante o processo de parsing
- Trata erros sintáticos e recuperação de erros

O `Program.cs` demonstra como usar o parser:
```csharp
var parser = new FibonacciParser(tokens);
IParseTree tree = parser.prog();

// Exibe a árvore de análise sintática
Console.WriteLine("ToStringTree:");
Console.WriteLine(tree.ToStringTree(parser));

// Exibe uma representação visual da árvore
PrintTree(tree, parser);
```
## Implementação do Visitor

Após a geração da árvore de análise sintática, precisamos de um mecanismo para percorrê-la e extrair informações ou executar ações com base em sua estrutura. É aqui que entra o padrão de design Visitor, implementado na classe `FibonacciVisitor`.

O padrão Visitor permite separar os algoritmos das estruturas de dados sobre as quais eles operam. No contexto de compiladores e interpretadores, isso significa separar a lógica de processamento da estrutura da árvore sintática.

Nossa implementação do Visitor para cálculo de Fibonacci é a seguinte:

```csharp
public class FibonacciVisitor : FibonacciBaseVisitor<int>
{
   // Visita a regra 'prog' e chama a visita à regra 'expr'
   public override int VisitProg(FibonacciParser.ProgContext context)
   {
      return Visit(context.expr());
   }

   // Visita a regra 'expr' e extrai o número para calcular o Fibonacci
   public override int VisitExpr(FibonacciParser.ExprContext context)
   {
      int n = int.Parse(context.NUMBER().GetText());
      return Fib(n);
   }

   // Função recursiva para calcular o Fibonacci (forma ingênua)
   private int Fib(int n)
   {
      if(n < 2)
         return n;
      return Fib(n - 1) + Fib(n - 2);
   }
}
```

Cada método `Visit` corresponde a uma regra na gramática:

- **VisitProg**: Responsável por processar a regra raiz da gramática. Simplesmente delega o processamento para a expressão filha, refletindo a estrutura da regra `prog: expr EOF`.

- **VisitExpr**: Este é o método principal que processa a expressão Fibonacci. Ele extrai o valor numérico do token `NUMBER` da árvore, e chama a função `Fib()` para calcular o resultado.

- **Fib**: Um método auxiliar que implementa o algoritmo recursivo clássico de Fibonacci. Para cada número n, ele retorna a soma dos dois números de Fibonacci anteriores.

O Visitor é invocado no `Program.cs` após a construção da árvore sintática:

```csharp
var visitor = new FibonacciVisitor();
int result = visitor.Visit(tree);
Console.WriteLine($"Resultado: {result}");
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

### Arquivos Gerados pelo ANTLR

O ANTLR gera automaticamente vários arquivos a partir da gramática:

1. **FibonacciLexer.cs**: Implementa o analisador léxico que converte a entrada em tokens.
2. **FibonacciParser.cs**: Implementa o analisador sintático que verifica se a sequência de tokens segue as regras gramaticais.
3. **FibonacciBaseVisitor.cs**: Classe base para implementar o padrão Visitor.
4. **FibonacciVisitor.cs**: Interface que define os métodos de visita para cada regra da gramática.



### Principais etapas em Program.cs
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

// 4) Árvore "desenhada"
PrintTree(tree, parser);

// 5) Cálculo via visitor
var visitor = new FibonacciVisitor();
int result = visitor.Visit(tree);
Console.WriteLine($"Resultado: {result}");
```

### Exemplo de Saída do Programa

Para a entrada "fib(10)":

```
Tokens:
  '{ fib }' -> 1
  '{ ( }' -> 2
  '{ 10 }' -> 4
  '{ ) }' -> 3
  '{ <EOF> }' -> -1

ToStringTree:
(prog (expr fib ( 10 )) <EOF>)

Árvore:
prog
  └── expr
       ├── fib
       ├── (
       ├── 10
       └── )
  └── <EOF>

Resultado: 55
```

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

## Referências

- [ANTLR4 Documentation](https://github.com/antlr/antlr4/blob/master/doc/index.md)
- [The Definitive ANTLR4 Reference](https://pragprog.com/titles/tpantlr2/the-definitive-antlr-4-reference/) por Terence Parr
- [C# Language Implementation Patterns](https://www.amazon.com/Language-Implementation-Patterns-Domain-Specific-Programming/dp/193435645X) por Terence Parr
- [Compiladores: Princípios, Técnicas e Ferramentas](https://www.amazon.com.br/Compiladores-princ%C3%ADpios-ferramentas-tecnologias-constru%C3%A7%C3%A3o/dp/8588639246) (Livro do Dragão)
- [Engineering a Compiler](https://www.amazon.com/Engineering-Compiler-Keith-Cooper/dp/012088478X) por Keith Cooper e Linda Torczon
- [Modern Compiler Implementation in C](https://www.amazon.com/Modern-Compiler-Implementation-Andrew-Appel/dp/0521607655) por Andrew W. Appel