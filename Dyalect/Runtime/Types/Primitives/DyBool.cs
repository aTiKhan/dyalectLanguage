﻿using System.IO;
namespace Dyalect.Runtime.Types;

public abstract class DyBool : DyObject
{
    public static readonly DyBool True = new DyBoolTrue();
    public static readonly DyBool False = new DyBoolFalse();

    private sealed class DyBoolTrue: DyBool
    {
        public override string ToString() => "true";

        public override int GetHashCode() => true.GetHashCode();
    }

    private sealed class DyBoolFalse: DyBool
    {
        public override string ToString() => "false";

        public override int GetHashCode() => false.GetHashCode();
    }

    private DyBool() : base(DyType.Bool) { }

    public override object ToObject() => this is DyBoolTrue;

    public override DyObject Clone() => this;

    public static explicit operator bool(DyBool v) => v is DyBoolTrue;

    public static explicit operator DyBool(bool v) => v ? True : False;

    public static DyBool Equals(ExecutionContext _, DyObject x, DyObject y)
    {
        if (ReferenceEquals(x, y))
            return True;

        return False;
    }

    internal override void Serialize(BinaryWriter writer)
    {
        writer.Write(TypeId);
        writer.Write(this is DyBoolTrue);
    }
}
