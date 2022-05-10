﻿using Dyalect.Runtime.Types;
using Dyalect.Codegen;
namespace Dyalect;

public static partial class Dy
{
    public const int Nil = 1;
    public const int Object = 2;
    public const int Integer = 3;
    public const int Float = 4;
    public const int Bool = 5;
    public const int Char = 6;
    public const int String = 7;
    public const int Function = 8;
    public const int Label = 9;
    public const int TypeInfo = 10;
    public const int Module = 11;
    public const int Array = 12;
    public const int Iterator = 13;
    public const int Tuple = 14;
    public const int Dictionary = 15;
    public const int Set = 16;
    public const int Variant = 17;
    public const int Interop = 18;

    [Mixin] public const int Number = 19;
    [Mixin] public const int Order = 20;
    [Mixin] public const int Lookup = 21;
    [Mixin] public const int Collection = 22;
    [Mixin] public const int Show = 23;
    [Mixin] public const int Equatable = 24;
    [Mixin] public const int Sequence = 25;
    [Mixin] public const int Identity = 26;
    [Mixin] public const int Functor = 27;
    [Mixin] public const int Disposable = 28;
    [Mixin] public const int Container = 29;

    internal static FastList<DyTypeInfo> GetAll()
    {
        var xs = new FastList<DyTypeInfo>();
        GetAllGenerated(xs);
        return xs;
    }
    static partial void GetAllGenerated(FastList<DyTypeInfo> types);

    public static int GetTypeCodeByName(string name)
    {
        int code = 0;
        GetTypeCodeByNameGenerated(name, ref code);
        return code;
    }
    static partial void GetTypeCodeByNameGenerated(string name, ref int code);

    public static string GetTypeNameByCode(int code)
    {
        string name = null!;
        GetTypeNameByCodeGenerated(code, ref name);
        return name;
    }
    static partial void GetTypeNameByCodeGenerated(int code, ref string name);
}

public class DyTypes
{
    public readonly static DyNil Nil = DyNil.Instance;
    public readonly static DyBool True = DyBool.True;
    public readonly static DyBool False = DyBool.False;
}
