using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

class Program
{
    static void Main(string[] args)
    {
        // Entrada fixa para o exemplo
        string input = "fib(10)";

        // Cria o stream de entrada para o ANTLR a partir da string
        AntlrInputStream inputStream = new AntlrInputStream(input);

        // Instancia o lexer com a entrada
        FibonacciLexer lexer = new FibonacciLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);

        // Instancia o parser com o stream de tokens
        FibonacciParser parser = new FibonacciParser(tokens);

        // Inicia a análise pela regra 'prog'
        IParseTree tree = parser.prog();

        // Cria o visitor personalizado para percorrer a árvore e calcular Fibonacci
        FibonacciVisitor visitor = new FibonacciVisitor();
        int result = visitor.Visit(tree);

        // Exibe o resultado no console
        Console.WriteLine($"Resultado: {result}");
    }
}
