using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;

namespace TVTower.AITest
{
    public static class Extensions
    {
        public static string GetValueByKey( this LuaTable table, string key )
        {
            var keyList = table.Keys.OfType<object>().ToList();
            if ( keyList.Contains( key ) )
            {
                return table.GetStringByIndex( keyList.IndexOf( key ) );
            }

            return null;
        }

        public static object GetObjectByIndex( this LuaTable table, int index )
        {
            var valueList = table.Values.OfType<object>().ToList();
            return valueList[index];
        }

        public static string GetStringByIndex( this LuaTable table, int index )
        {
            return table.GetObjectByIndex(index).ToString();
        }
    }
}
