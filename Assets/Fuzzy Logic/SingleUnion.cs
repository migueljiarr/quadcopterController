using System;
using System.Runtime.InteropServices;

namespace FuzzyLogic
{
	/**
	 * @brief Acts as a C union between a float and other 32-bit values.
	 * @details Set one value, and you can retrieve the others as the raw binary representation of the set value.
	 * */
	[StructLayout(LayoutKind.Explicit)]
	public struct SingleUnion
	{
		[FieldOffset(0)]
		public readonly float FloatValue;

		[FieldOffset(0)]
		public readonly int IntValue;

		[FieldOffset(0)]
		public readonly uint UIntValue;

		public SingleUnion(float value)
			: this()
		{
			FloatValue = value;
		}

		public SingleUnion(int value)
			: this()
		{
			IntValue = value;
		}

		public SingleUnion(uint value)
			: this()
		{
			UIntValue = value;
		}

		/**
		 * @brief Breaks the stored value down into its representative byte array.
		 * @details The use of a pre-allocated buffer allows this method to function without allocating any data.
		 * BitConvert.GetBytes(T value) will always allocate a new byte array, which is why this method was written.
		 * 
		 * @param buffer The byte buffer to fill.
		 * @returns The byte buffer filled with teh value broken into bytes.
		 * */
		public byte[] GetBytes(byte[] buffer)
		{
			if (buffer.GetLength(0) < sizeof(int))
			{
				throw new IndexOutOfRangeException(String.Format(
					"The supplied buffer only holds {0} bytes, but it must be large enough to store a float ({1} bytes).",
					buffer.GetLength(0), sizeof(float)));
			}

			buffer[0] = (byte)(IntValue & 0xff);
			buffer[1] = (byte)((IntValue >> 8) & 0xff);
			buffer[2] = (byte)((IntValue >> 16) & 0xff);
			buffer[3] = (byte)((IntValue >> 24) & 0xff);

			return buffer;
		}
	}
}