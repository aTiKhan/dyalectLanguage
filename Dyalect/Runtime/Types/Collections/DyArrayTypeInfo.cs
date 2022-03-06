﻿using Dyalect.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyalect.Runtime.Types
{
    internal sealed class DyArrayTypeInfo : DyCollectionTypeInfo
    {
        public DyArrayTypeInfo(DyTypeInfo typeInfo) : base(typeInfo, DyTypeCode.Array) { }

        public override string TypeName => DyTypeNames.Array;

        protected override SupportedOperations GetSupportedOperations() =>
            SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not
            | SupportedOperations.Get | SupportedOperations.Set | SupportedOperations.Len
            | SupportedOperations.Iter;

        protected override DyObject LengthOp(DyObject arg, ExecutionContext ctx) =>
            ctx.RuntimeContext.Integer.Get(((DyArray)arg).Count);

        protected override DyObject ToStringOp(DyObject arg, ExecutionContext ctx)
        {
            var arr = (DyArray)arg;
            var sb = new StringBuilder();
            sb.Append('[');

            for (var i = 0; i < arr.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                var str = arr[i].ToString(ctx);

                if (ctx.Error != null)
                    return ctx.RuntimeContext.String.Empty;

                sb.Append(str.GetString());
            }

            sb.Append(']');
            return new DyString(ctx.RuntimeContext.String, sb.ToString());
        }

        protected override DyObject AddOp(DyObject left, DyObject right, ExecutionContext ctx) =>
            new DyArray(ctx.RuntimeContext.Array, ((DyCollection)left).Concat(ctx, right));

        protected override DyObject GetOp(DyObject self, DyObject index, ExecutionContext ctx) => self.GetItem(index, ctx);

        protected override DyObject SetOp(DyObject self, DyObject index, DyObject value, ExecutionContext ctx)
        {
            self.SetItem(index, value, ctx);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject AddItem(ExecutionContext ctx, DyObject self, DyObject arg)
        {
            ((DyArray)self).Add(arg);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject InsertItem(ExecutionContext ctx, DyObject self, DyObject index, DyObject value)
        {
            var arr = (DyArray)self;

            if (index.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(index);

            var i = (int)index.GetInteger();

            if (i < 0 || i > arr.Count)
                return ctx.IndexOutOfRange();

            arr.Insert(i, value);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject AddRange(ExecutionContext ctx, DyObject self, DyObject val)
        {
            var arr = (DyArray)self;

            foreach (var o in DyIterator.ToEnumerable(ctx, val))
            {
                if (ctx.HasErrors)
                    break;
                arr.Add(o);
            }

            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject RemoveItem(ExecutionContext ctx, DyObject self, DyObject val) =>
            ((DyArray)self).Remove(ctx, val) ? ctx.RuntimeContext.Bool.True : ctx.RuntimeContext.Bool.False;

        private DyObject RemoveItemAt(ExecutionContext ctx, DyObject self, DyObject index)
        {
            if (index.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(index);

            var idx = (int)index.GetInteger();
            var arr = (DyArray)self;

            if (idx < 0 || idx >= arr.Count)
                return ctx.IndexOutOfRange();

            arr.RemoveAt(idx);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject ClearItems(ExecutionContext ctx, DyObject self)
        {
            ((DyArray)self).Clear();
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject IndexOf(ExecutionContext ctx, DyObject self, DyObject val)
        {
            var arr = (DyArray)self;
            var i = arr.IndexOf(ctx, val);
            return ctx.RuntimeContext.Integer.Get(i);
        }

        private DyObject LastIndexOf(ExecutionContext ctx, DyObject self, DyObject val)
        {
            var arr = (DyArray)self;
            var i = arr.LastIndexOf(ctx, val);
            return ctx.RuntimeContext.Integer.Get(i);
        }

        private DyObject SortBy(ExecutionContext ctx, DyObject self, DyObject fun)
        {
            var arr = (DyArray)self;
            var comparer = new SortComparer(fun as DyFunction, ctx);
            Array.Sort(arr.Values, 0, arr.Count, comparer);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject Reverse(ExecutionContext ctx, DyObject self)
        {
            var arr = (DyArray)self;
            Array.Reverse(arr.Values);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject Compact(ExecutionContext ctx, DyObject self)
        {
            var arr = (DyArray)self;

            if (arr.Count == 0)
                return ctx.RuntimeContext.Nil.Instance;

            var idx = 0;

            while (idx < arr.Count)
            {
                var e = arr[idx];

                if (ReferenceEquals(e, ctx.RuntimeContext.Nil.Instance))
                    arr.RemoveAt(idx);
                else
                    idx++;
            }

            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject Swap(ExecutionContext ctx, DyObject self, DyObject idx1, DyObject idx2)
        {
            var arr = (DyArray)self;
            var fst = arr.GetItem(idx1, ctx);

            if (ctx.HasErrors)
                return ctx.RuntimeContext.Nil.Instance;

            var snd = arr.GetItem(idx2, ctx);

            if (ctx.HasErrors)
                return ctx.RuntimeContext.Nil.Instance;

            arr.SetItem(idx1, snd, ctx);
            arr.SetItem(idx2, fst, ctx);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject InsertRange(ExecutionContext ctx, DyObject self, DyObject index, DyObject range)
        {
            if (index.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(index);

            var arr = (DyArray)self;
            var idx = (int)index.GetInteger();

            if (idx < 0 || idx > arr.Count)
                return ctx.IndexOutOfRange();

            var coll = DyIterator.ToEnumerable(ctx, range);

            if (ctx.HasErrors)
                return ctx.RuntimeContext.Nil.Instance;

            foreach (var e in coll)
                arr.Insert(idx++, e);

            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject RemoveRange(ExecutionContext ctx, DyObject self, DyObject items)
        {
            var arr = (DyArray)self;
            var coll = DyIterator.ToEnumerable(ctx, items);

            if (ctx.HasErrors)
                return ctx.RuntimeContext.Nil.Instance;

            var strict = coll.ToArray();

            foreach (var e in strict)
            {
                arr.Remove(ctx, e);

                if (ctx.HasErrors)
                    return ctx.RuntimeContext.Nil.Instance;
            }

            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject RemoveRangeAt(ExecutionContext ctx, DyObject self, DyObject start, DyObject len)
        {
            var arr = (DyArray)self;

            if (start.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(start);

            var sti = (int)start.GetInteger();

            if (sti < 0 || sti >= arr.Count)
                return ctx.IndexOutOfRange();

            int le;

            if (ReferenceEquals(len, ctx.RuntimeContext.Nil.Instance))
                le = arr.Count - sti;
            else if (len.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(len);
            else
                le = (int)len.GetInteger();

            if (sti + le > arr.Count)
                return ctx.IndexOutOfRange();

            arr.RemoveRange(sti, le);
            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject RemoveAll(ExecutionContext ctx, DyObject self, DyObject arg)
        {
            if (arg is not DyFunction fun)
                return ctx.InvalidType(arg);

            var arr = (DyArray)self;
            var toDelete = new List<DyObject>();

            foreach (var o in arr)
            {
                var res = fun.Call(ctx, o);

                if (ctx.HasErrors)
                    return ctx.RuntimeContext.Nil.Instance;

                if (res.GetBool())
                    toDelete.Add(o);
            }

            foreach (var o in toDelete)
            {
                arr.Remove(ctx, o);

                if (ctx.HasErrors)
                    return ctx.RuntimeContext.Nil.Instance;
            }

            return ctx.RuntimeContext.Nil.Instance;
        }

        private DyObject Contains(ExecutionContext ctx, DyObject self, DyObject item)
        {
            var arr = (DyArray)self;
            return arr.IndexOf(ctx, item) != -1 ? ctx.RuntimeContext.Bool.True : ctx.RuntimeContext.Bool.False;
        }

        protected override DyFunction? InitializeInstanceMember(DyObject self, string name, ExecutionContext ctx) =>
            name switch
            {
                "add" => Func.Member(name, AddItem, -1, new Par("item")),
                "insert" => Func.Member(name, InsertItem, -1, new Par("index"), new Par("item")),
                "insertRange" => Func.Member(name, InsertRange, -1, new Par("index"), new Par("items")),
                "addRange" => Func.Member(name, AddRange, -1, new Par("items")),
                "remove" => Func.Member(name, RemoveItem, -1, new Par("item")),
                "removeAt" => Func.Member(name, RemoveItemAt, -1, new Par("index")),
                "removeRange" => Func.Member(name, RemoveRange, -1, new Par("items")),
                "removeRangeAt" => Func.Member(name, RemoveRangeAt, -1, new Par("start"), new Par("len", StaticNil.Instance)),
                "removeAll" => Func.Member(name, RemoveAll, -1, new Par("predicate")),
                "clear" => Func.Member(name, ClearItems),
                "indexOf" => Func.Member(name, IndexOf, -1, new Par("item")),
                "lastIndexOf" => Func.Member(name, LastIndexOf, -1, new Par("item")),
                "sort" => Func.Member(name, SortBy, -1, new Par("by", StaticNil.Instance)),
                "swap" => Func.Member(name, Swap, -1, new Par("fst"), new Par("snd")),
                "compact" => Func.Member(name, Compact),
                "reverse" => Func.Member(name, Reverse),
                "contains" => Func.Member(name, Contains, -1, new Par("value")),
                _ => base.InitializeInstanceMember(self, name, ctx),
            };

        private DyObject New(ExecutionContext ctx, DyObject tuple) => new DyArray(ctx.RuntimeContext.Array, ((DyTuple)tuple).Values);

        private DyObject Empty(ExecutionContext ctx, DyObject sizeObj, DyObject val)
        {
            var size = sizeObj.GetInteger();
            var arr = new DyObject[size];

            if (val.DecType.TypeCode != DyTypeCode.Iterator)
                val = ((DyIterator)val).GetIteratorFunction();

            if (val is DyFunction func)
            {
                for (var i = 0; i < size; i++)
                {
                    var res = func.Call(ctx);

                    if (ctx.HasErrors)
                        return ctx.RuntimeContext.Nil.Instance;

                    arr[i] = res;
                }
            }
            else
            {
                for (var i = 0; i < size; i++)
                    arr[i] = val;
            }

            return new DyArray(ctx.RuntimeContext.Array, arr);
        }

        private static DyObject Concat(ExecutionContext ctx, DyObject values) =>
            new DyArray(ctx.RuntimeContext.Array, DyCollection.ConcatValues(ctx, values));

        private static DyObject Copy(ExecutionContext ctx, DyObject from, DyObject sourceIndex, DyObject to, DyObject destIndex, DyObject length)
        {
            if (sourceIndex.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(sourceIndex);

            var iSourceIndex = sourceIndex.GetInteger();

            if (destIndex.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(destIndex);

            var iDestIndex = destIndex.GetInteger();

            if (length.DecType.TypeCode != DyTypeCode.Integer)
                return ctx.InvalidType(length);

            var iLen = length.GetInteger();

            if (from.DecType.TypeCode != DyTypeCode.Array)
                return ctx.InvalidType(from);

            var sourceArr = (DyArray)from;

            if (to.DecType.TypeCode != DyTypeCode.Array && to.DecType.TypeCode != DyTypeCode.Nil)
                return ctx.InvalidType(to);

            var destArr = to == ctx.RuntimeContext.Nil.Instance 
                ? new DyArray(ctx.RuntimeContext.Array, new DyObject[iDestIndex + iLen]) : (DyArray)to;

            if (iSourceIndex < 0 || iSourceIndex >= sourceArr.Count)
                return ctx.IndexOutOfRange();

            if (iDestIndex < 0 || iDestIndex >= destArr.Count)
                return ctx.IndexOutOfRange();

            if (iSourceIndex + iLen < 0 || iSourceIndex + iLen > sourceArr.Count)
                return ctx.IndexOutOfRange();

            if (iDestIndex + iLen < 0 || iDestIndex + iLen > destArr.Count)
                return ctx.IndexOutOfRange();

            Array.Copy(sourceArr.Values, iSourceIndex, destArr.Values, iDestIndex, iLen);

            if (iDestIndex != 0 && to == ctx.RuntimeContext.Nil.Instance)
                for (var i = 0; i < iDestIndex; i++)
                    destArr[i] = ctx.RuntimeContext.Nil.Instance;

            return destArr;
        }

        protected override DyObject? InitializeStaticMember(string name, ExecutionContext ctx) =>
            name switch
            {
                "Array" => Func.Static(name, New, 0, new Par("values", true)),
                "sort" => Func.Static(name, SortBy, -1, new Par("values"), new Par("by", StaticNil.Instance)),
                "empty" => Func.Static(name, Empty, -1, new Par("size"), new Par("default", StaticNil.Instance)),
                "concat" => Func.Static(name, Concat, 0, new Par("values", true)),
                "copy" => Func.Static(name, Copy, -1, new Par("from"),
                    new Par("fromIndex", StaticInteger.Zero), new Par("to", StaticNil.Instance),
                    new Par("toIndex", StaticInteger.Zero), new Par("count")),
                _ => base.InitializeStaticMember(name, ctx)
            };
    }
}
