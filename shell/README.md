## Command Line Usage
### *Seeking patterns of similarity between data strings* <div id="logo-container"><img id="logo-shell" class="img-logo" align="right" src="https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png"></div>
The following notes summarise calling the Similarity application from the command line and applying arguments to invoke a variety of options available in the GUI. There have been a number of common questions asked by users which deserve, and indeed have now found, a more permanent placeholder here.
***

**Release Version v1.2.8.2b introduced command line usage to Similarity**

## Command Line FAQ

#### Shell (computing)

In computing, a shell is a user interface for access to an operating system's services. In general, operating system shells use either a command-line interface (CLI) or graphical user interface (GUI), depending on a computer's role and particular operation. It is named a shell because it is the outermost layer around the operating system.

Command-line shells require the user to be familiar with commands and their calling syntax, and to understand concepts about the shell-specific scripting language...

<sup>*ref.* [Shell (computing)][] Wikipedia, the free encyclopedia</sup>

**1. Why add a command line shell?**

Oddly, a very early iteration of Similarity only used a command line interface.  The premise was that this CLI would reside somewhere in the cloud and would be accessible from any assembly anywhere. This would enable a third party application to use all the functionality without the overhead.

In many ways establishing the application as a shell is still a better use of the software IMHO and I fully intend to create a C++ shell variant of the Similarity application, should there be enough interest. This variant would not just be smaller and fast, it would also need to take one, two, or many comparison sets and it would throw away the overhead associated with a GUI.  

For example, consider a postal service checking for valid address syntax targeting two countries. There is a FROM address and a TO address. Both these addresses are read (typically by software) in each country where both hope to see each address in their local syntax.  This requires the manipulation of each address twice. In other words, four separate addresses, language, culture and syntax specific. Its entirely conceivable that they do this simply by querying the CLI, at runtime.

Although largely language independent already, language interoperability would be a fine thing.  As would culture, syntax, and natural language parsing. Anyway, food for thought.
***

**2. So many parameters. Do I really need to use them all?**

No. To use the command line you only need to use two parameters.  The two comparison strings.

Ideally, you or your administrator will have setup predefined preferences for all of the parameters. In which case you can ignore them, or simply rely on the system defaults.
***

**3. Can't I just send it a file and get a completed one back?**  

The short answer is no, not yet. Obviously this is what the GUI does and, yes, I would really like to write this for the CLI as well, but it is entirely dependent on interest.  

There are more than just a few businesses out there making a living doing what Similarity already does for free. So, maybe.

In the mean time, Similarity enables command line usage, one comparison pair at a time.
***

**4. Lastly, the expanded command line usage definition**

	The command line can take zero, two or up to nine arguments. Zero arguments will launch the
	application, whereas any argument passed to the application will return an integer representing
	a percentage matching value

	For example:

	[location]\similarity.exe "comparison one"  "comparison two"

  If you wish to use the boolean parameters the only required argument other than the comparison strings is the MatchingAlgorithm attribute, otherwise the shell will not know what you are trying to do. The boolean attributes need to be in order but you don't need to use them all.  If you wish to make, for example, the fourth boolean argument (ReverseComparison) true, then you will need to let Similarity know what the preceding arguments are, otherwise it won't know which attribute you are referring to.

	Additionally, the command line also supports the following seven boolean arguments:

	NOTE: when an attribute is not found a predefined preference takes precedence

	Where:

	   a) in absence of an attribute the argument will default to a predefined user preference,
	   b) in absence of a user preference, the argument defaults to the client administrator preference,
	   c) in absence of a client administrator preference, the system default preference is used

	MatchingAlgorithm (0,1,2 - defaults to preference)

	Where:

	   0 = the Ratcliff/Obershelp matching algorithm
	   1 = the Levenshtein distance algorithm
	   2 = the Hamming distance algorithm

	MakeCaseInsensitive (true or false - defaults to preference)
	PadToEqualLength (true or false - defaults to preference)
	RemoveWhitespace (true or false - defaults to preference)
	ReverseComparison (true or false - defaults to preference)
	PhoneticFilter (true or false - defaults to preference)
	WholeWordComparison (true or false - defaults to preference)

	For example:

	[location]\similarity.exe "comparison one"  "comparison two" 2 true true true true true true

<sup>_ref._ [v1.2.8.2b notes][]</sup>

<br>

[v1.2.8.2b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.2.8.2b
[Shell (computing)]: https://en.wikipedia.org/wiki/Shell_(computing)

<style>
.img-pointer {
	max-width: 100%;
  vertical-align:bottom;
  float:left;
  margin: 0px 15px 0px 0px;
  z-index: -1;
}

.img-logo {
	max-width: 85%;
	position: relative;
  z-index: -1;
}
</style>
