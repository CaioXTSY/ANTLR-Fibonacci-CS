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

        // Substitua a impressão simples por:
        // Console.WriteLine(tree.ToStringTree(parser));
        PrintTree(tree, parser);

        // Cria o visitor personalizado para percorrer a árvore e calcular Fibonacci
        FibonacciVisitor visitor = new FibonacciVisitor();
        int result = visitor.Visit(tree);

        // Exibe o resultado no console
        Console.WriteLine($"Resultado: {result}");
    }

    static void PrintTree(IParseTree tree, Parser parser, string prefix = "", bool isLast = true)
    {
        // marcador de nó
        var marker = isLast ? "└─ " : "├─ ";

        if (tree is ParserRuleContext pr)
        {
            // imprime o nome da regra
            Console.WriteLine(prefix + marker + parser.RuleNames[pr.RuleIndex]);

            // ajusta o prefixo para os filhos
            prefix += isLast ? "   " : "│  ";

            for (int i = 0; i < pr.ChildCount; i++)
            {
                bool lastChild = (i == pr.ChildCount - 1);
                PrintTree(pr.GetChild(i), parser, prefix, lastChild);
            }
        }
        else if (tree is ITerminalNode term)
        {
            var text = term.Symbol.Text;
            if (!string.IsNullOrWhiteSpace(text) && text != "<EOF>")
                Console.WriteLine(prefix + marker + text);
        }
    }
}
