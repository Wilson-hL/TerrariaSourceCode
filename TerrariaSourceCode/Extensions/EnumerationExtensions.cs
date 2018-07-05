// Decompiled with JetBrains decompiler
// Type: Extensions.EnumerationExtensions
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;

namespace Extensions
{
    public static class EnumerationExtensions
    {
        public static T Include<T>(this Enum value, T append)
        {
            var type = value.GetType();
            var obj1 = value;
            var obj2 = new _Value(append, type);
            if (obj2.Signed.HasValue)
                obj1 = (Enum) (object) (Convert.ToInt64(value) | obj2.Signed.Value);
            else if (obj2.Unsigned.HasValue)
                obj1 = (Enum) (object) (ulong) ((long) Convert.ToUInt64(value) | (long) obj2.Unsigned.Value);
            return (T) Enum.Parse(type, obj1.ToString());
        }

        public static T Remove<T>(this Enum value, T remove)
        {
            var type = value.GetType();
            var obj1 = (object) value;
            var obj2 = new _Value(remove, type);
            if (obj2.Signed.HasValue)
                obj1 = Convert.ToInt64(value) & ~obj2.Signed.Value;
            else if (obj2.Unsigned.HasValue)
                obj1 = (ulong) ((long) Convert.ToUInt64(value) & ~(long) obj2.Unsigned.Value);
            return (T) Enum.Parse(type, obj1.ToString());
        }

        public static bool Has<T>(this Enum value, T check)
        {
            var type = value.GetType();
            var obj = new _Value(check, type);
            if (obj.Signed.HasValue)
                return (Convert.ToInt64(value) & obj.Signed.Value) == obj.Signed.Value;
            if (obj.Unsigned.HasValue)
                return ((long) Convert.ToUInt64(value) & (long) obj.Unsigned.Value) ==
                       (long) obj.Unsigned.Value;
            return false;
        }

        public static bool Missing<T>(this Enum obj, T value)
        {
            return !obj.Has(value);
        }

        private class _Value
        {
            private static readonly Type _UInt64 = typeof(ulong);
            private static readonly Type _UInt32 = typeof(long);
            public readonly long? Signed;
            public readonly ulong? Unsigned;

            public _Value(object value, Type type)
            {
                if (!type.IsEnum)
                    throw new ArgumentException("Value provided is not an enumerated type!");
                var underlyingType = Enum.GetUnderlyingType(type);
                if (underlyingType.Equals(_UInt32) ||
                    underlyingType.Equals(_UInt64))
                    Unsigned = Convert.ToUInt64(value);
                else
                    Signed = Convert.ToInt64(value);
            }
        }
    }
}