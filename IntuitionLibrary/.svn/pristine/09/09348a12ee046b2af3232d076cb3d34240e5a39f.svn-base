using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace QBits.Intuition.Text
{
    /// <summary>
    /// Converts amounts of bytes (such as disk capacities or RAM allocations) into string format representation,
    /// where - depending on the amount of bytes - Gigabytes, Terabytes, etc. are used instead of pure numeric values of billions of bytes.
    /// Sizes up to petabytes (PB) are supported.
    /// </summary>
    public static class ByteSizeConverter
    {
        /// <summary>
        /// Converts amounts of bytes (such as disk capacities or RAM allocations) into string format representation,
        /// where - depending on the amount of bytes - Gigabytes, Terabytes, etc. are used instead of pure numeric values of billions of bytes.
        /// Sizes up to petabytes (PB) are supported.
        /// For example, for a value of 1024, 1.00 KB is returned.
        /// <para/>CurrentCulture is used for string conversion.
        /// </summary>
        public static string ConvertByteSizeToString(long byteSize)
        {
            return ConvertByteSizeToString(byteSize, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts amounts of bytes (such as disk capacities or RAM allocations) into string format representation,
        /// where - depending on the amount of bytes - Gigabytes, Terabytes, etc. are used instead of pure numeric values of billions of bytes.
        /// Sizes up to petabytes (PB) are supported.
        /// For example, for a value of 1024, 1.00 KB is returned.
        /// </summary>
        public static string ConvertByteSizeToString(long byteSize, CultureInfo culture)
        {
            if (byteSize > (long)ByteSizes.Petabyte)
            {
                var size = (byteSize / (double)(long)ByteSizes.Petabyte);
                return string.Format(culture, "{0:n} PB", size);
            }
            if (byteSize > (long)ByteSizes.Terabyte)
            {
                var size = (byteSize / (double)(long)ByteSizes.Terabyte);
                return string.Format(culture, "{0:n} TB", size);
            }
            if (byteSize > (long)ByteSizes.Gigabyte)
            {
                var size = (byteSize / (double)(long)ByteSizes.Gigabyte);
                return string.Format(culture, "{0:n} GB", size);
            }
            if (byteSize > (long)ByteSizes.Megabyte)
            {
                var size = (byteSize / (double)(long)ByteSizes.Megabyte);
                return string.Format(culture, "{0:n} MB", size);
            }
            if (byteSize > (long)ByteSizes.Kilobyte)
            {
                var size = (byteSize / (double)(long)ByteSizes.Kilobyte);
                return string.Format(culture, "{0:n} KB", size);
            }
            if (byteSize == 1) return string.Format(culture, "{0:n} byte", byteSize); //Special (singular) case for 1 byte.
            return string.Format(culture, "{0:n} bytes", byteSize); //Regular (plural) case for x: x != 1 bytes.
        }
    }
}
