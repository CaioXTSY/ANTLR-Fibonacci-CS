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
