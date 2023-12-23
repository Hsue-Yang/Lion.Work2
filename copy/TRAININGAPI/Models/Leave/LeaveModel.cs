using System;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveModel : _BaseAPModel
    {
        public string GetSignString(int signValue)
        {
            string binaryString = Convert.ToString(signValue, 2); // 將數字轉換為二進制字串
            string result = "";
            // 根據二進制字串的位置判斷角色
            if (binaryString.Length >= 1 && binaryString[binaryString.Length - 1] == '1')
            {
                result += "直屬主管,";
            }
            if (binaryString.Length >= 2 && binaryString[binaryString.Length - 2] == '1')
            {
                result += "部門主管,";
            }
            if (binaryString.Length >= 3 && binaryString[binaryString.Length - 3] == '1')
            {
                result += "處主管,";
            }
            if (binaryString.Length >= 4 && binaryString[binaryString.Length - 4] == '1')
            {
                result += "群主管,";
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.TrimEnd(','); // 移除最後的逗號
            }
            else
            {
                result = "無角色";
            }
            return result;
        }
    }
}