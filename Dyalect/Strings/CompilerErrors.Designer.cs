﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dyalect.Strings {
    using System;


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CompilerErrors {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CompilerErrors() {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Dyalect.Strings.CompilerErrors", typeof(CompilerErrors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Functions declared with an &quot;auto&quot; modifier shouldn&apos;t accept any arguments..
        /// </summary>
        internal static string AutoNoParams {
            get {
                return ResourceManager.GetString("AutoNoParams", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A modifier &quot;auto&quot; is only allowed on member functions..
        /// </summary>
        internal static string AutoOnlyMethod {
            get {
                return ResourceManager.GetString("AutoOnlyMethod", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Keyword &quot;base&quot; is not available in the current context..
        /// </summary>
        internal static string BaseNotAllowed {
            get {
                return ResourceManager.GetString("BaseNotAllowed", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Pattern matching in variable binding requires initialization clause..
        /// </summary>
        internal static string BindingPatternNoInit {
            get {
                return ResourceManager.GetString("BindingPatternNoInit", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Empty code islands are not supported..
        /// </summary>
        internal static string CodeIslandEmpty {
            get {
                return ResourceManager.GetString("CodeIslandEmpty", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid code island inside of a string literal: {0}.
        /// </summary>
        internal static string CodeIslandInvalid {
            get {
                return ResourceManager.GetString("CodeIslandInvalid", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to use &quot;new&quot; operator outside of a method..
        /// </summary>
        internal static string CtorNoMethod {
            get {
                return ResourceManager.GetString("CtorNoMethod", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to use &quot;new&quot; operator for a non local type &quot;{0}&quot;..
        /// </summary>
        internal static string CtorOnlyLocalType {
            get {
                return ResourceManager.GetString("CtorOnlyLocalType", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An expression doesn&apos;t have a name..
        /// </summary>
        internal static string ExpressionNoName {
            get {
                return ResourceManager.GetString("ExpressionNoName", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Function &quot;{0}&quot; is deprecated..
        /// </summary>
        internal static string FunctionDeprecated {
            get {
                return ResourceManager.GetString("FunctionDeprecated", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Default parameter value for &apos;{0}&apos; must be of a primitive type (integer, float, character, string or nil)..
        /// </summary>
        internal static string InvalidDefaultValue {
            get {
                return ResourceManager.GetString("InvalidDefaultValue", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Named argument &quot;{0}&quot; cannot be specified multiple times..
        /// </summary>
        internal static string NamedArgumentMultipleTimes {
            get {
                return ResourceManager.GetString("NamedArgumentMultipleTimes", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to No enclosing loop out of which to break or continue..
        /// </summary>
        internal static string NoEnclosingLoop {
            get {
                return ResourceManager.GetString("NoEnclosingLoop", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Overriding of &quot;{0}&quot; method is not allowed..
        /// </summary>
        internal static string OverrideNotAllowed {
            get {
                return ResourceManager.GetString("OverrideNotAllowed", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Pattern &quot;{0}&quot; would never match..
        /// </summary>
        internal static string PatternNeverMatch {
            get {
                return ResourceManager.GetString("PatternNeverMatch", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Pattern &quot;{0}&quot; not supported in this context..
        /// </summary>
        internal static string PatternNotSupported {
            get {
                return ResourceManager.GetString("PatternNotSupported", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A modifier &quot;private&quot; is only valid on regular functions..
        /// </summary>
        internal static string PrivateMethod {
            get {
                return ResourceManager.GetString("PrivateMethod", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to access a private module member &quot;{0}&quot;..
        /// </summary>
        internal static string PrivateNameAccess {
            get {
                return ResourceManager.GetString("PrivateNameAccess", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Private functions should be declared in global scope..
        /// </summary>
        internal static string PrivateOnlyGlobal {
            get {
                return ResourceManager.GetString("PrivateOnlyGlobal", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Range is not supported in this context..
        /// </summary>
        internal static string RangeIndexNotSupported {
            get {
                return ResourceManager.GetString("RangeIndexNotSupported", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The &quot;return&quot; directive is supported only inside functions..
        /// </summary>
        internal static string ReturnNotAllowed {
            get {
                return ResourceManager.GetString("ReturnNotAllowed", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Function &quot;{0}&quot; cannot be declared as static. Only methods can be static..
        /// </summary>
        internal static string StaticOnlyMethods {
            get {
                return ResourceManager.GetString("StaticOnlyMethods", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Too many errors (error limit exceeded)..
        /// </summary>
        internal static string TooManyErrors {
            get {
                return ResourceManager.GetString("TooManyErrors", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Module already contains a definition for &quot;{0}&quot;..
        /// </summary>
        internal static string TypeAlreadyDeclared {
            get {
                return ResourceManager.GetString("TypeAlreadyDeclared", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to change a value of a constant &quot;{0}&quot;..
        /// </summary>
        internal static string UnableAssignConstant {
            get {
                return ResourceManager.GetString("UnableAssignConstant", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to assign a value to an expression: {0}..
        /// </summary>
        internal static string UnableAssignExpression {
            get {
                return ResourceManager.GetString("UnableAssignExpression", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to link module &apos;{0}&quot;. Compilation terminated..
        /// </summary>
        internal static string UnableToLinkModule {
            get {
                return ResourceManager.GetString("UnableToLinkModule", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Variable &quot;{0}&quot; is not declared in a parent scope..
        /// </summary>
        internal static string UndefinedBaseVariable {
            get {
                return ResourceManager.GetString("UndefinedBaseVariable", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unknown module &quot;{0}&quot;..
        /// </summary>
        internal static string UndefinedModule {
            get {
                return ResourceManager.GetString("UndefinedModule", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unknown type &quot;{0}&quot;..
        /// </summary>
        internal static string UndefinedType {
            get {
                return ResourceManager.GetString("UndefinedType", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Variable &quot;{0}&quot; is not declared..
        /// </summary>
        internal static string UndefinedVariable {
            get {
                return ResourceManager.GetString("UndefinedVariable", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Match entry &quot;{0}&quot; is unreachable..
        /// </summary>
        internal static string UnreachableMatchEntry {
            get {
                return ResourceManager.GetString("UnreachableMatchEntry", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Default values are not allowed for the argument lists..
        /// </summary>
        internal static string VarArgNoDefaultValue {
            get {
                return ResourceManager.GetString("VarArgNoDefaultValue", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Only one parameter of type argument list is allowed..
        /// </summary>
        internal static string VarArgOnlyOne {
            get {
                return ResourceManager.GetString("VarArgOnlyOne", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Variable &quot;{0}&quot; is already declared..
        /// </summary>
        internal static string VariableAlreadyDeclared {
            get {
                return ResourceManager.GetString("VariableAlreadyDeclared", resourceCulture);
            }
        }
    }
}
