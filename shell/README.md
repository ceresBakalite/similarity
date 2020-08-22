## Command Line Usage
### *Seeking patterns of similarity between data strings* <img align="right" src="../images/NAVSimilarityLogoSmall.png">
The following notes summarise calling the application and applying arguments to invoke a variety of options available within the application.  There have been a number of 
common questions asked by users which deserve, and indeed have now found, a more permanent placeholder here.
***

**Release Version v1.2.8.2b introduced command line usage to Similarity**

**FAQ**
1. Why a shell?

  Oddly, the original premise for Similarity was just a commandline application.  This application would reside somewhere in the cloud and would be accessable from
  any assembly anywhere. This would enable a third party application to use all the functionality without the overhead. 
  
  For example, consider a postal service checking for valid address syntax targetting two countries. There is a FROM address and a TO address. Both these addresses 
  are read (typically by software) in each country where both hope to see each address in their local syntax.  This requires the manipulation of each address twice. 
  In other words, four seperate addresses, language, culture and syntax specific.
  
  In many ways establishing the application as a shell is still a better use of the software IMHO and I fully intend to create a C++ shell variant of the Similarity 
  application, should there be enough interest. A C++ variant would not just be small and fast, it would also need to take one, two, or many comparison sets and 
  it would throw away the overhead associated with a GUI.  Anyway, food for thought. 
  
  In the mean time, Similarity now enables command line usage, one comparison pair at a time. 
  
2. Why so many parameters? Do I really need to use them all?  

2. Can I just send it a file and get a completed one back?  

Lastly, the expanded command line usage definition

	The command-line takes zero, two or nine arguments. Zero arguments will launch the application, whereas
	any argument passed to the application will return an integer representing a percentage matching value
	
	For example:
	
	[location]\similarity.exe "comparison one"  "comparison two"
	
	In addition the command line also supports the following seven arguments:
	
	NOTE: a predefined preference steps through the following rules
		a) where in absense of an attribute an argument defaults to a predefined user preference, 
		b) where a user preference does not exist, defaults to a predefined client administrator preference,
		c) where a client administrator preference does not exist, defaults to the predefined system administrator preference
	
	MatchingAlgorithm (0,1,2 - defaults to predefined preference)
	
	Where:
	
	   0 = the Ratcliff/Obershelp matching algorithm
	   1 = the Levenshtein distance algorithm
	   2 = the Hamming distance algorithm
	
	MakeCaseInsensitive (true or false - default to predefined preference)
	PadToEqualLength (true or false - defaults to predefined preference)
	RemoveWhitespace (true or false - defaults to predefined preference)
	ReverseComparison (true or false - defaults to predefined preference)
	PhoneticFilter (true or false - defaults to predefined user preference)
	WholeWordComparison (true or false - defaults to predefined user preference)
	
	For example:
	
	[location]\similarity.exe "comparison one"  "comparison two" 2 true true true true true true

***

[Hamming Distance as a Concept in DNA Molecular Recognition]: https://pubs.acs.org/doi/full/10.1021/acsomega.7b00053
[Journal of Biomedical Semantics]: https://jbiomedsem.biomedcentral.com/articles/10.1186/s13326-019-0216-2
[The Levenshtein distance algorithm]: https://www.educative.io/edpresso/the-levenshtein-distance-algorithm
[The Gestalt Approach]: https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
[read more]: https://ceresbakalite.github.io/similarity/ 
[The Apache log4net library]: https://logging.apache.org/log4net/
[v1.2.8.3b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.2.8.3b
[v1.2.8.2b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.2.8.2b
[v1.3.2b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.2b
[v1.3.3b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.3b
[v1.3.4b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.4b

