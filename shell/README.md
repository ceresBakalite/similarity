*<link rel="stylesheet" type="text/css" href="pretagwordwrap.css" media="screen">
## Command Line Usage
### *Seeking patterns of similarity between data strings* <img align="right" src="../images/NAVSimilarityLogoSmall.png">
The following notes summarise calling the application and applying arguments to invoke a variety of options available within the application.  There have been a number of
common questions asked by users which deserve, and indeed have now found, a more permanent placeholder here.
***
 
**Release Version v1.2.8.2b introduced command line usage to Similarity**

## Command Line FAQ
**1. Why a shell?**

  Oddly, a very early iteration of Similarity was just a command line application.  The premise was that this application would reside somewhere in the cloud and
  would be accessible from any assembly anywhere. This would enable a third party application to use all the functionality without the overhead.

  For example, consider a postal service checking for valid address syntax targeting two countries. There is a FROM address and a TO address. Both these addresses
  are read (typically by software) in each country where both hope to see each address in their local syntax.  This requires the manipulation of each address twice.
  In other words, four separate addresses, language, culture and syntax specific.

  In many ways establishing the application as a shell is still a better use of the software IMHO and I fully intend to create a C++ shell variant of the Similarity
  application, should there be enough interest. A C++ variant would not just be smaller and fast, it would also need to take one, two, or many comparison sets and
  it would throw away the overhead associated with a GUI.  

  Although largely language independent already, language interoperability would be a fine thing.  As would culture, syntax, and natural language parsing.
  Anyway, food for thought.

  In the mean time, Similarity enables command line usage, one comparison pair at a time.
***

**2. Why so many parameters? Do I really need to use them all?**

No. To use the command line you only need to use two parameters.  The two comparison strings.

Ideally, you or your administrator will have setup predefined preferences for all of the parameters. In which case you can ignore them, or simply rely on the
system defaults.

If you really do wish to use the boolean parameters the only required argument other than the comparison strings is the MatchingAlgorithm attribute, otherwise the
shell will not know what you are trying to do. The boolean attributes need to be in order but you don't need to use them all.  If you wish to make, for example, the
fourth boolean argument (ReverseComparison) true, then you will need to let Similarity know what the preceding boolean arguments are, otherwise it won't know which
boolean attribute you are referring to.
***

**3. Can't I just send it a file and get a completed one back?**  

The short answer is no, not yet. I would actually like to write this but it is entirely dependent on interest.  There are more than just a few businesses out there
making a living doing what Similarity already does for free.  So, maybe.
***

**4. Lastly, the expanded command line usage definition**

	The command line can take zero, two or up to nine arguments. Zero arguments will launch the application, whereas
	any argument passed to the application will return an integer representing a percentage matching value

	For example:

	[location]\similarity.exe "comparison one"  "comparison two"

	In addition the command line also supports the following seven arguments:

	NOTE: when attributes are not found, a predefined preference takes precedence, stepping through the following rules

	Where:

	   a) in absence of an attribute the argument will default to a predefined user preference,
	   b) in absence of a user preference, the argument defaults to a predefined client administrator preference,
	   c) in absence of a client administrator preference, the default system preference is used

	MatchingAlgorithm (0,1,2 - defaults to preference)

	Where:

	   0 = the Ratcliff/Obershelp matching algorithm
	   1 = the Levenshtein distance algorithm
	   2 = the Hamming distance algorithm

	MakeCaseInsensitive (true or false - defaults to predefined preference)
	PadToEqualLength (true or false - defaults to predefined preference)
	RemoveWhitespace (true or false - defaults to predefined preference)
	ReverseComparison (true or false - defaults to predefined preference)
	PhoneticFilter (true or false - defaults to predefined preference)
	WholeWordComparison (true or false - defaults to predefined preference)

	For example:

	[location]\similarity.exe "comparison one"  "comparison two" 2 true true true true true true

***
