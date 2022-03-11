﻿using System;
using System.IO;

namespace Dyalect.Runtime.Types
{
    public class DyLazy : DyObject
    {
        private DyFunction? func;
        private DyObject? value;

        public override int TypeId => value is not null ? value.TypeId : DyType.Lazy;

        internal DyLazy(DyFunction func) : base(DyType.Lazy) => this.func = func;

        public override object ToObject() => (value is not null ? value : func)!;

        internal override DyObject? Force(ExecutionContext ctx)
        {
            if (func is not null)
            {
                value = func.InternalCall(ctx);

                if (ctx.HasErrors)
                    return null;
                else
                    func = null;
            }

            return value;
        }

        public override DyTypeInfo GetTypeInfo(ExecutionContext ctx) => Force(ctx)?.GetTypeInfo(ctx) ?? ctx.RuntimeContext.Nil;

        protected internal override bool GetBool(ExecutionContext ctx) =>
            Force(ctx) is not null && value!.GetBool(ctx);

        protected internal override long GetInteger() => value is not null ? value.GetInteger() : base.GetInteger();
        protected internal override double GetFloat() => value is not null ? value.GetFloat() : base.GetFloat();
        protected internal override string GetString() => value is not null ? value.GetString() : base.GetString();
        protected internal override char GetChar() => value is not null ? value.GetChar() : base.GetChar();

        protected internal override bool HasItem(string name, ExecutionContext ctx) =>
            Force(ctx) is not null && value!.HasItem(name, ctx);

        public override string ToString() => "nil";

        public override DyObject Clone() => this;

        internal protected override DyObject GetItem(DyObject index, ExecutionContext ctx) =>
            ctx.IndexOutOfRange();

        internal override void Serialize(BinaryWriter writer) => writer.Write(TypeId);

        public override int GetHashCode() => HashCode.Combine(DyTypeNames.Nil, TypeId);
    }
}
