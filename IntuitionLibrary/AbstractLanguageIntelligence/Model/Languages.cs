using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model
{
    enum Language
    {
        /// <summary>
        /// Not a human language. This is the 'concept' language of the ALI brain.
        /// To see a human-readable version, it has to be 'narrowed' to of the specific languages.
        /// </summary>
        /// <remarks>This is the default value for internal representations of concepts and meanings.</remarks>
        AbstractALI,
        English,
        Polish,
    }
}
