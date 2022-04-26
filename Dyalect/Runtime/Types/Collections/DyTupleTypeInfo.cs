﻿using Dyalect.Codegen;
using Dyalect.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Dyalect.Runtime.Types;

[GeneratedType]
internal sealed partial class DyTupleTypeInfo : DyCollectionTypeInfo
{
    protected override SupportedOperations GetSupportedOperations() =>
        SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not
        | SupportedOperations.Get | SupportedOperations.Set | SupportedOperations.Len
        | SupportedOperations.Add | SupportedOperations.Iter | SupportedOperations.Lit;

    public override string ReflectedTypeName => nameof(DyType.Tuple);

    public override int ReflectedTypeId => DyType.Tuple;

    public DyTupleTypeInfo() => AddMixin(DyType.Collection, DyType.Comparable);

    #region Operations
    protected override DyObject AddOp(DyObject left, DyObject right, ExecutionContext ctx) =>
        new DyTuple(((DyCollection)left).Concat(ctx, right));

    protected override DyObject LengthOp(DyObject arg, ExecutionContext ctx)
    {
        var len = ((DyTuple)arg).Count;
        return DyInteger.Get(len);
    }

    protected override DyObject ToStringOp(DyObject arg, DyObject format, ExecutionContext ctx) => ((DyTuple)arg).ToString(false, ctx);

    protected override DyObject ToLiteralOp(DyObject arg, ExecutionContext ctx) => ((DyTuple)arg).ToString(true, ctx);

    protected override DyObject EqOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return DyBool.False;

        var (t1, t2) = ((DyTuple)left, (DyTuple)right);

        if (t1.Count != t2.Count)
            return DyBool.False;

        var t1v = t1.UnsafeAccessValues();
        var t2v = t2.UnsafeAccessValues();

        for (var i = 0; i < t1.Count; i++)
        {
            if (t1v[i].NotEquals(t2v[i], ctx))
                return DyBool.False;

            if (ctx.HasErrors)
                return DyNil.Instance;
        }

        return DyBool.True;
    }

    protected override DyObject GtOp(DyObject left, DyObject right, ExecutionContext ctx) => Compare(true, left, right, ctx);

    protected override DyObject LtOp(DyObject left, DyObject right, ExecutionContext ctx) => Compare(false, left, right, ctx);

    protected override DyObject ContainsOp(DyObject self, DyObject field, ExecutionContext ctx)
    {
        if (!field.IsString(ctx)) return Nil;
        return ((DyTuple)self).GetOrdinal(field.GetString()) is not -1 ? DyBool.True : DyBool.False;
    }

    private static DyObject Compare(bool gt, DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return ctx.OperationNotSupported(gt ? Builtins.Gt : Builtins.Lt, left, right);

        var xs = (DyTuple)left;
        var ys = (DyTuple)right;
        var xsv = xs.UnsafeAccessValues();
        var ysv = ys.UnsafeAccessValues();
        var len = xs.Count > ys.Count ? ys.Count : xs.Count;

        for (var i = 0; i < len; i++)
        {
            var x = xsv[i];
            var y = ysv[i];
            var res = gt ? x.Greater(y, ctx) : x.Lesser(y, ctx);

            if (ctx.HasErrors)
                return DyNil.Instance;

            if (res)
                return DyBool.True;

            res = x.Equals(y, ctx);

            if (ctx.HasErrors)
                return DyNil.Instance;

            if (!res)
                return DyBool.False;
        }

        return DyBool.False;
    }

    protected override DyObject GetOp(DyObject self, DyObject index, ExecutionContext ctx) => self.GetItem(index, ctx);

    protected override DyObject SetOp(DyObject self, DyObject index, DyObject value, ExecutionContext ctx)
    {
        self.SetItem(index, value, ctx);
        return DyNil.Instance;
    }
    #endregion

    [InstanceMethod(Method.Add)]
    internal static DyObject AddItem(DyTuple self, DyObject value)
    {
        var arr = new DyObject[self.Count + 1];
        Array.Copy(self.UnsafeAccessValues(), arr, self.Count);
        arr[^1] = value;
        return new DyTuple(arr);
    }

    [InstanceMethod]
    internal static DyObject Remove(ExecutionContext ctx, DyTuple self, DyObject value)
    {
        var tv = self.UnsafeAccessValues();

        for (var i = 0; i < tv.Length; i++)
        {
            var e = tv[i].GetTaggedValue();

            if (e.Equals(value, ctx))
                return RemoveAt(self, i);
        }

        return self;
    }

    [InstanceMethod]
    internal static DyObject RemoveAt(ExecutionContext ctx, DyTuple self, int index)
    {
        index = index < 0 ? self.Count + index : index;

        if (index < 0 || index >= self.Count)
            return ctx.IndexOutOfRange(index);

        return RemoveAt(self, index);
    }

    internal static DyTuple RemoveAt(DyTuple self, int index)
    {
        var arr = new DyObject[self.Count - 1];
        var c = 0;
        var sv = self.UnsafeAccessValues();

        for (var i = 0; i < self.Count; i++)
        {
            if (i != index)
                arr[c++] = sv[i];
        }

        return new DyTuple(arr);
    }

    [InstanceMethod]
    internal static DyObject Concat(ExecutionContext ctx, DyObject values) =>
        new DyTuple(DyCollection.ConcatValues(ctx, values));

    [InstanceMethod]
    internal static DyObject Insert(ExecutionContext ctx, DyTuple self, int index, DyObject value)
    {
        index = index < 0 ? self.Count + index : index;

        if (index < 0 || index > self.Count)
            return ctx.IndexOutOfRange(index);

        var arr = new DyObject[self.Count + 1];
        arr[index] = value;

        if (index == 0)
            Array.Copy(self.UnsafeAccessValues(), 0, arr, 1, self.Count);
        else if (index == self.Count)
            Array.Copy(self.UnsafeAccessValues(), 0, arr, 0, self.Count);
        else
        {
            Array.Copy(self.UnsafeAccessValues(), 0, arr, 0, index);
            Array.Copy(self.UnsafeAccessValues(), index, arr, index + 1, self.Count - index);
        }

        return new DyTuple(arr);
    }

    [InstanceMethod]
    internal static DyObject Keys(DyTuple self)
    {
        IEnumerable<DyObject> Iterate()
        {
            for (var i = 0; i < self.Count; i++)
            {
                var k = self.GetKey(i);
                if (k is not null)
                    yield return new DyString(k);
            }
        }

        return DyIterator.Create(Iterate());
    }

    [InstanceMethod]
    internal static DyObject First(ExecutionContext ctx, DyObject self) =>
        self.GetItem(DyInteger.Zero, ctx);

    [InstanceMethod]
    internal static DyObject Second(ExecutionContext ctx, DyObject self) =>
        self.GetItem(DyInteger.One, ctx);

    [InstanceMethod]
    internal static DyObject Sort(ExecutionContext ctx, DyTuple self, DyObject? comparer = null)
    {
        var sortComparer = new SortComparer(comparer, ctx);
        var newArr = new DyObject[self.Count];
        Array.Copy(self.UnsafeAccessValues(), newArr, newArr.Length);
        Array.Sort(newArr, 0, newArr.Length, sortComparer);
        return new DyTuple(newArr);
    }

    [InstanceMethod]
    internal static DyObject ToDictionary(ExecutionContext ctx, DyTuple self) =>
        new DyDictionary(self.ConvertToDictionary(ctx));

    [InstanceMethod]
    internal static DyObject ToArray(DyTuple self) => new DyArray(self.GetValues());

    [InstanceMethod]
    internal static DyObject Compact(ExecutionContext ctx, DyTuple self, DyObject? predicate = null)
    {
        var xs = new List<DyObject>();

        foreach (var val in self.GetValues())
        {
            if (predicate is not null)
            {
                var res = predicate.Invoke(ctx, val);

                if (ctx.HasErrors)
                    return DyNil.Instance;

                if (ReferenceEquals(res, DyBool.False))
                    xs.Add(val);
            }
            else if (!ReferenceEquals(val, DyNil.Instance))
                xs.Add(val);
        }

        return new DyTuple(xs.ToArray());
    }

    [InstanceMethod]
    internal static DyObject Alter(DyTuple self, [VarArg]DyTuple values)
    {
        var xs = new List<DyObject>(self.UnsafeAccessValues());

        foreach (var o in values.UnsafeAccessValues())
        {
            if (o is DyLabel lab)
            {
                var exist = xs.FirstOrDefault(i => i.GetLabel() == lab.Label) as DyLabel;

                if (exist is not null)
                {
                    exist.Value = lab.Value;
                    continue;
                }
            }

            xs.Add(o);
        }

        return new DyTuple(xs.ToArray());
    }

    [StaticMethod(Method.Sort)]
    internal static DyObject StaticSort(ExecutionContext ctx, DyTuple value, DyObject? comparer = null) =>
        Sort(ctx, value, comparer);

    [StaticMethod]
    internal static DyObject Pair(DyObject first, DyObject second) =>
        new DyTuple(new[] { first, second });

    [StaticMethod]
    internal static DyObject Triple(DyObject first, DyObject second, DyObject third) =>
        new DyTuple(new[] { first, second, third });

    [StaticMethod(Method.Concat)]
    internal static DyObject StaticConcat(ExecutionContext ctx, [VarArg]DyObject values) =>
        new DyTuple(DyCollection.ConcatValues(ctx, values));

    [StaticMethod(Method.Tuple)]
    internal static DyObject MakeNew([VarArg]DyObject values) => values;
}
