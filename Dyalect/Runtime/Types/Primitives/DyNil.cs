﻿using System;
using System.IO;

namespace Dyalect.Runtime.Types
{
    public class DyNil : DyObject
    {
        public static readonly DyNil Instance = new();
        internal static readonly DyNil Terminator = new DyNilTerminator();
        
        private sealed class DyNilTerminator : DyNil { }

        private DyNil() : base(DyType.Nil) { }

        public override object ToObject() => this;

        protected internal override bool GetBool() => false;

        public override string ToString() => "nil";

        public override DyObject Clone() => this;

        internal protected override DyObject GetItem(DyObject index, ExecutionContext ctx) =>
            ctx.IndexOutOfRange();

        internal override void Serialize(BinaryWriter writer) => writer.Write(TypeCode);

        public override int GetHashCode() => HashCode.Combine(DyTypeNames.Nil, TypeCode);
    }
}
