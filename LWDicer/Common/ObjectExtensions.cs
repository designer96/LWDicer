using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

using System.ArrayExtensions;

namespace System
{
    public static class ObjectExtensions
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// Deep Copy Method for structure
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 아래의 DeepCopyStruct 함수를 좀 더 쓰기 쉽도록 Generic type으로 변환하여 사용하는 함수.
        /// ex) cylData2 = DeepCopyStruct(cylData1);
        /// </summary>
        /// <returns></returns>
        public static T DeepCopyStruct<T>(T source) where T : struct
        {
            return (T)DeepCopyStruct(source, typeof(T));
        }

        /// <summary>
        /// 배열을 가지고 있는 구조체의 deep copy를 위한 함수.
        /// ex) cylData2 = (CCylinderData)DeepCopyStruct(cylData1, typeof(CCylinderData));
        /// </summary>
        private static object DeepCopyStruct(object anything, Type anyType)
        {
            return RawDeserialize(RawSerialize(anything), 0, anyType);
        }

        /* Source: http://bytes.com/topic/c-sharp/answers/249770-byte-structure */
        private static object RawDeserialize(byte[] rawData, int position, Type anyType)
        {
            int rawsize = Marshal.SizeOf(anyType);
            if (rawsize > rawData.Length)
                return null;
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anyType);
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }

        /* Source: http://bytes.com/topic/c-sharp/answers/249770-byte-structure */
        private static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// Deep Copy Method for Class
        ///////////////////////////////////////////////////////////////////////////////////////////
        private static readonly MethodInfo CloneMethod = typeof(Object).GetTypeInfo().GetDeclaredMethod("MemberwiseClone");

        public static bool IsValue(this Type type)
        {
            if (type == typeof(String)) return true;
            return type.GetTypeInfo().IsValueType;
        }

        private static Object Copy(this Object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
        }
        private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
        {
            if (originalObject == null) return null;
            var typeToReflect = originalObject.GetType();
            if (IsValue(typeToReflect)) return originalObject;
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(typeToReflect.GetTypeInfo())) return null;
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsValue(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect, info => !info.IsStatic && !info.FieldType.GetTypeInfo().IsPrimitive);
            RecursiveCopyBaseTypeFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypeFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.GetTypeInfo().BaseType != null)
            {
                RecursiveCopyBaseTypeFields(originalObject, visited, cloneObject, typeToReflect.GetTypeInfo().BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.GetTypeInfo().BaseType, info => !info.IsStatic && !info.FieldType.GetTypeInfo().IsPrimitive);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, Predicate<FieldInfo> filter = null)
        {
            List<FieldInfo> filtered = new List<FieldInfo>(typeToReflect.GetTypeInfo().DeclaredFields);
            if (filter != null)
            {
                filtered = filtered.FindAll(filter);
            }
            foreach (FieldInfo fieldInfo in filtered)
            {
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        public static T Copy<T>(this T original) where T : class
        {
            return (T)Copy((Object)original);
        }

        // 아래의 ToStringDictionary, FromStringDictionary functions are pair set by ranian
        public static Dictionary<string, string> ToStringDictionary(this object source, Type type)
        {
            Dictionary<string, string> fieldBook = new Dictionary<string, string>();

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                // 1.1 element
                if (field.FieldType.IsValueType || field.FieldType.Name.ToLower() == "string")
                {
                    fieldBook.Add(field.Name, field.GetValue(source)?.ToString());
                }
                // 1.2 array
                else if (field.FieldType.IsArray)
                {
                    Array array = (Array)field.GetValue(source);

                    // 1.2.1 1-D array
                    if (array.Rank == 1)
                    {
                        for (int i = 0; i < array.GetLength(0); i++)
                        {
                            fieldBook.Add($"{field.Name}__{i}", array.GetValue(i)?.ToString());
                        }
                    }
                    // 1.2.2 2-D array
                    else if (array.Rank == 2)
                    {
                        for (int i = 0; i < array.GetLength(0); i++)
                        {
                            for (int j = 0; j < array.GetLength(1); j++)
                            {
                                fieldBook.Add($"{field.Name}__{i}__{j}", array.GetValue(i, j)?.ToString());
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Not support {field.Name}'s array {array.Rank} dimension.");
                    }
                }
                else
                {
                    Debug.WriteLine($"Not support to handle {field.Name}'s {field.FieldType.ToString()}");
                }
            }

            for (int i = 0; i < fieldBook.Count; i++)
            {
                if (fieldBook.Values.ToList()[i] == null)
                {
                    fieldBook[fieldBook.Keys.ToList()[i]] = "";
                }

            }

            return fieldBook;
        }

        public static void FromStringDicionary(object target, Type type, Dictionary<string, string> fieldBook)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                // 3.1 handle element
                if (field.FieldType.IsValueType || field.FieldType.Name.ToLower() == "string")
                {
                    if (fieldBook.ContainsKey(field.Name))
                    {
                        SetFieldValue(target, field, fieldBook[field.Name]);
                    }
                }
                // 3.2 handle array
                else if (field.FieldType.IsArray)
                {
                    Array array = (Array)field.GetValue(target);
                    string key, value;

                    // 3.2.1 1-D array
                    if (array.Rank == 1)
                    {
                        var arr_1d = new string[array.GetLength(0)];
                        for (int i = 0; i < array.GetLength(0); i++)
                        {
                            key = $"{field.Name}__{i}";
                            value = fieldBook.ContainsKey(key) ? fieldBook[key] : "";
                            arr_1d.SetValue(value, i);
                        }
                        SetFieldValue(target, field, arr_1d);
                    }
                    // 3.2.1 2-D array
                    else if (array.Rank == 2)
                    {
                        var arr_2d = new string[array.GetLength(0), array.GetLength(1)];
                        for (int i = 0; i < array.GetLength(0); i++)
                        {
                            for (int j = 0; j < array.GetLength(1); j++)
                            {
                                key = $"{field.Name}__{i}__{j}";
                                value = fieldBook.ContainsKey(key) ? fieldBook[key] : "";
                                arr_2d.SetValue(value, i, j);
                            }
                        }
                        SetFieldValue(target, field, arr_2d);
                    }
                    else
                    {
                        Debug.WriteLine($"Not support {field.Name}'s array {array.Rank} dimension.");
                    }
                }
                // 3.3 not support
                else
                {
                    Debug.WriteLine($"Not support to handle {field.Name}'s {field.FieldType.ToString()}");
                }
            }
        }

        private static void SetFieldValue(Object target, FieldInfo fieldInfo, string value)
        {
            string fieldType = fieldInfo.FieldType.Name;
            fieldType = fieldType.ToLower();

            switch (fieldType)
            {
                case "boolean":
                    bool b;
                    fieldInfo.SetValue(target, bool.TryParse(value, out b) ? b : false);
                    break;

                case "int32":
                    int n;
                    fieldInfo.SetValue(target, int.TryParse(value, out n) ? n : 0);
                    break;

                case "double":
                    double d;
                    fieldInfo.SetValue(target, double.TryParse(value, out d) ? d : 0);
                    break;

                case "string":
                    fieldInfo.SetValue(target, value ?? "");
                    break;
            }
        }

        private static void SetFieldValue(Object target, FieldInfo fieldInfo, string[] arr)
        {
            string fieldType = fieldInfo.FieldType.GetElementType().Name;
            fieldType = fieldType.ToLower();

            switch (fieldType)
            {
                case "boolean":
                    bool b;
                    bool[] arr_b = Array.ConvertAll(arr, s => bool.TryParse(s, out b) ? b : false);
                    fieldInfo.SetValue(target, arr_b);
                    break;

                case "int32":
                    int n;
                    int[] arr_n = Array.ConvertAll(arr, s => int.TryParse(s, out n) ? n : 0);
                    //int[] arr_n1 = Array.ConvertAll(arr, int.Parse);
                    //int[] arr_n2 = arr.Select(s => int.TryParse(s, out n) ? n : 0).ToArray();
                    fieldInfo.SetValue(target, arr_n);
                    break;

                case "double":
                    double d;
                    double[] arr_d = Array.ConvertAll(arr, s => double.TryParse(s, out d) ? d : 0);
                    fieldInfo.SetValue(target, arr_d);
                    break;

                case "string":
                    fieldInfo.SetValue(target, arr);
                    break;
            }
        }

        private static void SetFieldValue(Object target, FieldInfo fieldInfo, string[,] arr)
        {
            string fieldType = fieldInfo.FieldType.GetElementType().Name;
            fieldType = fieldType.ToLower();

            // 0. string return
            switch (fieldType)
            {
                case "string":
                    fieldInfo.SetValue(target, arr);
                    return;
                    break;
            }

            // 1. initialize
            int n;
            double d;
            bool b;

            dynamic output = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), arr.GetLength(0), arr.GetLength(1));
            var converter = TypeDescriptor.GetConverter(fieldInfo.FieldType.GetElementType());

            // 2. convert
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    switch (fieldType)
                    {
                        case "boolean":
                            output[i, j] = (bool)converter.ConvertFromString((string)arr[i, j]);
                            break;

                        case "int32":
                            output[i, j] = (int)converter.ConvertFromString((string)arr[i, j]);
                            break;

                        case "double":
                            output[i, j] = (double)converter.ConvertFromString((string)arr[i, j]);
                            break;
                    }
                }
            }

            // 2. setvalue
            fieldInfo.SetValue(target, output);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    namespace ArrayExtensions
    {
        public static class ArrayExtensions
        {
            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.Length == 0) return;
                ArrayTraverse walker = new ArrayTraverse(array);
                do action(array, walker.Position);
                while (walker.Step());
            }
        }

        internal class ArrayTraverse
        {
            public int[] Position;
            private int[] maxLengths;

            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    maxLengths[i] = array.GetLength(i) - 1;
                }
                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < Position.Length; ++i)
                {
                    if (Position[i] < maxLengths[i])
                    {
                        Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
