﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4200
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamilyFinance2.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FamilyFinance2.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to -------------------------------------------------------------------------------
        ///-------------------------------------------------------------------------------
        ///--CREATE TABLE AccountCatagory
        ///--(
        ///--	id					tinyint			NOT NULL,
        ///--	[name]				nvarchar(10)	NOT NULL,
        ///--	CONSTRAINT PK_AccountCatagory_id    PRIMARY KEY (id)
        ///--);
        ///--INSERT INTO AccountCatagory VALUES (0, &apos;NULL&apos;);
        ///--INSERT INTO AccountCatagory VALUES (1, &apos;Income&apos;);
        ///--INSERT INTO AccountCatagory VALUES (2, &apos;Account&apos;);
        ///--INSERT INTO AccountCat [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Build_Tables {
            get {
                return ResourceManager.GetString("Build_Tables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE EnvelopeLine;
        ///DROP TABLE LineItem;
        ///DROP TABLE LineType;
        ///DROP TABLE AEBalance;
        ///DROP TABLE Envelope;
        ///DROP TABLE Account;
        ///DROP TABLE AccountType;
        ///
        ///
        ///
        ///.
        /// </summary>
        internal static string Drop_Tables {
            get {
                return ResourceManager.GetString("Drop_Tables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///
        ///-------------------------------------------------------------------------------
        ///-------------------------------------------------------------------------------
        ///--TABLE (id, name(30), typeID, catagory(1), eBalance, closed(b), creditDebit(b), envelopes(b), creditColumnName(15), debitColumnName(15))
        ///INSERT INTO Account VALUES (101, &apos;Main Checking&apos;, 4, 2, 0, 1, 1, 0.0);
        ///INSERT INTO Account VALUES (102, &apos;Vacation Savings&apos;, 5, 2, 0, 1, 1, 0.0);
        ///INSERT INTO Account VALUES (103, &apos;Local Bank MC&apos;, 6, 2, 0, 0 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Test_Data {
            get {
                return ResourceManager.GetString("Test_Data", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap TLVBank {
            get {
                object obj = ResourceManager.GetObject("TLVBank", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap TLVEnvelope {
            get {
                object obj = ResourceManager.GetObject("TLVEnvelope", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap TLVMoney {
            get {
                object obj = ResourceManager.GetObject("TLVMoney", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap TLVRedFlag {
            get {
                object obj = ResourceManager.GetObject("TLVRedFlag", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
