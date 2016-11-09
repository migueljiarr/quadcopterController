using System;
using System.Runtime.InteropServices;

namespace FuzzyLogic
{
	/**
	 * @brief Acts as a C union between a double and other 64-bit values.
	 * @details Set one value, and you can retrieve the others as the raw binary representation of the set value.
	 * */
	[StructLayout(LayoutKind.Explicit)]
	public struct DoubleUnion
	{
		[FieldOffset(0)]
		public readonly double DoubleValue;

		[FieldOffset(0)]
		public readonly long LongValue;

		[FieldOffset(0)]
		public readonly ulong ULongValue;

		public DoubleUnion(double value)
			: this()
		{
			DoubleValue = value;
		}

		public DoubleUnion(long value)
			: this()
		{
			LongValue = value;
		}

		public DoubleUnion(ulong value)
			: this()
		{
			ULongValue = value;
		}

		/**
		 * @brief Breaks the stored value down into its representative byte array.
		 * @details The use of a pre-allocated buffer allows this method to function without allocating any data. 
		 * BitConverter.GetBytes(T value) will always allocate a new byte array, which is why this method was written.
		 * 
		 * @param buffer The byte buffer to fill.
		 * @returns The byte buffer filled with the value broken into bytes.
		 * */
		public byte[] GetBytes(byte[] buffer)
		{
			if (buffer.GetLength(0) < sizeof(long))
			{
				throw new IndexOutOfRangeException(String.Format(
					"The supplied buffer only holds {0} bytes, but it must be large enough to store a double ({1} bytes).",
					buffer.GetLength(0), sizeof(double)));
			}

			buffer[0] = (byte)(LongValue & 0xFF);
			buffer[1] = (byte)((LongValue >> 8) & 0xFF);
			buffer[2] = (byte)((LongValue >> 16) & 0xFF);
			buffer[3] = (byte)((LongValue >> 24) & 0xFF);
			buffer[4] = (byte)((LongValue >> 32) & 0xFF);
			buffer[5] = (byte)((LongValue >> 40) & 0xFF);
			buffer[6] = (byte)((LongValue >> 48) & 0xFF);
			buffer[7] = (byte)((LongValue >> 56) & 0xFF);

			return buffer;
		}
	}
}