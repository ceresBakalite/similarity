## Welcome to Similarity Pattern Matching
### *Seeking similarity between data strings*
Similarity estimates the similarities or dissimilarities between things. It compares any group of characters with any other group of characters and estimates the percentage similarity between one or more items. [read more][]

***

**Release Notes v1.2.5b**

Simplified assigning user defined attributes on the following
    
* Treat whitespace characters as noise characters
* Apply case insensitivity to each string
* Pad the smallest string to be of equal length to the largest
* Determine if one string is similar to the converse of the other
* Apply a phonetic filter to further determine similarity
* Weight distinct case insensitive whole words shared by both strings

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

