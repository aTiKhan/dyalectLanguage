﻿using Dyalect.Debug;
using System;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyErrorTypeInfo : DyTypeInfo
    {
        public DyErrorTypeInfo(DyTypeInfo typeInfo) : base(typeInfo, DyTypeCode.Error) { }

        protected override SupportedOperations GetSupportedOperations() =>
            SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not
            | SupportedOperations.Len | SupportedOperations.Get;

        protected override DyObject LengthOp(DyObject arg, ExecutionContext ctx) =>
            ctx.RuntimeContext.Integer.Get(((DyError)arg).DataItems?.Length ?? 0);

        protected override DyObject GetOp(DyObject self, DyObject index, ExecutionContext ctx)
        {
            var err = (DyError)self;

            if (index.DecType.TypeCode == DyTypeCode.Integer)
            {
                var idx = index.GetInteger();

                if (idx < 0 || idx >= err.DataItems.Length)
                    return ctx.IndexOutOfRange();

                return TypeConverter.ConvertFrom(err.DataItems[idx]);
            }
            else if (index.DecType.TypeCode == DyTypeCode.String)
                return err.GetItem(index, ctx);
            else
                return ctx.InvalidType(index);
        }

        public override string TypeName => DyTypeNames.Error;

        protected override DyObject ToStringOp(DyObject arg, ExecutionContext ctx) =>
            new DyString(ctx.RuntimeContext.String, ctx.RuntimeContext.Char, arg.ToString());

        protected override DyFunction InitializeStaticMember(string name, ExecutionContext ctx)
        {
            return Func.Static(ctx, name, (c, args) =>
            {
                if (!Enum.TryParse(name, out DyErrorCode code))
                    code = DyErrorCode.UnexpectedError;

                if (args is not null && args is DyTuple t)
                    return new DyError(ctx.RuntimeContext.Error, name, code, t.Values);
                else
                    return new DyError(ctx.RuntimeContext.Error, name, code);
            }, 0, new Par("values"));
        }
    }
}
