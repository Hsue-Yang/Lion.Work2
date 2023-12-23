using System;

namespace LionTech.Entity.TRAINING
{
    public static class Utility
    {
        public static EnumCultureID GetCultureID(string cultureID)
        {
            if (string.IsNullOrWhiteSpace(cultureID))
            {
                return EnumCultureID.zh_TW;
            }
            else
            {
                if (Enum.IsDefined(typeof(EnumCultureID), cultureID))
                {
                    return (EnumCultureID)Enum.Parse(typeof(EnumCultureID), cultureID);
                }
                else
                {
                    throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { "EnumCultureID", cultureID });
                }
            }
        }
    }
}