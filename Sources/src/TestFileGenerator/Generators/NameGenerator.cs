using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestFileGenerator.Generators
{
    // TODO: try to find more robust solution to generate names based on Markov Chain.
    
    /// <summary>
    /// Implementation of the custom name generator based on input file with syllables.
    /// </summary>
    /// <remarks>
    /// Found here:
    ///  - 1. http://www.cyberforum.ru/csharp-beginners/thread1652657.html
    ///  - 2. http://forum.codecall.net/topic/49665-java-random-name-generator/
    /// Original implementation on Java.
    /// </remarks>
    public sealed class NameGenerator
    {
        List<string> _pre = new List<string>();
        List<string> _mid = new List<string>();
        List<string> _sur = new List<string>();

        private static char[] _vowels = {'a', 'e', 'i', 'o', 'u', 'ä', 'ö', 'õ', 'ü', 'y'};

        private static char[] _consonants =
            {'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y'};

        Random _rnd = new Random();
        private string _fileName;

        /// <summary>
        /// Create new random name generator object. refresh() is automatically called.
        /// </summary>
        /// <param name="fileName">fileName insert file name, where syllables are located</param>
        public NameGenerator(string fileName)
        {
            _fileName = fileName;
            Refresh();
        }

        public NameGenerator(TextReader reader)
        {
            Load(reader);
        }

        /// <summary>
        /// Refresh names from file. No need to call that method, if you are not changing the file during the operation of program, as this method
        /// is called every time file name is changed or new NameGenerator object created.
        /// </summary>
        public void Refresh()
        {
            if (_fileName == null) return;

            using (var reader = new StreamReader(_fileName))
            {
                Load(reader);
            }
        }

        private void Load(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    if (line[0] == '-')
                        _pre.Add(line.Substring(1).ToLower());
                    else if (line[0] == '+')
                        _sur.Add(line.Substring(1).ToLower());
                    else
                        _mid.Add(line.ToLower());
                }
            }
        }

        private string Upper(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        private bool ContainsConsFirst(List<string> array)
        {
            foreach (string s in array)
            {
                if (ConsonantFirst(s)) return true;
            }

            return false;
        }

        private bool ContainsVocFirst(List<string> array)
        {
            foreach (string s in array)
            {
                if (VowelFirst(s)) return true;
            }

            return false;
        }

        private bool AllowCons(List<string> array)
        {
            foreach (string s in array)
            {
                if (HatesPreviousVowels(s) || !HatesPreviousConsonants(s)) return true;
            }

            return false;
        }

        private bool AllowVocs(List<string> array)
        {
            foreach (string s in array)
            {
                if (HatesPreviousConsonants(s) || HatesPreviousVowels(s) == false) return true;
            }

            return false;
        }

        private bool ExpectsVowel(string s)
        {
            if (s.Substring(1).Contains("+v")) return true;
            else return false;
        }

        private bool ExpectsConsonant(string s)
        {
            if (s.Substring(1).Contains("+c")) return true;
            else return false;
        }

        private bool HatesPreviousVowels(string s)
        {
            if (s.Substring(1).Contains("-c")) return true;
            else return false;
        }

        private bool HatesPreviousConsonants(string s)
        {
            if (s.Substring(1).Contains("-v")) return true;
            else return false;
        }

        private string PureSyl(string s)
        {
            s = s.Trim();
            if (s[0] == '+' || s[0] == '-') s = s.Substring(1);
            return s.Split(' ')[0];
        }

        private bool VowelFirst(string s)
        {
            return _vowels.Contains(char.ToLower(s[0]));
        }

        private bool ConsonantFirst(string s)
        {
            return _consonants.Contains(char.ToLower(s[0]));
        }

        private bool VowelLast(string s)
        {
            return _vowels.Contains(char.ToLower(s[s.Length - 1]));
        }

        private bool ConsonantLast(string s)
        {
            return _consonants.Contains(char.ToLower(s[s.Length - 1]));
        }

        /// <summary>
        /// Compose a new name.
        /// </summary>
        /// <param name="syls">The number of syllables used in name.</param>
        /// <returns>Returns composed name as a string</returns>
        /// <exception cref="ApplicationException">when logical mistakes are detected inside chosen file, and program is unable to complete the name.</exception>
        public string Compose(int syls)
        {
            if (syls > 2 && _mid.Count == 0)
                throw new ApplicationException(
                    "You are trying to create a name with more than 3 parts, which requires middle parts, which you have none in the file " +
                    _fileName +
                    ". You should add some. Every word, which doesn't have + or - for a prefix is counted as a middle part.");
            if (_pre.Count == 0)
                throw new ApplicationException("You have no prefixes to start creating a name. add some and use " +
                                               "prefix, to identify it as a prefix for a name. (example: -asd)");
            if (_sur.Count == 0)
                throw new ApplicationException("You have no suffixes to end a name. add some and use " +
                                               " prefix, to identify it as a suffix for a name. (example: +asd)");
            if (syls < 1) throw new ApplicationException("compose(int syls) can't have less than 1 syllable");
            int expecting = 0; // 1 for Vowel, 2 for consonant
            int last = 0; // 1 for Vowel, 2 for consonant
            string name;
            int a = (int) (_rnd.NextDouble() * _pre.Count);

            if (VowelLast(PureSyl(_pre[a]))) last = 1;
            else last = 2;

            if (syls > 2)
            {
                if (ExpectsVowel(_pre[a]))
                {
                    expecting = 1;
                    if (ContainsVocFirst(_mid) == false)
                        throw new ApplicationException("Expecting 'middle' part starting with Vowel, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }

                if (ExpectsConsonant(_pre[a]))
                {
                    expecting = 2;
                    if (ContainsConsFirst(_mid) == false)
                        throw new ApplicationException("Expecting 'middle' part starting with consonant, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }
            }
            else
            {
                if (ExpectsVowel(_pre[a]))
                {
                    expecting = 1;
                    if (ContainsVocFirst(_sur) == false)
                        throw new ApplicationException("Expecting 'suffix' part starting with Vowel, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }

                if (ExpectsConsonant(_pre[a]))
                {
                    expecting = 2;
                    if (ContainsConsFirst(_sur) == false)
                        throw new ApplicationException("Expecting 'suffix' part starting with consonant, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }
            }

            if (VowelLast(PureSyl(_pre[a])) && AllowVocs(_mid) == false)
                throw new ApplicationException(
                    "Expecting 'middle' part that allows last character of prefix to be a Vowel, " +
                    "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the prefix used, was : " +
                    _pre[a] + ", which" +
                    "means there should be a part available, that has 'v' requirement or no requirements for previous syllables at all.");

            if (ConsonantLast(PureSyl(_pre[a])) && AllowCons(_mid) == false)
                throw new ApplicationException(
                    "Expecting 'middle' part that allows last character of prefix to be a consonant, " +
                    "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the prefix used, was : " +
                    _pre[a] + ", which" +
                    "means there should be a part available, that has 'c' requirement or no requirements for previous syllables at all.");

            int[] b = new int[syls];
            for (int i = 0; i < b.Length - 2; i++)
            {
                do
                {
                    b[i] = (int) (_rnd.NextDouble() * _mid.Count);
                    //System.out.println("exp " +expecting+" VowelF:"+VowelFirst(mid.get(b[i]))+" syl: "+mid.get(b[i]));
                } while (expecting == 1 && VowelFirst(PureSyl(_mid[b[i]])) == false || expecting == 2 &&
                                                                                    ConsonantFirst(
                                                                                        PureSyl(_mid[b[i]])) ==
                                                                                    false
                                                                                    || last == 1 &&
                                                                                    HatesPreviousVowels(_mid[b[i]]) ||
                                                                                    last == 2 &&
                                                                                    HatesPreviousConsonants(_mid[b[i]])
                );

                expecting = 0;
                if (ExpectsVowel(_mid[b[i]]))
                {
                    expecting = 1;
                    if (i < b.Length - 3 && ContainsVocFirst(_mid) == false)
                        throw new ApplicationException("Expecting 'middle' part starting with Vowel, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                    if (i == b.Length - 3 && ContainsVocFirst(_sur) == false)
                        throw new ApplicationException("Expecting 'suffix' part starting with Vowel, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }

                if (ExpectsConsonant(_mid[b[i]]))
                {
                    expecting = 2;
                    if (i < b.Length - 3 && ContainsConsFirst(_mid) == false)
                        throw new ApplicationException("Expecting 'middle' part starting with consonant, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                    if (i == b.Length - 3 && ContainsConsFirst(_sur) == false)
                        throw new ApplicationException("Expecting 'suffix' part starting with consonant, " +
                                                       "but there is none. You should add one, or remove requirement for one.. ");
                }

                if (VowelLast(PureSyl(_mid[b[i]])) && AllowVocs(_mid) == false && syls > 3)
                    throw new ApplicationException(
                        "Expecting 'middle' part that allows last character of last syllable to be a Vowel, " +
                        "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the part used, was : " +
                        _mid[b[i]] + ", which " +
                        "means there should be a part available, that has 'v' requirement or no requirements for previous syllables at all.");

                if (ConsonantLast(PureSyl(_mid[b[i]])) && AllowCons(_mid) == false && syls > 3)
                    throw new ApplicationException(
                        "Expecting 'middle' part that allows last character of last syllable to be a consonant, " +
                        "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the part used, was : " +
                        _mid[b[i]] + ", which " +
                        "means there should be a part available, that has 'c' requirement or no requirements for previous syllables at all.");
                if (i == b.Length - 3)
                {
                    if (VowelLast(PureSyl(_mid[b[i]])) && AllowVocs(_sur) == false)
                        throw new ApplicationException(
                            "Expecting 'suffix' part that allows last character of last syllable to be a Vowel, " +
                            "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the part used, was : " +
                            _mid[b[i]] + ", which " +
                            "means there should be a suffix available, that has 'v' requirement or no requirements for previous syllables at all.");

                    if (ConsonantLast(PureSyl(_mid[b[i]])) && AllowCons(_sur) == false)
                        throw new ApplicationException(
                            "Expecting 'suffix' part that allows last character of last syllable to be a consonant, " +
                            "but there is none. You should add one, or remove requirements that cannot be fulfilled.. the part used, was : " +
                            _mid[b[i]] + ", which " +
                            "means there should be a suffix available, that has 'c' requirement or no requirements for previous syllables at all.");
                }

                if (VowelLast(PureSyl(_mid[b[i]]))) last = 1;
                else last = 2;
            }

            int c;
            do
            {
                c = (int) (_rnd.NextDouble() * _sur.Count);
            } while (expecting == 1 && VowelFirst(PureSyl(_sur[c])) == false || expecting == 2 &&
                                                                             ConsonantFirst(PureSyl(_sur[c])) == false
                                                                             || last == 1 &&
                                                                             HatesPreviousVowels(_sur[c]) ||
                                                                             last == 2 &&
                                                                             HatesPreviousConsonants(_sur[c]));

            name = Upper(PureSyl(_pre[a].ToLower()));
            for (int i = 0; i < b.Length - 2; i++)
            {
                name += PureSyl(_mid[b[i]].ToLower());
            }

            if (syls > 1)
                name += PureSyl(_sur[c].ToLower());
            return name;
        }
    }
}