﻿using Dyalect.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using static Dyalect.Parser.ParserError;

namespace Dyalect.Parser
{
    internal static class ErrorProcessor
    {
        private static readonly Dictionary<string, ParserError> errors =
            new()
            {
                 { "invalid StandardOperators", InvalidStandardOperators }
                ,{ "invalid FunctionName", InvalidFunctionName}
                ,{ "invalid ImportToken", InvalidImport }
                ,{ "invalid Region", InvalidRegion }
                ,{ "invalid Statement", InvalidStatement }
                ,{ "invalid Control", InvalidExpression }
                ,{ "invalid Binding", InvalidBinding }
                ,{ "invalid ControlFlow", InvalidStatement }
                ,{ "invalid If", InvalidIf }
                ,{ "invalid Loops", InvalidStatement }
                ,{ "invalid Expr", InvalidExpression }
                ,{ "invalid FunctionBody", InvalidFunction}
                ,{ "invalid FunctionStatement", InvalidFunction}
                ,{ "invalid NamePattern", InvalidPattern }
                ,{ "invalid Pattern", InvalidPattern }
                ,{ "invalid BooleanPattern", InvalidPattern }
                ,{ "invalid TuplePattern", InvalidPattern }
                ,{ "invalid MethodCheckPattern", InvalidPattern }
                ,{ "invalid ComparisonPattern", InvalidPattern }
                ,{ "invalid Yield", InvalidYield }
                ,{ "invalid Lambda", InvalidFunctionExpression }
                ,{ "invalid TryCatch", InvalidTryCatch }
                ,{ "invalid Range", InvalidRange }
                ,{ "invalid Unary", InvalidUnary }
                ,{ "invalid Literal", InvalidLiteral }
                ,{ "invalid ApplicationArguments", InvalidApplicationArguments }
                ,{ "invalid Label", InvalidLabel }
                ,{ "invalid Name", InvalidIdentifier }
                ,{ "invalid String", InvalidLiteral }
                ,{ "invalid Bool", InvalidLiteral }
                ,{ "invalid Tuple", InvalidLiteral }
                ,{ "invalid DyalectItem", InvalidStatement }

                ,{ "invalid Assignment", InvalidStatement }
                ,{ "invalid Ternary", InvalidExpression }
                ,{ "invalid Index", InvalidExpression }
                ,{ "invalid IndexBody", InvalidExpression }
                ,{ "invalid NullaryLambda", InvalidFunctionExpression }
                ,{ "invalid CtorPattern", InvalidPattern }
                ,{ "??? expected", Undefined }
            };

        private static readonly Dictionary<string, string> tokens =
            new()
            {
                 { "EOF", "end of file" }
                ,{ "identToken", "identifier" }
                ,{ "directive", "compiler directive" }
                ,{ "intToken", "integer literal" }
                ,{ "floatToken", "float literal" }
                ,{ "stringToken", "string literal" }
                ,{ "verbatimStringToken", "multiline string literal" }
                ,{ "charToken", "char literal" }
                ,{ "implicitToken", "implicit" }
                ,{ "privateToken", "private" }
                ,{ "inToken", "in" }
                ,{ "autoToken", "auto" }
                ,{ "varToken", "var" }
                ,{ "letToken", "let" }
                ,{ "lazyToken", "lazy" }
                ,{ "funcToken", "func" }
                ,{ "returnToken", "return" }
                ,{ "continueToken", "continue" }
                ,{ "breakToken", "break" }
                ,{ "yieldToken", "yield" }
                ,{ "ifToken", "if" }
                ,{ "forToken", "for" }
                ,{ "whileToken", "while" }
                ,{ "typeToken", "type" }
                ,{ "arrowToken", "=>" }
                ,{ "doToken", "do" }
                ,{ "dotToken", "." }
                ,{ "commaToken", "," }
                ,{ "semicolonToken", ";" }
                ,{ "colonToken", ":" }
                ,{ "equalToken", "==" }
                ,{ "parenLeftToken", "(" }
                ,{ "parenRightToken", ")" }
                ,{ "curlyLeftToken", "{" }
                ,{ "curlyRightToken", "}" }
                ,{ "squareLeftToken", "[" }
                ,{ "squareRightToken", "]" }
                ,{ "eq_coa", "??=" }
                ,{ "eq_add", "+=" }
                ,{ "eq_sub", "-=" }
                ,{ "eq_mul", "*=" }
                ,{ "eq_div", "/=" }
                ,{ "eq_rem", "%=" }
                ,{ "eq_and", "&&&=" }
                ,{ "eq_or", "|||=" }
                ,{ "eq_xor", "^=" }
                ,{ "eq_lsh", "<<<=" }
                ,{ "eq_rsh", ">>>=" }
                ,{ "minus", "-" }
                ,{ "plus", "+" }
                ,{ "not", "!" }
                ,{ "bitnot", "~~~" }
                ,{ "coalesce", "??" }
            };

        public static void ProcessError(string source, out string detail, out ParserError error)
        {
            if (errors.TryGetValue(source, out error))
                detail = ParserErrors.ResourceManager.GetString(error.ToString()) ?? "";
            else
            {
                var twoParts = source.Split(new char[] { '\u0020' }, StringSplitOptions.RemoveEmptyEntries);

                if (twoParts.Length != 2)
                {
                    error = Undefined;
                    detail = ParserErrors.Undefined;
                    return;
                }

                var token = twoParts[0].Trim('\"');

                if (tokens.TryGetValue(token, out var nt))
                    token = nt;
                else if (token == "invalid")
                {
                    error = InvalidSyntax;
                    detail = string.Format(ParserErrors.InvalidSyntax, source);
                    return;
                }

                if (!token.All(char.IsLetter))
                    token = "\"" + token + "\"";

                error = TokenExpected;
                detail = string.Format(ParserErrors.TokenExpected, token);
            }
        }
    }
}
