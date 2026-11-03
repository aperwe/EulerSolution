using Microsoft.VisualStudio.TestTools.UnitTesting;
using QBits.Intuition.Assemblies;
using QBits.Intuition.Collections;
using QBits.Intuition.Crosswords;
using QBits.Intuition.Crosswords.Elements;
using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Logger;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Settings;
using QBits.Intuition.Text;
using QBits.Intuition.Text.Palindroms;
using QBits.Intuition.Threading;
using QBits.Intuition.UI;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows.Controls;

namespace EulerProblemsTests;

/// <summary>
/// Records how IntuitionLibrary behaves, so that cleanups cannot change it unnoticed. Most of these types have no
/// callers outside the library, so the solver tests do not protect them.
/// These tests are here because IntuitionLibraryTests can be blocked on the development machine (see AGENTS.md).
/// </summary>
[TestClass]
public class LibraryBehaviorTests
{
    private static readonly BigInteger Seven = new BigInteger(7);

    [DataTestMethod]
    [DataRow(12L, 5L, "17", "7", "60", "2", "2")]
    [DataRow(-12L, 5L, "-7", "-17", "-60", "-2", "-2")]
    [DataRow(4294967296L, 4294967295L, "8589934591", "1", "18446744069414584320", "1", "1")]
    public void BigIntegerArithmeticGivesCorrectResults(long left, long right, string sum, string difference, string product, string quotient, string remainder)
    {
        BigInteger a = left, b = right;
        Assert.AreEqual(sum, (a + b).ToString());
        Assert.AreEqual(difference, (a - b).ToString());
        Assert.AreEqual(product, (a * b).ToString());
        Assert.AreEqual(quotient, (a / b).ToString());
        Assert.AreEqual(remainder, (a % b).ToString());
    }

    [TestMethod]
    public void BigIntegerParsesAndPrintsLargeValues()
    {
        var value = new BigInteger("123456789012345678901234567890", 10);
        Assert.AreEqual("123456789012345678901234567890", value.ToString());
        Assert.AreEqual("-123456789012345678901234567890", (-value).ToString());
    }

    [TestMethod]
    public void BigIntegerComparesEqualValuesAsEqual()
    {
        BigInteger other = 7;
        Assert.IsTrue(Seven == other);
        Assert.IsFalse(Seven != other);
        Assert.IsTrue(Seven.Equals(other));
        Assert.IsTrue(Seven >= other);
        Assert.IsTrue(Seven <= other);
        Assert.AreEqual(Seven.GetHashCode(), other.GetHashCode());
    }

    [TestMethod]
    public void BigIntegerComparesDifferentValuesAsDifferent()
    {
        BigInteger other = 8;
        Assert.IsFalse(Seven == other);
        Assert.IsTrue(Seven != other);
        Assert.IsFalse(Seven.Equals(other));
        Assert.IsTrue(Seven < other);
        Assert.IsTrue(other > Seven);
    }

    [TestMethod]
    public void BigIntegerWithNullOnTheRightIsNotEqual()
    {
        Assert.IsFalse(Seven == null);
        Assert.IsTrue(Seven != null);
        Assert.IsFalse(Seven.Equals(null));
    }

    [TestMethod]
    public void BigIntegerSortsThroughIComparable()
    {
        BigInteger[] values = { 5, -3, 100, 0, -50 };
        var sorted = values.OrderBy(value => value).Select(value => value.ToString());
        CollectionAssert.AreEqual(new[] { "-50", "-3", "0", "5", "100" }, sorted.ToArray());
    }

    [TestMethod]
    public void BigIntegerDistinctRemovesEqualValues()
    {
        BigInteger[] values = { 5, 5, 6, -6, new BigInteger(5) };
        Assert.AreEqual(3, values.Distinct().Count());
    }

    [TestMethod]
    public void BigIntegerWithNullOnTheLeftIsNotEqual()
    {
        BigInteger? nothing = null;
        Assert.IsFalse(nothing == Seven);
        Assert.IsTrue(nothing != Seven);
    }

    [TestMethod]
    public void BigIntegerNullEqualsNull()
    {
        BigInteger? first = null, second = null;
        Assert.IsTrue(first == second);
        Assert.IsFalse(first != second);
    }

    [TestMethod]
    public void BigIntegerIsNotEqualToOtherTypes()
    {
        Assert.IsFalse(Seven.Equals("7"));
        Assert.IsFalse(Seven.Equals(7));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("7")]
    public void BigIntegerSortsAfterNullAndOtherTypes(object? other)
    {
        Assert.AreEqual(1, ((IComparable)Seven).CompareTo(other));
    }

    [DataTestMethod]
    [DataRow(12, 18, 6)]
    [DataRow(17, 5, 1)]
    [DataRow(0, 5, 0)]
    [DataRow(9, 9, 9)]
    public void GreatestCommonDivisorIsCorrect(int first, int second, int expected)
    {
        Assert.AreEqual(expected, MoreMath.NWD(first, second));
        Assert.AreEqual((long)expected, MoreMath.NWD((long)first, (long)second));
    }

    [TestMethod]
    public void MoreMathHelpersAreCorrect()
    {
        Assert.AreEqual(120, MoreMath.Factorial(5));
        Assert.AreEqual(0, MoreMath.Factorial(-1));
        Assert.IsTrue(MoreMath.IsEven(4));
        Assert.IsFalse(MoreMath.IsEven(5));
        Assert.AreEqual(0.0, MoreMath.RoundoffNearZero(0.001));
        Assert.AreEqual(0.5, MoreMath.RoundoffNearZero(0.5));
        Assert.AreEqual(321L, MoreMath.IntFromDigits(new[] { 1, 2, 3 }));
        Assert.AreEqual(123L, MoreMath.IntFromDigits("123"));
    }

    [TestMethod]
    public void NumberRepresentationExposesItsDigits()
    {
        var number = new NumberRepresentation(1406357289);
        CollectionAssert.AreEqual("1406357289".ToCharArray(), number.Chars);
        Assert.AreSame(number.Chars, number.Chars);
        Assert.AreEqual(1406357289L, number.Value);
        Assert.AreEqual("1406357289", number.ToString());
    }

    [DataTestMethod]
    [DataRow(1406357289L, true)]
    [DataRow(1406357288L, false)]
    public void NumberRepresentationChecksSubstringDivisibility(long value, bool expected)
    {
        Assert.AreEqual(expected, new NumberRepresentation(value).IsSubstringDivisible());
    }

    [TestMethod]
    public void RealAddsAndCompares()
    {
        Real one = Real.CreateReal(1.5), two = Real.CreateReal(2.5);
        Assert.AreEqual(4.0, (one + two).ToDouble());
        Assert.AreEqual(1.5, (double)one);
        Assert.IsTrue(one.CompareTo(two) < 0);
        Assert.IsTrue(two.CompareTo(one) > 0);
        Assert.AreEqual(0, one.CompareTo(Real.CreateReal(1.5)));
    }

    [TestMethod]
    public void RealComparedToNullIsReportedAsNotImplemented()
    {
        Assert.ThrowsException<NotImplementedException>(() => Real.CreateReal(1.5).CompareTo(null!));
        Assert.ThrowsException<NotImplementedException>(() => Real.CreateReal(1.5).Add(null!));
    }

    [TestMethod]
    public void StringCachesStartEmptyAndReturnTheSameCopy()
    {
        foreach (var cache in new[] { new StringCache(), new StringCache(5), new SortedStringCache() })
        {
            var strings = cache.CachedStrings;
            Assert.AreEqual(0, strings.Count());
            Assert.AreSame(strings, cache.CachedStrings);
        }
    }

    [TestMethod]
    public void ContinuedFractionFindsSquareRootSequence()
    {
        var found = new List<int>();
        var fraction = new ContinuedFractionSquare(23) { ZnalezionaNastępnaLiczba = found.Add };

        Assert.IsTrue(fraction.Znajdź());

        Assert.AreEqual(4, fraction.A);
        CollectionAssert.AreEqual(new[] { 4, 1, 3, 1, 8 }, found);
        CollectionAssert.AreEqual(new[] { 1, 3, 1, 8 }, fraction.SekwencjaNieobrobiona.ToArray());
        CollectionAssert.AreEqual(new[] { 1, 3, 1 }, fraction.Palindrom.ToArray());
    }

    [TestMethod]
    public void ContinuedFractionWorksWithoutCallback()
    {
        var fraction = new ContinuedFractionSquare(23);
        Assert.IsTrue(fraction.Znajdź());
        CollectionAssert.AreEqual(new[] { 1, 3, 1 }, fraction.Palindrom.ToArray());
    }

    [TestMethod]
    public void AddAndReferenceReturnsTheAddedChild() => RunOnUIThread(() =>
    {
        var panel = new StackPanel();
        var child = new Button();
        Assert.AreSame(child, panel.Children.AddAndReference(child));
        Assert.AreEqual(1, panel.Children.Count);
        Assert.AreSame(child, panel.Children[0]);
    });

    [TestMethod]
    public void AddGridItemPlacesChildInGrid() => RunOnUIThread(() =>
    {
        var grid = new Grid();
        var first = grid.AddGridItem(2, new Button());
        var second = grid.AddGridItem(3, 4, new Button());
        Assert.AreEqual(2, Grid.GetRow(first));
        Assert.AreEqual(3, Grid.GetRow(second));
        Assert.AreEqual(4, Grid.GetColumn(second));
        Assert.AreEqual(2, grid.Children.Count);
    });

    [TestMethod]
    public void FindAncestorSearchesTheVisualTree() => RunOnUIThread(() =>
    {
        var panel = new StackPanel();
        Assert.AreSame(panel, panel.FindAncestor<StackPanel>());
        Assert.IsNull(panel.FindAncestor<Grid>());
    });

    [TestMethod]
    public void InvokeOnUIThreadRunsTheCode() => RunOnUIThread(() =>
    {
        bool ran = false;
        new StackPanel().InvokeOnUIThread(() => ran = true);
        Assert.IsTrue(ran);
    });

    [TestMethod]
    public void UpdateIconReportsMissingResourceAsArgumentNull() => RunOnUIThread(() =>
    {
        using var form = new System.Windows.Forms.Form();
        Assert.ThrowsException<ArgumentNullException>(
            () => UIHelper.UpdateIcon(form, Assembly.GetExecutingAssembly(), "no.such.resource"));
    });

    [TestMethod]
    public void SetWindowStateToDialogAppliesDialogSettings() => RunOnUIThread(() =>
    {
        using var form = new System.Windows.Forms.Form();
        UIHelper.SetWindowStateToDialog(form);
        Assert.IsFalse(form.MaximizeBox);
        Assert.IsFalse(form.ShowInTaskbar);
        Assert.AreEqual(System.Windows.Forms.FormBorderStyle.FixedDialog, form.FormBorderStyle);
    });

    [TestMethod]
    public void AssemblyVersionFindsTheRequestedAssembly()
    {
        var thisAssembly = Assembly.GetExecutingAssembly().GetName().Version!.ToString(4);
        var entryAssembly = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(4) ?? string.Empty;
        Assert.AreEqual(thisAssembly, AssemblyHelper.AssemblyVersion(WhichAssembly.ThisAssembly));
        Assert.AreEqual(entryAssembly, AssemblyHelper.AssemblyVersion(WhichAssembly.EntryAssembly));
        Assert.AreEqual(string.Empty, AssemblyHelper.AssemblyVersion((WhichAssembly)99));
    }

    [TestMethod]
    public void UniversalFactoryCreatesRegisteredTypes()
    {
        var factory = new UniversalFactory<string, object>();
        factory.RegisterConstructor("text", () => "created");
        Assert.AreEqual("created", factory.CreateObject("text"));
    }

    [TestMethod]
    public void UniversalFactoryRejectsDuplicateAndUnknownTypes()
    {
        var factory = new UniversalFactory<string, object>();
        factory.RegisterConstructor("text", () => "created");
        Assert.ThrowsException<ApplicationException>(() => factory.RegisterConstructor("text", () => "again"));
        Assert.ThrowsException<KeyNotFoundException>(() => factory.CreateObject("unknown"));
    }

    [TestMethod]
    public void UniversalFactoryHasOneSharedInstance()
    {
        Assert.AreSame(UniversalFactory<int, LibraryBehaviorTests>.SAP, UniversalFactory<int, LibraryBehaviorTests>.SAP);
    }

    [TestMethod]
    public void CrosswordFactoriesCreateKnownTypes() => RunOnUIThread(() =>
    {
        CrosswordTypeRegistrar.RegisterKnownCrosswordTypes();
        CrosswordTypeRegistrar.RegisterKnownElementTypes();

        var crossword = UniversalFactory<string, Crossword>.SAP.CreateObject("Jolka");
        Assert.AreEqual("Jolka", crossword.GetType().Name);
        foreach (var type in new[] { "Letter", "EmptyElement" })
        {
            Assert.AreEqual(type, ElementsFactory.SAP.CreateObject(type, crossword, 1, 2).GetType().Name);
        }

        // Records existing faults: the factory reaches Definition, but its constructor cannot complete.
        Assert.ThrowsException<NullReferenceException>(() => ElementsFactory.SAP.CreateObject("Definition", crossword, 1, 2));
        Assert.ThrowsException<ApplicationException>(() => ElementsFactory.SAP.CreateObject("Definition", crossword, 1, 2));
    });

    [TestMethod]
    public void TextLoggerWritesEntriesAndClosesFileOnExit()
    {
        var originalFileName = TextLogger.textLogFileName;
        var folder = CreateTemporaryFolder();
        try
        {
            TextLogger.textLogFileName = Path.Combine(folder, "log.txt");
            using var exit = new ManualResetEvent(false);
            var logger = new TextLogger();
            ((IEventWatcher)logger).WatchThisEvent(exit);
            logger.AcceptLogEntry(LogLevel.Default, "first");
            logger.AcceptLogEntry(LogLevel.Default, "second");
            exit.Set();

            CollectionAssert.AreEqual(new[] { "first", "second" }, ReadLinesOnceFileIsClosed(TextLogger.textLogFileName));
        }
        finally
        {
            TextLogger.textLogFileName = originalFileName;
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void SaveDataCreatesTheFolderAndWritesTheFile()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var file = Path.Combine(folder, "new", "settings.xml");
            var data = new System.Data.DataSet("settings");
            data.Tables.Add("values").Columns.Add("name");
            data.Tables[0].Rows.Add("stored");

            data.SaveData(file);

            var loaded = new System.Data.DataSet();
            loaded.ReadXml(file);
            Assert.AreEqual("stored", loaded.Tables[0].Rows[0]["name"]);
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void SaveDataIgnoresMissingDataSet()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var file = Path.Combine(folder, "settings.xml");
            System.Data.DataSet missing = null!;
            missing.SaveData(file);
            Assert.IsFalse(File.Exists(file));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void SaveDataRejectsPathsWithoutFolder()
    {
        var data = new System.Data.DataSet();
        Assert.ThrowsException<ArgumentNullException>(() => data.SaveData(null!));
        Assert.ThrowsException<ArgumentNullException>(() => data.SaveData(string.Empty));
        Assert.ThrowsException<ArgumentNullException>(() => data.SaveData(Path.GetPathRoot(Path.GetTempPath())!));
        Assert.ThrowsException<ArgumentException>(() => data.SaveData("settings.xml"));
    }

    [TestMethod]
    public void SettingsPathBuildsPathsFromItsParts()
    {
        var settings = new SettingsPath("Product", "root", "Company", "default.xml");
        var expectedRoot = Path.Combine("root", "Company", "Product");
        Assert.AreEqual(expectedRoot, settings.RootSettingsPath);
        Assert.AreEqual(Path.Combine(expectedRoot, "default.xml"), settings.DefaultFilePath);
        Assert.AreEqual(Path.Combine(expectedRoot, "other.xml"), settings.GetFilePath("other.xml"));
    }

    [TestMethod]
    public void SettingsPathUsesAppDataAndCompanyByDefault()
    {
        var expected = Path.Combine(Environment.ExpandEnvironmentVariables("%APPDATA%"), "Q-Bits", "Product");
        Assert.AreEqual(expected, new SettingsPath("Product").RootSettingsPath);
    }

    [TestMethod]
    public void SettingsPathRequiresDefaultFileOnlyWhenItIsMissing()
    {
        Assert.ThrowsException<ApplicationException>(() => new SettingsPath("Product").DefaultFilePath);
        var emptyName = new SettingsPath("Product", "root", "Company", string.Empty);
        Assert.AreEqual(emptyName.RootSettingsPath, emptyName.DefaultFilePath);
    }

    [DataTestMethod]
    [DataRow(0.0, "zero złotych 0/100")]
    [DataRow(1.0, "jeden złoty 0/100")]
    [DataRow(-1.0, "minus jeden złoty 0/100")]
    [DataRow(21.5, "dwadzieścia jeden złotych 50/100")]
    [DataRow(1000.0, "jeden tysiąc złotych 0/100")]
    [DataRow(2118.48, "dwa tysiące sto osiemnaście złotych 48/100")]
    [DataRow(16000001.0, "szesnaście milionów jeden złotych 0/100")]
    [DataRow(1000000000.0, "jeden miliard złotych 0/100")]
    public void QuotaIsConvertedToPolishText(double quota, string expected)
    {
        Assert.AreEqual(expected, FinancialHelper.ConvertQuotaToText(quota));
    }

    [TestMethod]
    public void PalindromerSampleDictionaryIsCreatedOnce()
    {
        var dictionary = Palindromer.SampleDictonary;
        Assert.AreSame(dictionary, Palindromer.SampleDictonary);
        Assert.AreEqual(53, dictionary.Elements("w").Count());
    }

    [TestMethod]
    public void PalindromerAddsLowerCaseUniqueWords()
    {
        var dictionary = new System.Xml.Linq.XElement("dict");
        Palindromer.AddWord(dictionary, "Kot");
        Palindromer.AddWord(dictionary, "KOT");
        Assert.AreEqual("kot", dictionary.Elements("w").Single().Value);
    }

    [TestMethod]
    public void PalindromerSearchIsNotImplemented()
    {
        Assert.ThrowsException<NotImplementedException>(() => Palindromer.FindPalindrom("kot"));
    }

    [TestMethod]
    public void ThreadSafeObjectWrapsAndReturnsItsValue()
    {
        const string value = "wrapped";
        ThreadSafeObject<string> wrapped = value;
        string unwrapped = wrapped;
        Assert.AreSame(value, unwrapped);
        Assert.AreEqual(value, wrapped.ToString());
        Assert.AreEqual(value.GetHashCode(), wrapped.GetHashCode());
    }

    [TestMethod]
    public void ThreadSafeObjectHasItsOwnStableLockKey()
    {
        ThreadSafeObject<string> first = "a", second = "a";
        Assert.AreSame(first.Key, first.Key);
        Assert.AreNotSame(first.Key, second.Key);
    }

    [TestMethod]
    public void ThreadSafeObjectWithoutValueFailsOnlyWhenTheValueIsUsed()
    {
        var empty = new ThreadSafeObject<string>();
        string unwrapped = empty;
        Assert.IsNull(unwrapped);
        Assert.ThrowsException<NullReferenceException>(() => empty.ToString());
        Assert.ThrowsException<NullReferenceException>(() => empty.GetHashCode());
    }

    private static string CreateTemporaryFolder()
    {
        return Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "LibraryBehaviorTests", Guid.NewGuid().ToString())).FullName;
    }

    /// <summary>Waits for the logger thread to close the file. Opening without sharing fails while it is still open.</summary>
    private static string[] ReadLinesOnceFileIsClosed(string path)
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (true)
        {
            try
            {
                using var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None));
                return reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (IOException) when (DateTime.UtcNow < deadline)
            {
                Thread.Sleep(100);
            }
        }
    }

    /// <summary>WPF and Windows Forms objects must be created on a thread in single-threaded apartment mode.</summary>
    private static void RunOnUIThread(Action test)
    {
        ExceptionDispatchInfo? failure = null;
        var thread = new Thread(() =>
        {
            try { test(); }
            catch (Exception e) { failure = ExceptionDispatchInfo.Capture(e); }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        failure?.Throw();
    }
}
