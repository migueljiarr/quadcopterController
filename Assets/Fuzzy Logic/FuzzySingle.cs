using System;

namespace FuzzyLogic
{
	/**
	 * @brief A float value that supports fuzzy logic.
	 * @details Does not support hashing; see FuzzyHashedSingle for a float value that supports hashing in addition 
	 * to fuzzy logic. FuzzySingle should be the preferred choice for any fuzzy number where hashing isn't needed. 
	 * This is because it doesn't need to make any compromises to support valid hashing, making it slightly faster 
	 * than FuzzyHashedSingle, and with a more predictable comparison algorithm.
	 * */
	public struct FuzzySingle : IEquatable<FuzzySingle>, IEquatable<float>, IComparable<FuzzySingle>, IComparable<float>
	{
		private float _value;
		private float _marginOfError;
		private int _ulpTolerance;

		#region Constructors

		public FuzzySingle(float value, float marginOfError, int ulpTolerance)
		{
			_value = value;

			_marginOfError = Math.Abs(marginOfError);
			_ulpTolerance = Math.Abs(ulpTolerance);
		}

		public FuzzySingle(FuzzyHashedSingle fuzzyHashedSingle) : 
			this(fuzzyHashedSingle.Value, fuzzyHashedSingle.MarginOfError, fuzzyHashedSingle.ULPTolerance)
		{
		}

		/**
		 * @brief Static constructor available for convenience.
		 * */
		public static FuzzySingle MakeFuzzy(float value, float marginOfError, int ulpTolerance)
		{
			return new FuzzySingle(value, marginOfError, ulpTolerance);
		}

		#endregion

		#region Accessors

		/** @brief The stored value. */
		public float Value
		{
			get { return _value; }
		}

		/** @brief The margin of error. */
		public float MarginOfError
		{
			get { return _marginOfError; }
		}

		/** @brief The allowed ULP tolerance. */
		public int ULPTolerance
		{
			get { return _ulpTolerance; }
		}

		#endregion

		#region Equality [FuzzySingle]

		public bool Equals(FuzzySingle other)
		{
			return FuzzyCompare.AreEqual(_value, other._value, _marginOfError, _ulpTolerance);
		}

		public override bool Equals(object obj)
		{
			if (obj is FuzzySingle)
			{
				return Equals((FuzzySingle)obj);
			}
			else if (obj is float)
			{
				return Equals((float)obj);
			}

			return false;
		}

		public static bool operator ==(FuzzySingle first, FuzzySingle second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(FuzzySingle first, FuzzySingle second)
		{
			return !(first.Equals(second));
		}

		/**
		 * @brief This struct doesn't support hashing and shouldn't be used in any context that requires it. If you require 
		 * a fuzzy float that's also hashable, use FuzzyHashedSingle instead.
		 * @details This hash code method only exists to fully implement the IEquatable interface, and conforms to the 
		 * requirement that if A == B, then A.GetHashCode() == B.GetHashCode. It does this by simply returning 0, which 
		 * technically meets the specification. If you require a fuzzy single that's also hashable, use FuzzyHashedSingle 
		 * instead. It makes several compromises to its equality testing and comparators, that allows it to generate a 
		 * usable hash.
		 * */
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region Equality [float]

		/**
		 * @overload Equals(float other)
		 * */
		public bool Equals(float other)
		{
			return FuzzyCompare.AreEqual(_value, other, _marginOfError, _ulpTolerance);
		}

		public static bool operator ==(FuzzySingle first, float second)
		{
			return first.Equals(second);
		}

		public static bool operator ==(float first, FuzzySingle second)
		{
			return second.Equals(first);
		}

		public static bool operator !=(FuzzySingle first, float second)
		{
			return !(first.Equals(second));
		}

		public static bool operator !=(float first, FuzzySingle second)
		{
			return !(second.Equals(first));
		}

		#endregion

		#region Comparison [FuzzySingle]

		/**
		 * @brief Compares the stored value to the given value.
		 * @details When two fuzzy numbers are compared to each other, the parameters (margin of error and ulp tolerance) of the first 
		 * operand are used for the comparison, and the parameters of the second operand are ignored.
		 * @param other The other value.
		 * @returns 0 if equal, -1 if the first operand is smaller than the second, 1 if the first operand is larger.
		 * */
		public int CompareTo(FuzzySingle other)
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

		public static bool operator >(FuzzySingle first, FuzzySingle second)
		{
			if (first.CompareTo(second) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(FuzzySingle first, FuzzySingle second)
		{
			if (first.CompareTo(second) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(FuzzySingle first, FuzzySingle second)
		{
			if (first.CompareTo(second) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(FuzzySingle first, FuzzySingle second)
		{
			if (first.CompareTo(second) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Comparison [FuzzySingle to Float]

		/**
		 * @overload CompareTo(float other)
		 * */
		public int CompareTo(float other)
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

		public static bool operator >(FuzzySingle first, float second)
		{
			if (first.CompareTo(second) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(FuzzySingle first, float second)
		{
			if (first.CompareTo(second) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(FuzzySingle first, float second)
		{
			if (first.CompareTo(second) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(FuzzySingle first, float second)
		{
			if (first.CompareTo(second) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Comparison [Float to FuzzySingle]

		public static bool operator >(float first, FuzzySingle second)
		{
			if (second.CompareTo(first) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(float first, FuzzySingle second)
		{
			if (second.CompareTo(first) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(float first, FuzzySingle second)
		{
			if (second.CompareTo(first) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(float first, FuzzySingle second)
		{
			if (second.CompareTo(first) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Casts and Conversions

		public static explicit operator double(FuzzySingle fuzzySingle)
		{
			return fuzzySingle._value;
		}

		public static explicit operator float(FuzzySingle fuzzySingle)
		{
			return fuzzySingle._value;
		}

		public static explicit operator long(FuzzySingle fuzzySingle)
		{
			return (long)fuzzySingle._value;
		}

		public static explicit operator int(FuzzySingle fuzzySingle)
		{
			return (int)fuzzySingle._value;
		}

		public static explicit operator bool(FuzzySingle fuzzySingle)
		{
			if (fuzzySingle.Equals(0.0))
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}