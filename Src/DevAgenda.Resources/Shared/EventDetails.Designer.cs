﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17020
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevAgenda.Resources.Shared {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EventDetails {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EventDetails() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DevAgenda.Resources.Shared.EventDetails", typeof(EventDetails).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event date.
        /// </summary>
        public static string EventDate {
            get {
                return ResourceManager.GetString("EventDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event location.
        /// </summary>
        public static string EventLocation {
            get {
                return ResourceManager.GetString("EventLocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event is not free.
        /// </summary>
        public static string EventNotFree {
            get {
                return ResourceManager.GetString("EventNotFree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event Twitter profile.
        /// </summary>
        public static string EventTwitterProfile {
            get {
                return ResourceManager.GetString("EventTwitterProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event type.
        /// </summary>
        public static string EventType {
            get {
                return ResourceManager.GetString("EventType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event website.
        /// </summary>
        public static string EventWebsite {
            get {
                return ResourceManager.GetString("EventWebsite", resourceCulture);
            }
        }
    }
}