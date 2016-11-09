using System;

namespace FuzzyLogic
{
	/**
	 * @brief A double value that supports fuzzy logic.
	 * @details Does not support hashing; see FuzzyHashedDouble for a double value that supports hashing in addition 
	 * to fuzzy logic. FuzzyDouble should be the preferred choice for any fuzzy number where hashing isn't needed. 
	 * This is because it doesn't need to make any compromises to support valid hashing, making it slightly faster 
	 * than FuzzyHashedDouble, and with a more predictable comparison algorithm.
	 * */
	public struct FuzzyDouble : IEquatable<FuzzyDouble>, IEquatable<double>, IComparable<FuzzyDouble>, IComparable<double>
	{
		private double _value;
		private double _marginOfError;
		private int _ulpTolerance;

		#region Constructors

		public FuzzyDouble(double value, double marginOfError, int ulpTolerance)
		{
			_value = value;

			_marginOfError = Math.Abs(marginOfError);
			_ulpTolerance = Math.Abs(ulpTolerance);
		}

		public FuzzyDouble(FuzzyHashedDouble fuzzyHashedDouble) : 
			this(fuzzyHashedDouble.Value, fuzzyHashedDouble.MarginOfError, fuzzyHashedDouble.ULPTolerance)
		{
		}

		/**
		 * @brief Static constructor available for convenience.
		 * */
		public static FuzzyDouble MakeFuzzy(double value, double marginOfError, int ulpTolerance)
		{
			return new FuzzyDouble(value, marginOfError, ulpTolerance);
		}

		#endregion

		#region Accessors

		/** @brief The stored value. */
		public double Value
		{
			get { return _value; }
		}

		/** @brief The margin of error. */
		public double MarginOfError
		{
			get { return _marginOfError; }
		}

		/** @brief The allowed ULP tolerance. */
		public int ULPTolerance
		{
			get { return _ulpTolerance; }
		}

		#endregion

		#region Equality [FuzzyDouble]

		public bool Equals(FuzzyDouble other)
		{
			return FuzzyCompare.AreEqual(_value, other._value, _marginOfError, _ulpTolerance);
		}

		public override bool Equals(object obj)
		{
			if (obj is FuzzyDouble)
			{
				return Equals((FuzzyDouble)obj);
			}
			else if (obj is double)
			{
				return Equals((double)obj);
			}

			return false;
		}

		public static bool operator ==(FuzzyDouble first, FuzzyDouble second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(FuzzyDouble first, FuzzyDouble second)
		{
			return !(first.Equals(second));
		}

		/**
		 * @brief This struct doesn't support hashing and shouldn't be used in any context that requires it. If you require 
		 * a fuzzy double that's also hashable, use FuzzyHashedDouble instead.
		 * @details This hash code method only exists to fully implement the IEquatable interface, and conforms to the 
		 * requirement that if A == B, then A.GetHashCode() == B.GetHashCode. It does this by simply returning 0, which 
		 * technically meets the specification. If you require a fuzzy double that's also hashable, use FuzzyHashedDouble 
		 * instead. It makes several compromises to its equality testing and comparators, that allows it to generate a 
		 * usable hash.
		 * */
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region Equality [Double]

		/**
		 * @overload Equals(double other)
		 * */
		public bool Equals(double other)
		{
			return FuzzyCompare.AreEqual(_value, other, _marginOfError, _ulpTolerance);
		}

		public static bool operator ==(FuzzyDouble first, double second)
		{
			return first.Equals(second);
		}

		public static bool operator ==(double first, FuzzyDouble second)
		{
			return second.Equals(first);
		}

		public static bool operator !=(FuzzyDouble first, double second)
		{
			return !(first.Equals(second));
		}

		public static bool operator !=(double first, FuzzyDouble second)
		{
			return !(second.Equals(first));
		}

		#endregion

		#region Comparison [FuzzyDouble]

		/**
		 * @brief Compares the stored value to the given value.
		 * @details When two fuzzy numbers are compared to each other, the parameters (margin of error and ulp tolerance) of the first 
		 * operand are used for the comparison, and the parameters of the second operand are ignored.
		 * @param other The other value.
		 * @returns 0 if equal, -1 if the first operand is smaller than the second, 1 if the first operand is larger.
		 * */
		public int CompareTo(FuzzyDouble other)
		{
			if (Equals(other))
			{
				return 0;
			}
			else
			{
				return _value < other._value ? -1 : 1;
			}
		}

		public static bool operator >(FuzzyDouble first, FuzzyDouble second)
		{
			if (first.CompareTo(second) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(FuzzyDouble first, FuzzyDouble second)
		{
			if (first.CompareTo(second) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(FuzzyDouble first, FuzzyDouble second)
		{
			if (first.CompareTo(second) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(FuzzyDouble first, FuzzyDouble second)
		{
			if (first.CompareTo(second) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Comparison [FuzzyDouble to Double]

		/**
		 * @overload CompareTo(double other)
		 * */
		public int CompareTo(double other)
		{
			if (Equals(other))
			{
				return 0;
			}
			else
			{
				return _value < other ? -1 : 1;
			}
		}

		public static bool operator >(FuzzyDouble first, double second)
		{
			if (first.CompareTo(second) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(FuzzyDouble first, double second)
		{
			if (first.CompareTo(second) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(FuzzyDouble first, double second)
		{
			if (first.CompareTo(second) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(FuzzyDouble first, double second)
		{
			if (first.CompareTo(second) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Comparison [Double to FuzzyDouble]

		public static bool operator >(double first, FuzzyDouble second)
		{
			if (second.CompareTo(first) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(double first, FuzzyDouble second)
		{
			if (second.CompareTo(first) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(double first, FuzzyDouble second)
		{
			if (second.CompareTo(first) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(double first, FuzzyDouble second)
		{
			if (second.CompareTo(first) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Casts and Conversions

		public static explicit operator double(FuzzyDouble fuzzyDouble)
		{
			return fuzzyDouble._value;
		}

		public static explicit operator float(FuzzyDouble fuzzyDouble)
		{
			return (float)fuzzyDouble._value;
		}

		public static explicit operator long(FuzzyDouble fuzzyDouble)
		{
			return (long)fuzzyDouble._value;
		}

		public static explicit operator int(FuzzyDouble fuzzyDouble)
		{
			return (int)fuzzyDouble._value;
		}

		public static explicit operator bool(FuzzyDouble fuzzyDouble)
		{
			if (fuzzyDouble.Equals(0.0))
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}