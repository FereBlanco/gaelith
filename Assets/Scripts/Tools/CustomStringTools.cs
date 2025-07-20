using System;
using System.Text;
using Unity.VisualScripting;

namespace Scripts.Tools
{
    static public class CustomStringTools
    {

        static public string CreateString(string str1, string str2, string str3 = "", string str4 = "", string str5 = "", string str6 = "", string str7 = "", string str8 = "", string str9 = "", string str10 = "", string str11 = "", string str12 = "")
        {
            // Why are we using 12 params instead of: static public string CreateString(params string[] strings)
            // Every call to a params string[] method generates a new string array at the callsite, and this is compiled like: new string[] { ... }
            // This is a (low) performance issue, especially in a game loop.
            // So we use a fixed number of parameters to avoid the overhead of creating an array.
            // This way, no array is allocated, just stack variables.

            StringBuilder sb = new StringBuilder();

            sb.Append(str1); sb.Append(str2); sb.Append(str3); sb.Append(str4); sb.Append(str5); sb.Append(str6);
            sb.Append(str7); sb.Append(str8); sb.Append(str9); sb.Append(str10); sb.Append(str11); sb.Append(str12);

            return sb.ToString();
        }
    }
}