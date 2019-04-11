﻿using Dyalect.Runtime;
using Dyalect.Runtime.Types;

namespace Dyalect
{
    internal static class StandardType
    {
        public static readonly FastList<DyTypeInfo> All = new FastList<DyTypeInfo>
        {
            DyNilTypeInfo.Instance,
            DyIntegerTypeInfo.Instance,
            DyFloatTypeInfo.Instance,
            DyBoolTypeInfo.Instance,
            DyStringTypeInfo.Instance,
            DyFunctionTypeInfo.Instance,
            DyTypeTypeInfo.Instance,
            DyTupleTypeInfo.Instance,
            DyModuleTypeInfo.Instance
        };

        public const int Nil = 0;
        public const int Integer = 1;
        public const int Float = 2;
        public const int Bool = 3;
        public const int String = 4;
        public const int Function = 5;
        public const int Type = 6;
        public const int Tuple = 7;
        public const int Module = 8;

        public const string NilName = "Nil";
        public const string IntegerName = "Integer";
        public const string FloatName = "Float";
        public const string BoolName = "Bool";
        public const string StringName = "String";
        public const string FunctionName = "Function";
        public const string TypeName = "Type";
        public const string TupleName = "Tuple";
        public const string ModuleName = "Module";

        public static int GetTypeCodeByName(string name)
        {
            switch (name)
            {
                case NilName: return Nil;
                case IntegerName: return Integer;
                case FloatName: return Float;
                case BoolName: return Bool;
                case StringName: return String;
                case FunctionName: return Function;
                case TypeName: return Type;
                case TupleName: return Tuple;
                case ModuleName: return Module;
                default: return -1;
            }
        }
    }
}
