using System;
using System.Runtime.InteropServices;

namespace FuzzyLogic
{
	/**
	 * @brief Allows access to detailed information about a float.
	 * */
	public struct SingleInfo
	{
		private float _value;
		private int _bits;

		public SingleInfo(float value)
		{
			_value = value;
			_bits = SingleInfo.SingleToInt32Bits(value);
		}

		#region Accessors

		/** @brief The stored value. */
		public float Value
		{
			get { return _value; }
		}

		/** @brief The underlying bit representation of the value, returned as an int. */
		public int Bits
		{
			get { return _bits; }
		}

		/** @brief Returns `true` if the number is negative, `false` if positive. */
		public bool IsNegative
		{
			get { return (_bits < 0); }
		}

		/** @brief Returns the exponent component of the value. */
		public int Exponent
		{
			get { return (int)((_bits >> 23) & 0xff); }
		}

		/** @brief Returns the mantissa component of the value. */
		public int Mantissa
		{
			get { return _bits & 0x7fffff; }
		}

		#endregion

		/**
		 * @brief Converts a float into a 32-bit int, preserving the exact binary representation of the float.
		 * @param value The value to convert.
		 * @returns An int that stores the exact binary representation of the given value.
		 * */
		public static int SingleToInt32Bits(float value)
		{
			// This allows us to perform the same operations as BitConverter, but without allocating a new array of bytes.
			return new SingleUnion(value).IntValue;
		}
	}
}