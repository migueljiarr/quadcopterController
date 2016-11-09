using System;

namespace FuzzyLogic
{
	/**
	 * @brief Static class that allows for fuzzy comparison between floating point numbers, and calculating ULP based information 
	 * from numbers.
	 * */
	public static class FuzzyCompare
	{
		public const float MaxFloatULP = 2.0282409603651670423947251286016E+31f;
		public const double MaxDoubleULP = 1.9958403095347198116563727130368E+292;

		#region Comparison [Double]

		/**
		 * @brief Compares two floating point numbers to see if they're equal.
		 * @details This method makes use of both an absolute margin of error and an ULP tolerance. If the difference between two 
		 * values is equal to or smaller than the margin of error, the two values are equal. If they're outside the margin of error, 
		 * then the difference in ULP between the numbers is calculated. One ULP is the smallest representable change between two 
		 * floating point numbers, and the actual size of the ULP changes with the scale of the values. This means that ULP can more 
		 * accurately compare two values for equality than a straight epsilon, or difference comparison. Unfortunately it breaks down 
		 * near 0 and in several other edge cases, which is why the margin of error check is performed first. For more information on 
		 * how this method is implemented, see http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/.
		 * @param first The first number.
		 * @param second The second number.
		 * @param marginOfError The acceptable margin of error between the two numbers; any difference between the two numbers smaller 
		 * than this equates to equal. As a general rule, this should be a very small value.
		 * @param ulpTolerance The maximum difference in ULP (unit of least precision) allowed between the two numbers.
		 * @returns `true` if considered equal, `false` otherwise.
		 * */
		public static bool AreEqual(double first, double second, double marginOfError, int ulpTolerance)
		{
			// When numbers are very close to zero, this initial check of absolute values is needed. Otherwise 
			// we can safely use the ULP difference.
			double absoluteDiff = Math.Abs(first - second);

			if (absoluteDiff <= marginOfError)
			{
				return true;
			}

			DoubleInfo firstInfo = new DoubleInfo(first);
			DoubleInfo secondInfo = new DoubleInfo(second);

			// Different signs mean the numbers don't match, period.
			if (firstInfo.IsNegative != secondInfo.IsNegative)
			{
				return false;
			}

			// Find the difference in ULPs (unit of least precision).
			long ulpDiff = Math.Abs(firstInfo.Bits - secondInfo.Bits);

			if (ulpDiff <= ulpTolerance)
			{
				return true;
			}

			return false;
		}

		/**
		 * @brief Compares the first value to the second.
		 * @param first The first operand.
		 * @param second The second operand.
		 * @param marginOfError The acceptable margin of error between the two numbers; any difference between the two numbers smaller 
		 * than this equates to equal. As a general rule, this should be a very small value.
		 * @param ulpTolerance The maximum difference in ULP (unit of least precision) allowed between the two numbers.
		 * @returns 0 if equal, -1 if the first operand is smaller than the second, 1 if the first operand is larger.
		 * */
		public static int Compare(double first, double second, double marginOfError, int ulpTolerance)
		{
			if (AreEqual(first, second, marginOfError, ulpTolerance))
			{
				return 0;
			}
			else
			{
				return first < second ? -1 : 1;
			}
		}

		#endregion

		#region Comparison [Float]

		/**
		 * @overload static bool AreEqual(float first, float second, float marginOfError, int ulpTolerance)
		 * */
		public static bool AreEqual(float first, float second, float marginOfError, int ulpTolerance)
		{
			// When numbers are very close to zero, this initial check of absolute values is needed. Otherwise 
			// we can safely use the ULP difference.
			float absoluteDiff = Math.Abs(first - second);

			if (absoluteDiff <= marginOfError)
			{
				return true;
			}

			SingleInfo firstInfo = new SingleInfo(first);
			SingleInfo secondInfo = new SingleInfo(second);

			// Different signs mean the numbers don't match, period.
			if (firstInfo.IsNegative != secondInfo.IsNegative)
			{
				return false;
			}

			// Find the difference in ULPs (unit of least precision).
			int ulpDiff = Math.Abs(firstInfo.Bits - secondInfo.Bits);

			if (ulpDiff <= ulpTolerance)
			{
				return true;
			}

			return false;
		}

		/**
		 * @overload static int Compare(float first, float second, float marginOfError, int ulpTolerance)
		 * */
		public static int Compare(float first, float second, float marginOfError, int ulpTolerance)
		{
			if (AreEqual(first, second, marginOfError, ulpTolerance))
			{
				return 0;
			}
			else
			{
				return first < second ? -1 : 1;
			}
		}

		#endregion

		#region ULP Info [Double]

		/**
		 * @brief Returns the size of an ULP of the argument.
		 * @details Behaves similar to Java's Math.ulp(float value).
		 *
		 * Special Cases:
		 *		- If the argument is NaN, then the result is NaN.
		 *		- If the argument is positive or negative infinity, then the result is positive infinity.
		 *		- If the argument is positive or negative zero, then the result is Double.Epsilon.
		 *		- If the argument is Double.MaxValue or Double.MinValue, then the result is equal to 2^971.
		 * @param value The value to get the ULP size of.
		 * @returns The ULP size.
		 * */
		public static double ULP(double value)
		{
			if (Double.IsNaN(value))
			{
				return Double.NaN;
			}
			else if (Double.IsPositiveInfinity(value) || Double.IsNegativeInfinity(value))
			{
				return Double.PositiveInfinity;
			}
			else if (value == 0.0)
			{
				return Double.Epsilon;
			}
			else if (Math.Abs(value) == Double.MaxValue)
			{
				return MaxDoubleULP;
			}

			DoubleInfo info = new DoubleInfo(value);
			return Math.Abs((double)(info.Bits + 1) - value);
		}

		/**
		 * @brief Calculates the number of ULP between two numbers.
		 * @exception System.ArgumentException Arguments have mixed signs (both must be positive or negative).
		 * @param first The first number.
		 * @param second The second number.
		 * @returns The number of ULP between the two numbers.
		 * */
		public static long ULPDistance(double first, double second)
		{
			DoubleInfo firstInfo = new DoubleInfo(first);
			DoubleInfo secondInfo = new DoubleInfo(second);

			if (firstInfo.IsNegative != secondInfo.IsNegative)
			{
				throw new ArgumentException("Numbers have mixed signs; cannot calculate the ULP distance across the 0 boundary.");
			}

			return Math.Abs(firstInfo.Bits - secondInfo.Bits);
		}

		#endregion

		#region ULP Info [Float]

		/**
		 * @overload static float ULP(float value)
		 * */
		public static float ULP(float value)
		{
			if (Single.IsNaN(value))
			{
				return Single.NaN;
			}
			else if (Single.IsPositiveInfinity(value) || Single.IsNegativeInfinity(value))
			{
				return Single.PositiveInfinity;
			}
			else if (value == 0.0)
			{
				return Single.Epsilon;
			}
			else if (Math.Abs(value) == Single.MaxValue)
			{
				return MaxFloatULP;
			}

			SingleInfo info = new SingleInfo(value);
			return Math.Abs((float)(info.Bits + 1) - value);
		}

		/**
		 * @overload static int ULPDistance(float first, float second)
		 * */
		public static int ULPDistance(float first, float second)
		{
			SingleInfo firstInfo = new SingleInfo(first);
			SingleInfo secondInfo = new SingleInfo(second);

			if (firstInfo.IsNegative != secondInfo.IsNegative)
			{
				throw new ArgumentException("Numbers have mixed signs; cannot calculate the ULP distance across the 0 boundary.");
			}

			return Math.Abs(firstInfo.Bits - secondInfo.Bits);
		}

		#endregion

		#region Boundaries [Double]

		/**
		 * @brief Returns the bit representation of the given value, rounded to the given boundary.
		 * @param value The value to round.
		 * @param boundary The boundary to round to.
		 * @returns The bit representation of the given value, rounded to the given boundary.
		 * */
		public static long RoundToBoundary(double value, long boundary)
		{
			return (new DoubleInfo(value).Bits / boundary) * boundary;
		}

		/**
		 * @brief Calculates the boundary for the given value.
		 * @details A boundary in this context is an interval that all fuzzy numbers with the same parameters are rounded to. 
		 * This is important for generating a hash for values that use fuzzy comparison.
		 * @param value The value to calculate the boundary of.
		 * @param marginOfError The margin of error.
		 * @param ulpTolerance The allowed tolerance in number of ULP.
		 * @param boundaryScale Factor to scale the boundary by; values other than 1 break the transitivity of equality.
		 * @returns The boundary interval of the given value.
		 * */
		public static long Boundary(double value, double marginOfError, int ulpTolerance, int boundaryScale)
		{
			DoubleInfo valueInfo = new DoubleInfo(Math.Abs(value));
			DoubleInfo valueWithErrorInfo = new DoubleInfo(Math.Abs(value) + marginOfError);

			long ulpBoundary = ulpTolerance * boundaryScale;
			long marginBoundary = (valueWithErrorInfo.Bits - valueInfo.Bits) * boundaryScale;

			return Math.Max(ulpBoundary, marginBoundary);
		}

		/**
		 * @brief Determines if the first given value is within the same boundary as the second given value.
		 * @param first The first value.
		 * @param second The second value.
		 * @param marginOfError The margin of error.
		 * @param ulpTolerance The allowed tolerance in number of ULP.
		 * @param boundaryScale Factor to scale the boundary by; values other than 1 break the transitivity of equality.
		 * @returns `true` if both values are within the same boundary; `false` if they lie in different boundaries.
		 * */
		public static bool InSameBoundary(double first, double second, double marginOfError, int ulpTolerance, int boundaryScale)
		{
			long boundary = Boundary(first, marginOfError, ulpTolerance, boundaryScale);

			long firstAlongBoundary = RoundToBoundary(first, boundary);
			long secondAlongBoundary = RoundToBoundary(second, boundary);

			return firstAlongBoundary == secondAlongBoundary;
		}

		#endregion

		#region Boundaries [Float]

		/**
		 * @overload static long RoundToBoundary(float value, int boundary)
		 * */
		public static int RoundToBoundary(float value, int boundary)
		{
			return (new SingleInfo(value).Bits / boundary) * boundary;
		}

		/**
		 * @overload static int Boundary(float value, float marginOfError, int ulpTolerance, int boundaryScale)
		 * */
		public static int Boundary(float value, float marginOfError, int ulpTolerance, int boundaryScale)
		{
			SingleInfo valueInfo = new SingleInfo(Math.Abs(value));
			SingleInfo valueWithErrorInfo = new SingleInfo(Math.Abs(value) + marginOfError);

			int ulpBoundary = ulpTolerance * boundaryScale;
			int marginBoundary = (valueWithErrorInfo.Bits - valueInfo.Bits) * boundaryScale;

			return Math.Max(ulpBoundary, marginBoundary);
		}

		/**
		 * @overload static bool InSameBoundary(float first, float second, float marginOfError, int ulpTolerance, int boundaryScale)
		 * */
		public static bool InSameBoundary(float first, float second, float marginOfError, int ulpTolerance, int boundaryScale)
		{
			int boundary = Boundary(first, marginOfError, ulpTolerance, boundaryScale);

			int firstAlongBoundary = RoundToBoundary(first, boundary);
			int secondAlongBoundary = RoundToBoundary(second, boundary);

			return firstAlongBoundary == secondAlongBoundary;
		}

		#endregion
	}
}