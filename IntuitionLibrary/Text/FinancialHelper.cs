using System;
using System.Text;

namespace QBits.Intuition.Text
{
    /// <summary>
    /// Helper methods for working with financial strings.
    /// </summary>
    public static class FinancialHelper
    {
        /// <summary>
        /// Maximum number of supported digits.
        /// </summary>
        public const int MAX_DIGS = 10;
        /// <summary>
        /// Base for textual representation - should be 10.
        /// </summary>
        public const int nBase = 10;
        /// <summary>
        /// Converts a double value to textual currency string.
        /// <para/>For example:
        /// <para/>dwa tysiące sto osiemnaście złotych 48/100
        /// <para/>Maximum supported digits is: 8 (hudreds of millions); positive or negative values.
        /// <para/>Supported languages: Polish.
        /// </summary>
        public static string ConvertQuotaToText(double quotaToConvert)
        {
            QuotaTransformer transformer = new QuotaTransformer(quotaToConvert, 10);
            transformer.Restart();

            //#region Tens of billions 10^10
            //transformer.SwitchToNextDigit();
            //transformer.ProcessTensOf("miliardów");
            //#endregion

            #region Billions 10^9
            transformer.SwitchToNextDigit();
            transformer.ProcessOnesOf("miliard", "miliardy", "miliardów");
            #endregion

            #region Hundreds of millions 10^8
            transformer.SwitchToNextDigit();
            transformer.ProcessHundreds();
            #endregion

            #region Tens of millions 10^7
            transformer.SwitchToNextDigit();
            transformer.ProcessTensOf("milionów");
            #endregion

            #region Millions 10^6
            transformer.SwitchToNextDigit();
            transformer.ProcessOnesOf("milion", "miliony", "milionów");
            #endregion

            #region Hundreds of thousands 10^5
            transformer.SwitchToNextDigit();
            transformer.ProcessHundreds();
            #endregion

            #region Tenths of thousands 10^4
            transformer.SwitchToNextDigit();
            transformer.ProcessTensOf("tysięcy");
            #endregion

            #region Thousands 10^3
            transformer.SwitchToNextDigit();
            transformer.ProcessOnesOf("tysiąc", "tysiące", "tysięcy");
            #endregion

            #region Hundreds 10^2
            transformer.SwitchToNextDigit();
            transformer.ProcessHundreds();
            #endregion

            #region Tens 10^1
            transformer.SwitchToNextDigit();
            transformer.ProcessTensOf("złotych");
            #endregion

            #region Ones 10^0
            transformer.SwitchToNextDigit();
            transformer.ProcessCustomOnesOf("złoty", "złote", "złotych");
            #endregion

            #region Add 'złotych' string?
            if (!transformer.SkipNextDigit)
            {
            }
            else
            {
                transformer.SkipNextDigit = false;
            }
            #endregion

            #region Add groszy's part
            transformer.SwitchToNextDigit();
            transformer.AppendToCurrentPart(string.Format(" {0}/100", transformer.LiczbaGroszy));
            transformer.AppendCurrentPart();
            #endregion

            return transformer.QuotaString;
        }
    }
    /// <summary>
    /// Class used by <see cref="FinancialHelper.ConvertQuotaToText(double)"/> to store context while converting number to string.
    /// </summary>
    internal class QuotaTransformer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="quota">Quota to convert</param>
        /// <param name="maxDigitsSupported">Indicates how many digits the calling code supports.</param>
        internal QuotaTransformer(double quota, int maxDigitsSupported)
        {
            Quota = quota;
            MaxDigitsSupported = maxDigitsSupported;
            QuotaTextBuilder = new StringBuilder();
            FixQuotaSign();
            LiczbaZłotych = (int)Quota;
            LiczbaGroszy = (int)((Quota - LiczbaZłotych) * 100.05);
            AnyStringAlreadySet = false;
            CurrentUnitsPresent = false;
            DigitsArray = new int[FinancialHelper.MAX_DIGS];
            FillDecimalArray();
        }
        /// <summary>
        /// Makes sure the converted quota is always positive.
        /// <para/>Stores the negative sign in a flag for reference.
        /// </summary>
        private void FixQuotaSign()
        {
            if (Quota < 0.0)
            {
                QuotaIsNegative = true;
                Quota = -Quota;
            }
            if (QuotaIsNegative)
            {
                Append("minus");
            }
        }
        /// <summary>
        /// Fills the decimals array with all digits from the quota.
        /// </summary>
        private void FillDecimalArray()
        {
            int helperCounter = LiczbaZłotych;
            for (int fillIterator = FinancialHelper.MAX_DIGS - 1; fillIterator >= 0; fillIterator--)
            {
                int helperIterator = (int)(Math.Pow((double)FinancialHelper.nBase, fillIterator));
                while (helperCounter >= helperIterator)
                {
                    DigitsArray[fillIterator]++;
                    helperCounter -= helperIterator;
                }
            }
        }
        /// <summary>
        /// Input quota used to initialize this instance of QuotaTransformer.
        /// </summary>
        private double Quota { get; set; }
        /// <summary>
        /// Indicates to the Transformer how many digits (at maximum) are supposed to be handled.
        /// <para/>Digits after decimal place are not counted.
        /// </summary>
        private int MaxDigitsSupported { get; set; }
        /// <summary>
        /// Integer number of full złoty in the quota to be converted.
        /// </summary>
        internal int LiczbaZłotych { get; private set; }
        /// <summary>
        /// Integer number of full grosz in the quota to be converted.
        /// </summary>
        internal int LiczbaGroszy { get; private set; }
        /// <summary>
        /// Indicates that the original quota used to initialize this instance of QuotaTransformer was negative.
        /// </summary>
        bool QuotaIsNegative { get; set; }
        /// <summary>
        /// Working copy of the string.
        /// </summary>
        internal StringBuilder QuotaTextBuilder;
        /// <summary>
        /// Returns complete string of the quota (use it after conversion is complete, or the string will be partial).
        /// </summary>
        internal string QuotaString { get { return QuotaTextBuilder.ToString().Trim(); } }
        /// <summary>
        /// Indicates that at least one part of the partial quota string has been already set.
        /// <para/>Used to determine whether to add a trailing portion, like "... złotych" or "... tysięcy" after a few '0' digits.
        /// </summary>
        internal bool AnyStringAlreadySet { get; private set; }
        /// <summary>
        /// Stores digits at consecutive decimal places.
        /// </summary>
        private int[] DigitsArray { get; set; }
        /// <summary>
        /// Gets the digit currently being processed.
        /// </summary>
        internal int CurrentDigit { get { return DigitsArray[DigitIterator]; } }
        /// <summary>
        /// Peeks the digit next to the one currently being processed.
        /// <para/>Used when determining the proper representation of '-naście' values, such as 'jedenaście', 'dwanaście', etc. where two digits determine the string.
        /// </summary>
        internal int PeekNextDigit { get { return DigitsArray[DigitIterator - 1]; } }
        /// <summary>
        /// Cross-call flag, reset at <see cref="ProcessHundreds()"/>, then set/tested at <see cref="ProcessTensOf(string)"/>, then set/tested at <see cref="ProcessOnesOf(string, string, string)"/>
        /// to determine whether to present a unit name when going to the next level unit processing.
        /// <para/>I.e. If this flag is reset at <see cref="ProcessHundreds()"/> and not set later either in <see cref="ProcessTensOf(string)"/> or <see cref="ProcessOnesOf(string, string, string)"/>
        /// then in <see cref="ProcessOnesOf(string, string, string)"/> the unit name will not be appended to quota string.
        /// <para/>This is to avoid situations, when a number such as: 16,000,001.00 would produce a string containing 'thousands' name as in Polish:
        /// <para/>"szesnaście milionów tysięcy jeden złotych 0/100". In this case, "tysięcy" should not be added.
        /// </summary>
        private bool CurrentUnitsPresent { get; set; }
        /// <summary>
        /// Backing field for CurrentPart property.
        /// </summary>
        string currentPart;
        /// <summary>
        /// Gets or sets currently processed string part of the quota.
        /// </summary>
        internal string CurrentPart
        {
            get { return currentPart; }
            set
            {
                currentPart = value;
                if (currentPart.Length > 0)
                {
                    //Set this flag only if non-empty string is assigned to CurrentPart
                    AnyStringAlreadySet = true;
                }
            }
        }
        /// <summary>
        /// Automatic iterator over digits of the quota as it is being processed.
        /// </summary>
        internal int DigitIterator { get; private set; }
        /// <summary>
        /// Flag that indicates whether the next part should be skipped, because it had been looked-ahead and processed by a previous processing step.
        /// For example in '-naście', where 10's portion is processed, it also consumes the singles digit to build a proper 'jedenaście', 'dwanaście', etc. string.
        /// </summary>
        internal bool SkipNextDigit { get; set; }

        #region API
        /// <summary>
        /// Appends next part of the quota string.
        /// </summary>
        /// <param name="stringPart">Next part to be appended to final quota string.</param>
        private void Append(string stringPart)
        {
            QuotaTextBuilder.Append(stringPart);
        }
        /// <summary>
        /// Appends a completed current part to the working quota string.
        /// </summary>
        internal void AppendCurrentPart()
        {
            Append(CurrentPart);
        }
        /// <summary>
        /// Assembles the current part by adding the specified string to it.
        /// <para/>Note: to have this string added to the working string, you need to call <see cref="AppendCurrentPart()"/>.
        /// </summary>
        /// <param name="newSegment">New segment to be added to current part.</param>
        internal void AppendToCurrentPart(string newSegment)
        {
            CurrentPart += newSegment;
        }
        /// <summary>
        /// Assembles the current part by adding the specified string to it.
        /// <para/>Note: to have this string added to the working string, you need to call <see cref="AppendCurrentPart()"/>.
        /// </summary>
        /// <param name="newSegmentFormat">Format string for the new segment to be added.</param>
        /// <param name="args">Arguments for the format string</param>
        private void AppendToCurrentPart(string newSegmentFormat, params object[] args)
        {
            AppendToCurrentPart(string.Format(newSegmentFormat, args));
        }
        /// <summary>
        /// After appending current part to the working quota string, call this method to begin processing the next digit.
        /// <para/>It also resets the current part string to be empty so that next part can be computed.
        /// </summary>
        internal void SwitchToNextDigit()
        {
            DigitIterator--;
            CurrentPart = string.Empty;
        }
        /// <summary>
        /// Restarts the internal state for processing of new quota.
        /// </summary>
        internal void Restart()
        {
            DigitIterator = MaxDigitsSupported;
            SkipNextDigit = false; //The first digit to process should be processed by default.
            CurrentPart = string.Empty;
        }
        /// <summary>
        /// Processes a position assuming it contains a number of hundreds of something (units, thousands, millions, billions, etc.)
        /// <para/>The caller must ensure the current digit is the correct 'hundreds of' digit.
        /// </summary>
        internal void ProcessHundreds()
        {
            CurrentUnitsPresent = false; //At hundreds of units (maximum before higher level units (i.e. thousands are higher than hundreds, but are processed as units of thousands)), reset the indicator for any such units being detected.
            if (!SkipNextDigit)
            {
                switch (CurrentDigit)
                {
                    case 0: { } break;
                    case 1: { AppendToCurrentPart(" sto"); CurrentUnitsPresent = true; } break;
                    case 2: { AppendToCurrentPart(" dwieście"); CurrentUnitsPresent = true; } break;
                    case 3: { AppendToCurrentPart(" trzysta"); CurrentUnitsPresent = true; } break;
                    case 4: { AppendToCurrentPart(" czterysta"); CurrentUnitsPresent = true; } break;
                    case 5: { AppendToCurrentPart(" pięćset"); CurrentUnitsPresent = true; } break;
                    case 6: { AppendToCurrentPart(" sześćset"); CurrentUnitsPresent = true; } break;
                    case 7: { AppendToCurrentPart(" siedemset"); CurrentUnitsPresent = true; } break;
                    case 8: { AppendToCurrentPart(" osiemset"); CurrentUnitsPresent = true; } break;
                    case 9: { AppendToCurrentPart(" dziewięćset"); CurrentUnitsPresent = true; } break;
                }
                AppendCurrentPart();
            }
            else
            {
                SkipNextDigit = false;
            }
        }
        /// <summary>
        /// Processes a position assuming it contains a number of tens of something (units, thousands, millions, billions, etc.).
        /// <para/>The caller must ensure the current digit is the correct 'tens of' digit.
        /// </summary>
        /// <param name="unitsMultiple">Name of the unit in multiple form (such as 5 of more 'units'). For example: 'tysięcy'.</param>
        internal void ProcessTensOf(string unitsMultiple)
        {
            if (!SkipNextDigit)
            {
                switch (CurrentDigit)
                {
                    case 0: { } break;
                    case 1:
                        {
                            #region Naście jednostek
                            switch (PeekNextDigit)
                            {
                                case 0: { AppendToCurrentPart(" dziesięć {0}", unitsMultiple); } break;
                                case 1: { AppendToCurrentPart(" jedenaście {0}", unitsMultiple); } break;
                                case 2: { AppendToCurrentPart(" dwanaście {0}", unitsMultiple); } break;
                                case 3: { AppendToCurrentPart(" trzynaście {0}", unitsMultiple); } break;
                                case 4: { AppendToCurrentPart(" czternaście {0}", unitsMultiple); } break;
                                case 5: { AppendToCurrentPart(" piętnaście {0}", unitsMultiple); } break;
                                case 6: { AppendToCurrentPart(" szesnaście {0}", unitsMultiple); } break;
                                case 7: { AppendToCurrentPart(" siedemnaście {0}", unitsMultiple); } break;
                                case 8: { AppendToCurrentPart(" osiemnaście {0}", unitsMultiple); } break;
                                case 9: { AppendToCurrentPart(" dziewiętnaście {0}", unitsMultiple); } break;
                            }
                            #endregion
                            SkipNextDigit = true;
                            CurrentUnitsPresent = true;
                        } break;
                    case 2: { AppendToCurrentPart(" dwadzieścia"); CurrentUnitsPresent = true; } break;
                    case 3: { AppendToCurrentPart(" trzydzieści"); CurrentUnitsPresent = true; } break;
                    case 4: { AppendToCurrentPart(" czterdzieści"); CurrentUnitsPresent = true; } break;
                    case 5: { AppendToCurrentPart(" pięćdziesiąt"); CurrentUnitsPresent = true; } break;
                    case 6: { AppendToCurrentPart(" sześćdziesiąt"); CurrentUnitsPresent = true; } break;
                    case 7: { AppendToCurrentPart(" siedemdziesiąt"); CurrentUnitsPresent = true; } break;
                    case 8: { AppendToCurrentPart(" osiemdziesiąt"); CurrentUnitsPresent = true; } break;
                    case 9: { AppendToCurrentPart(" dziewięćdziesiąt"); CurrentUnitsPresent = true; } break;
                }
                AppendCurrentPart();
            }
            else
            {
                SkipNextDigit = false;
            }
        }
        /// <summary>
        /// Processes a position assuming it contains a number of units (złotys, thousands, millions, billions, etc.).
        /// <para/>The caller must ensure the current digit is the correct 'units of' digit.
        /// </summary>
        /// <param name="singleUnit">Name of the unit in singular form (such as 1 'unit'). For example: 'tysiąc'.</param>
        /// <param name="twoToFourUnits">Name of the unit in plural form of 2-4 items (such as 2 'units'). For example: 'tysiące'.</param>
        /// <param name="fiveOrMoreUnits">Name of the unit in plural form of 5-10 items (such as 5 'units'). For example: 'tysięcy'.</param>
        internal void ProcessOnesOf(string singleUnit, string twoToFourUnits, string fiveOrMoreUnits)
        {
            if (!SkipNextDigit)
            {
                switch (CurrentDigit)
                {
                    case 0:
                        {
                            if (CurrentUnitsPresent)
                            {
                                AppendToCurrentPart(" {0}", fiveOrMoreUnits);
                            }
                            else
                            {
                            }
                        } break;
                    case 1: { AppendToCurrentPart(" jeden {0}", singleUnit); CurrentUnitsPresent = true; } break;
                    case 2: { AppendToCurrentPart(" dwa {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 3: { AppendToCurrentPart(" trzy {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 4: { AppendToCurrentPart(" cztery {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 5: { AppendToCurrentPart(" pięć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 6: { AppendToCurrentPart(" sześć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 7: { AppendToCurrentPart(" siedem {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 8: { AppendToCurrentPart(" osiem {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 9: { AppendToCurrentPart(" dziewięć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                }
                AppendCurrentPart();
            }
            else
            {
                SkipNextDigit = false;
            }
        }
        /// <summary>
        /// Processes the least significant (last) position assuming it contains a number of units (e.g. złotys.).
        /// <para/>The caller must ensure the current digit is the correct (last) 'units of' digit.
        /// <para/>The behavior for the last digit is slightly different than in <see cref="ProcessOnesOf(string, string, string)"/>, hence the need for a separate method.
        /// </summary>
        /// <param name="singleUnit">Name of the unit in singular form (such as 1 'unit'). For example: 'złoty'.</param>
        /// <param name="twoToFourUnits">Name of the unit in plural form of 2-4 items (such as 2 'units'). For example: 'złote'.</param>
        /// <param name="fiveOrMoreUnits">Name of the unit in plural form of 5-10 items (such as 5 'units'). For example: 'złotych'.</param>
        internal void ProcessCustomOnesOf(string singleUnit, string twoToFourUnits, string fiveOrMoreUnits)
        {
            if (!SkipNextDigit)
            {
                switch (CurrentDigit)
                {
                    case 0:
                        {
                            if (AnyStringAlreadySet)
                            {
                                AppendToCurrentPart(" {0}", fiveOrMoreUnits);
                            }
                            else
                            {
                                AppendToCurrentPart(" zero {0}", fiveOrMoreUnits);
                            }
                        } break;
                    case 1:
                        {
                            switch (LiczbaZłotych)
                            {
                                case 1:
                                    {
                                        AppendToCurrentPart(" jeden {0}", singleUnit);
                                    } break;
                                default:
                                    {
                                        AppendToCurrentPart(" jeden {0}", fiveOrMoreUnits);
                                    } break;
                            }
                            CurrentUnitsPresent = true;
                        } break;
                    case 2: { AppendToCurrentPart(" dwa {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 3: { AppendToCurrentPart(" trzy {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 4: { AppendToCurrentPart(" cztery {0}", twoToFourUnits); CurrentUnitsPresent = true; } break;
                    case 5: { AppendToCurrentPart(" pięć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 6: { AppendToCurrentPart(" sześć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 7: { AppendToCurrentPart(" siedem {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 8: { AppendToCurrentPart(" osiem {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                    case 9: { AppendToCurrentPart(" dziewięć {0}", fiveOrMoreUnits); CurrentUnitsPresent = true; } break;
                }
                AppendCurrentPart();
            }
            else
            {
                SkipNextDigit = false;
            }
        }
        #endregion
    }
}
