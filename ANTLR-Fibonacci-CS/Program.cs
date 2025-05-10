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
        var inputStream = new AntlrInputStream(input);

        // Instancia o lexer com a entrada e coleta tokens
        var lexer = new FibonacciLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);

        // Garante que todos os tokens sejam lidos antes de iterar
        tokens.Fill();

        // Exibe os tokens
        Console.WriteLine("Tokens:");
        foreach (var tk in tokens.GetTokens())
        {
            Console.WriteLine($"  {{ {tk.Text} }} -> {tk.Type}");
        }

        // Instancia o parser com o stream de tokens
        var parser = new FibonacciParser(tokens);

        // Inicia a análise sintática pela regra 'prog'
        IParseTree tree = parser.prog();

        // Imprime a representação prefixa da árvore
        Console.WriteLine("==========================");
        Console.WriteLine("Árvore de Análise (ToStringTree):");
        Console.WriteLine(tree.ToStringTree(parser));
        Console.WriteLine("==========================");

        // Imprime a árvore “desenhada”
        PrintTree(tree, parser);
        Console.WriteLine("==========================");

        // Visitor que calcula Fibonacci
        var visitor = new FibonacciVisitor();
        int result = visitor.Visit(tree);

        // Exibe o resultado
        Console.WriteLine($"Resultado: {result}");
    }

    static void PrintTree(IParseTree tree, Parser parser, string prefix = "", bool isLast = true)
    {
        var marker = isLast ? "└─ " : "├─ ";

        if (tree is ParserRuleContext pr)
        {
            Console.WriteLine(prefix + marker + parser.RuleNames[pr.RuleIndex]);
            prefix += isLast ? "   " : "│  ";
            for (int i = 0; i < pr.ChildCount; i++)
            {
                bool last = i == pr.ChildCount - 1;
                PrintTree(pr.GetChild(i), parser, prefix, last);
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
