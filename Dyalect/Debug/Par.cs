﻿using Dyalect.Parser.Model;
using Dyalect.Runtime.Types;

namespace Dyalect.Debug
{
    public struct Par
    {
        public readonly string Name;
        public readonly bool IsVarArg;
        public readonly DyObject? Value;
        internal readonly Qualident? TypeAnnotation;

        internal Par(string name, DyObject? val, bool isVarArg, Qualident? ta) =>
            (Name, Value, IsVarArg, TypeAnnotation) = (name, val, isVarArg, ta);

        internal Par(string name, DyObject? val, bool isVarArg) =>
            (Name, Value, IsVarArg, TypeAnnotation) = (name, val, isVarArg, null);

        public Par(string name, bool isVarArg) => (Name, Value, IsVarArg, TypeAnnotation) = (name, null, isVarArg, null);

        public Par(string name, DyObject? value) => (Name, Value, IsVarArg, TypeAnnotation) = (name, value, false, null);

        public Par(string name) => (Name, Value, IsVarArg, TypeAnnotation) = (name, null, false, null);

        public override string ToString() => Name;
    }
}
