using System.Linq;

namespace PatternMatching
{
    public class ComparisonMethods
    {
        public partial class RatcliffObershelp
        {
            #region

            public static int ROCompare(string str1, string str2)
            {
                /*

                  The Ratcliff/Obershelp pattern-matching algorithm was developed by John W. Ratcliff and John A. Obershelp 
                  in 1983 to address concerns about educational software (Ratcliff, 1988).

                  Approximate string matching: takes two strings and returns a similarity score based on co-occurrence of 
                  sub patterns.

                  The algorithm itself has become part of the Python pantheon of comparison functions, albeit in modified 
                  form where some pre-processing of both string pairs and noise elements has been introduced. The algorithm 
                  can be found in The Python Standard Library: class difflib.SequenceMatcher (The Python Standard Library 3.8.1 
                  Text Processing Services).

                  The Compare method calls the MatchPattern method, which in turn calls itself until all patterns 
                  are matched and a percentage value is returned.

                  The GetWeightedComparison method can apply up to six weights (bRemoveNoiseCharacters, bMakeCaseInsensitve, 
                  bPadToEqualLength, bReverseComparison, bSoundexComparison, and bWholeWordComparison) before settling on a 
                  final comparison value.

                  Notes:

                  1.   To paraphrase the original usage text and the TEXT SEGMENT portion in SIMIL.ASM published on July 01, 1988 
                       by John W. Ratcliff and David E. Metzener:

                          November 10, 1987

                          Uses the Ratcliff/Obershelp pattern recognition algorithm. This program provides a new function to C on 
                          an 8086 based machine. The function SIMIL returns a percentage value corresponding to how alike any two 
                          strings are.  Be certain to upper case the two strings passed if you are not concerned about case 
                          sensitivity...

                          ...the similarity routine begins by computing the total length of both strings passed and placing that 
                          value in TOTAL.  It then takes the beginning and ending of both strings passed and pushes them on the 
                          stack.  It then falls into the main line code. The original two strings are immediately popped off of 
                          the stack and are passed to the compare routine.

                          Ratcliff, J & Metzener, D 1988, 'Pattern Matching: the Gestalt Approach: The Ratcliff/Obershelp
                          pattern-matching algorithm', Dr Dobb's Journal, pp. 46-51.

                  The following source code replicates this behaviour in C#

                  Alexander Munro - January 29, 2020

                */

                int strLen1 = str1.Length;
                int strLen2 = str2.Length;

                if (strLen1 == 0 || strLen2 == 0) return 0;

                return (ROMatchPattern(str1, strLen1, str2, strLen2) * 200) / (strLen1 + strLen2);
            }

            public static int GetWeightedComparison(string str1, string str2,
                bool bRemoveNoiseCharacters = true, // remove whitespace and punctuation characters characters
                bool bMakeCaseInsensitve = true,    // apply case insensitivity to each string
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest
                bool bReverseComparison = true,     // determine if one string is similar to the converse of the other
                bool bSoundexComparison = true,     // apply a phonetic filter to further determine similarity
                bool bWholeWordComparison = true)   // weight distinct case insensitive whole words shared by both strings
            {
                if ((string.IsNullOrWhiteSpace(str1)) || (string.IsNullOrWhiteSpace(str2))) return 0;

                int wholeWordResult = bWholeWordComparison ? ROCompareWords(str1, str2, bPadToEqualLength) : 0;
                int soundexResult = bSoundexComparison ? ROSoundexWords(str1, str2) : 0;
                int wordComparison = wholeWordResult > soundexResult ? wholeWordResult : soundexResult;

                str1 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str1) : str1;
                str2 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str2) : str2;

                str1 = bMakeCaseInsensitve ? str1.ToUpperInvariant() : str1;
                str2 = bMakeCaseInsensitve ? str2.ToUpperInvariant() : str2;

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = ROCompare(str1, str2);

                if (bReverseComparison)
                {
                    int reverseResult = ROCompare(str1, ReverseString(str2));
                    result = (result > reverseResult) ? result : reverseResult;
                }

                return (result > wordComparison) ? result : wordComparison;
            }

            private static int ROCompareWords(string str1, string str2,
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest
                decimal iWeightPercentage = 15)     // arbitrarily reduces the result by the weight percentage specified
            {
                str1 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str1.ToUpperInvariant())));
                str2 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str2.ToUpperInvariant())));

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = ROCompare(str1, str2);

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            private static int ROSoundexWords(string str1, string str2,
                decimal iWeightPercentage = 10)         // arbitrarily reduces the result by the weight percentage specified
            {
                int result = ROCompare(SoundexWordsInString(str1), SoundexWordsInString(str2));

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            private static int ROMatchPattern(string strLeft, int iStrLengthLeft, string strRight, int iStrLengthRight)
            {
                int i, j;
                int k = 0;
                int m = 0;
                int iStrLength = 0;
                int iStrLengthLeftReset = 0;
                int iStrLengthRightReset = 0;

                for (i = 0; i < iStrLengthLeft - iStrLength; i++) ExtractBoundary();

                return iStrLength == 0 ? 0 : MatchPattern();

                void ExtractBoundary()
                {
                    for (j = 0; j < iStrLengthRight - iStrLength; j++)
                    {
                        if (k < iStrLengthLeft && m < iStrLengthRight && strLeft[i] == strRight[j] && strLeft[i + iStrLength] == strRight[j + iStrLength])
                        {
                            TestBoundary();
                        }

                    }

                }

                void TestBoundary()
                {
                    for (k = i + 1, m = j + 1; k < iStrLengthLeft && m < iStrLengthRight && strLeft[k] == strRight[m] && k < iStrLengthLeft && m < iStrLengthRight; k++, m++) ;

                    if (k - i > iStrLength)
                    {
                        iStrLengthLeftReset = i;
                        iStrLengthRightReset = j;
                        iStrLength = k - i;
                    }

                }

                int MatchPattern()
                {
                    i = iStrLengthLeftReset + iStrLength;
                    j = iStrLengthRightReset + iStrLength;

                    iStrLengthLeft -= i;
                    iStrLengthRight -= j;

                    int iRightMatch = (iStrLengthLeft != 0 && iStrLengthRight != 0) ? ROMatchPattern(strLeft.Substring(i, iStrLengthLeft), iStrLengthLeft, strRight.Substring(j, iStrLengthRight), iStrLengthRight) : 0;

                    return iStrLength + ((iStrLengthLeftReset != 0 && iStrLengthRightReset != 0) ? ROMatchPattern(strLeft, iStrLengthLeftReset, strRight, iStrLengthRightReset) : 0) + iRightMatch;
                }

            }

            #endregion
        }

        public partial class LevenshteinDistance
        {
            #region

            public static int LSCompare(string str1, string str2)
            {
                /*

                The Levenshtein distance is a string metric for measuring the difference between two sequences. The Levenshtein distance 
                between two words is the minimum number of single-character edits (i.e.insertions, deletions or substitutions) required 
                to change one word into the other.

                */

                return (int)(100 - (decimal)LSMatchPattern(str1, str2) * 100 / (str1.Length + str2.Length));
            }

            public static int GetWeightedComparison(string str1, string str2,
                bool bRemoveNoiseCharacters = true, // remove whitespace and punctuation characters characters
                bool bMakeCaseInsensitve = true,    // apply case insensitivity to each string
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest
                bool bReverseComparison = true,     // determine if one string is similar to the converse of the other
                bool bSoundexComparison = true,     // apply a phonetic filter to further determine similarity
                bool bWholeWordComparison = true)   // weight distinct case insensitive whole words shared by both strings
            {
                if ((string.IsNullOrWhiteSpace(str1)) || (string.IsNullOrWhiteSpace(str2))) return 0;

                int wholeWordResult = bWholeWordComparison ? LSCompareWords(str1, str2, bPadToEqualLength) : 0;
                int soundexResult = bSoundexComparison ? LSSoundexWords(str1, str2) : 0;
                int wordComparison = wholeWordResult > soundexResult ? wholeWordResult : soundexResult;

                str1 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str1) : str1;
                str2 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str2) : str2;

                str1 = bMakeCaseInsensitve ? str1.ToUpperInvariant() : str1;
                str2 = bMakeCaseInsensitve ? str2.ToUpperInvariant() : str2;

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = LSCompare(str1, str2);

                if (bReverseComparison)
                {
                    int reverseResult = LSCompare(str1, ReverseString(str2));
                    result = (result > reverseResult) ? result : reverseResult;
                }

                return (result > wordComparison) ? result : wordComparison;
            }

            private static int LSCompareWords(string str1, string str2,
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest
                decimal iWeightPercentage = 15)     // arbitrarily reduces the result by the weight percentage specified
            {
                str1 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str1.ToUpperInvariant())));
                str2 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str2.ToUpperInvariant())));

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = LSCompare(str1, str2);

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            private static int LSSoundexWords(string str1, string str2,
                decimal iWeightPercentage = 10)     // arbitrarily reduces the result by the weight percentage specified
            {
                int result = LSCompare(SoundexWordsInString(str1), SoundexWordsInString(str2));

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            public static int LSMatchPattern(string strLeft, string strRight)
            {
                if (strRight.Length == 0) return strLeft.Length;

                int[] strRightArray = new int[strRight.Length];

                for (int i = 0; i < strRightArray.Length;) strRightArray[i] = ++i;

                for (int i = 0; i < strLeft.Length; i++) CalculateCost(i, strLeft[i]);

                void CalculateCost(int iStrLeftPosition, char strLeftChar)
                {
                    int iStrRightPosition = iStrLeftPosition;

                    for (int j = 0; j < strRight.Length; j++)
                    {
                        int k = iStrLeftPosition;
                        iStrLeftPosition = iStrRightPosition;
                        iStrRightPosition = strRightArray[j];

                        if (!strLeftChar.Equals(strRight[j]))
                        {
                            if (k < iStrLeftPosition) iStrLeftPosition = k;
                            if (iStrRightPosition < iStrLeftPosition) iStrLeftPosition = iStrRightPosition;
                            ++iStrLeftPosition;
                        }

                        strRightArray[j] = iStrLeftPosition;
                    }

                }

                return strRightArray[strRightArray.Length - 1];
            }

            #endregion
        }

        public partial class HammingDistance
        {
            #region

            public static int HDCompare(string str1, string str2)
            {
                /*

                The Hamming Distance measures the minimum number of substitutions required to change one string into 
                the other.The Hamming distance between two strings of equal length is the number of positions at which 
                the corresponding symbols are different.

                */

                return (int)(100 - (decimal)HDMatchPattern(str1, str2) * 100 / (str1.Length + str2.Length));
            }

            public static int GetWeightedComparison(string str1, string str2,
                bool bRemoveNoiseCharacters = true, // remove whitespace and punctuation characters characters
                bool bMakeCaseInsensitve = true,    // apply case insensitivity to each string
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest (required)
                bool bReverseComparison = true,     // determine if one string is similar to the converse of the other
                bool bSoundexComparison = true,     // apply a phonetic filter to further determine similarity
                bool bWholeWordComparison = true)   // weight distinct case insensitive whole words shared by both strings
            {
                if ((string.IsNullOrWhiteSpace(str1)) || (string.IsNullOrWhiteSpace(str2))) return 0;

                int wholeWordResult = bWholeWordComparison ? HDCompareWords(str1, str2, bPadToEqualLength) : 0;
                int soundexResult = bSoundexComparison ? HDSoundexWords(str1, str2) : 0;
                int wordComparison = wholeWordResult > soundexResult ? wholeWordResult : soundexResult;

                str1 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str1) : str1;
                str2 = bRemoveNoiseCharacters ? RemoveNoiseCharacters(str2) : str2;

                str1 = bMakeCaseInsensitve ? str1.ToUpperInvariant() : str1;
                str2 = bMakeCaseInsensitve ? str2.ToUpperInvariant() : str2;

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = HDCompare(str1, str2);

                if (bReverseComparison)
                {
                    int reverseResult = HDCompare(str1, ReverseString(str2));
                    result = (result > reverseResult) ? result : reverseResult;
                }

                return (result > wordComparison) ? result : wordComparison;
            }

            private static int HDCompareWords(string str1, string str2,
                bool bPadToEqualLength = true,      // pad the smallest string to be of equal length to the largest
                decimal iWeightPercentage = 15)     // arbitrarily reduces the result by the weight percentage specified
            {
                str1 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str1.ToUpperInvariant())));
                str2 = RemoveNoiseCharacters(SortWordsInString(RemoveDuplicateWords(str2.ToUpperInvariant())));

                str1 = bPadToEqualLength ? str1.PadRight(str2.Length, (char)32) : str1;
                str2 = bPadToEqualLength ? str2.PadRight(str1.Length, (char)32) : str2;

                int result = HDCompare(str1, str2);

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            private static int HDSoundexWords(string str1, string str2,
                decimal iWeightPercentage = 5)      // arbitrarily reduces the result by the weight percentage specified
            {
                int result = HDCompare(SoundexWordsInString(str1), SoundexWordsInString(str2));

                return System.Convert.ToInt32(result - (result * (iWeightPercentage / 100)));
            }

            public static int HDMatchPattern(string strLeft, string strRight)
            {
                if (strLeft.Length == strRight.Length)
                {
                    int distance = strLeft.ToCharArray()
                        .Zip(strRight.ToCharArray(), (c1, c2) => new { c1, c2 })
                        .Count(m => m.c1 != m.c2);

                    return distance;
                }

                return System.Math.Max(strLeft.Length, strRight.Length);
            }

            #endregion
        }

        public partial class PhoneticPatterns
        {
            #region

            public static string Soundex(string data)
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();

                if (data != null && data.Length > 0)
                {
                    result.Append(char.ToUpper(data[0]));

                    string previousCode = string.Empty;

                    for (int i = 1; i < data.Length; i++)
                    {
                        string currentCode = EncodeChar(data[i]);

                        if (currentCode != previousCode) result.Append(currentCode);

                        if (result.Length == 4) break;

                        if (!currentCode.Equals(string.Empty)) previousCode = currentCode;
                    }

                }

                if (result.Length < 4) result.Append(new string('0', 4 - result.Length));

                return result.ToString();

                string EncodeChar(char c)
                {
                    switch (char.ToLower(c))
                    {
                        case 'b':
                        case 'f':
                        case 'p':
                        case 'v':
                            return "1";

                        case 'c':
                        case 'g':
                        case 'j':
                        case 'k':
                        case 'q':
                        case 's':
                        case 'x':
                        case 'z':
                            return "2";

                        case 'd':
                        case 't':
                            return "3";

                        case 'l':
                            return "4";

                        case 'm':
                        case 'n':
                            return "5";

                        case 'r':
                            return "6";

                        default:
                            return string.Empty;
                    }

                }

            }

            #endregion
        }

        private static string SortWordsInString(string str, char delimiter = (char)32)
        {
            string[] wordArray = str.Split(delimiter);
            System.Array.Sort(wordArray);

            return string.Join(delimiter.ToString(), wordArray);
        }

        private static string RemoveDuplicateWords(string str, char delimiter = (char)32)
        {
            return string.Join(delimiter.ToString(), str.Split(delimiter).Distinct(System.StringComparer.CurrentCultureIgnoreCase));
        }

        private static string SoundexWordsInString(string str, char delimiter = (char)32)
        {
            string[] wordArray = str.Split(delimiter);
            string words = null;

            foreach (string word in wordArray)
            {
                words = words + PhoneticPatterns.Soundex(word) + delimiter;
            }

            string[] strArray = RemoveDuplicateWords(words.TrimEnd(delimiter)).Split(delimiter);
            System.Array.Sort(strArray);

            return string.Join(delimiter.ToString(), strArray);
        }

        private static string RemoveNoiseCharacters(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? str : RemovePunctuation(RemoveWhitespace(str));
        }

        private static string RemovePunctuation(string str)
        {
            return new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        private static string RemoveWhitespace(string str)
        {
            return new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private static string ReverseString(string str)
        {
            char[] charArray = new char[str.Length];
            int j = 0;

            for (int i = str.Length - 1; i > -1; i--)
            {
                charArray[j++] = str[i];
            }

            return new string(charArray);
        }
 
    }

}
