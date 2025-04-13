using System;
using Antlr4.Runtime.Tree;

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
