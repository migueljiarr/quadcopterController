using System;
using System.Runtime.InteropServices;

namespace FuzzyLogic
{
	/**
	 * @brief Allows access to detailed information about a double.
	 * */
	public struct DoubleInfo
	{
		private double _value;
		private long _bits;

		public DoubleInfo(double value)
		{
			_value = value;
			_bits = DoubleInfo.DoubleToInt64Bits(value);
		}

		#region Accessors

		/** @brief The stored value. */
		public double Value
		{
			get { return _value; }
		}

		/** @brief The underlying bit representation of the value, returned as a long. */
		public long Bits
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
			get { return (int)((_bits >> 52) & 0x7ffL); }
		}

		/** @brief Returns the mantissa component of the value. */
		public long Mantissa
		{
			get { return _bits & 0xfffffffffffffL; }
		}

		#endregion

		/**
		 * @brief Converts a double into a 64-bit long, preserving the exact binary representation of the double.
		 * @param value The value to convert.
		 * @returns A long that stores the exact binary representation of the given value.
		 * */
		public static long DoubleToInt64Bits(double value)
		{
			// This allows us to perform the same operations as BitConverter, but without allocating a new array of bytes.
			return new DoubleUnion(value).LongValue;
		}
	}
}