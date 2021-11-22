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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProfanityDetector.Interfaces
{
    public interface IProfanityFilter
    {
        bool IsProfanity(string word);
        ReadOnlyCollection<string> DetectAllProfanities(string sentence);
        ReadOnlyCollection<string> DetectAllProfanities(string sentence, bool removePartialMatches);
        bool ContainsProfanity(string term);
        
        IAllowList AllowList { get; }
        string CensorString(string sentence);
        string CensorString(string sentence, char censorCharacter);
        string CensorString(string sentence, char censorCharacter, bool ignoreNumbers);
        (int, int, string)? GetCompleteWord(string toCheck, string profanity);

        void AddProfanity(string profanity);
        void AddProfanity(string[] profanityList);
        void AddProfanity(List<string> profanityList);

        bool RemoveProfanity(string profanity);
        bool RemoveProfanity(List<string> profanities);
        bool RemoveProfanity(string [] profanities);

        void Clear();

        int Count { get; }
    }
}