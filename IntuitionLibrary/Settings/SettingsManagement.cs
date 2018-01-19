using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QBits.Intuition.Settings
{
    /// <summary>
    /// Extension methods for managing settings files for an application.
    /// </summary>
    public static class SettingsManagement
    {
        /// <summary>
        /// Persists the data from database on disk to the file pointed to by <paramref name="dbFileName"/>.
        /// </summary>
        /// <param name="dbFileName">Name of the settings database file.</param>
        /// <param name="me">Dataset to which settings are serialized/deseralized.</param>
        public static void SaveData(this DataSet me, string dbFileName)
        {
            if (me == null) return;
            var dbDir = System.IO.Path.GetDirectoryName(dbFileName);
            if (!System.IO.Directory.Exists(dbDir))
            {
                System.IO.Directory.CreateDirectory(dbDir);
            }
            me.WriteXml(dbFileName);
        }
    }
}
