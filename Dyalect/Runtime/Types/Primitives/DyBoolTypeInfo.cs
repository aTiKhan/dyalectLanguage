﻿using Dyalect.Debug;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyBoolTypeInfo : DyTypeInfo
    {
        protected override SupportedOperations GetSupportedOperations() =>
            SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not;

        public override string TypeName => DyTypeNames.Bool;

        public override int ReflectedTypeCode => DyType.Bool;

        protected override DyObject EqOp(DyObject left, DyObject right, ExecutionContext ctx) =>
            left.GetBool() == right.GetBool() ? DyBool.True : DyBool.False;

        protected override DyObject ToStringOp(DyObject arg, ExecutionContext ctx) =>
            new DyString(ReferenceEquals(arg, DyBool.True) ? "true" : "false");

        private DyObject Convert(ExecutionContext ctx, DyObject val) => val.GetBool() ? DyBool.True : DyBool.False;

        protected override DyObject? InitializeStaticMember(string name, ExecutionContext ctx)
        {
            if (name is "Bool")
                return Func.Static(name, Convert, -1, new Par("value"));

            if (name is "default")
                return Func.Static(name, _ => DyBool.False);

            return base.InitializeStaticMember(name, ctx);
        }
    }
}
