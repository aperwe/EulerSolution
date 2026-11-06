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
        /// A file name without a folder is saved in the current folder.
        /// </summary>
        /// <param name="dbFileName">Name of the settings database file.</param>
        /// <param name="me">Dataset to which settings are serialized/deseralized.</param>
        public static void SaveData(this DataSet me, string dbFileName)
        {
            if (me == null) return;
            var dbDir = System.IO.Path.GetDirectoryName(dbFileName)
                ?? throw new ArgumentNullException(nameof(dbFileName), $"Path '{dbFileName}' does not name a folder.");
            if (dbDir.Length > 0 && !System.IO.Directory.Exists(dbDir)) //An empty folder name means the current folder, which always exists.
            {
                System.IO.Directory.CreateDirectory(dbDir);
            }
            me.WriteXml(dbFileName);
        }
    }
}
