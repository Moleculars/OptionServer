using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer
{

    public static class MapExtension
    {

        public static T RemapFrom<T>(T self, T source)
        {


            return self;

        }

    }

    public static class AccessExtension
    {

        public static bool CanDoIt(this AccessEntityEnum self, AccessEntityEnum acc)
        {

            if ((self & acc) == acc)
                return true;

            return false;

        }

        public static string[] GetPrivilegesToString(this AccessEntityEnum self)
        {

            var values = Enum.GetValues(typeof(AccessEntityEnum));


            List<AccessEntityEnum> result = new List<AccessEntityEnum>();

            foreach (AccessEntityEnum item in values)
                if (item != default(AccessEntityEnum))
                    if ((self & item) == item)
                        result.Add(item);

            result = result.OrderByDescending(c => (int)c).ToList();

            var v1 = result.ToList();
            var v2 = result.ToList();

            foreach (var item1 in v1)
                foreach (var item2 in v2)
                    if (item1 != item2)
                        if ((item1 & item2) == item2)
                            if (result.Contains(item2))
                                result.Remove(item2);

            List<string> results = new List<string>();
            foreach (AccessEntityEnum item in result)
                if ((self & item) == item)
                    results.Add(item.ToString());


            return results.ToArray();

        }

    }

}
