﻿using Dyalect.Compiler;
using System.Collections.Generic;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyIterator : DyForeignFunction
    {
        private readonly IEnumerator<DyObject> enumerator;

        public DyIterator(IEnumerator<DyObject> enumerator) : base(Builtins.Iterator, 0, StandardType.Iterator)
        {
            this.enumerator = enumerator;
        }

        internal static DyFunction CreateIterator(int unitId, int handle, DyMachine vm, FastList<DyObject[]> captures, DyObject[] locals)
        {
            var vars = new FastList<DyObject[]>(captures);
            vars.Add(locals);
            return new DyNativeIterator(unitId, handle, vm, vars);
        }

        protected override string GetFunctionName() => Builtins.Iterator;

        public override DyObject Call(ExecutionContext ctx, params DyObject[] args)
        {
            if (enumerator.MoveNext())
                return enumerator.Current;
            return DyNil.Terminator;
        }

        internal override DyFunction Clone(DyObject arg) => new DyIterator(enumerator) { Self = arg };
    }

    internal sealed class DyNativeIterator : DyNativeFunction
    {
        public DyNativeIterator(int unitId, int funcId, DyMachine vm, FastList<DyObject[]> captures) : base(unitId, funcId, 0, vm, captures, StandardType.Iterator)
        {

        }

        protected override string GetFunctionName() => Builtins.Iterator;

        internal override DyFunction Clone(DyObject arg) =>
            new DyNativeIterator(UnitId, FunctionId, Machine, Captures) { Self = arg };
    }

    internal sealed class DyIteratorTypeInfo : DyTypeInfo
    {
        public static readonly DyIteratorTypeInfo Instance = new DyIteratorTypeInfo();

        private DyIteratorTypeInfo() : base(StandardType.Bool)
        {

        }

        public override string TypeName => StandardType.BoolName;

        protected override DyString ToStringOp(DyObject arg, ExecutionContext ctx) => $"{Builtins.Iterator}()";

        private DyObject ToArray(ExecutionContext ctx, DyObject self, DyObject[] args)
        {
            var fn = (DyFunction)self;
            var arr = new List<DyObject>();
            DyObject res = null;

            while (!ReferenceEquals(res, DyNil.Terminator))
            {
                res = fn.Call0(ctx);

                if (ctx.HasErrors)
                    return DyNil.Instance;

                if (!ReferenceEquals(res, DyNil.Terminator))
                    arr.Add(res);
            }

            return new DyArray(arr);
        }

        protected override DyFunction GetTrait(string name, ExecutionContext ctx)
        {
            if (name == "toArray")
                return DyForeignFunction.Create(name, ToArray);

            return null;
        }
    }
}
