﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TeaTimeAdvance.Common
{

    public static class UnsafeHelper
    {
        // NOTE: From Ryujinx graphics device project.
        public static int SizeOf(Type type)
        {
            // Is type a enum type?
            if (type.IsEnum)
            {
                type = type.GetEnumUnderlyingType();
            }

            // Is type a pointer type?
            if (type.IsPointer || type == typeof(IntPtr) || type == typeof(UIntPtr))
            {
                return IntPtr.Size;
            }

            // Is type a struct type?
            if (type.IsValueType && !type.IsPrimitive)
            {
                // Check if the struct has a explicit size, if so, return that.
                if (type.StructLayoutAttribute.Size != 0)
                {
                    return type.StructLayoutAttribute.Size;
                }

                // Otherwise we calculate the sum of the sizes of all fields.
                int size = 0;
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                {
                    size += SizeOf(fields[fieldIndex].FieldType);
                }

                return size;
            }

            // Primitive types.
            return (Type.GetTypeCode(type)) switch
            {
                TypeCode.SByte => sizeof(sbyte),
                TypeCode.Byte => sizeof(byte),
                TypeCode.Int16 => sizeof(short),
                TypeCode.UInt16 => sizeof(ushort),
                TypeCode.Int32 => sizeof(int),
                TypeCode.UInt32 => sizeof(uint),
                TypeCode.Int64 => sizeof(long),
                TypeCode.UInt64 => sizeof(ulong),
                TypeCode.Char => sizeof(char),
                TypeCode.Single => sizeof(float),
                TypeCode.Double => sizeof(double),
                TypeCode.Decimal => sizeof(decimal),
                TypeCode.Boolean => sizeof(bool),
                _ => throw new ArgumentException($"Length for type \"{type.Name}\" is unknown.")
            };
        }

        public static int OffsetOf<T>(string fieldName) where T: unmanaged
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int offset = 0;

            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
            {
                var field = fields[fieldIndex];

                int sizeOfField = SizeOf(field.FieldType);

                for (int i = 0; i < sizeOfField; i++)
                {
                    int index = offset + i;

                    if (field.Name == fieldName)
                    {
                        return index;
                    }
                }

                offset += sizeOfField;
            }

            return -1;
        }
    }
}
