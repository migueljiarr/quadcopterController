using System;
using System.Collections.Generic;

namespace FuzzyLogic
{
	public class FuzzySingleComparer : IEqualityComparer<float>, IComparer<float>
	{
		private float _marginOfError;
		private int _ulpTolerance;

		#region Accessors

		public float MarginOfError
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

		public FuzzySingleComparer(float marginOfError, int ulpTolerance)
		{
			MarginOfError = marginOfError;
			ULPTolerance = ulpTolerance;
		}

		public bool Equals(float first, float second)
		{
			return FuzzyCompare.AreEqual(first, second, _marginOfError, _ulpTolerance);
		}

		public int Compare(float first, float second)
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
		public int GetHashCode(float value)
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