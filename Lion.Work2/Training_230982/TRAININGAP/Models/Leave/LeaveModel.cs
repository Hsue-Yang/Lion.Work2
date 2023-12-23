using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRAININGAP.Models.Leave
{
    public class LeaveModel : _BaseAPModel
    {
        //十進制轉二進制
        public string ConvertToInt(List<int> value)
        {
            var result = value.Sum();
            string binaryString = Convert.ToString(result, 2).PadLeft(4, '0');
            return binaryString;
        }

        //二進制轉十進制
        public List<int> ConvertTo2Ary(string value)
        {

            List<int> result = new List<int>();
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '1') //逐一比對value的值是否=1，是的話用2的次方得出數值
                {
                    int pow = value.Length - i - 1;
                    int intValue = (int)Math.Pow(2, pow);
                    result.Add(intValue);
                }
            }
            return result;

        }
    }
}