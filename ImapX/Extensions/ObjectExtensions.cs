using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ImapX.Extensions
{
    /// <summary>
    /// http://stackoverflow.com/a/11047578/1941460
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// A DeepClone method for types that are not serializable.
        /// </summary>
        public static T DeepCloneWithoutSerialization<T>(this T original)
        {
            return original.DeepClone(new Dictionary<object, object>());
        }

        static T DeepClone<T>(this T original, Dictionary<object, object> copies)
        {
            return (T)original.DeepClone(typeof(T), copies);
        }

        /// <summary>
        /// Deep clone an object without using serialisation.
        /// Creates a copy of each field of the object (and recurses) so that we end up with
        /// a copy that doesn't include any reference to the original object.
        /// </summary>
        static object DeepClone(this object original, Type t, Dictionary<object, object> copies)
        {
            // Check if object is immutable or copy on update
            if (t.IsValueType || original == null || t == typeof(string) || t == typeof(Guid))
                return original;

            // Interfaces aren't much use to us
            if (t.IsInterface)
                t = original.GetType();

            object tmpResult;
            // Check if the object already has been copied
            if (copies.TryGetValue(original, out tmpResult))
                return tmpResult;

            object result;
            if (!t.IsArray)
            {
                try
                {
                    result = Activator.CreateInstance(t);
                }
                catch
                {
                    result = Activator.CreateInstance(t, true);
                }
                copies.Add(original, result);
                
                // Maybe you need here some more BindingFlags
                foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance))
                {
                    var fieldValue = field.GetValue(original);
                    field.SetValue(result, fieldValue.DeepClone(field.FieldType, copies));
                }
            }
            else
            {
                // Handle arrays here
                var originalArray = (Array)original;
                var resultArray = (Array)originalArray.Clone();
                copies.Add(original, resultArray);

                var elementType = t.GetElementType();
                // If the type is not a value type we need to copy each of the elements
                if (!elementType.IsValueType)
                {
                    var lengths = new int[t.GetArrayRank()];
                    var indicies = new int[lengths.Length];
                    // Get lengths from original array
                    for (var i = 0; i < lengths.Length; i++)
                        lengths[i] = resultArray.GetLength(i);

                    var p = lengths.Length - 1;

                    /* Now we need to iterate though each of the ranks
                     * we need to keep it generic to support all array ranks */
                    while (Increment(indicies, lengths, p))
                    {
                        var value = resultArray.GetValue(indicies);
                        if (value != null)
                            resultArray.SetValue(value.DeepClone(elementType, copies), indicies);
                    }
                }
                result = resultArray;
            }
            return result;
        }

        static bool Increment(int[] indicies, int[] lengths, int p)
        {
            if (p > -1)
            {
                indicies[p]++;
                if (indicies[p] < lengths[p])
                    return true;

                if (Increment(indicies, lengths, p - 1))
                {
                    indicies[p] = 0;
                    return true;
                }
            }
            return false;
        }
    }
}
