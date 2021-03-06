﻿using System;

namespace FuzzyLogic
{
	/**
	 * @brief A double value that supports fuzzy logic and valid hashing.
	 * @details Some compromises to the equality algorithm need to be made to support valid hashing. FuzzyHashedDouble 
	 * still provides high quality fuzzy logic, but if you don't need hashing, use FuzzyDouble for slightly faster and more 
	 * predictable fuzzy logic.
	 * 
	 *	Compromises
	 *		- Both values must have the same fuzzy logic parameters. The parameters are used in the hash generation, and 
	 *		thus become part of the equality.
	 *		- The hashable range of values is reduced by an amount relative to the amount of error that is allowed. This 
	 *		allows for the potential for more hash conflicts, but admittedly only for values that are very close together.
	 *		- The hash works by introducing imaginary boundaries into the underlying bit representation of the floating 
	 *		point number (in contrast to the real 0.0 boundary). That means that fuzzy values that would otherwise equate 
	 *		to equal, could potentially equate to different values, if one number lies across a boundary from the other. 
	 *		Actual equal values will always be equal, since it wouldn't be possible for the same value to lie across a 
	 *		boundary. The likelihood of this occuring is inverse to the reduction in the hashable range of values. i.e. 
	 *		The less likely this is to happen, the more potential hash conflicts, and vice versa.
	 *		
	 * @note As long as the boundary width is the size of the largest error (margin of error or ULP tolerance), the equality 
	 * remains transative. That is, if X == Y and Y == Z, then X == Z. This is the default behavior, but if you find that too 
	 * many equality comparisons fail because they fall across a boundary, and you don't need transative behavior, then you 
	 * can increase the boundary scale.
	 * */
	public struct FuzzyHashedDouble : IEquatable<FuzzyHashedDouble>, IComparable<FuzzyHashedDouble>
	{
		private double _value;
		private double _marginOfError;
		private int _ulpTolerance;
		private int _boundaryScale;

		#region Constructors

		/**
		 * @brief Constructs a FuzzyHashedDouble.
		 * @note The margin of error should always be an explicitely defined value, and never a derived value, unless 
		 * that exact value is retained and used for any fuzzy values relying on it. This is because the hashing algorithm 
		 * relies on the margin of error, so its exact value is important. For the same reason that fuzzy logic is needed, 
		 * two derived values for the margin of error are unlikely to be exactly equal.
		 * 
		 * @param value The value.
		 * @param marginOfError Margin of Error
		 * @param ulpTolerance ULP Tolerance
		 * @param boundaryScale Factor to scale the boundary by.
		 * 
		 * @warning A boundary scale other than 1, causes the equality comparison to lose its transative nature. Only change 
		 * this if you have too many equality comparisons fail because they fall across a boundary, and you know you don't 
		 * need to retain transitivity.
		 * */
		public FuzzyHashedDouble(double value, double marginOfError, int ulpTolerance, int boundaryScale = 1)
		{
			_value = value;

			_marginOfError = Math.Abs(marginOfError);
			_ulpTolerance = Math.Abs(ulpTolerance);

			if (boundaryScale < 1)
			{
				_boundaryScale = 1;
			}
			else
			{
				_boundaryScale = boundaryScale;
			}
		}

		public FuzzyHashedDouble(FuzzyDouble fuzzyDouble, int boundaryScale = 1) : 
			this(fuzzyDouble.Value, fuzzyDouble.MarginOfError, fuzzyDouble.ULPTolerance, boundaryScale)
		{
		}

		/**
		 * @brief Static constructor available for convenience.
		 * */
		public static FuzzyHashedDouble MakeFuzzy(double value, double marginOfError, int ulpTolerance, int boundaryScale = 1)
		{
			return new FuzzyHashedDouble(value, marginOfError, ulpTolerance, boundaryScale);
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

		/** @brief The boundary scale. */
		public int BoundaryScale
		{
			get { return _boundaryScale; }
		}

		#endregion

		#region Equality

		public bool Equals(FuzzyHashedDouble other)
		{
			if (_ulpTolerance == other._ulpTolerance && 
				_marginOfError == other._marginOfError && 
				_boundaryScale == other._boundaryScale && 
				FuzzyCompare.InSameBoundary(_value, other._value, _marginOfError, _ulpTolerance, _boundaryScale) && 
				FuzzyCompare.AreEqual(_value, other._value, _marginOfError, _ulpTolerance))
			{
				return true;
			}

			return false;
		}

		public override bool Equals(object obj)
		{
			return obj is FuzzyHashedDouble && Equals((FuzzyHashedDouble)obj);
		}

		public static bool operator ==(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			return !(first.Equals(second));
		}

		/**
		 * @brief This hash code algorithm produces a hash that complies with the guidelines set forth in the .NET documentation, 
		 * yet is able to represent a number that is capable of fuzzy logic.
		 * @returns Hash for the fuzzy number.
		 * */
		public override int GetHashCode()
		{
			long boundary = FuzzyCompare.Boundary(_value, _marginOfError, _ulpTolerance, _boundaryScale);
			long valueAlongBoundary = FuzzyCompare.RoundToBoundary(_value, boundary);

			unchecked // Overflow is fine, just wrap.
			{
				int hash = 17;
				hash = hash * 29 + valueAlongBoundary.GetHashCode();
				hash = hash * 29 + _ulpTolerance.GetHashCode();
				hash = hash * 29 + _marginOfError.GetHashCode();
				hash = hash * 29 + _boundaryScale.GetHashCode();

				return hash;
			}
		}

		#endregion

		#region Comparison

		public int CompareTo(FuzzyHashedDouble other)
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

		public static bool operator >(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			if (first.CompareTo(second) > 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator >=(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			if (first.CompareTo(second) >= 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			if (first.CompareTo(second) < 0)
			{
				return true;
			}

			return false;
		}

		public static bool operator <=(FuzzyHashedDouble first, FuzzyHashedDouble second)
		{
			if (first.CompareTo(second) <= 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Casts and Conversions

		public static explicit operator double(FuzzyHashedDouble fuzzyDouble)
		{
			return fuzzyDouble._value;
		}

		public static explicit operator float(FuzzyHashedDouble fuzzyDouble)
		{
			return (float)fuzzyDouble._value;
		}

		public static explicit operator long(FuzzyHashedDouble fuzzyDouble)
		{
			return (long)fuzzyDouble._value;
		}

		public static explicit operator int(FuzzyHashedDouble fuzzyDouble)
		{
			return (int)fuzzyDouble._value;
		}

		public static explicit operator bool(FuzzyHashedDouble fuzzyDouble)
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