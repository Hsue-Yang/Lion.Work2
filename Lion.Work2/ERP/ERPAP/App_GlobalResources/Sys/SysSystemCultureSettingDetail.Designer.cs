//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   用於查詢當地語系化字串等的強型別資源類別。
    /// </summary>
    // 這個類別為自動產生，方法是利用 StronglyTypedResourceBuilder
    // 類別透過 ResGen 或 Visual Studio 這類工具。
    // 若要加入或移除成員，請編輯您的 .ResX 檔，然後重新執行 ResGen
    // (利用 /str 選項)，或重建 Visual Studio 專案。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SysSystemCultureSettingDetail {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SysSystemCultureSettingDetail() {
        }
        
        /// <summary>
        ///   傳回這個類別所使用的快取 ResourceManager 執行個體。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.SysSystemCultureSettingDetail", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   覆寫目前執行緒的 CurrentUICulture 屬性，對象是所有
        ///   使用這個強型別資源類別的資源查閱。
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
        ///   查詢類似 刪除語系代碼明細失敗 的當地語系化字串。
        /// </summary>
        internal static string DeleteSystemCultureDetail_Failure {
            get {
                return ResourceManager.GetString("DeleteSystemCultureDetail_Failure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 編輯語系代碼明細失敗 的當地語系化字串。
        /// </summary>
        internal static string EditSystemCultureDetail_Failure {
            get {
                return ResourceManager.GetString("EditSystemCultureDetail_Failure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 語系代碼 的當地語系化字串。
        /// </summary>
        internal static string Label_CultureID {
            get {
                return ResourceManager.GetString("Label_CultureID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 語系名稱 的當地語系化字串。
        /// </summary>
        internal static string Label_CultureNM {
            get {
                return ResourceManager.GetString("Label_CultureNM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 顯示名稱 的當地語系化字串。
        /// </summary>
        internal static string Label_DisplayNM {
            get {
                return ResourceManager.GetString("Label_DisplayNM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 停用 的當地語系化字串。
        /// </summary>
        internal static string Label_IsDisable {
            get {
                return ResourceManager.GetString("Label_IsDisable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 SERP使用 的當地語系化字串。
        /// </summary>
        internal static string Label_IsSerpUse {
            get {
                return ResourceManager.GetString("Label_IsSerpUse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 使用 的當地語系化字串。
        /// </summary>
        internal static string Label_IsSerpUseDisable {
            get {
                return ResourceManager.GetString("Label_IsSerpUseDisable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 語系代碼設定明細 的當地語系化字串。
        /// </summary>
        internal static string Label_SystemCultureSettingDetail {
            get {
                return ResourceManager.GetString("Label_SystemCultureSettingDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 語系代碼明細已存在 的當地語系化字串。
        /// </summary>
        internal static string SystemMsg_IsExistSystemCultureDetail {
            get {
                return ResourceManager.GetString("SystemMsg_IsExistSystemCultureDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 無法取得語系代碼明細列表 的當地語系化字串。
        /// </summary>
        internal static string SystemMsg_UnGetSystemCultureDetail {
            get {
                return ResourceManager.GetString("SystemMsg_UnGetSystemCultureDetail", resourceCulture);
            }
        }
    }
}
