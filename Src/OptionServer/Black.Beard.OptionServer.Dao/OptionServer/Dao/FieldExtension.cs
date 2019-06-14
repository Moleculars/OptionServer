using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bb.OptionServer
{
    public static class FieldExtension
    {

        public static void Reset(this object self, ObjectMapping mapping = null)
        {

            var prop = self.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var properties = prop.Where(c => typeof(IField).IsAssignableFrom(c.PropertyType))
                .ToArray()
                ;

            foreach (var item in properties)
            {

                var field = item.GetValue(self) as IField;

                if (field == null)
                    continue;

                if (field.HasModified)
                    field.Reset();

                if (mapping != null)
                    field.Properties = mapping.IndexByName[item.Name];

            }

        }

        /// <summary>
        /// Determines all properties changed.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns>IEnumerable<("property name", 'new value', 'old value')></returns>
        public static IEnumerable<(string, IField, object, object)> Changed(this object self)
        {

            var properties = self.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(c => typeof(IField).IsAssignableFrom(c.PropertyType))
                ;

            foreach (var item in properties)
            {
                var field = item.GetValue(self) as IField;
                if (field != null && field.HasModified)
                        yield return (item.Name, field, field.Value, field.OldValue);
            }

        }


    }


}