﻿using System.Text;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyCustomTypeInfo : DyTypeInfo
    {
        private readonly bool autoGenMethods;

        public DyCustomTypeInfo(int typeCode, string typeName, bool autoGenMethods) : base(typeCode)
        {
            TypeName = typeName;
            this.autoGenMethods = autoGenMethods;
        }

        protected override SupportedOperations GetSupportedOperations() =>
            (SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not)
            | (autoGenMethods ? (SupportedOperations.Get | SupportedOperations.Set | SupportedOperations.Len)
                : SupportedOperations.None);

        protected override DyObject EqOp(DyObject left, DyObject right, ExecutionContext ctx)
        {
            if (!autoGenMethods)
                return base.EqOp(left, right, ctx);

            var self = (DyCustomType)left;

            if (self.TypeId == right.TypeId && right is DyCustomType t && t.Constructor == self.Constructor)
            {
                var res = ctx.RuntimeContext.Types[((DyTuple)self.Privates).TypeId].Eq(ctx, (DyTuple)self.Privates, (DyTuple)t.Privates);
             
                if (ctx.HasErrors)
                    return DyNil.Instance;

                return res;
            }

            return DyBool.False;
        }

        protected override DyObject ToStringOp(DyObject arg, ExecutionContext ctx)
        {
            if (!autoGenMethods)
                return base.ToStringOp(arg, ctx);

            var cust = (DyCustomType)arg;
            var priv = (DyTuple)cust.Privates;

            if (TypeName == cust.Constructor && priv.Count == 0)
                return new DyString($"{TypeName}()");
            else if (TypeName == cust.Constructor)
                return new DyString($"{TypeName}{priv.ToString(ctx)}");
            else if (priv.Count == 0)
                return new DyString($"{TypeName}.{cust.Constructor}()");
            else
                return new DyString($"{TypeName}.{cust.Constructor}{priv.ToString(ctx)}");
        }

        protected override DyObject LengthOp(DyObject arg, ExecutionContext ctx)
        {
            if (autoGenMethods)
                return DyInteger.Get(((DyTuple)((DyCustomType)arg).Privates).Count);

            return base.LengthOp(arg, ctx);
        }

        protected override DyObject GetOp(DyObject self, DyObject index, ExecutionContext ctx)
        {
            if (autoGenMethods)
                return ((DyTuple)((DyCustomType)self).Privates).GetItem(index, ctx);

            return base.GetOp(self, index, ctx);
        }

        protected override DyObject SetOp(DyObject self, DyObject index, DyObject value, ExecutionContext ctx)
        {
            if (autoGenMethods)
            {
                ((DyTuple)((DyCustomType)self).Privates).SetItem(index, value, ctx);
                return DyNil.Instance;
            }

            return base.SetOp(self, index, value, ctx);
        }

        protected override DyFunction? InitializeInstanceMember(DyObject self, string name, ExecutionContext ctx)
        {
            var obj = (DyCustomType)self;
            var idx = ((DyAssemblage)obj.Privates).GetOrdinal(name);
            return new DyGetterFunction(idx);
        }

        public override string TypeName { get; }
    }
}
