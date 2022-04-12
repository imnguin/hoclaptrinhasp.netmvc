using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNTCiname
{
    public class RandomCode
    {
        public static string MaXacNhan()
        {
            List<int> list = new List<int>();
            int max = 9;
            for (int i = 1; i <= max; i++)
            {
                list.Add(i);
            }
            Random random = new Random();
            string result = "";
            while (list.Count > 0)
            {
                int next = list[random.Next(list.Count)];
                list.Remove(next);
                result += next;
            }
            return result;
        }
    }
}