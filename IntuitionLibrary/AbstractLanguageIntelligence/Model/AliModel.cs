using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model
{
    /// <summary>
    /// A basic model used in ALI. Other models kinds should derive from this class,
    /// as this is a base implementation of all models.
    /// </summary>
    public class AliModel
    {
        private static AliModel ALIModelGeneratorSpecialized()
        {
            AliModel retVal = new AliModel();
            //Initialize and maintain our context. ALIContext will create its own thread.
            retVal.context = new Context();
            return retVal;
        }
        Context context = null;
        /// <summary>
        /// Method called to register default (built-in) model types.
        /// </summary>
        public static void RegisterDefaultModels()
        {
            ModelFactory.AddModelGenerator("AliModel", ALIModelGeneratorSpecialized);
            //Add further models here if you develop any.
            //They will be registered when the application starts.
        }
    }
}
