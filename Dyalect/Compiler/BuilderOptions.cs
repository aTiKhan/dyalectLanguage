﻿namespace Dyalect.Compiler
{
    public sealed class BuilderOptions
    {
        public readonly static BuilderOptions Default = new BuilderOptions
        {
            Debug = false,
            NoLangModule = false,
            NoWarnings = false,
            NoWarningsLinker = false,
            LinkerSkipChecksum = false
        };

        public bool Debug { get; set; }

        public bool NoLangModule { get; set; }

        public bool NoWarnings { get; set; }

        public bool NoWarningsLinker { get; set; }

        public bool LinkerSkipChecksum { get; set; }
    }
}
