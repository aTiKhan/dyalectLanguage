﻿using Dyalect.Compiler;
using Dyalect.Runtime.Types;
using System.Collections.Generic;
using System;
using Dyalect.Runtime;
using System.Reflection;
using Dyalect.Debug;
using Dyalect.Strings;

namespace Dyalect.Linker
{
    public abstract class ForeignUnit : Unit
    {
        internal List<DyObject> Values { get; } = new List<DyObject>();

        protected ForeignUnit()
        {
            InitializeMembers();
        }

        internal protected void Add(string name, DyObject obj)
        {
            ExportList.Add(name, new ScopeVar(0 | ExportList.Count << 8, VarFlags.Foreign));
            Values.Add(obj);
        }

        internal void Modify(int id, DyObject obj)
        {
            Values[id] = obj;
        }

        public virtual void Execute(ExecutionContext ctx)
        {

        }

        protected virtual void InitializeMembers()
        {
            var methods = GetType().GetMethods();

            foreach (var mi in methods)
            {
                var attr = Attribute.GetCustomAttribute(mi, typeof(FunctionAttribute));

                if (attr != null)
                {
                    var name = attr.ToString() ?? mi.Name;
                    var obj = ProcessMethod(name, mi);
                    Add(name, obj);
                }
            }
        }

        private DyObject ProcessMethod(string name, MethodInfo mi)
        {
            var pars = mi.GetParameters();
            var parsMeta = pars.Length > 1 ? new Par[pars.Length - 1] : null;
            var varArgIndex = -1;

            if (pars.Length == 0 || pars[0].ParameterType != typeof(ExecutionContext))
                throw new DyException(LinkerErrors.MethodNotSupported.Format(mi.Name));

            for (var i = 1; i < pars.Length; i++)
            {
                var p = pars[i];

                if (p.ParameterType != Dyalect.Types.DyObject)
                    throw new DyException(LinkerErrors.ParameterNotSupported.Format(p.ParameterType, mi.Name));

                var va = false;

                if (Attribute.IsDefined(p, typeof(VarArgAttribute)))
                {
                    if (varArgIndex != -1)
                        throw new DyException(LinkerErrors.DuplicateVarArgs.Format(mi.Name));

                    va = true;
                    varArgIndex = i - 1;
                }

                DyObject def = null;

                if (Attribute.GetCustomAttribute(p, typeof(DefaultAttribute)) is DefaultAttribute defAttr)
                    def = defAttr.Value;

                parsMeta[i - 1] = new Par(p.Name, def, va);
            }

            if (parsMeta == null)
                return DyForeignFunction.Static(name, (Func<ExecutionContext, DyObject>)mi.CreateDelegate(typeof(Func<ExecutionContext, DyObject>), this));

            if (parsMeta.Length == 1)
                return DyForeignFunction.Static(name, (Func<ExecutionContext, DyObject, DyObject>)mi.CreateDelegate(typeof(Func<ExecutionContext, DyObject, DyObject>), this), varArgIndex, parsMeta);

            if (parsMeta.Length == 2)
                return DyForeignFunction.Static(name, (Func<ExecutionContext, DyObject, DyObject, DyObject>)mi.CreateDelegate(typeof(Func<ExecutionContext, DyObject, DyObject, DyObject>), this), varArgIndex, parsMeta);

            if (parsMeta.Length == 3)
                return DyForeignFunction.Static(name, (Func<ExecutionContext, DyObject, DyObject, DyObject, DyObject>)mi.CreateDelegate(typeof(Func<ExecutionContext, DyObject, DyObject, DyObject, DyObject>), this), varArgIndex, parsMeta);

            if (parsMeta.Length == 4)
                return DyForeignFunction.Static(name, (Func<ExecutionContext, DyObject, DyObject, DyObject, DyObject, DyObject>)mi.CreateDelegate(typeof(Func<ExecutionContext, DyObject, DyObject, DyObject, DyObject, DyObject>), this), varArgIndex, parsMeta);

            throw new DyException(LinkerErrors.TooManyParameters.Format(mi.Name));
        }

        protected DyObject Default() => DyNil.Instance;
    }
}
