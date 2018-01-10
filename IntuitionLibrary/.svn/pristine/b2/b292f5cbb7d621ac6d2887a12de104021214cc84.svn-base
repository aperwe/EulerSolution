using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model
{
    /// <summary>
    /// Factory class that is to be used to create instances of 
    /// various ALI models.
    /// (Currently we support only one ALIModel.)
    /// </summary>
    public class ModelFactory
    {
        #region Model factory stuff
        /// <summary>
        /// Currently we know only one model, so no parameters are required,
        /// and we are always returning ALIModel object specifically.
        /// </summary>
        /// <returns></returns>
        public static AliModel GenerateModel(string modelType)
        {
            return SAP._GenerateModelInternal(modelType);
        }
        private AliModel _GenerateModelInternal(string modelType)
        {
            return modelGenerators[modelType]();
        }
        /// <summary>
        /// Returns the singleton access point to the model factory.
        /// </summary>
        public static ModelFactory SAP
        {
            get
            {
                if (_SAP == null) _SAP = new ModelFactory();
                return _SAP;
            }
        }
        /// <summary>
        /// Private field.
        /// </summary>
        static ModelFactory _SAP;
        /// <summary>
        /// Type of the method that knows how to construct a model type.
        /// </summary>
        /// <returns>Model of a specific type</returns>
        public delegate AliModel ModelGeneratorDelegate();
        Dictionary<string, ModelGeneratorDelegate> modelGenerators = new Dictionary<string, ModelGeneratorDelegate>();
        /// <summary>
        /// Registers a factory method for generating the specified model type.
        /// </summary>
        /// <param name="modelType">Recognized type of models</param>
        /// <param name="generator">Method delegate that knows how to construct this type of model</param>
        public static void AddModelGenerator(string modelType, ModelGeneratorDelegate generator)
        {
            SAP.modelGenerators.Add(modelType, generator);
        }
        #endregion
    }
}
