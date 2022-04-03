﻿using Dyalect.Linker;

namespace Dyalect.Library.Core
{
    [DyUnit("core")]
    public sealed class CoreModule : ForeignUnit
    {
        public DyByteArrayTypeInfo ByteArray { get; }
        public DyStringBuilderTypeInfo StringBuilder { get; }
        public DyRegexTypeInfo Regex { get; }
        public DyResultTypeInfo Result { get; }
        public DyGuidTypeInfo Guid { get; }
        public DyConsoleTypeInfo Console { get; }
        public DyDateTimeTypeInfo DateTime { get; }
        public DyTimeDeltaTypeInfo TimeDelta { get; }

        public CoreModule()
        {
            ByteArray = AddType<DyByteArrayTypeInfo>();
            StringBuilder = AddType<DyStringBuilderTypeInfo>();
            Regex = AddType<DyRegexTypeInfo>();
            Result = AddType<DyResultTypeInfo>();
            Guid = AddType<DyGuidTypeInfo>();
            Console = AddType<DyConsoleTypeInfo>();
            DateTime = AddType<DyDateTimeTypeInfo>();
            TimeDelta = AddType<DyTimeDeltaTypeInfo>();
        }
    }
}
