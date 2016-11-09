Fuzzy Logic is a minimal yet robust library for comparing floating point numbers. 
It can continue to accurately compare two values, even when Mathf.Approximately and 
other comparison techniques begin to break down.

See http://rabbitb.bitbucket.org/FuzzyLogic/ for extensive library documentation, or the unit
tests available at https://bitbucket.org/RabbitB/fuzzy-logic for more examples on how to use 
the library.

Comparing two fuzzy values is as easy as: 

	[A] The value
	[B] The margin of error
	[C] The ULP tolerance

										 [A]   [B]  [C]
	bool isEqual = FuzzySingle.MakeFuzzy(5.5f, 1E-4, 2) == 2.75f;
	bool isLargerThan = FuzzyDouble.MakeFuzzy(6.0, 1E-6, 2) > FuzzyDouble.MakeFuzzy(3.6, 1E-6, 2);
	
	When you compare two non-hashed fuzzy numbers, the parameters of the first operand are used for 
	the fuzzy comparison. When you compare a non-hashed fuzzy number against a regular floating point 
	number, the parameters of the fuzzy number will be used, regardless of whether it's the first or 
	second operand. Hashed fuzzy numbers can only be compared against other hashed fuzzy numbers, as 
	the hash (and thus their equality) is based off the parameters as well as the actual value. 
	
	This means that: FuzzyHashedDouble.MakeFuzzy(1.0, 1E-6, 1) == FuzzyHashedDouble.MakeFuzzy(1.0, 1E-4, 2) 
	will not equate to true.
	
	It's also important to mention that the use of hashed fuzzy numbers should largely be limited to 
	cases where hashed values are actually needed. Hashed numbers are slightly slower and have a bit 
	more unpredictable comparison algorithm in contrast to non-hashed numbers. Hashing works by 
	introducing false boundaries into the underlying bit representation, which means that two numbers 
	that are within each other's margin of error, or ULP tolerance, have the possibility of equating to 
	non-equal, if they fall across a boundary line. Two exactly equal values will always be equal, as 
	it's not possible for them to fall across a boundary.

Using floating point numbers as keys in a collection can now be done easily as well:

	FuzzyHashedSingleComparer singleComparer = new FuzzyHashedSingleComparer(1E-4, 2);
	Dictionary<float, MyValues> myDictionary = new Dictionary<float, MyValues>(singleComparer);

The fuzzy parameters:

	Margin of Error: The absolute amount of error that is inherently accounted for; any 
		difference between two floating point numbers that is smaller than this, and the 
		numbers will be considered equal. Margin of error is most important for smaller 
		numbers, where ULP is not as effective. As numbers grow increasingly large, margin 
		of error becomes less effective, as it's possible for the margin of error to eventually 
		become smaller than the smallest representable difference between two values.
		
	ULP (Unit of Least Precision) Tolerance: ULP is a scaling value, with the smallest representable 
		difference between two values (one ULP) growing increasingly large as the number 
		increases in magnitude. The opposite is true as well, and ULP breaks down as a number 
		nears zero. As you approach zero, the ULP size grows so minuscule, that two numbers 
		are likely to never be close to each other in terms of number of ULP difference. This 
		is one of the reasons why a two-attack approach is so important: both ULP and margin 
		of error are needed to accurately compare floating point numbers in different scenarios.
	
	So how much of both?

		The margin of error that you should allow is entirely dependent on the numbers you'll 
		be working with, and what you consider to be an acceptable amount of error under regular 
		circumstances. If you were working with a length in meters, you might want to set your 
		margin of error to be 1 micrometer, or 1E-6.
		
		In contrast to the margin of error, the ULP is more dependent on how many arithmetic 
		operations you plan to perform on the same number, and the magnitude of the number. 
		A ULP of 1 or 2 is more than enough for when you only perform some arithmetic on a value 
		before comparing it, but if you're continually performing arithmetic on a value over time, 
		the error will continue to gradually grow. These types of situations should be avoided 
		when possible, but a ULP of 4 is generally a good rule of thumb for situations where more 
		error is to be expected, or greater tolerance is allowed.
		
		Always remember that as a number dips below one and approaches zero, ULP becomes decreasingly 
		effective for comparing values, and the margin of error becomes much more important in the 
		comparison. The opposite is true as well; as the magnitude (in base 2) of a number increases, 
		the size of the ULP increases as well, and with extremely large numbers, the margin of error may 
		be smaller than the smallest representable change in a value.

The components: 

	FuzzyDouble | FuzzySingle -- A fuzzy comparable (double | float) value.
	
	FuzzyHashedDouble | FuzzyHashedSingle -- A fuzzy comparable (double | float) value 
		that also supports hashing, for use as a key in collections or wherever else 
		hashed values are needed.
		
	FuzzyDoubleComparer | FuzzySingleComparer -- Compares two (double | float) values, 
		using its own fuzzy logic parameters.
		
	FuzzyHashedDoubleComparer | FuzzyHashedSingleComparer -- Compares two (double | float) 
		values, using its own fuzzy logic parameters. Is also capable of generating hashes 
		of these fuzzy values.
		
	FuzzyCompare -- A static class that provides numerous fuzzy logic functionality and 
		convenience methods.
		
	DoubleInfo | SingleInfo -- Breaks down a floating point number into its individual 
		components, allowing access to detailed information about the number.
		
	DoubleUnion | SingleUnion -- Behaves similar to unions in C; these structs allow (64 | 32) 
		bit numbers to be accessed as different types of the same size, without disturbing the 
		underlying bit representation of the values.