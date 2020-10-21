## Welcome to Similarity Pattern Matching
### *Seeking patterns of similarity between data strings* <div id="logo-container"><img id="logo-default" title="No readable content. Just a page logo" class="img-logo" align="right" src="../../images/NAVSimilarityLogoShell.png"></div>
Similarity estimates the similarities or dissimilarities between things. It compares any group of characters with any other group of characters and estimates the percentage similarity between one or more items. [read more][]
***

**Release Version v1.3.5b**

[v1.3.5b notes][]

	1. Minor cumulative additions
	2. Fatal null reference exception handling changes

[v1.3.4b notes][]

	1. Minor cumulative additions
	2. Placed open file methods in their own thread
	3. Placed save file methods in their own thread

[v1.3.3b notes][]

	1. Minor cumulative additions
	2. Dependency updates
	3. Separated .NET Framework and .NET Standard libraries to reflect their constituent components

[v1.3.2b notes][]

	1. Minor cumulative changes applied
	2. Dependencies updated
	3. Reverted to Semantic Versioning (SemVer - ie major.minor.patch) notation

[v1.2.8.3b notes][]

[v1.2.8.2b notes][]

***

#### User defined pattern matching algorithms enabled

**Ratcliff Obershelp**

      The Ratcliff/Obershelp pattern-matching algorithm was developed by John W. Ratcliff and
      John A. Obershelp in 1983 to address concerns about educational software (Ratcliff, 1988).

<sup>_ref._ [The Gestalt Approach][]</sup>

**Levenshtein Distance**

      The Levenshtein distance is a string metric for measuring the difference between two
      sequences. The Levenshtein distance between two words is the minimum number of single-
      character edits (i.e. insertions, deletions or substitutions) required to change one
      word into the other.

<sup>_ref._ [The Levenshtein distance algorithm][]</sup>

**Hamming Distance**

      The Hamming Distance measures the minimum number of substitutions required to change
      one string into the other. The Hamming distance between two strings of equal length
      is the number of positions at which the corresponding symbols are different.

<sup>_ref._ [Hamming Distance as a Concept in DNA Molecular Recognition][]</sup>

**Phonetic Pattern matching**

      A phonetic algorithm is a method for comparing strings by sound, specifically as
      pronounced in English, where differences in spelling can be overlooked.

<sup>_ref._ [Journal of Biomedical Semantics][]</sup>

***

### Recent command line, monitoring, and encryption additions

**Introduction of a [Command Line][] (CLI) to Similarity**

     A short summary of the syntax used to call the Similarity application from the
     command line, providing a means to invoke a wide variety of options available
     in the GUI.

**Introduced a method to obfuscate pointers to cryptographic and authentication key pairs**

AESThenHMAC Authentication

     You've gotta love a good cipher methodology.

<sup>_ref._ @https://gist.github.com/jbtule/4336842</sup>
<sup>_and_ [v1.2.8.2b notes][]</sup>

***

**Additional log, debug and trace monitoring**

Trace monitoring

      This has enabled us to significantly reduce the time required for a typical application
      initialisation.

      Background debug trace markers have been added to the pre-production and dev
      environment to monitor seek and process time intervals, particularly related to cloud
      server activity. This code is ignored by the compiler on release.

      It was noted that timeout errors (commonly 258, 165, 298 and -2 et al SQL and .NET
      timeouts) occurred during specific periods during the day.  This in turn brought to
      attention the need to invoke some background processes at different intervals during
      initialisation.

      In addition, these delays enabled us to accurately target retry attempts on stored
      procedures that we now knew did not experience a fatal error, which was a bonus.

Runtime error logging

      Enabled runtime error logging as a semi-permanent feature. It is now a user-defined
      option whose default is set to log to the cloud and works quietly in the background.
      Logging to file and to the console remain, but are disabled in production.

<sup>_ref._ [The Apache log4net library][]</sup>
<sup>_and_ [v1.2.8.2b notes][]</sup>

<br>

[Hamming Distance as a Concept in DNA Molecular Recognition]: https://pubs.acs.org/doi/full/10.1021/acsomega.7b00053
[Journal of Biomedical Semantics]: https://jbiomedsem.biomedcentral.com/articles/10.1186/s13326-019-0216-2
[The Levenshtein distance algorithm]: https://www.educative.io/edpresso/the-levenshtein-distance-algorithm
[The Gestalt Approach]: https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
[read more]: ./
[The Apache log4net library]: https://logging.apache.org/log4net/
[v1.2.8.3b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.2.8.3b
[v1.2.8.2b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.2.8.2b
[v1.3.2b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.2b
[v1.3.3b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.3b
[v1.3.4b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.4b
[v1.3.5b notes]: https://github.com/ceresBakalite/similarity/releases/tag/v1.3.5b
[Command Line]: https://github.com/ceresBakalite/similarity/tree/master/shell

<style>
.img-pointer {
  max-width: 100%;
  vertical-align:bottom;
  float:left;
  margin: 0px 15px 0px 0px;
}

.img-logo {
  width: 35%;
  opacity: 0.999;
  margin: 15px 0px 15px 0px;
  position: relative;
  z-index: -1;
}
</style>
