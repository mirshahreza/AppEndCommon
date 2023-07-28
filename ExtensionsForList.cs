﻿using System.Text;
using System.Text.RegularExpressions;

namespace AppEnd
{
    public static class ExtensionsForList
    {
        public static void AddIfNotContains(this List<string> list, string name)
        {
            if (!list.Contains(name))
            {
                list.Add(name);
            }
        }

    }
}