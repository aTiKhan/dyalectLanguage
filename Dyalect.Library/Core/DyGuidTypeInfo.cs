﻿using Dyalect.Codegen;
using Dyalect.Runtime;
using Dyalect.Runtime.Types;
using System;
namespace Dyalect.Library.Core;

[GeneratedType]
public sealed partial class DyGuidTypeInfo : DyForeignTypeInfo<CoreModule>
{
    private const string GuidType = "Guid";

    public override string ReflectedTypeName => GuidType;

    public DyGuidTypeInfo() => AddMixin(Dy.Comparable);

    protected override SupportedOperations GetSupportedOperations() =>
        SupportedOperations.Eq | SupportedOperations.Neq | SupportedOperations.Not;

    #region Operations
    protected override DyObject ToStringOp(DyObject arg, DyObject format, ExecutionContext ctx) =>
        new DyString("{" + arg.ToString().ToUpper() + "}");

    protected override DyObject EqOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return DyBool.False;

        return ((DyGuid)left).Value == ((DyGuid)right).Value ? DyBool.True : DyBool.False;
    }

    protected override DyObject GtOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return ctx.InvalidType(left.TypeId, right);

        return (DyBool)(((DyGuid)left).Value.CompareTo(((DyGuid)right).Value) > 0);
    }

    protected override DyObject GteOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return ctx.InvalidType(left.TypeId, right);

        return (DyBool)(((DyGuid)left).Value.CompareTo(((DyGuid)right).Value) >= 0);
    }

    protected override DyObject LtOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return ctx.InvalidType(left.TypeId, right);

        return (DyBool)(((DyGuid)left).Value.CompareTo(((DyGuid)right).Value) < 0);
    }

    protected override DyObject LteOp(DyObject left, DyObject right, ExecutionContext ctx)
    {
        if (left.TypeId != right.TypeId)
            return ctx.InvalidType(left.TypeId, right);

        return (DyBool)(((DyGuid)left).Value.CompareTo(((DyGuid)right).Value) <= 0);
    }

    protected override DyObject CastOp(DyObject self, DyTypeInfo targetType, ExecutionContext ctx)
    {
        if (targetType.ReflectedTypeId == Dy.String)
            return self.ToString(ctx);
        else if (targetType.ReflectedTypeId == DeclaringUnit.ByteArray.ReflectedTypeId)
            return ToByteArray(ctx, (DyGuid)self);
        else
            return base.CastOp(self, targetType, ctx);
    }
    #endregion

    [InstanceMethod]
    internal static DyObject ToByteArray(ExecutionContext ctx, DyGuid self) =>
        new DyByteArray(ctx.Type<DyByteArrayTypeInfo>(), self.Value.ToByteArray());

    [StaticMethod]
    internal static DyObject Parse(ExecutionContext ctx, string value)
    {
        try
        {
            return new DyGuid(ctx.Type<DyGuidTypeInfo>(), Guid.Parse(value));
        }
        catch (FormatException)
        {
            return ctx.InvalidValue(value);
        }
    }

    [StaticMethod]
    internal static DyObject FromByteArray(ExecutionContext ctx, DyByteArray value)
    {
        try
        {
            return new DyGuid(ctx.Type<DyGuidTypeInfo>(), new(value.GetBytes()));
        }
        catch (ArgumentException)
        {
            return ctx.InvalidValue(value);
        }
    }

    [StaticMethod]
    internal static DyObject Default(ExecutionContext ctx) => new DyGuid(ctx.Type<DyGuidTypeInfo>(), Guid.Empty);

    [StaticMethod]
    internal static DyObject Empty(ExecutionContext ctx) => Default(ctx);

    [StaticMethod(GuidType)]
    internal static DyObject NewGuid(ExecutionContext ctx) => new DyGuid(ctx.Type<DyGuidTypeInfo>(), Guid.NewGuid());
}
