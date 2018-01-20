using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QBits.Intuition.Settings
{
    public static class SettingsManagement
    {
        /// <summary>
        /// Persists the data from database on disk to the file pointed to by <see cref="dbFileName"/>.
        /// </summary>
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
