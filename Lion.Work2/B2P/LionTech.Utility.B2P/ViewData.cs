using System.Web.Mvc;

namespace LionTech.Utility.B2P
{
    public enum EnumViewDataItem
    {
        UserID, UserNM,
        UserMenu,
        JsMsg, EditionNo
    }

    public static class ViewDataExtensions
    {
        public static T Get<T>(this ViewDataDictionary viewData, EnumViewDataItem viewDataItem)
        {
            return (T)viewData[viewDataItem.ToString()];
        }

        public static void Set<T>(this ViewDataDictionary viewData, EnumViewDataItem viewDataItem, object value)
        {
            viewData[viewDataItem.ToString()] = value;
        }
    }
}
