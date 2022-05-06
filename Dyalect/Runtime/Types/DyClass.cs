﻿using Dyalect.Compiler;
namespace Dyalect.Runtime.Types;

public sealed class DyClass : DyObject, IProduction
{
    public string Constructor { get; }

    internal Unit DeclaringUnit { get; }

    internal DyTuple Fields { get; }

    internal DyTuple Inits { get; }

    internal DyTypeInfo DecType { get; }

    public override string TypeName => DecType.ReflectedTypeName;
    
    internal DyClass(DyTypeInfo type, string ctor, DyTuple fields, DyTuple inits, Unit unit) : base(type.ReflectedTypeId) =>
        (DecType, Constructor, Fields, Inits, DeclaringUnit) = (type, ctor, fields, inits, unit);

    public override object ToObject() => this;

    public override int GetHashCode() => HashCode.Combine(Constructor, Fields);

    public override bool Equals(DyObject? other) =>
        other is not null && DecType.TypeId == other.TypeId && other is DyClass t 
            && t.Constructor == Constructor && t.Fields.Equals(Fields);

    public override DyObject Clone() => new DyClass(DecType, Constructor, Fields, Inits, DeclaringUnit);
}
