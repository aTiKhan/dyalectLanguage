﻿using Dyalect.Debug;
using Dyalect.Runtime.Types;
using System;
using System.Collections.Generic;

namespace Dyalect.Compiler
{
    public class Unit
    {
        internal Unit()
        {
            Layouts = new();
            Ops = new();
            Layouts = new();
            ExportList = new();
            UnitIds = new();
            References = new();
            Strings = new();
            Objects = new();
        }

        private Unit(Unit unit, DebugInfo di)
        {
            Ops = new(unit.Ops.ToArray());
            Symbols = di;

            if (unit.GlobalScope is not null)
                GlobalScope = unit.GlobalScope.Clone();

            Layouts = unit.Layouts;
            ExportList = unit.ExportList;
            UnitIds = unit.UnitIds;
            References = unit.References;
            Strings = unit.Strings;
            Objects = unit.Objects;
        }

        internal int Checksum { get; set; }

        internal Unit Clone(DebugInfo di) => new(this, di);

        internal int Id { get; set; }

        internal List<Reference> References { get; }

        internal List<int> UnitIds { get; }

        internal List<HashString> Strings { get; }

        internal List<DyObject> Objects { get; }

        internal List<Op> Ops { get; }

        public string? FileName { get; internal set; }

        public DebugInfo Symbols { get; internal set; } = null!;

        public Scope? GlobalScope { get; internal set; }

        public List<MemoryLayout> Layouts { get; }

        public Dictionary<HashString, ScopeVar> ExportList { get; }
    }
}