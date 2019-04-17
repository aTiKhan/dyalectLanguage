
using System;
using System.Linq;
using System.Collections.Generic;
using Dyalect.Parser.Model;


namespace Dyalect.Parser
{
    partial class InternalParser
    {
	public const int _EOF = 0;
	public const int _identToken = 1;
	public const int _intToken = 2;
	public const int _floatToken = 3;
	public const int _stringToken = 4;
	public const int _charToken = 5;
	public const int _implicitToken = 6;
	public const int _varToken = 7;
	public const int _constToken = 8;
	public const int _funcToken = 9;
	public const int _returnToken = 10;
	public const int _continueToken = 11;
	public const int _breakToken = 12;
	public const int _ifToken = 13;
	public const int _forToken = 14;
	public const int _whileToken = 15;
	public const int _switchToken = 16;
	public const int _doToken = 17;
	public const int _typeToken = 18;
	public const int _arrowToken = 19;
	public const int _dotToken = 20;
	public const int _commaToken = 21;
	public const int _semicolonToken = 22;
	public const int _colonToken = 23;
	public const int _equalToken = 24;
	public const int _parenLeftToken = 25;
	public const int _parenRightToken = 26;
	public const int _curlyLeftToken = 27;
	public const int _curlyRightToken = 28;
	public const int _squareLeftToken = 29;
	public const int _squareRightToken = 30;
	public const int maxT = 69;




        private void Get() 
        {
            for (;;)
            {
                t = la;
                la = scanner.Scan();
            
                if (la.kind <= maxT)
                {
                    ++errDist;
                    break;
                }

                la = t;
            }
        }

	void Separator() {
		Expect(22);
	}

	void AnyString(out string val) {
		val = null; 
		if (la.kind == 4) {
			Get();
			val = ParseString(); 
		} else if (la.kind == 5) {
			Get();
			val = ParseChar(); 
		} else SynErr(70);
	}

	void StandardOperators() {
		switch (la.kind) {
		case 31: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 33: {
			Get();
			break;
		}
		case 34: {
			Get();
			break;
		}
		case 35: {
			Get();
			break;
		}
		case 36: {
			Get();
			break;
		}
		case 37: {
			Get();
			break;
		}
		case 38: {
			Get();
			break;
		}
		case 39: {
			Get();
			break;
		}
		case 40: {
			Get();
			break;
		}
		case 41: {
			Get();
			break;
		}
		case 42: {
			Get();
			break;
		}
		case 43: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 45: {
			Get();
			break;
		}
		case 46: {
			Get();
			break;
		}
		case 47: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		case 49: {
			Get();
			break;
		}
		case 50: {
			Get();
			break;
		}
		default: SynErr(71); break;
		}
	}

	void FunctionName() {
		if (la.kind == 1) {
			Get();
		} else if (StartOf(1)) {
			StandardOperators();
		} else SynErr(72);
	}

	void Qualident(out string s1, out string s2, out string s3) {
		s1 = null; s2 = null; s3 = null; 
		FunctionName();
		s1 = t.val; 
		if (StartOf(2)) {
			if (la.kind == 20) {
				Get();
			}
			FunctionName();
			s2 = t.val; 
			if (StartOf(2)) {
				if (la.kind == 20) {
					Get();
				}
				FunctionName();
				s3 = t.val; 
			}
		}
	}

	void Import(out DNode node) {
		Expect(51);
		var inc = new DImport(t); 
		Expect(1);
		inc.ModuleName = t.val;
		node = inc;
		
		if (la.kind == 24) {
			Get();
			Expect(1);
			inc.Alias = inc.ModuleName;
			inc.ModuleName = t.val;
			
		}
		if (la.kind == 25) {
			Get();
			AnyString(out var str);
			inc.Dll = str; 
			Expect(26);
		}
	}

	void Type(out DNode node) {
		Expect(18);
		var typ = new DTypeDeclaration(t);
		node = typ;
		
		Expect(1);
		typ.Name = t.val; 
		if (la.kind == 27) {
			Get();
			if (la.kind == 1) {
				TypeField(out var fld);
				typ.Fields.Add(fld); 
				while (la.kind == 21) {
					Get();
					Expect(1);
					typ.Fields.Add(new DFieldDeclaration(t) { Name = t.val }); 
				}
			}
			Expect(28);
		}
	}

	void TypeField(out DFieldDeclaration node) {
		Expect(1);
		node = new DFieldDeclaration(t) { Name = t.val }; 
	}

	void Statement(out DNode node) {
		node = null; 
		switch (la.kind) {
		case 7: case 8: {
			Binding(out node);
			Separator();
			break;
		}
		case 1: case 2: case 3: case 4: case 5: case 25: case 29: case 32: case 38: case 39: case 49: case 50: case 66: case 67: case 68: {
			SimpleExpr(out node);
			Separator();
			break;
		}
		case 10: case 11: case 12: {
			ControlFlow(out node);
			Separator();
			break;
		}
		case 51: {
			Import(out node);
			Separator();
			break;
		}
		case 18: {
			Type(out node);
			Separator();
			break;
		}
		case 27: {
			Block(out node);
			break;
		}
		case 13: {
			If(out node);
			break;
		}
		case 15: {
			Loops(out node);
			break;
		}
		case 9: {
			Function(out node);
			break;
		}
		default: SynErr(73); break;
		}
	}

	void Binding(out DNode node) {
		if (la.kind == 7) {
			Get();
		} else if (la.kind == 8) {
			Get();
		} else SynErr(74);
		var bin = new DBinding(t) { Constant = t.val == "const" }; 
		Expect(1);
		bin.Name = t.val; 
		if (la.kind == 24) {
			Get();
			Expr(out node);
			bin.Init = node; 
		}
		node = bin; 
	}

	void SimpleExpr(out DNode node) {
		node = null; 
		if (IsFunction()) {
			FunctionExpr(out node);
		} else if (StartOf(3)) {
			Assignment(out node);
		} else SynErr(75);
	}

	void ControlFlow(out DNode node) {
		node = null; 
		if (la.kind == 12) {
			Break(out node);
		} else if (la.kind == 11) {
			Continue(out node);
		} else if (la.kind == 10) {
			Return(out node);
		} else SynErr(76);
	}

	void Block(out DNode node) {
		node = null; 
		Expect(27);
		var block = new DBlock(t); 
		Statement(out node);
		block.Nodes.Add(node); 
		while (StartOf(4)) {
			Statement(out node);
			block.Nodes.Add(node); 
		}
		node = block; 
		Expect(28);
	}

	void If(out DNode node) {
		node = null; 
		Expect(13);
		var @if = new DIf(t); 
		Expr(out node);
		@if.Condition = node; 
		Block(out node);
		@if.True = node; 
		if (la.kind == 53) {
			Get();
			if (la.kind == 27) {
				Block(out node);
				@if.False = node; 
			} else if (la.kind == 13) {
				If(out node);
				@if.False = node; 
			} else SynErr(77);
		}
		node = @if; 
	}

	void Loops(out DNode node) {
		node = null; 
		While(out node);
	}

	void Function(out DNode node) {
		node = null; 
		Expect(9);
		var f = new DFunctionDeclaration(t); 
		Qualident(out var s1, out var s2, out var s3);
		if (s2 == null && s3 == null)
		   f.Name = s1;
		else if (s3 == null)
		{
		   f.Name = s2;
		   f.TypeName = new Qualident(s1);
		}
		else
		{
		   f.Name = s3;
		   f.TypeName = new Qualident(s1, s2);
		}
		
		if (la.kind == 25) {
			FunctionArguments(f);
		}
		if (la.kind == 27) {
			Block(out node);
		} else if (la.kind == 24) {
			Get();
			Expr(out node);
			Separator();
		} else SynErr(78);
		f.Body = node;
		node = f;
		
	}

	void FunctionArguments(DFunctionDeclaration node) {
		Expect(25);
		if (la.kind == 1) {
			Get();
			node.Parameters.Add(t.val); 
			while (la.kind == 21) {
				Get();
				Expect(1);
				node.Parameters.Add(t.val); 
			}
			if (la.kind == 52) {
				Get();
				node.Variadic = true; 
			}
		}
		Expect(26);
	}

	void Expr(out DNode node) {
		node = null; 
		if (StartOf(3)) {
			SimpleExpr(out node);
		} else if (la.kind == 13) {
			If(out node);
		} else if (la.kind == 27) {
			Block(out node);
		} else if (la.kind == 15) {
			Loops(out node);
		} else SynErr(79);
	}

	void FunctionArgument(DFunctionDeclaration node) {
		Expect(1);
		node.Parameters.Add(t.val); 
	}

	void While(out DNode node) {
		node = null; 
		Expect(15);
		var @while = new DWhile(t); 
		Expr(out node);
		@while.Condition = node; 
		Block(out node);
		@while.Body = node;
		node = @while;
		
	}

	void Break(out DNode node) {
		Expect(12);
		var br = new DBreak(t); node = br; 
		if (StartOf(5)) {
			Expr(out var exp);
			br.Expression = exp; 
		}
	}

	void Continue(out DNode node) {
		Expect(11);
		node = new DContinue(t); 
	}

	void Return(out DNode node) {
		Expect(10);
		var br = new DReturn(t); node = br; 
		if (StartOf(5)) {
			Expr(out var exp);
			br.Expression = exp; 
		}
	}

	void FunctionExpr(out DNode node) {
		var f = new DFunctionDeclaration(t);
		node = f;
		
		if (la.kind == 1) {
			FunctionArgument(f);
		} else if (la.kind == 25) {
			FunctionArguments(f);
		} else SynErr(80);
		Expect(19);
		Expr(out var exp);
		f.Body = exp; 
	}

	void Assignment(out DNode node) {
		Or(out node);
		if (StartOf(6)) {
			var ass = new DAssignment(t) { Target = node };
			node = ass;
			BinaryOperator? op = null;
			
			switch (la.kind) {
			case 24: {
				Get();
				break;
			}
			case 54: {
				Get();
				op = BinaryOperator.Add; 
				break;
			}
			case 55: {
				Get();
				op = BinaryOperator.Sub; 
				break;
			}
			case 56: {
				Get();
				op = BinaryOperator.Mul; 
				break;
			}
			case 57: {
				Get();
				op = BinaryOperator.Div; 
				break;
			}
			case 58: {
				Get();
				op = BinaryOperator.Rem; 
				break;
			}
			case 59: {
				Get();
				op = BinaryOperator.And; 
				break;
			}
			case 60: {
				Get();
				op = BinaryOperator.Or; 
				break;
			}
			case 61: {
				Get();
				op = BinaryOperator.Xor; 
				break;
			}
			case 62: {
				Get();
				op = BinaryOperator.ShiftLeft; 
				break;
			}
			case 63: {
				Get();
				op = BinaryOperator.ShiftRight; 
				break;
			}
			}
			Expr(out node);
			ass.Value = node;
			ass.AutoAssign = op;
			node = ass;
			
		}
	}

	void Or(out DNode node) {
		And(out node);
		while (la.kind == 64) {
			Get();
			var ot = t; 
			And(out DNode exp);
			node = new DBinaryOperation(node, exp, BinaryOperator.Or, ot); 
		}
	}

	void And(out DNode node) {
		Eq(out node);
		while (la.kind == 65) {
			Get();
			var ot = t; 
			Eq(out DNode exp);
			node = new DBinaryOperation(node, exp, BinaryOperator.And, ot); 
		}
	}

	void Eq(out DNode node) {
		Shift(out node);
		while (StartOf(7)) {
			var op = default(BinaryOperator);
			var ot = default(Token);
			
			switch (la.kind) {
			case 42: {
				Get();
				ot = t; op = BinaryOperator.Gt; 
				break;
			}
			case 43: {
				Get();
				ot = t; op = BinaryOperator.Lt; 
				break;
			}
			case 44: {
				Get();
				ot = t; op = BinaryOperator.GtEq; 
				break;
			}
			case 45: {
				Get();
				ot = t; op = BinaryOperator.LtEq; 
				break;
			}
			case 40: {
				Get();
				ot = t; op = BinaryOperator.Eq; 
				break;
			}
			case 41: {
				Get();
				ot = t; op = BinaryOperator.NotEq; 
				break;
			}
			}
			Shift(out DNode exp);
			node = new DBinaryOperation(node, exp, op, ot); 
		}
	}

	void Shift(out DNode node) {
		BitOr(out node);
		while (la.kind == 47 || la.kind == 48) {
			var op = default(BinaryOperator);
			var ot = default(Token);
			
			if (la.kind == 47) {
				Get();
				ot = t; op = BinaryOperator.ShiftLeft; 
			} else {
				Get();
				ot = t; op = BinaryOperator.ShiftRight; 
			}
			BitOr(out DNode exp);
			node = new DBinaryOperation(node, exp, op, ot); 
		}
	}

	void BitOr(out DNode node) {
		Xor(out node);
		while (la.kind == 36) {
			Get();
			var ot = t; 
			Xor(out DNode exp);
			node = new DBinaryOperation(node, exp, BinaryOperator.BitwiseOr, ot); 
		}
	}

	void Xor(out DNode node) {
		BitAnd(out node);
		while (la.kind == 46) {
			Get();
			var ot = t; 
			BitAnd(out DNode exp);
			node = new DBinaryOperation(node, exp, BinaryOperator.Xor, ot); 
		}
	}

	void BitAnd(out DNode node) {
		Add(out node);
		while (la.kind == 37) {
			Get();
			var ot = t; 
			Add(out DNode exp);
			node = new DBinaryOperation(node, exp, BinaryOperator.BitwiseAnd, ot); 
		}
	}

	void Add(out DNode node) {
		Mul(out node);
		while (la.kind == 31 || la.kind == 32) {
			var op = default(BinaryOperator);
			var ot = default(Token);
			
			if (la.kind == 31) {
				Get();
				ot = t; op = BinaryOperator.Add; 
			} else {
				Get();
				ot = t; op = BinaryOperator.Sub; 
			}
			Mul(out DNode exp);
			node = new DBinaryOperation(node, exp, op, ot); 
		}
	}

	void Mul(out DNode node) {
		Unary(out node);
		while (la.kind == 33 || la.kind == 34 || la.kind == 35) {
			var op = default(BinaryOperator);
			var ot = default(Token);
			
			if (la.kind == 33) {
				Get();
				ot = t; op = BinaryOperator.Mul; 
			} else if (la.kind == 34) {
				Get();
				ot = t; op = BinaryOperator.Div; 
			} else {
				Get();
				ot = t; op = BinaryOperator.Rem; 
			}
			Unary(out var exp);
			node = new DBinaryOperation(node, exp, op, ot); 
		}
	}

	void Unary(out DNode node) {
		node = null;
		var op = default(UnaryOperator);
		var ot = default(Token);
		
		if (StartOf(8)) {
			if (la.kind == 38) {
				Get();
				ot = t; op = UnaryOperator.Not; 
			} else if (la.kind == 32) {
				Get();
				ot = t; op = UnaryOperator.Neg; 
			} else if (la.kind == 49) {
				Get();
				ot = t; op = UnaryOperator.BitwiseNot; 
			} else if (la.kind == 39) {
				Get();
				ot = t; op = UnaryOperator.Length; 
			} else {
				Get();
				ot = t; op = UnaryOperator.ToString; 
			}
		}
		Application(out node);
		if (op != UnaryOperator.None)
		   node = new DUnaryOperation(node, op, ot);
		
	}

	void Application(out DNode node) {
		node = null; 
		FieldOrIndex(out node);
		while (la.kind == 25) {
			if (la.AfterEol) return;
			var app = new DApplication(node, t); 
			
			ApplicationArguments(app);
			node = app; 
		}
	}

	void FieldOrIndex(out DNode node) {
		Literal(out node);
		while (la.kind == 20 || la.kind == 29) {
			if (la.kind == 20) {
				Get();
				var ot = t; 
				Expect(1);
				var fld = new DTrait(ot) { Target = node };
				fld.Name = t.val;
				node = fld;
				
			} else {
				if (la.AfterEol) return; 
				Get();
				var idx = new DIndexer(t) { Target = node }; 
				Literal(out node);
				idx.Index = node;
				node = idx;
				
				Expect(30);
			}
		}
	}

	void ApplicationArguments(DApplication app) {
		var node = default(DNode); 
		Expect(25);
		if (StartOf(5)) {
			Expr(out node);
			app.Arguments.Add(node); 
		}
		while (la.kind == 21) {
			Get();
			Expr(out node);
			app.Arguments.Add(node); 
		}
		Expect(26);
	}

	void Literal(out DNode node) {
		node = null; 
		if (la.kind == 1) {
			Name(out node);
		} else if (la.kind == 2) {
			Integer(out node);
		} else if (la.kind == 3) {
			Float(out node);
		} else if (la.kind == 4 || la.kind == 5) {
			String(out node);
		} else if (la.kind == 67 || la.kind == 68) {
			Bool(out node);
		} else if (la.kind == 66) {
			Nil(out node);
		} else if (IsTuple()) {
			Tuple(out node);
		} else if (la.kind == 29) {
			Array(out node);
		} else if (la.kind == 25) {
			Group(out node);
		} else SynErr(81);
	}

	void Name(out DNode node) {
		Expect(1);
		node = new DName(t) { Value = t.val }; 
	}

	void Integer(out DNode node) {
		Expect(2);
		node = new DIntegerLiteral(t) { Value = ParseInteger() }; 
	}

	void Float(out DNode node) {
		Expect(3);
		node = new DFloatLiteral(t) { Value = ParseFloat() }; 
	}

	void String(out DNode node) {
		AnyString(out var str);
		node = new DStringLiteral(t) { Value = str }; 
	}

	void Bool(out DNode node) {
		if (la.kind == 67) {
			Get();
		} else if (la.kind == 68) {
			Get();
		} else SynErr(82);
		node = new DBooleanLiteral(t) { Value = t.val == "true" }; 
	}

	void Nil(out DNode node) {
		Expect(66);
		node = new DNilLiteral(t); 
	}

	void Tuple(out DNode node) {
		node = null; 
		Expect(25);
		var tup = new DTupleLiteral(t); 
		TupleElement(out node);
		tup.Elements.Add(node); 
		while (la.kind == 21) {
			Get();
			TupleElement(out node);
			tup.Elements.Add(node); 
		}
		node = tup; 
		Expect(26);
	}

	void Array(out DNode node) {
		node = null; 
		Expect(29);
		var arr = new DArrayLiteral(t); 
		Expr(out node);
		arr.Elements.Add(node); 
		while (la.kind == 21) {
			Get();
			Expr(out node);
			arr.Elements.Add(node); 
		}
		node = arr; 
		Expect(30);
	}

	void Group(out DNode node) {
		node = null; 
		Expect(25);
		Expr(out node);
		Expect(26);
	}

	void TupleElement(out DNode node) {
		node = null; 
		if (IsNamedTupleElement()) {
			string name = null; 
			if (la.kind == 1) {
				Get();
				name = t.val; 
			} else if (la.kind == 4 || la.kind == 5) {
				AnyString(out var str);
				name = str; 
			} else SynErr(83);
			Expect(23);
			var tag = new DLabelLiteral(t) { Label = name }; 
			Expr(out node);
			tag.Expression = node; node = tag; 
		} else if (StartOf(5)) {
			Expr(out node);
		} else SynErr(84);
	}

	void Dyalect() {
		Statement(out var node);
		Root.Nodes.Add(node); 
		while (StartOf(4)) {
			Statement(out node);
			Root.Nodes.Add(node); 
		}
	}



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
		Dyalect();
		Expect(0);

        }

        static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_T,_x, _x,_x,_x,_x, _x,_T,_x,_T, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_T, _x,_T,_x,_x, _T,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x}

        };

        private void SynErr(int line, int col, int n)
        {
            string s;

            switch (n)
            {
			case 0: s = "EOF expected"; break;
			case 1: s = "identToken expected"; break;
			case 2: s = "intToken expected"; break;
			case 3: s = "floatToken expected"; break;
			case 4: s = "stringToken expected"; break;
			case 5: s = "charToken expected"; break;
			case 6: s = "implicitToken expected"; break;
			case 7: s = "varToken expected"; break;
			case 8: s = "constToken expected"; break;
			case 9: s = "funcToken expected"; break;
			case 10: s = "returnToken expected"; break;
			case 11: s = "continueToken expected"; break;
			case 12: s = "breakToken expected"; break;
			case 13: s = "ifToken expected"; break;
			case 14: s = "forToken expected"; break;
			case 15: s = "whileToken expected"; break;
			case 16: s = "switchToken expected"; break;
			case 17: s = "doToken expected"; break;
			case 18: s = "typeToken expected"; break;
			case 19: s = "arrowToken expected"; break;
			case 20: s = "dotToken expected"; break;
			case 21: s = "commaToken expected"; break;
			case 22: s = "semicolonToken expected"; break;
			case 23: s = "colonToken expected"; break;
			case 24: s = "equalToken expected"; break;
			case 25: s = "parenLeftToken expected"; break;
			case 26: s = "parenRightToken expected"; break;
			case 27: s = "curlyLeftToken expected"; break;
			case 28: s = "curlyRightToken expected"; break;
			case 29: s = "squareLeftToken expected"; break;
			case 30: s = "squareRightToken expected"; break;
			case 31: s = "\"+\" expected"; break;
			case 32: s = "\"-\" expected"; break;
			case 33: s = "\"*\" expected"; break;
			case 34: s = "\"/\" expected"; break;
			case 35: s = "\"%\" expected"; break;
			case 36: s = "\"|\" expected"; break;
			case 37: s = "\"&\" expected"; break;
			case 38: s = "\"!\" expected"; break;
			case 39: s = "\"#\" expected"; break;
			case 40: s = "\"==\" expected"; break;
			case 41: s = "\"!=\" expected"; break;
			case 42: s = "\">\" expected"; break;
			case 43: s = "\"<\" expected"; break;
			case 44: s = "\">=\" expected"; break;
			case 45: s = "\"<=\" expected"; break;
			case 46: s = "\"^\" expected"; break;
			case 47: s = "\"<<\" expected"; break;
			case 48: s = "\">>\" expected"; break;
			case 49: s = "\"~\" expected"; break;
			case 50: s = "\"`\" expected"; break;
			case 51: s = "\"import\" expected"; break;
			case 52: s = "\"...\" expected"; break;
			case 53: s = "\"else\" expected"; break;
			case 54: s = "\"+=\" expected"; break;
			case 55: s = "\"-=\" expected"; break;
			case 56: s = "\"*=\" expected"; break;
			case 57: s = "\"/=\" expected"; break;
			case 58: s = "\"%=\" expected"; break;
			case 59: s = "\"&=\" expected"; break;
			case 60: s = "\"|=\" expected"; break;
			case 61: s = "\"^=\" expected"; break;
			case 62: s = "\"<<=\" expected"; break;
			case 63: s = "\">>=\" expected"; break;
			case 64: s = "\"||\" expected"; break;
			case 65: s = "\"&&\" expected"; break;
			case 66: s = "\"nil\" expected"; break;
			case 67: s = "\"true\" expected"; break;
			case 68: s = "\"false\" expected"; break;
			case 69: s = "??? expected"; break;
			case 70: s = "invalid AnyString"; break;
			case 71: s = "invalid StandardOperators"; break;
			case 72: s = "invalid FunctionName"; break;
			case 73: s = "invalid Statement"; break;
			case 74: s = "invalid Binding"; break;
			case 75: s = "invalid SimpleExpr"; break;
			case 76: s = "invalid ControlFlow"; break;
			case 77: s = "invalid If"; break;
			case 78: s = "invalid Function"; break;
			case 79: s = "invalid Expr"; break;
			case 80: s = "invalid FunctionExpr"; break;
			case 81: s = "invalid Literal"; break;
			case 82: s = "invalid Bool"; break;
			case 83: s = "invalid TupleElement"; break;
			case 84: s = "invalid TupleElement"; break;

                default:
                    s = "unknown " + n;
                    break;
            }

            AddError(s, n, line, col);
        }
    }
}
