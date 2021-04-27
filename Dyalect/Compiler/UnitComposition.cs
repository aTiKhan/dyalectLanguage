﻿using Dyalect.Runtime.Types;
using System;
using System.Collections.Generic;

namespace Dyalect.Compiler
{
    public sealed class UnitComposition
    {
        public Guid Id { get; } = Guid.NewGuid();

        public UnitComposition(List<Unit> units)
        {
            Units = units;
            Types = DyType.GetAll();
            TypeCodes = new();
        }

        public List<Unit> Units { get; }

        public List<DyTypeInfo> Types { get; }

        internal Dictionary<Guid, int> TypeCodes { get; }
    }
}
