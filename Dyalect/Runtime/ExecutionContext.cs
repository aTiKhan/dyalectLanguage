﻿using Dyalect.Runtime.Types;
using System;
using System.Collections.Generic;

namespace Dyalect.Runtime
{
    public class ExecutionContext
    {
        internal int RgDI; //RgDI register
        internal int RgFI; //RgFI register
        internal int CallCnt; //Call counter

        public static readonly ExecutionContext External = new ExternalExecutionContext();

        private sealed class ExternalExecutionContext : ExecutionContext
        {
            internal ExternalExecutionContext() : base(null!, null!) { }

            internal override DyError? Error
            {
                get => base.Error;
                set
                {
                    base.Error = value;
                    ThrowIf();
                }
            }
        }

        internal ExecutionContext(CallStack callStack, RuntimeContext rtx)
        {
            CallStack = callStack;
            CatchMarks = new();
            RuntimeContext = rtx;
        }

        public RuntimeContext RuntimeContext { get; }

        public bool HasErrors => Error != null;

        public ExecutionContext Clone() => new(CallStack, RuntimeContext);

        internal CallStack CallStack { get; }

        internal SectionStack CatchMarks { get; }

        private DyError? _error;
        internal virtual DyError? Error
        {
            get => _error;
            set
            {
                if (_error is null || value is null)
                    _error = value;
            }
        }

        internal Stack<int>? Sections { get; set; }

        internal Stack<ArgContainer> Arguments { get; } = new(6);

        internal int UnitId;
        internal int CallerUnitId;

        public DyError? GetError() => Error;

        public void ThrowIf()
        {
            if (Error is not null)
            {
                var err = Error;
                Error = null;
                throw new DyRuntimeException(err.GetDescription());
            }
        }
    }

    internal struct ArgContainer
    {
        public DyObject[] Locals;
        public FastList<DyObject>? VarArgs;
        public int VarArgsIndex;
    }
}
