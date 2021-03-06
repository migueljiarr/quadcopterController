﻿using System;
using System.Collections.Generic;

namespace FuzzyLogic
{
	public class FuzzyHashedDoubleComparer : IEqualityComparer<double>, IComparer<double>
	{
		private double _marginOfError;
		private int _ulpTolerance;
		private int _boundaryScale;

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

		public int BoundaryScale
		{
			get { return _boundaryScale; }
			set
			{
				if (value < 1)
				{
					_boundaryScale = 1;
				}
				else
				{
					_boundaryScale = value;
				}
			}
		}

		#endregion

		public FuzzyHashedDoubleComparer(double marginOfError, int ulpTolerance, int boundaryScale = 1)
		{
			MarginOfError = marginOfError;
			ULPTolerance = ulpTolerance;
			BoundaryScale = boundaryScale;
		}

		public bool Equals(double first, double second)
		{
			if (FuzzyCompare.InSameBoundary(first, second, _marginOfError, _ulpTolerance, _boundaryScale) && 
				FuzzyCompare.AreEqual(first, second, _marginOfError, _ulpTolerance))
			{
				return true;
			}

			return false;
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
		 * @brief This hash code algorithm produces a hash that complies with the guidelines set forth in the .NET documentation, 
		 * yet is able to represent a number that is capable of fuzzy logic.
		 * @returns Hash for the fuzzy number.
		 * */
		public int GetHashCode(double value)
		{
			long boundary = FuzzyCompare.Boundary(value, _marginOfError, _ulpTolerance, _boundaryScale);
			long valueAlongBoundary = FuzzyCompare.RoundToBoundary(value, boundary);

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

		/**
		 * @overload int GetHashCode(object obj)
		 * */
		public int GetHashCode(object obj)
		{
			if (obj is Double)
			{
				return GetHashCode((double)obj);
			}
			else
			{
				throw new ArgumentException(String.Format("Argument must be of type double; argument was of type {0}.", obj.GetType()));
			}
		}

		#endregion
	}
}
