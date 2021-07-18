using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace HZDCoreEditor.Util
{
    public static class Bits
    {
        // Converts a Boolean into an array of bytes with length one.
        public static byte[] GetBytes(bool value)
        {
            byte[] r = new byte[1];
            r[0] = (value ? (byte)1 : (byte)0);
            return r;
        }

        // Converts a Boolean into a Span of bytes with length one.
        public static bool TryWriteBytes(Span<byte> destination, bool value)
        {
            if (destination.Length < sizeof(byte))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value ? (byte)1 : (byte)0);
            return true;
        }

        // Converts a char into an array of bytes with length two.
        public static byte[] GetBytes(char value)
        {
            byte[] bytes = new byte[sizeof(char)];
            Unsafe.As<byte, char>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a char into a Span
        public static bool TryWriteBytes(Span<byte> destination, char value)
        {
            if (destination.Length < sizeof(char))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts a short into an array of bytes with length
        // two.
        public static byte[] GetBytes(short value)
        {
            byte[] bytes = new byte[sizeof(short)];
            Unsafe.As<byte, short>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a short into a Span
        public static bool TryWriteBytes(Span<byte> destination, short value)
        {
            if (destination.Length < sizeof(short))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts an int into an array of bytes with length 
        // four.
        public static byte[] GetBytes(int value)
        {
            byte[] bytes = new byte[sizeof(int)];
            Unsafe.As<byte, int>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts an int into a Span
        public static bool TryWriteBytes(Span<byte> destination, int value)
        {
            if (destination.Length < sizeof(int))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts a long into an array of bytes with length 
        // eight.
        public static byte[] GetBytes(long value)
        {
            byte[] bytes = new byte[sizeof(long)];
            Unsafe.As<byte, long>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a long into a Span
        public static bool TryWriteBytes(Span<byte> destination, long value)
        {
            if (destination.Length < sizeof(long))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts an ushort into an array of bytes with
        // length two.
        public static byte[] GetBytes(ushort value)
        {
            byte[] bytes = new byte[sizeof(ushort)];
            Unsafe.As<byte, ushort>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a ushort into a Span
        public static bool TryWriteBytes(Span<byte> destination, ushort value)
        {
            if (destination.Length < sizeof(ushort))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts an uint into an array of bytes with
        // length four.
        public static byte[] GetBytes(uint value)
        {
            byte[] bytes = new byte[sizeof(uint)];
            Unsafe.As<byte, uint>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a uint into a Span
        public static bool TryWriteBytes(Span<byte> destination, uint value)
        {
            if (destination.Length < sizeof(uint))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts an unsigned long into an array of bytes with
        // length eight.
        public static byte[] GetBytes(ulong value)
        {
            byte[] bytes = new byte[sizeof(ulong)];
            Unsafe.As<byte, ulong>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a ulong into a Span
        public static bool TryWriteBytes(Span<byte> destination, ulong value)
        {
            if (destination.Length < sizeof(ulong))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts a float into an array of bytes with length 
        // four.
        public static byte[] GetBytes(float value)
        {
            byte[] bytes = new byte[sizeof(float)];
            Unsafe.As<byte, float>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a float into a Span
        public static bool TryWriteBytes(Span<byte> destination, float value)
        {
            if (destination.Length < sizeof(float))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts a double into an array of bytes with length 
        // eight.
        public static byte[] GetBytes(double value)
        {
            byte[] bytes = new byte[sizeof(double)];
            Unsafe.As<byte, double>(ref bytes[0]) = value;
            return bytes;
        }

        // Converts a double into a Span
        public static bool TryWriteBytes(Span<byte> destination, double value)
        {
            if (destination.Length < sizeof(double))
                return false;

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
            return true;
        }

        // Converts an array of bytes into an int.  
        public static int ToInt32(byte[] value, int startIndex)
        {
            return Unsafe.ReadUnaligned<int>(ref value[startIndex]);
        }

        // Converts a Span into an int
        public static int ToInt32(ReadOnlySpan<byte> value)
        {
            return Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(value));
        }

        // Converts an array of bytes into a long.  
        public static long ToInt64(byte[] value, int startIndex)
        {
            return Unsafe.ReadUnaligned<long>(ref value[startIndex]);
        }

        // Converts a Span into a long
        public static long ToInt64(ReadOnlySpan<byte> value)
        {
            return Unsafe.ReadUnaligned<long>(ref MemoryMarshal.GetReference(value));
        }

        public static uint ToUInt32(byte[] value, int startIndex) => unchecked((uint)ToInt32(value, startIndex));

        // Convert a Span into a uint
        public static uint ToUInt32(ReadOnlySpan<byte> value)
        {
            return Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(value));
        }

        // Converts an array of bytes into an unsigned long.
        // 
        public static ulong ToUInt64(byte[] value, int startIndex) => unchecked((ulong)ToInt64(value, startIndex));

        // Converts a Span into an unsigned long
        public static ulong ToUInt64(ReadOnlySpan<byte> value)
        {
            return Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(value));
        }
    }
}
