// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

class Unscramble
{
     private static bool _dictionaryLoaded = false;
     private static string _wordToUnscramble = "";
     private static int _totalEntries = 0;
     private static Dictionary<string, string> _sortedDictionary =
                                       new Dictionary<string, string>();
     private static List<string> _results = new List<string>();
     private static Stopwatch _stopwatch;

     //====================================================================================
     /** We don't really need a constructor **/
     //public Unscramble(string wordToUnscramble)
     //{
     //    _WordToUnscramble = wordToUnscramble;
     //}

     //====================================================================================
     public List<string> UnscrambleWord(string wordToUnscramble, bool useFiltering = true)
     {
         _stopwatch = Stopwatch.StartNew();
 if (string.IsNullOrEmpty(_wordToUnscramble))
         {
             _wordToUnscramble = wordToUnscramble;
         }
         else if (!_wordToUnscramble.Equals
              (wordToUnscramble, StringComparison.OrdinalIgnoreCase) && useFiltering)
         {   //If re-using the object and the word is different,
             //we'll need to reload the dictionary
             _dictionaryLoaded = false;
             _wordToUnscramble = wordToUnscramble;
             _results.Clear();
         }
         else if (_wordToUnscramble.Equals
                 (wordToUnscramble, StringComparison.OrdinalIgnoreCase))
         {
             _results.Clear(); //we should clear the results array so they don't stack
         }

         if (!_dictionaryLoaded) //the first call will be slightly slower
             LoadEmbeddedDictionary(wordToUnscramble.ToUpper(), useFiltering);

         string scrambleSorted = SortWord(wordToUnscramble);

         //var kvp = SortedDictionary.FirstOrDefault
         //(p => SortedDictionary.Comparer.Equals(p.Value, scrambledSort));
         var matchList = _sortedDictionary.Where
             (kvp => kvp.Value == scrambleSorted).Select(kvp => kvp.Key).ToList();

         if (matchList.Count > 0)
         {
             foreach (string result in matchList)
             {
                 System.Diagnostics.Debug.WriteLine($"> Match: {result}");
                 _results.Add(result);
                 }

             _stopwatch.Stop();
             System.Diagnostics.Debug.WriteLine($"> Elapsed time: {_stopwatch.Elapsed}");
             return _results;
         }
         else //no matches
         {
             _stopwatch.Stop();
             _results.Clear();
             System.Diagnostics.Debug.WriteLine($"> Elapsed time: {_stopwatch.Elapsed}");
             return _results;
         }
     }

     //==================================================================================
     private static void LoadEmbeddedDictionary(string wordText, bool filter = false)
     {
         char[] delims = new char[1] { '\n' };
         string[] chunks;
         int chunkCount = 0;
         if (filter)
             chunks = global::Utility.Properties.Resources.
                                      DictionaryNums.ToUpper().Split(delims);
         else
             chunks = global::Utility.Properties.Resources.
                                      DictionaryNums.ToUpper().Split(delims);

         System.Diagnostics.Debug.WriteLine($"> Length filter: {wordText.Length}");
         _sortedDictionary.Clear();
         foreach (string str in chunks)
         {
             chunkCount++;
             if (filter)
             {
                 //we're assuming the word will have at least 3 characters...
                 //I mean would you really need this program if it was only two?
                 if ((str.Length == wordText.Length) &&
                      str.Contains(wordText.Substring(0, 1)) &&
                      str.Contains(wordText.Substring(1, 1)) &&
                      str.Contains(wordText.Substring(2, 1))) //just checking the 1st,
                                    //2nd & 3rd letter will trim our search considerably
                 {
                     try
                     {
                         _sortedDictionary.Add(str, SortWord(str));
                     }
                     catch
                     {
                         //probably a key collision, just ignore
                     }
                 }
             }
             else
             {
                 try
                 {
                     _sortedDictionary.Add(str, SortWord(str));
                 }
                 catch
                 {
                     //probably a key collision, just ignore
                 }
                   }
             }
         }
         System.Diagnostics.Debug.WriteLine($">
         Loaded {_sortedDictionary.Count} possible matches out of {chunkCount.ToString()}");
         _totalEntries = chunkCount;
         _dictionaryLoaded = true;
     }

     //=================================================================================
     private static string SortWord(string str)
     {
         return String.Concat(str.OrderBy(c => c));

         /*** Character Array Method ***
         return String.Concat(str.OrderBy(c => c).ToArray());
         *******************************/

         /*** Traditional Method ***
         char[] chars = input.ToArray();
         Array.Sort(chars);
         return new string(chars);
         ***************************/
     }

     #region [Helper Methods]
     //=================================================================================
     public TimeSpan GetMatchTime()
     {
        return _stopwatch.Elapsed;
     }
     //=================================================================================
     public List<string> GetMatchResults()
     {
        return _results;
     }

     //=================================================================================
     public int GetMatchCount()
     {
        return _results.Count;
     }

     //=================================================================================
     public int GetFilterCount()
     {
        return _sortedDictionary.Count;
     }

     //=================================================================================
     public int GetDictionaryCount()
     {
        return _totalEntries;
     }
     #endregion
}
string scrambled = "mctmouicnaino";
Unscramble obj1 = new Unscramble();
List<string> results = obj1.UnscrambleWord(scrambled);
if (results.Count > 0)
{
    Console.WriteLine($"> Total matches: {obj1.GetMatchCount()}");
    foreach (string str in results)
    {
        Console.WriteLine($">> {str}");
    }
    Console.WriteLine($"> Total time: {obj1.GetMatchTime()}");
    Console.WriteLine($"> Filtered set: {obj1.GetFilterCount()}
                          out of {obj1.GetDictionaryCount()}");
}
else
{
    Console.WriteLine("> No matches available:
            Check your spelling, or the dictionary may be missing this word.");
}
