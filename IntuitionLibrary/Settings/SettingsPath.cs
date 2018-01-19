using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Settings
{
    /// <summary>
    /// Object that manages access to settings folder of your application.
    /// By default the settings are stored in %APPDATA% (preferred), but you can choose a different storage.
    /// </summary>
    public class SettingsPath
    {
        #region Private members
        string Product;
        string InitialPath;
        string Company;
        string DefaultFile;
        bool DefaultFileSpecified;
        #endregion

        /// <summary>
        /// Constructor that espects - by default - to have initialRootPath to be specified to point to %APPDATA%.
        /// </summary>
        /// <param name="product">Name of the product, whose settings you want to have managed.</param>
        /// <param name="initialRootPath">By default, this is %APPDATA%.</param>
        /// <param name="defaultFile">Optional name of the default file, if you are using only a single settings file.</param>
        public SettingsPath(string product, string initialRootPath = "%APPDATA%", string company = "Q-Bits", string defaultFile = null)
        {
            Product = product;
            InitialPath = initialRootPath;
            Company = company;
            DefaultFileSpecified = defaultFile != null;
            DefaultFile = defaultFile;
        }
        /// <summary>
        /// Path for settings for this app in %APPDATA% folder. Default file name is not included.
        /// </summary>
        public string RootSettingsPath
        {
            get
            {
                var initialPath = Environment.ExpandEnvironmentVariables(InitialPath);
                var rootPath = System.IO.Path.Combine(initialPath, Company, Product);
                return rootPath;
            }
        }

        /// <summary>
        /// Full path to a default file, if you are interested in the location of that single file only. Must be specified when this object is constructed.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if this object has been constructed with defaultFile argument not set.</exception>
        public string DefaultFilePath
        {
            get
            {
                if (!DefaultFileSpecified) throw new ApplicationException("Default path has not been specified.");
                return System.IO.Path.Combine(RootSettingsPath, DefaultFile);
            }
        }
        /// <summary>
        /// Gets full path to the specified file that should be located under the <see cref="RootSettingsPath"/>.
        /// </summary>
        /// <param name="settingsFile">Relative name of the file. Will be combined with <see cref="RootSettingsPath"/>.</param>
        /// <returns>Full path to the specified file, under the <see cref="RootSettingsPath"/>.</returns>
        public string GetFilePath(string settingsFile)
        {
            return System.IO.Path.Combine(RootSettingsPath, settingsFile);
        }
    }
}
