﻿using Dyalect.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyTupleTypeInfo : DyCollectionTypeInfo
    {
        public DyTuple Empty => new(this, Array.Empty<DyObject>());
        
        public DyTupleTypeInfo(DyTypeInfo typeInfo) : base(typeInfo, DyTypeCode.Tuple) { }

        protected override SupportedOperations GetSupportedOperations() =>
            SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not
            | SupportedOperations.Get | SupportedOperations.Set | SupportedOperations.Len
            | SupportedOperations.Add | SupportedOperations.Iter;

        public override string TypeName => DyTypeNames.Tuple;

        protected override DyObject AddOp(DyObject left, DyObject right, ExecutionContext ctx) =>
            new DyTuple(ctx.RuntimeContext.Tuple, ((DyCollection)left).Concat(ctx, right));

        protected override DyObject LengthOp(DyObject arg, ExecutionContext ctx)
        {
            var len = ((DyTuple)arg).Count;
            return ctx.RuntimeContext.Integer.Get(len);
        }

        protected override DyObject ToStringOp(DyObject arg, ExecutionContext ctx)
        {
            var tup = (DyTuple)arg;
            return tup.ToString(ctx);
        }

        protected override DyObject EqOp(DyObject left, DyObject right, ExecutionContext ctx)
        {
            if (left.DecType.TypeCode != right.DecType.TypeCode)
                return ctx.RuntimeContext.Bool.False;

            var (t1, t2) = ((DyTuple)left, (DyTuple)right);

            if (t1.Count != t2.Count)
                return ctx.RuntimeContext.Bool.False;

            for (var i = 0; i < t1.Count; i++)
            {
                if (ReferenceEquals(t1.Values[i].DecType.Eq(ctx, t1.Values[i], t2.Values[i]),
                    ctx.RuntimeContext.Bool.False))
                    return ctx.RuntimeContext.Bool.False;

                if (ctx.HasErrors)
                    return ctx.RuntimeContext.Nil.Instance;
            }

            return ctx.RuntimeContext.Bool.True;
        }

        protected override DyObject GetOp(DyObject self, DyObject index, ExecutionContext ctx) => self.GetItem(index, ctx);

        protected override DyObject SetOp(DyObject self, DyObject index, DyObject value, ExecutionContext ctx)
        {
            self.SetItem(index, value, ctx);
            return ctx.RuntimeContext.Nil.Instance;
        }

        internal static DyObject Concat(ExecutionContext ctx, DyObject values) =>
            new DyTuple(ctx.RuntimeContext.Tuple, DyCollection.ConcatValues(ctx, values));

        private DyObject GetKeys(ExecutionContext ctx, DyObject self)
        {
            IEnumerable<DyObject> Iterate()
            {
                var tup = (DyTuple)self;
                for (var i = 0; i < tup.Count; i++)
                {
                    var k = tup.GetKey(i);
                    if (k is not null)
                        yield return new DyString(ctx.RuntimeContext.String, ctx.RuntimeContext.Char, k);
                }
            }

            return DyIterator.Create(ctx.RuntimeContext.Iterator, Iterate());
        }

        private DyObject GetFirst(ExecutionContext ctx, DyObject self) =>
            self.GetItem(ctx.RuntimeContext.Integer.Zero, ctx);

        private DyObject GetSecond(ExecutionContext ctx, DyObject self) =>
            self.GetItem(ctx.RuntimeContext.Integer.One, ctx);

        private DyObject SortBy(ExecutionContext ctx, DyObject self, DyObject fun)
        {
            var tup = (DyTuple)self;
            var comparer = new SortComparer(fun as DyFunction, ctx);
            var newArr = new DyObject[tup.Count];
            Array.Copy(tup.Values, newArr, newArr.Length);
            Array.Sort(newArr, 0, newArr.Length, comparer);
            return new DyTuple(ctx.RuntimeContext.Tuple, newArr);
        }

        private DyObject AddItem(ExecutionContext ctx, DyObject self, DyObject item)
        {
            var t = (DyTuple)self;
            var arr = new DyObject[t.Count + 1];
            Array.Copy(t.Values, arr, t.Count);
            arr[^1] = item;
            return new DyTuple(ctx.RuntimeContext.Tuple, arr);
        }

        private DyObject Remove(ExecutionContext ctx, DyObject self, DyObject item)
        {
            var t = (DyTuple)self;

            for (var i = 0; i < t.Values.Length; i++)
            {
                var e = t.Values[i].GetTaggedValue();

                if (e.DecType.Eq(ctx, e, item).GetBool())
                    return RemoveAt(ctx, t, i);
            }

            return self;
        }

        private DyObject RemoveAt(ExecutionContext ctx, DyObject self, DyObject index)
        {
            if (index.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(index);

            var t = (DyTuple)self;

            var idx = (int)index.GetInteger();
            idx = idx < 0 ? t.Count + idx : idx;

            if (idx < 0 || idx >= t.Count)
                return ctx.IndexOutOfRange();

            return RemoveAt(ctx, t, idx);
        }

        private static DyTuple RemoveAt(ExecutionContext ctx, DyTuple self, int index)
        {
            var arr = new DyObject[self.Count - 1];
            var c = 0;

            for (var i = 0; i < self.Values.Length; i++)
            {
                if (i != index)
                    arr[c++] = self.Values[i];
            }

            return new DyTuple(ctx.RuntimeContext.Tuple, arr);
        }

        private DyObject Insert(ExecutionContext ctx, DyObject self, DyObject index, DyObject value)
        {
            if (index.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(index);

            var tuple = (DyTuple)self;

            var idx = (int)index.GetInteger();
            idx = idx < 0 ? tuple.Count + idx : idx;

            if (idx < 0 || idx > tuple.Count)
                return ctx.IndexOutOfRange();

            var arr = new DyObject[tuple.Count + 1];
            arr[idx] = value;

            if (idx == 0)
                Array.Copy(tuple.Values, 0, arr, 1, tuple.Count);
            else if (idx == tuple.Count)
                Array.Copy(tuple.Values, 0, arr, 0, tuple.Count);
            else
            {
                Array.Copy(tuple.Values, 0, arr, 0, idx);
                Array.Copy(tuple.Values, idx, arr, idx + 1, tuple.Count - idx);
            }

            return new DyTuple(ctx.RuntimeContext.Tuple, arr);
        }

        private DyObject ToDictionary(ExecutionContext ctx, DyObject self)
        {
            var tuple = (DyTuple)self;
            return new DyDictionary(ctx.RuntimeContext, tuple.ConvertToDictionary(ctx));
        }

        private DyObject Contains(ExecutionContext ctx, DyObject self, DyObject item)
        {
            if (item.DecType.TypeCode != DyTypeCode.String)
                return ctx.InvalidType(item);

            var tuple = (DyTuple)self;

            return tuple.HasItem(item.GetString(), ctx) ? ctx.RuntimeContext.Bool.True : ctx.RuntimeContext.Bool.False;
        }

        protected override DyFunction? InitializeInstanceMember(DyObject self, string name, ExecutionContext ctx) =>
            name switch
            {
                "add" => Func.Member(ctx, name, AddItem, -1, new Par("item")),
                "remove" => Func.Member(ctx, name, Remove, -1, new Par("item")),
                "removeAt" => Func.Member(ctx, name, RemoveAt, -1, new Par("index")),
                "insert" => Func.Member(ctx, name, Insert, -1, new Par("index"), new Par("item")),
                "keys" => Func.Member(ctx, name, GetKeys),
                "fst" => Func.Member(ctx, name, GetFirst),
                "snd" => Func.Member(ctx, name, GetSecond),
                "sort" => Func.Member(ctx, name, SortBy, -1, new Par("comparator", StaticNil.Instance)),
                "toDictionary" => Func.Member(ctx, name, ToDictionary),
                "contains" => Func.Member(ctx, name, Contains, -1, new Par("label")),
                _ => base.InitializeInstanceMember(self, name, ctx)
            };

        private DyObject GetPair(ExecutionContext ctx, DyObject fst, DyObject snd) =>
            new DyTuple(ctx.RuntimeContext.Tuple, new DyObject[] { fst, snd });

        private DyObject GetTriple(ExecutionContext ctx, DyObject fst, DyObject snd, DyObject thd) =>
            new DyTuple(ctx.RuntimeContext.Tuple, new DyObject[] { fst, snd, thd });

        private DyObject MakeNew(ExecutionContext ctx, DyObject obj) => obj;

        protected override DyObject? InitializeStaticMember(string name, ExecutionContext ctx) =>
            name switch
            {
                "sort" => Func.Static(ctx, name, SortBy, -1, new Par("tuple"), new Par("comparator", StaticNil.Instance)),
                "pair" => Func.Static(ctx, name, GetPair, -1, new Par("first"), new Par("second")),
                "triple" => Func.Static(ctx, name, GetTriple, -1, new Par("first"), new Par("second"), new Par("third")),
                "concat" => Func.Static(ctx, name, Concat, 0, new Par("values", true)),
                "Tuple" => Func.Static(ctx, name, MakeNew, 0, new Par("values")),
                _ => base.InitializeStaticMember(name, ctx)
            };
    }
}
