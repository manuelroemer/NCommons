﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NCommons.Monads.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NCommons.Monads.Resources.ExceptionStrings", typeof(ExceptionStrings).Assembly);
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
        ///   Looks up a localized string similar to It is impossible to retrieve a value from an empty optional which does not hold a value..
        /// </summary>
        internal static string Optional_IsEmpty {
            get {
                return ResourceManager.GetString("Optional_IsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An attempt was made to read the value of an either of type &quot;{0}&quot;. The either is, however, of type &quot;{1}&quot;..
        /// </summary>
        internal static string UnexpectedEitherTypeException_DefaultMessage {
            get {
                return ResourceManager.GetString("UnexpectedEitherTypeException_DefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot retrieve a value of type &quot;{0}&quot; from the variant, because it is empty (i.e. it doesn&apos;t hold any value)..
        /// </summary>
        internal static string Variant_GetValue_Empty {
            get {
                return ResourceManager.GetString("Variant_GetValue_Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot retrieve a value of type &quot;{0}&quot; from the variant, because it holds a value of type &quot;{1}&quot;..
        /// </summary>
        internal static string Variant_GetValue_HoldsOtherValue {
            get {
                return ResourceManager.GetString("Variant_GetValue_HoldsOtherValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The variant holds a type for which no match case was implemented. This is a bug within the library and must be fixed..
        /// </summary>
        internal static string Variant_Match_TypeShouldHaveBeenImplementedButWasnt {
            get {
                return ResourceManager.GetString("Variant_Match_TypeShouldHaveBeenImplementedButWasnt", resourceCulture);
            }
        }
    }
}
