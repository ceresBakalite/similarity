# Similarity
Pattern matching used to seek similarity between data strings

Release Notes

  Pattern Matching attribute control
  
    Enable / Disable the following
    
      1. Treat whitespace characters as noise characters
      2. Apply case insensitivity to each string
      3. Pad the smallest string to be of equal length to the largest
      4. Determine if one string is similar to the converse of the other
      5. Apply a phonetic filter determine similarity
      6. Weight distinct case insensitive whole words shared by both strings
    
  Ratcliff Obershelp
  
      The Ratcliff/Obershelp pattern-matching algorithm was developed by John W. Ratcliff and John A. Obershelp 
      in 1983 to address concerns about educational software (Ratcliff, 1988).

  Levenshtein Distance
  
      The Levenshtein distance is a string metric for measuring the difference between two sequences. The 
      Levenshtein distance between two words is the minimum number of single-character edits (i.e.insertions, 
      deletions or substitutions) required to change one word into the other.
      
  Hamming Distance
  
    The Hamming Distance measures the minimum number of substitutions required to change one string into 
    the other.The Hamming distance between two strings of equal length is the number of positions at which 
    the corresponding symbols are different.
    
  Phonetic Pattern matching 
  
    A phonetic algorithm is a method for comparing strings by sound, specifically as pronounced in English, where 
    differences in spelling can be overlooked
    
  AESThenHMAC Authentication
  
    https://gist.github.com/4336842
  
  Trace monitoring
