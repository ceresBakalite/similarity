## Command Line Usage
### *Seeking patterns of similarity between data strings* <img align="right" src="./images/NAVSimilarityLogoSmall.png">
The following notes summarise calling the application and applying arguments to invoke a variety of options available within the application.  There have been a number of 
common questions asked by users which deserve, and indeed have now found, a more permanent placeholder here.
***

**Release Version v1.2.8.2b introduced command line usage to Similarity**

** *FAQ* **
1. Why a shell?

  Oddly, the original premise for Similarity was just a commandline application.  This application would reside somewhere in the cloud and would be accessable from
  any assembly anywhere. This would enable a third party application to use all the functionality without the overhead. 
  
  For example, consider a postal service checking for valid address syntax targetting two countries. There is a FROM address and a TO address. Both these addresses 
  are read (typically by software) in each country where both hope to see each address in their local syntax.  This requires the manipulation of each address twice. 
  In other words, four seperate addresses, language, culture and syntax specific.
  
  In many ways this is still a better usage of the software and I fully intend to create a C++ shell variant of the Similarity application, should 
  there be enough interest. A C++ variant would not just be small and fast, it would also need to take one, two, or many comparison sets and it would throw 
  away the overhead associated with a GUI.  Anyway, food for thought. 
  
2. Why so many parameters? Do I really have to use them all?  

	The command-line takes zero, two or nine arguments. Zero arguments will launch the application, whereas
	any argument passed to the application will return an integer representing a percentage matching value
	
	For example:
	
	[location]\similarity.exe "comparison one"  "comparison two"
	
	In addition the command line also supports the following seven arguments:
	
	MatchingAlgorithm (0,1,2)
	
	Where:
	
	   0 = the Ratcliff/Obershelp matching algorithm
	   1 = the Levenshtein distance algorithm
	   2 = the Hamming distance algorithm
	
	MakeCaseInsensitive (true or false)
	PadToEqualLength (true or false)
	RemoveWhitespace (true or false)
	ReverseComparison (true or false)
	PhoneticFilter (true or false)
	WholeWordComparison (true or false)
	
	For example:
	
	[location]\similarity.exe "comparison one"  "comparison two" 2 true true true true true true

***

**User defined pattern matching algorithms enabled**

Ratcliff Obershelp
  
      The Ratcliff/Obershelp pattern-matching algorithm was developed by John W. Ratcliff and 
      John A. Obershelp in 1983 to address concerns about educational software (Ratcliff, 1988).

_ref._ [The Gestalt Approach][]

Levenshtein Distance
  
      The Levenshtein distance is a string metric for measuring the difference between two sequences. 
      The Levenshtein distance between two words is the minimum number of single-character edits 
      (i.e.insertions, deletions or substitutions) required to change one word into the other.

_ref._ [The Levenshtein distance algorithm][]
   
Hamming Distance
  
      The Hamming Distance measures the minimum number of substitutions required to change one string into 
      the other.The Hamming distance between two strings of equal length is the number of positions at which 
      the corresponding symbols are different.
    
_ref._ [Hamming Distance as a Concept in DNA Molecular Recognition][]
   
Phonetic Pattern matching 
  
     A phonetic algorithm is a method for comparing strings by sound, specifically as pronounced in English, 
     where differences in spelling can be overlooked.
    
_ref._ [Journal of Biomedical Semantics][]

***

**Added encrypted pointer obfuscation referencing crytographic and authentication key pairs**

AESThenHMAC Authentication

     You've gotta love a good cipher methodology.
     
_ref._ @https://gist.github.com/jbtule/4336842

***

**Added additional log, debug and trace monitoring**

Trace monitoring
  
     This has enabled us to significantly reduce the time required for a typical application initialisation.
     
     Background debug trace markers have been added to the pre-production and dev environment to monitor seek 
     and process time intervals, particularly related to cloud server activity. This code is ignored by the 
     compiler on release. 
     
     It was noted that timeout errors (commonly 258, 165, 298 and -2 et al SQL and .NET timeouts) occurred 
     during specific periods during the day.  This in turn brought to attention the need to invoke some 
     background processes at different intervals during initialisation.
     
     In addition, these delays enabled us to accurately target retry attempts on stored procedures that we now 
     knew did not experience a fatal error, which was a bonus.

Runtime error logging

     Enabled runtime error logging as a semi-permanent feature. It is now a user-defined option whose default 
     is set to log to the cloud and works quietly in the background.  Logging to file and to the console 
     remain, but are disabled in production.

_ref._ [The Apache log4net library][]

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

