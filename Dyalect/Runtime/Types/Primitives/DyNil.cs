﻿using System;
using System.IO;

namespace Dyalect.Runtime.Types
{
    public class DyNil : DyObject
    {
        internal const string Literal = "nil";
        public static readonly DyNil Instance = new();
        internal static readonly DyNil Terminator = new DyNilTerminator();
        
        private sealed class DyNilTerminator : DyNil { }

        private DyNil() : base(DyType.Nil) { }

        public override object ToObject() => null;

        public override string ToString() => Literal;

        public override DyObject Clone() => this;

        protected internal override DyObject GetItem(DyObject index, ExecutionContext ctx) =>
            ctx.IndexOutOfRange(index);

        internal override void Serialize(BinaryWriter writer) => writer.Write(TypeId);

        public override int GetHashCode() => HashCode.Combine(DyTypeNames.Nil, TypeId);
    }
}
