﻿using Dyalect.Parser.Model;
using Dyalect.Strings;
using System.Collections.Generic;
using System.Globalization;

namespace Dyalect.Parser
{
    internal sealed partial class InternalParser
    {
        private const bool _T = true;
        private const bool _x = false;
        private const int minErrDist = 2;

        private readonly Scanner scanner;

        public Token t;
        public Token la;
        private int errDist = minErrDist;

        public List<DImport> Imports { get; } = new List<DImport>();

        public DBlock Root { get; } = new DBlock(new Location(0, 0));

        public List<BuildMessage> Errors { get; } = new List<BuildMessage>();

        private Stack<DFunctionDeclaration> functions = new Stack<DFunctionDeclaration>();

        private List<int> implicits;

        public InternalParser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        private void AddError(ParserError error, Location loc, params object[] args)
        {
            var str = string.Format(ParserErrors.ResourceManager.GetString(error.ToString()) ?? error.ToString(), args);
            AddError(new BuildMessage(str, BuildMessageType.Error, (int)error, loc.Line, loc.Column, this.scanner.Buffer.FileName));
        }

        private void AddError(string message, int line, int col)
        {
            ErrorProcessor.ProcessError(message, out var detail, out var code);
            AddError(new BuildMessage(detail, BuildMessageType.Error, (int)code, line, col, this.scanner.Buffer.FileName));
        }

        private void AddError(BuildMessage msg)
        {
            Errors.Add(msg);
        }

        private void SynErr(int n)
        {
            if (errDist >= minErrDist)
                SynErr(la.line, la.col, n);

            errDist = 0;
        }

        private void SemErr(string msg)
        {
            if (errDist >= minErrDist)
                SemErr(t.line, t.col, msg);
            errDist = 0;
        }

        private void SemErr(int line, int col, string s)
        {
            AddError(ParserError.SemanticError, new Location(line, col), s);
        }

        private void Warning(int line, int col, string s)
        {

        }

        private void Warning(string s)
        {

        }

        private void Expect(int n)
        {
            if (la.kind == n)
                Get();
            else
            {
                if (n == _semicolonToken 
                    && (t.kind == _curlyLeftToken || la.kind == 0 || la.kind == _curlyRightToken || la.AfterEol))
                    return;

                SynErr(n);
            }
        }

        private bool StartOf(int s)
        {
            return set[s, la.kind];
        }

        private void ExpectWeak(int n, int follow)
        {
            if (la.kind == n)
                Get();
            else
            {
                SynErr(n);

                while (!StartOf(follow))
                    Get();
            }
        }

        private bool WeakSeparator(int n, int syFol, int repFol)
        {
            int kind = la.kind;
            if (kind == n) { Get(); return true; }
            else if (StartOf(repFol)) { return false; }
            else
            {
                SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind]))
                {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }

        private bool IsRecord()
        {
            if (la.kind != _parenLeftToken)
                return false;

            scanner.ResetPeek();
            Token x;

            if ((x = scanner.Peek()).kind != _identToken && x.kind != _stringToken)
                return false;

            if (scanner.Peek().kind != _colonToken)
                return false;

            return true;
        }

        private bool IsMap()
        {
            if (la.kind != _squareLeftToken)
                return false;

            scanner.ResetPeek();

            if (scanner.Peek().kind != _stringToken)
                return false;

            if (scanner.Peek().kind != _colonToken)
                return false;

            return true;
        }

        private bool IsLabel()
        {
            if (la.kind != _identToken)
                return false;

            scanner.ResetPeek();
            return scanner.Peek().kind == _colonToken;
        }

        private bool IsTuple()
        {
            if (la.kind != _parenLeftToken)
                return false;

            scanner.ResetPeek();
            var x = la;
            var balance = 0;

            while (true)
            {
                if ((x.kind == _commaToken || x.kind == _colonToken) && balance == 1)
                    return true;

                if (x.kind == _parenLeftToken)
                    balance++;
                else if (x.kind == _parenRightToken)
                {
                    balance--;
                    if (balance == 0)
                        break;
                }

                x = scanner.Peek();
            }

            return false;
        }

        private bool IsNextUnary() => la.kind == _minus || la.kind == _plus
            || la.kind == _bitnot || la.kind == _not;

        private bool IsFunction()
        {
            if (la.kind != _parenLeftToken && la.kind != _identToken)
                return false;

            scanner.ResetPeek();
            var x = la;

            if (la.kind == _identToken)
            {
                x = scanner.Peek();
                return x.kind == _arrowToken;
            }

            var balance = 0;

            while (x.kind != _arrowToken)
            {
                if (x.kind == _parenLeftToken)
                    balance++;
                else if (x.kind == _parenRightToken)
                {
                    balance--;
                    if (balance == 0)
                        return scanner.Peek().kind == _arrowToken;
                }

                x = scanner.Peek();
            }

            return false;
        }

        private DStringLiteral ParseString()
        {
            if (!EscapeCodeParser.Parse(scanner.Buffer.FileName, t, t.val, Errors, out var result, out var chunks))
                return null;

            return new DStringLiteral(t) { Value = result, Chunks = chunks };
        }

        private string ParseSimpleString()
        {
            if (!EscapeCodeParser.Parse(scanner.Buffer.FileName, t, t.val, Errors, out var result, out var chunks))
                return null;

            if (chunks != null)
            {
                AddError(ParserError.CodeIslandsNotAllowed, t);
                return null;
            }

            return result;
        }

        private char ParseChar()
        {
            var str = ParseSimpleString();
            return str == null ? '\0' : str[0];
        }

        private int GetImplicit()
        {
            return int.Parse(t.val.Substring(1));
        }

        private long ParseInteger()
        {
            if (t.val.Length > 2 && t.val[0] == '0' && char.ToUpper(t.val[1]) == 'X')
                return long.Parse(t.val.Substring(2), NumberStyles.HexNumber);

            return long.Parse(t.val);
        }

        private double ParseFloat()
        {
            return double.Parse(t.val, CI.NumberFormat);
        }
    }
}
