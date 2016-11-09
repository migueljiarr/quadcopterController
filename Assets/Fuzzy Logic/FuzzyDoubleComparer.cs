using System;
using System.Collections.Generic;

namespace FuzzyLogic
{
	public class FuzzyDoubleComparer : IEqualityComparer<double>, IComparer<double>
	{
		private double _marginOfError;
		private int _ulpTolerance;

		#region Accessors

		public double MarginOfError 
		{
			get { return _marginOfError; }
			set { _marginOfError = Math.Abs(value); }
		}

		public int ULPTolerance 
		{
			get { return _ulpTolerance; }
			set { _ulpTolerance = Math.Abs(value); }
		}

		#endregion

		public FuzzyDoubleComparer(double marginOfError, int ulpTolerance)
		{
			MarginOfError = marginOfError;
			ULPTolerance = ulpTolerance;
		}

		public bool Equals(double first, double second)
		{
			return FuzzyCompare.AreEqual(first, second, _marginOfError, _ulpTolerance);
		}

		public int Compare(double first, double second)
		{
			if (Equals(first, second))
			{
				return 0;
			}

			return first < second ? -1 : 1;
		}

		#region GetHashCode

		/**
		 * @warning [Do Not Use] This is not a hashable fuzzy value, and the hash returned is not representative of the value.
		 * */
		public int GetHashCode(double value)
		{
			return 0;
		}

		/**
		 * @overload int GetHashCode(object obj)
		 * */
		public int GetHashCode(object obj)
		{
			return 0;
		}

		#endregion
	}
}
