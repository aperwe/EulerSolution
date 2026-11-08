using Microsoft.VisualStudio.TestTools.UnitTesting;
using QBits.Intuition.AbstractLanguageIntelligence.Model;
using QBits.Intuition.AbstractLanguageIntelligence.Model.Brains;
using QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds;
using QBits.Intuition.Assemblies;
using QBits.Intuition.Collections;
using QBits.Intuition.Crosswords;
using QBits.Intuition.Crosswords.Definitions;
using QBits.Intuition.Crosswords.Elements;
using QBits.Intuition.DesignPatterns.Factory;
using QBits.Intuition.Financials.Banking;
using QBits.Intuition.Logger;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Settings;
using QBits.Intuition.Text;
using QBits.Intuition.Text.Palindroms;
using QBits.Intuition.Threading;
using QBits.Intuition.UI;
using QBits.Intuition.Xml;
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

    /// <summary>Before #139 a derived class could fill the cache only by writing to Cache itself. That must keep working.</summary>
    [TestMethod]
    public void StringCachesReturnStringsStoredDirectlyByADerivedClass()
    {
        var cache = new DirectStringCache(2);
        Assert.AreEqual(2, cache.CurrentCapacity);
        Assert.AreEqual(10, new DirectStringCache().CurrentCapacity);

        cache.Store("b");
        var first = cache.CachedStrings;
        CollectionAssert.AreEquivalent(new[] { "b" }, first.ToArray());
        Assert.AreSame(first, cache.CachedStrings);

        // Direct writes are not limited by Capacity, and a copy returned earlier does not change.
        cache.Store("c");
        cache.Store("a");
        CollectionAssert.AreEquivalent(new[] { "a", "b", "c" }, cache.CachedStrings.ToArray());
        CollectionAssert.AreEquivalent(new[] { "b" }, first.ToArray());

        var sorted = new DirectSortedStringCache();
        foreach (var value in new[] { "c", "a", "b" }) sorted.Store(value);
        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, sorted.CachedStrings.ToArray());
    }

    [TestMethod]
    public void StringCacheAddStoresEachStringOnce()
    {
        var cache = new StringCache();
        Assert.IsTrue(cache.Add("a"));
        Assert.IsTrue(cache.Add("b"));
        Assert.IsFalse(cache.Add("a"));
        Assert.IsTrue(cache.Add("A"), "Comparison is case-sensitive.");
        CollectionAssert.AreEquivalent(new[] { "a", "b", "A" }, cache.CachedStrings.ToArray());
        Assert.ThrowsException<ArgumentNullException>(() => cache.Add(null!));
    }

    [TestMethod]
    public void StringCacheAddRemovesTheLeastRecentlyUsedStringWhenFull()
    {
        var cache = new StringCache(3);
        foreach (var value in new[] { "a", "b", "c" }) cache.Add(value);

        cache.Add("a"); // "a" becomes the most recently used, so "b" is now the least recently used.
        Assert.IsTrue(cache.Add("d"));
        CollectionAssert.AreEquivalent(new[] { "a", "c", "d" }, cache.CachedStrings.ToArray());

        Assert.IsTrue(cache.Add("e"));
        CollectionAssert.AreEquivalent(new[] { "a", "d", "e" }, cache.CachedStrings.ToArray());
    }

    [TestMethod]
    public void StringCacheKeepsTenStringsByDefault()
    {
        var cache = new StringCache();
        for (int i = 0; i <= 10; i++) cache.Add(i.ToString());
        CollectionAssert.AreEquivalent(Enumerable.Range(1, 10).Select(i => i.ToString()).ToArray(), cache.CachedStrings.ToArray());
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void StringCacheWithoutCapacityStoresNothing(int capacity)
    {
        var cache = new StringCache(capacity);
        Assert.IsFalse(cache.Add("a"));
        Assert.AreEqual(0, cache.CachedStrings.Count());
    }

    [TestMethod]
    public void StringCacheReturnsANewCopyOnlyAfterItsContentsChange()
    {
        var cache = new StringCache();
        cache.Add("a");
        var first = cache.CachedStrings;

        cache.Add("a");
        Assert.AreSame(first, cache.CachedStrings, "Using a cached string again does not change the contents.");

        cache.Add("b");
        var second = cache.CachedStrings;
        Assert.AreNotSame(first, second);
        CollectionAssert.AreEquivalent(new[] { "a" }, first.ToArray());
        CollectionAssert.AreEquivalent(new[] { "a", "b" }, second.ToArray());
    }

    [TestMethod]
    public void SortedStringCacheAddsAndRemovesLikeStringCache()
    {
        var cache = new SortedStringCache();
        foreach (var value in new[] { "c", "a", "b" }) cache.Add(value);
        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, cache.CachedStrings.ToArray());

        for (int i = 0; i < 8; i++) cache.Add("z" + i);
        CollectionAssert.AreEqual(new[] { "a", "b", "z0", "z1", "z2", "z3", "z4", "z5", "z6", "z7" }, cache.CachedStrings.ToArray(),
            "The 11th string removes \"c\", the first one added.");
    }

    [TestMethod]
    public void StringCacheAddWorksWithStringsChangedDirectlyByADerivedClass()
    {
        var cache = new DirectStringCache(2);
        cache.Store("x");
        cache.Add("a");
        Assert.IsTrue(cache.Add("b"));
        CollectionAssert.AreEquivalent(new[] { "a", "b" }, cache.CachedStrings.ToArray(), "A string stored directly counts as the least recently used.");

        cache.Forget("a");
        Assert.IsTrue(cache.Add("c"));
        CollectionAssert.AreEquivalent(new[] { "b", "c" }, cache.CachedStrings.ToArray(), "A string removed directly frees its place.");
        Assert.IsTrue(cache.Add("d"));
        CollectionAssert.AreEquivalent(new[] { "c", "d" }, cache.CachedStrings.ToArray());
    }

    private sealed class DirectStringCache : StringCache
    {
        public DirectStringCache() { }
        public DirectStringCache(int capacity) : base(capacity) { }
        public int CurrentCapacity => Capacity;
        public void Store(string value) { Cache.Add(value); CacheDirty = true; }
        public void Forget(string value) { Cache.Remove(value); CacheDirty = true; }
    }

    private sealed class DirectSortedStringCache : SortedStringCache
    {
        public void Store(string value) { Cache.Add(value); CacheDirty = true; }
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
        EnsureCrosswordTypesRegistered();

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
    public void CrosswordWithoutParentFillsLettersButCannotShowThem()
    {
        EnsureCrosswordTypesRegistered();
        var crossword = new TestCrossword();
        crossword.SetDimensions(2, 1);
        Assert.ThrowsException<InvalidOperationException>(() => crossword.MakeEmptyCrossword());
        Assert.AreEqual(
            "<crossword columns=\"2\" rows=\"1\" type=\"TestCrossword\"><elements>" +
            "<crosswordItem column=\"0\" row=\"0\" type=\"Letter\" letter=\"\" definite=\"False\" />" +
            "<crosswordItem column=\"1\" row=\"0\" type=\"Letter\" letter=\"\" definite=\"False\" /></elements></crossword>",
            SerializeCrossword(crossword));
    }

    [TestMethod]
    public void CrosswordShowsAndReplacesItsElements() => RunOnUIThread(() =>
    {
        EnsureCrosswordTypesRegistered();
        using var panel = new System.Windows.Forms.Panel();
        var crossword = new TestCrossword { Parent = panel };
        crossword.SetDimensions(2, 3);
        crossword.MakeEmptyCrossword();
        var positions = panel.Controls.Cast<System.Windows.Forms.Control>().Select(control => (control.Left, control.Top));
        CollectionAssert.AreEquivalent(new[] { (0, 0), (55, 0), (0, 44), (55, 44), (0, 88), (55, 88) }, positions.ToArray());

        crossword.SubstituteCrosswordElement(ElementsFactory.SAP.CreateObject("EmptyElement", crossword, 1, 2), 1, 2);
        Assert.AreEqual(6, panel.Controls.Count);
        var replaced = panel.Controls.Cast<System.Windows.Forms.Control>().Single(control => control.Left == 55 && control.Top == 88);
        Assert.AreEqual(System.Drawing.Color.Green, replaced.BackColor);

        crossword.RemoveWindowsControls();
        Assert.AreEqual(0, panel.Controls.Count);
    });

    [TestMethod]
    public void CrosswordIsLoadedAndSavedThroughXml()
    {
        EnsureCrosswordTypesRegistered();
        const string saved = "<crossword columns=\"2\" rows=\"1\" type=\"Jolka\"><elements>" +
            "<crosswordItem column=\"0\" row=\"0\" type=\"Letter\" letter=\"A\" definite=\"True\" />" +
            "<crosswordItem column=\"1\" row=\"0\" type=\"EmptyElement\" /></elements></crossword>";
        var file = new System.Xml.XmlDocument();
        file.LoadXml(saved);

        var crossword = CrosswordLoader.LoadCrosswordFromXml(file);

        Assert.AreEqual("Jolka", crossword.GetType().Name);
        Assert.AreEqual(saved, SerializeCrossword(crossword));
    }

    [DataTestMethod]
    [DataRow("<crossword columns=\"1\" rows=\"1\" />", "type")]
    [DataRow("<crossword columns=\"1\" type=\"Jolka\" />", "rows")]
    [DataRow("<crossword columns=\"1\" rows=\"1\" type=\"Jolka\"><elements><crosswordItem row=\"0\" type=\"EmptyElement\" /></elements></crossword>", "column")]
    [DataRow("<crossword columns=\"1\" rows=\"1\" type=\"Jolka\"><elements><crosswordItem column=\"0\" row=\"0\" type=\"Letter\" definite=\"False\" /></elements></crossword>", "letter")]
    [DataRow("<crossword columns=\"1\" rows=\"1\" type=\"Jolka\"><elements><crosswordItem column=\"0\" row=\"0\" type=\"Letter\" letter=\"A\" /></elements></crossword>", "definite")]
    public void CrosswordLoaderNamesTheMissingAttribute(string saved, string attribute)
    {
        EnsureCrosswordTypesRegistered();
        var file = new System.Xml.XmlDocument();
        file.LoadXml(saved);
        var error = Assert.ThrowsException<System.Xml.XmlException>(() => CrosswordLoader.LoadCrosswordFromXml(file));
        StringAssert.Contains(error.Message, $"'{attribute}'");
    }

    [TestMethod]
    public void CrosswordLoaderRejectsEmptyDocument()
    {
        Assert.ThrowsException<System.Xml.XmlException>(() => CrosswordLoader.LoadCrosswordFromXml(new System.Xml.XmlDocument()));
    }

    [TestMethod]
    public void CrosswordElementsShowTheirLoadedState() => RunOnUIThread(() =>
    {
        EnsureCrosswordTypesRegistered();
        var saved = new System.Xml.XmlDocument();
        saved.LoadXml("<crossword columns=\"2\" rows=\"1\"><elements>" +
            "<crosswordItem column=\"0\" row=\"0\" type=\"Letter\" letter=\"A\" definite=\"True\" />" +
            "<crosswordItem column=\"1\" row=\"0\" type=\"EmptyElement\" /></elements></crossword>");
        using var panel = new System.Windows.Forms.Panel();
        var crossword = new TestCrossword { Parent = panel };
        crossword.DeserializeFromNode(saved.DocumentElement!);
        crossword.CreateLetters();

        var letter = crossword.ElementAt(0, 0);
        var empty = crossword.ElementAt(1, 0);
        Assert.AreEqual("Letter", letter.GetObjectType());
        Assert.AreEqual("EmptyElement", empty.GetObjectType());
        Assert.AreEqual("A", letter.ToString());
        var letterButton = panel.Controls.Cast<System.Windows.Forms.Control>().Single(control => control.Left == 0);
        var emptyButton = panel.Controls.Cast<System.Windows.Forms.Control>().Single(control => control.Left == 55);
        Assert.AreEqual("A", letterButton.Text);
        Assert.IsTrue(letterButton.Font.Bold);
        Assert.AreEqual(System.Drawing.Color.Green, emptyButton.BackColor);
        Assert.AreEqual(System.Windows.Forms.FlatStyle.Flat, ((System.Windows.Forms.Button)emptyButton).FlatStyle);
    });

    [TestMethod]
    public void CrosswordLetterUpdatesItsButton() => RunOnUIThread(() =>
    {
        EnsureCrosswordTypesRegistered();
        using var panel = new System.Windows.Forms.Panel();
        var crossword = new TestCrossword { Parent = panel };
        crossword.SetDimensions(1, 1);
        crossword.MakeEmptyCrossword();
        var button = panel.Controls[0];
        Assert.AreEqual(string.Empty, button.Text);
        Assert.IsFalse(button.Font.Bold);

        var update = new System.Xml.XmlDocument();
        update.LoadXml("<crosswordItem letter=\"B\" definite=\"True\" />");
        crossword.ElementAt(0, 0).DeserializeFromNode(update.DocumentElement!);
        Assert.AreEqual("B", button.Text);
        Assert.IsTrue(button.Font.Bold);
        Assert.AreEqual("B", crossword.ElementAt(0, 0).ToString());

        crossword.ElementAt(0, 0).RemoveWindowsControl();
        crossword.ElementAt(0, 0).RemoveWindowsControl();
        Assert.AreEqual(0, panel.Controls.Count);
    });

    [TestMethod]
    public void ElementsFactoryHasOneInstanceAndRejectsUnknownAndDuplicateTypes()
    {
        EnsureCrosswordTypesRegistered();
        Assert.AreSame(ElementsFactory.SAP, ElementsFactory.SAP);
        Assert.IsNull(BaseCrosswordElement.ObjectType);
        Assert.ThrowsException<KeyNotFoundException>(() => ElementsFactory.SAP.CreateObject("unknown", new TestCrossword(), 0, 0));
        Assert.ThrowsException<ApplicationException>(() => ElementsFactory.SAP.RegisterConstructor("Letter", (crossword, column, row) => null!));
    }

    [TestMethod]
    public void CrosswordContainsEachComponentOnce()
    {
        System.ComponentModel.IContainer container = new TestCrossword();
        using var component = new System.ComponentModel.Component();
        container.Add(component);
        Assert.ThrowsException<Exception>(() => container.Add(component));
        CollectionAssert.AreEqual(new[] { component }, container.Components.Cast<System.ComponentModel.IComponent>().ToArray());

        container.Remove(component);
        Assert.AreEqual(0, container.Components.Count);
        Assert.ThrowsException<Exception>(() => container.Remove(component));
        Assert.ThrowsException<Exception>(() => container.Remove(null));
        Assert.ThrowsException<Exception>(() => container.Add(component, "named"));
    }

    [TestMethod]
    public void CrosswordDisposesItsComponents()
    {
        System.ComponentModel.IContainer container = new TestCrossword();
        var component = new System.ComponentModel.Component();
        bool disposed = false;
        component.Disposed += (_, _) => disposed = true;
        container.Add(component);
        container.Dispose();
        Assert.IsTrue(disposed);
    }

    [TestMethod]
    public void CrosswordIgnoresMissingComponent()
    {
        System.ComponentModel.IContainer container = new TestCrossword();
        container.Add(null);
        Assert.AreEqual(0, container.Components.Count);
        container.Dispose();
    }

    [TestMethod]
    public void BaseDefinitionStartsEmpty()
    {
        var definition = new TestDefinition();
        Assert.IsNull(definition.definicja);
        Assert.AreEqual(Kierunek.Nieokreślony, definition.Kierunek);
        Assert.AreEqual(0, definition.propozycje.Count);
    }

    [TestMethod]
    public void BaseDefinitionReportsChangedText()
    {
        var definition = new TestDefinition();
        object? sender = null;
        EventArgs? args = null;
        definition.DefinitionChangedEvt += (s, e) => { sender = s; args = e; };
        definition.definicja = "clue";
        Assert.AreEqual("clue", definition.definicja);
        Assert.AreSame(definition, sender);
        Assert.AreSame(EventArgs.Empty, args);
    }

    [TestMethod]
    public void BaseDefinitionIsItsOwnSite()
    {
        var definition = new TestDefinition();
        System.ComponentModel.ISite site = definition;
        Assert.AreSame(definition, definition.Site);
        Assert.AreSame(definition, site.Component);
        Assert.IsNull(site.Container);
        Assert.IsFalse(site.DesignMode);
        EventHandler handler = (_, _) => { };
        definition.Disposed += handler;
        definition.Disposed -= handler;
    }

    [TestMethod]
    public void BaseDefinitionReportsUnimplementedMembers()
    {
        var definition = new TestDefinition();
        System.ComponentModel.ISite site = definition;
        Assert.ThrowsException<Exception>(() => definition.Site = site);
        Assert.ThrowsException<Exception>(() => definition.Dispose());
        Assert.ThrowsException<Exception>(() => site.Name);
        Assert.ThrowsException<Exception>(() => site.Name = "name");
        Assert.ThrowsException<Exception>(() => site.GetService(typeof(object)));
    }

    private sealed class TestCrossword : Crossword
    {
        public override void DrawCrossword() { }
        public override string GetObjectType() => "TestCrossword";
        public BaseCrosswordElement ElementAt(int column, int row) => _letters[column, row];
    }

    private sealed class TestDefinition : BaseDefinition { }

    private static readonly object CrosswordRegistrationLock = new();
    private static bool crosswordTypesRegistered;

    /// <summary>Registering a crossword type twice throws, so all tests share one registration.</summary>
    private static void EnsureCrosswordTypesRegistered()
    {
        lock (CrosswordRegistrationLock)
        {
            if (crosswordTypesRegistered) return;
            CrosswordTypeRegistrar.RegisterKnownCrosswordTypes();
            CrosswordTypeRegistrar.RegisterKnownElementTypes();
            crosswordTypesRegistered = true;
        }
    }

    private static string SerializeCrossword(Crossword crossword)
    {
        var document = new NonPersistentXMLDocument("crossword");
        crossword.SerializeToNode(document.RootNode!, document);
        return document.RootNode!.OuterXml;
    }

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
    public void TextLoggerDisposeWritesEntriesAndClosesFile()
        => WithTemporaryLogFile(file =>
        {
            var logger = new TextLogger();
            logger.AcceptLogEntry(LogLevel.Default, "first");
            logger.AcceptLogEntry(LogLevel.Default, "second");

            ((IDisposable)logger).Dispose();

            // Opening the file without sharing succeeds only if the logger has already closed it.
            using var reader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None));
            CollectionAssert.AreEqual(new[] { "first", "second" }, reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));
        });

    [TestMethod]
    public void TextLoggerWritesEntriesPromptlyWhileWatchingAnExitEvent()
        => WithTemporaryLogFile(file =>
        {
            using var exit = new ManualResetEvent(false);
            var logger = new TextLogger();
            ((IEventWatcher)logger).WatchThisEvent(exit);
            try
            {
                logger.AcceptLogEntry(LogLevel.Default, "first");
                Assert.IsTrue(SpinWait.SpinUntil(() => ReadOpenFile(file).Contains("first"), TimeSpan.FromSeconds(3)),
                    "The entry did not reach the file within 3 seconds.");
            }
            finally
            {
                exit.Set();
                ReadLinesOnceFileIsClosed(file);
            }
        });

    [TestMethod]
    public void TextLoggerHonoursAnExitEventGivenLater()
        => WithTemporaryLogFile(file =>
        {
            using var exit = new ManualResetEvent(false);
            var logger = new TextLogger();
            Thread.Sleep(1500); // Longer than the 1 second the logger used to wait before it looked for an exit event.
            ((IEventWatcher)logger).WatchThisEvent(exit);
            logger.AcceptLogEntry(LogLevel.Default, "first");

            exit.Set();

            CollectionAssert.AreEqual(new[] { "first" }, ReadLinesOnceFileIsClosed(file));
        });

    [TestMethod]
    public void TextLoggerTreatsADisposedExitEventAsExit()
        => WithTemporaryLogFile(file =>
        {
            var exit = new ManualResetEvent(false);
            var logger = new TextLogger();
            ((IEventWatcher)logger).WatchThisEvent(exit);
            logger.AcceptLogEntry(LogLevel.Default, "first");

            exit.Dispose();

            CollectionAssert.AreEqual(new[] { "first" }, ReadLinesOnceFileIsClosed(file));
        });

    [TestMethod]
    public void TextLoggerAnnouncesItsShutdownWithItsFilePath()
        => WithTemporaryLogFile(file =>
        {
            var recorder = new RecordingLogger();
            LoggerSAP.RegisterLogger(recorder);
            try
            {
                ((IDisposable)new TextLogger()).Dispose();

                Assert.IsTrue(recorder.Entries.Any(entry => entry.EndsWith("] Text logger exiting: " + file)), string.Join(Environment.NewLine, recorder.Entries));
            }
            finally
            {
                LoggerSAP.UnregisterLogger(recorder);
            }
        });

    [TestMethod]
    public void TextLoggerWritesToTheFileGivenToItsConstructor()
        => WithTemporaryLogFile(staticFile =>
        {
            var file = Path.Combine(Path.GetDirectoryName(staticFile)!, "given.txt");
            var logger = new TextLogger(file);
            logger.AcceptLogEntry(LogLevel.Default, "first");

            ((IDisposable)logger).Dispose();

            CollectionAssert.AreEqual(new[] { "first" }, ReadLinesOnceFileIsClosed(file));
            Assert.IsFalse(File.Exists(staticFile), "The logger also wrote to textLogFileName.");
        });

    [TestMethod]
    public void TwoTextLoggersWriteToTheirOwnFilesAtTheSameTime()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var firstFile = Path.Combine(folder, "first.txt");
            var secondFile = Path.Combine(folder, "second.txt");
            var first = new TextLogger(firstFile);
            var second = new TextLogger(secondFile);
            for (int i = 0; i < 3; i++)
            {
                first.AcceptLogEntry(LogLevel.Default, "first " + i);
                second.AcceptLogEntry(LogLevel.Default, "second " + i);
            }

            ((IDisposable)first).Dispose();
            ((IDisposable)second).Dispose();

            CollectionAssert.AreEqual(new[] { "first 0", "first 1", "first 2" }, ReadLinesOnceFileIsClosed(firstFile));
            CollectionAssert.AreEqual(new[] { "second 0", "second 1", "second 2" }, ReadLinesOnceFileIsClosed(secondFile));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void TextLoggerRejectsAMissingFilePath(string? path)
    {
        Assert.ThrowsException<ArgumentException>(() => new TextLogger(path!));
    }

    [TestMethod]
    public void TextLoggerReportsAMissingFolderToTheCaller()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            Assert.ThrowsException<DirectoryNotFoundException>(() => new TextLogger(Path.Combine(folder, "missing", "log.txt")));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void TextLoggerReportsALockedFileToTheCaller()
        => WithTemporaryLogFile(file =>
        {
            using var lockedFile = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            Assert.ThrowsException<IOException>(() => new TextLogger());
        });

    /// <summary>TextLogger writes to a file named by a static field, so each test points it to its own temporary file.</summary>
    private static void WithTemporaryLogFile(Action<string> test)
    {
        var originalFileName = TextLogger.textLogFileName;
        var folder = CreateTemporaryFolder();
        try
        {
            TextLogger.textLogFileName = Path.Combine(folder, "log.txt");
            test(TextLogger.textLogFileName);
        }
        finally
        {
            TextLogger.textLogFileName = originalFileName;
            Directory.Delete(folder, true);
        }
    }

    /// <summary>Reads a file that the logger may still have open for writing.</summary>
    private static string ReadOpenFile(string path)
    {
        if (!File.Exists(path)) return "";
        using var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        return reader.ReadToEnd();
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
    }

    /// <summary>Changes the current folder for the test process. This is safe because the tests here do not run in parallel.</summary>
    [TestMethod]
    public void SaveDataWritesFileNameWithoutFolderToCurrentFolder()
    {
        var folder = CreateTemporaryFolder();
        var originalFolder = Environment.CurrentDirectory;
        try
        {
            Environment.CurrentDirectory = folder;
            var data = new System.Data.DataSet("settings");
            data.Tables.Add("values").Columns.Add("name");
            data.Tables[0].Rows.Add("stored");

            data.SaveData("settings.xml");

            var loaded = new System.Data.DataSet();
            loaded.ReadXml(Path.Combine(folder, "settings.xml"));
            Assert.AreEqual("stored", loaded.Tables[0].Rows[0]["name"]);
        }
        finally
        {
            Environment.CurrentDirectory = originalFolder;
            Directory.Delete(folder, true);
        }
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

    [TestMethod]
    public void TransactionRecordStartsEmpty()
    {
        var record = new TransactionRecord();
        Assert.IsNull(record.TransactionKind);
        Assert.IsNull(record.TransactionTitle);
        Assert.IsNull(record.ReceivingAccount);
        Assert.IsNull(record.ReceiverName);
        Assert.IsNull(record.ReceiverAddressLine1);
        Assert.IsNull(record.ReceiverAddressLine2);
        Assert.IsNull(record.SenderAccount);
        Assert.IsNull(record.SenderName);
        Assert.IsNull(record.SenderAddressLine1);
        Assert.IsNull(record.SenderAddressLine2);
        Assert.AreEqual(default(DateTime), record.TransactionDate);
        Assert.AreEqual(0m, record.TransactionAmount);
    }

    [TestMethod]
    public void TransactionRecordKeepsItsValues()
    {
        var record = new TransactionRecord
        {
            TransactionKind = "kind", TransactionTitle = "title", ReceivingAccount = "receiving",
            ReceiverName = "receiver", ReceiverAddressLine1 = "r1", ReceiverAddressLine2 = "r2",
            SenderAccount = "sending", SenderName = "sender", SenderAddressLine1 = "s1", SenderAddressLine2 = "s2",
            TransactionNumber = 4547, TransactionAmount = 188.43m,
        };
        CollectionAssert.AreEqual(
            new[] { "kind", "title", "receiving", "receiver", "r1", "r2", "sending", "sender", "s1", "s2" },
            new[] { record.TransactionKind, record.TransactionTitle, record.ReceivingAccount, record.ReceiverName,
                record.ReceiverAddressLine1, record.ReceiverAddressLine2, record.SenderAccount, record.SenderName,
                record.SenderAddressLine1, record.SenderAddressLine2 });
        Assert.AreEqual(4547, record.TransactionNumber);
        Assert.AreEqual(188.43m, record.TransactionAmount);
    }

    [TestMethod]
    public void SimpleXmlDocumentStartsWithDeclarationAndRoot()
    {
        var document = new SimpleXmlDocument("unused.xml", "root");
        Assert.AreEqual("<?xml version=\"1.0\"?><root />", document.Document.OuterXml);
        Assert.AreSame(document.Document.DocumentElement, document.RootNode);
    }

    [TestMethod]
    public void SimpleXmlDocumentWithoutRootHasOnlyDeclaration()
    {
        var document = new SimpleXmlDocument("unused.xml");
        Assert.AreEqual("<?xml version=\"1.0\"?>", document.Document.OuterXml);
        Assert.IsNull(document.RootNode);
    }

    [TestMethod]
    public void SimpleXmlDocumentAddsChildrenAndAttributes()
    {
        var document = new SimpleXmlDocument("unused.xml", "root");
        var child = document.CreateChildNode(document.RootNode!, "param");
        document.AddAttribute("name", "value", child);
        Assert.AreSame(child, document.RootNode!.FirstChild);
        Assert.AreEqual("<root><param name=\"value\" /></root>", document.RootNode!.OuterXml);
    }

    [TestMethod]
    public void SimpleXmlDocumentWritesMissingAttributeValueAsEmpty()
    {
        var document = new SimpleXmlDocument("unused.xml", "root");
        document.AddAttribute("name", null, document.RootNode!);
        Assert.AreEqual("<root name=\"\" />", document.RootNode!.OuterXml);
    }

    [TestMethod]
    public void SimpleXmlDocumentCannotAddAttributeToTextNode()
    {
        var document = new SimpleXmlDocument("unused.xml", "root");
        var text = document.Document.CreateTextNode("text");
        Assert.ThrowsException<NullReferenceException>(() => document.AddAttribute("name", "value", text));
    }

    [TestMethod]
    public void SimpleXmlDocumentSavesToItsFile()
    {
        var folder = CreateTemporaryFolder();
        try
        {
            var file = Path.Combine(folder, "document.xml");
            var document = new SimpleXmlDocument(file, "root");
            document.AddAttribute("name", "value", document.CreateChildNode(document.RootNode!, "param"));
            document.Save();

            var loaded = new System.Xml.XmlDocument();
            loaded.Load(file);
            Assert.AreEqual("<root><param name=\"value\" /></root>", loaded.DocumentElement!.OuterXml);
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [TestMethod]
    public void NonPersistentXmlDocumentBuildsNodes()
    {
        var document = new NonPersistentXMLDocument("root");
        var loose = document.CreateNode("loose");
        Assert.AreEqual("loose", loose.Name);
        Assert.IsNull(loose.ParentNode);

        var item = document.CreateTextNode(document.RootNode!, "item", "text");
        Assert.AreSame(item, document.RootNode!.FirstChild);
        Assert.AreEqual("<root><item>text</item></root>", document.RootNode!.OuterXml);
    }

    [TestMethod]
    public void NonPersistentXmlDocumentCannotBeSaved()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new NonPersistentXMLDocument("root").Save());
    }

    [TestMethod]
    public void NetworkStartsEmptyAndUnnamed()
    {
        var network = new Network();
        Assert.IsNull(network.Name);
        Assert.AreEqual(0L, network.CellCount);
        Assert.AreEqual("Name=, Size=0", network.ToString());
        Assert.AreEqual("Name=brain, Size=0", new Network("brain").ToString());
    }

    [TestMethod]
    public void NetworkCreatesInterfaceLayersOnce()
    {
        var network = new Network();
        Assert.AreSame(network.InputLayer, network.InputLayer);
        Assert.AreSame(network.OutputLayer, network.OutputLayer);
        Assert.AreSame(network, network.InputLayer.Owner);
        Assert.AreSame(network, network.OutputLayer.Owner);
    }

    [TestMethod]
    public void NetworkKeepsItsCellsAndLayers()
    {
        var network = new Network();
        var cell = network.CreateCell<Neuron>();
        var layer = network.AddMiddleLayer();

        Assert.AreEqual(1L, network.CellCount);
        Assert.AreSame(cell, network.FindCell(cell.UniqueID));
        Assert.AreSame(cell, network.GetRandomCell());
        Assert.ThrowsException<InvalidOperationException>(() => network.FindCell(Guid.NewGuid()));
        CollectionAssert.AreEqual(new[] { layer }, network.AllMiddleLayers.ToArray());
    }

    [TestMethod]
    public void NeuronFiresOnlyForStrongSignals()
    {
        var neuron = new Neuron();
        int fired = 0;
        EventHandler handler = (sender, _) => { if (sender == neuron) Interlocked.Increment(ref fired); };
        Neuron.NeuronExcited += handler;
        try
        {
            neuron.Excite(100);
            Assert.AreEqual(0, fired);
            neuron.Excite(101);
            Assert.AreEqual(1, fired);
        }
        finally
        {
            Neuron.NeuronExcited -= handler;
        }
    }

    [TestMethod]
    public void NeuronWithoutListenersCanFire()
    {
        new Neuron().Excite(500);
    }

    [TestMethod]
    public void NeuronPassesSignalToConnectedNeuron()
    {
        Neuron first = new(), second = new();
        first.ConnectTo(second);
        using var secondFired = new ManualResetEventSlim(false);
        EventHandler handler = (sender, _) => { if (sender == second) secondFired.Set(); };
        Neuron.NeuronExcited += handler;
        try
        {
            first.Excite(150);
            Assert.IsTrue(secondFired.Wait(TimeSpan.FromSeconds(30)));
            Assert.AreEqual(1, first.OutputCount);
            Assert.IsFalse(second.HasOutputs);
        }
        finally
        {
            Neuron.NeuronExcited -= handler;
        }
    }

    [TestMethod]
    public void ModelFactoryUsesRegisteredGenerators()
    {
        var type = "LibraryBehaviorTests-" + Guid.NewGuid();
        var model = new AliModel();
        ModelFactory.AddModelGenerator(type, () => model);

        Assert.AreSame(model, ModelFactory.GenerateModel(type));
        Assert.AreSame(ModelFactory.SAP, ModelFactory.SAP);
        Assert.ThrowsException<ArgumentException>(() => ModelFactory.AddModelGenerator(type, () => model));
        Assert.ThrowsException<KeyNotFoundException>(() => ModelFactory.GenerateModel("unknown-" + Guid.NewGuid()));
    }

    [TestMethod]
    public void QuantStartsEmptyAndKeepsValues()
    {
        var quant = new Quant();
        Assert.AreEqual(0.0, quant.Weight);
        Assert.IsNull(quant.Particle);
        quant.Weight = 0.5;
        quant.Particle = "particle";
        Assert.AreEqual(0.5, quant.Weight);
        Assert.AreEqual("particle", quant.Particle);
    }

    [TestMethod]
    public void LoggerFormatsMissingArgumentsAsEmpty()
    {
        var logger = new RecordingLogger();
        LoggerSAP.RegisterLogger(logger);
        try
        {
            string? missing = null;
            LoggerSAP.Log("[{0}|{1}]", missing, "value");
            Assert.IsTrue(logger.Entries.Single().EndsWith(" [|value]"), logger.Entries.Single());
        }
        finally
        {
            LoggerSAP.UnregisterLogger(logger);
        }
    }

    private sealed class RecordingLogger : ILogger
    {
        public List<string> Entries { get; } = new();
        public void AcceptLogEntry(LogLevel logLevel, string entry) => Entries.Add(entry);
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
