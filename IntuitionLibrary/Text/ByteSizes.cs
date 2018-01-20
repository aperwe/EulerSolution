using System;

namespace QBits.Intuition.Text
{
    /// <summary>
    /// Supported sizes for string formatting.
    /// </summary>
    public enum ByteSizes : long
    {
        KiloUnit = 1024,
        /// <summary>
        /// One kilobyte (KB). Equal to 1024 bytes.
        /// </summary>
        Kilobyte = KiloUnit,
        /// <summary>
        /// One megabyte (MB). Equal to 1024 kilobytes.
        /// </summary>
        Megabyte = Kilobyte * KiloUnit,
        /// <summary>
        /// One gigabyte (GB). Equal to 1024 gigabytes.
        /// </summary>
        Gigabyte = Megabyte * KiloUnit,
        /// <summary>
        /// One terabyte (TB). Equal to 1024 gigabytes.
        /// </summary>
        Terabyte = Gigabyte * KiloUnit,
        /// <summary>
        /// One petabyte (PB). Equal to 1024 terabytes.
        /// </summary>
        Petabyte = Terabyte * KiloUnit,
    }
}
