## Welcome to Similarity Pattern Matching
### *Seeking patterns of similarity between data strings*
Similarity estimates the similarities or dissimilarities between things. It compares any group of characters with any other group of characters and estimates the percentage similarity between one or more items. [read more][]

***

**Release Notes v1.2.8b**

Applied predominately cosmetic changes

v1.2.8.3b notes:

	1. Stopped further unnecessary display catch-up builds, eliminating flicker (deleting multiple rows 
	   in the Result View now refreshes only once when all processing is complete).

v1.2.8.2b notes:

	1. Smoothed display-text transition
	2. Stopped unnecessary display catchup builds, eliminating flicker
	3. Added a splash screen
	4. Added a drag & drop presentation display
	5. Removed a display threading bug that appeared only when an unusually large number of duplicates
	   were present
	6. Normalised method calls so that methods applying similar or cross over activities did not repeat 
	   tasks
	7. Applied system diagnostic compilation directives to accurately reflect the run-time environment
	8. Set pre-processor and conditional compiler directives to reflect the run-time environment
	9. Applied debug directives to accurately time tasks
	10. Reallocated and repositioned methods to create a smoother on load transition
	11. Redesigned threading start-up tasks to create a smoother on load transition
	12. Set threading tasks to perform only in the environment in which they are required
	13. Applied a pull data request as a new default, permitting a user to optionally change from the 
	    now redundant push data request when applying abbreviations to search criteria.  This greatly 
	    enhances performance on all but the smallest data sets
	14. Applied command-line usage to enable the application to be called from an external assembly.  
	
	The command-line takes zero, two or ten arguments. Zero arguments will launch the application, whereas
	any argument passed to the application will return an integer representing a percentage matching value
	
	For example:
	
	[location]\Similarity.exe "comparison one"  "comparison two"
	
	In addition the command line also supports the following eight arguments:
	
	MatchingAlgorithm (0,1,2)
	
	Where:
	
	   0 = the Ratcliff/Obershelp matching algorithm
	   1 = the Levenshtein distance algorithm
	   2 = the Hamming distance algorithm
	
	MakeCaseInsensitive (true or false)
	PadToEqualLength (true or false)
	RemoveWhitespace (true or false)
	ReverseComparison (true or false)
	MakeCaseInsensitive (true or false)
	PhoneticFilter (true or false)
	WholeWordComparison (true or false)
	
	For example:
	
	[location]\Similarity.exe "comparison one"  "comparison two" 2 true true true true true true true

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

