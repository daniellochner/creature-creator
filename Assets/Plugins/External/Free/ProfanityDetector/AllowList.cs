/*
MIT License
Copyright (c) 2019 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ProfanityDetector.Interfaces;

namespace ProfanityDetector
{
    public class AllowList : IAllowList
    {
        List<string> _allowList;

        public AllowList()
        {
            _allowList = new List<string>();
        }

        /// <summary>
        /// Return an instance of a read only collection containing allow list
        /// </summary>
        public ReadOnlyCollection<string> ToList
        {
            get
            {
                return new ReadOnlyCollection<string>(_allowList);
            }
        }

        /// <summary>
        /// Add a word to the profanity allow list. This means a word that is in the allow list
        /// can be ignored. All words are treated as case insensitive.
        /// </summary>
        /// <param name="wordToAllowlist">The word that you want to allow list.</param>
        public void Add(string wordToAllowlist)
        {
            if (string.IsNullOrEmpty(wordToAllowlist))
            {
                throw new ArgumentNullException(nameof(wordToAllowlist));
            }

            if (!_allowList.Contains(wordToAllowlist.ToLower(CultureInfo.InvariantCulture)))
            {
                _allowList.Add(wordToAllowlist.ToLower(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordToCheck"></param>
        /// <returns></returns>
        public bool Contains(string wordToCheck)
        {
            if (string.IsNullOrEmpty(wordToCheck))
            {
                throw new ArgumentNullException(nameof(wordToCheck));
            }

            return _allowList.Contains(wordToCheck.ToLower(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Return the number of items in the allow list.
        /// </summary>
        /// <returns>The number of items in the allow list.</returns>
        public int Count
        {
            get
            {
                return _allowList.Count;
            }
        }

        /// <summary>
        /// Remove all words from the allow list.
        /// </summary>  
        public void Clear()
        {
            _allowList.Clear();
        }

        /// <summary>
        /// Remove a word from the profanity allow list. All words are treated as case insensitive.
        /// </summary>
        /// <param name="wordToRemove">The word that you want to use</param>
        /// <returns>True if the word is successfuly removes, False otherwise.</returns>
        public bool Remove(string wordToRemove)
        {
            if (string.IsNullOrEmpty(wordToRemove))
            {
                throw new ArgumentNullException(nameof(wordToRemove));
            }

            return _allowList.Remove(wordToRemove.ToLower(CultureInfo.InvariantCulture));
        }
    }
}
