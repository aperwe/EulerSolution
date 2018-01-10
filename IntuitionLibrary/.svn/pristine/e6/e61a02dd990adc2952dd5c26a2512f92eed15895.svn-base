using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model
{
    /// <summary>
    /// Concept is a meta class defining the meaning language-independently.
    /// </summary>
    /// <remarks>
    /// Concept is what we are talking about currently.
    /// It should contain its 'source', which would be the input data (currently - simple string) in the source language.
    /// For example, when operator wants to translate sentence SA="This is a dog.", 
    /// he inputs the SA and SA is marked to be in English language, 
    /// then the generated IALIConcept object would - except for 
    /// its internal generated representation - contain its source "This is a dog." 
    /// along with the language designator of "English".
    /// </remarks>
    interface IConcept
    {
        IConcept NarrowToLanguage(Language targetLanguage);
    }
}
