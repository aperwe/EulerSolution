using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

namespace QBits.Intuition.UI
{
    /// <summary>
    /// Extension methods for convenient access to various UI elements.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Invokes the specified piece of code on the UI thread that owns this UI element.
        /// </summary>
        /// <param name="me">DispatcherObject that has a dispatcher.</param>
        /// <param name="method">Piece of code to be executed on UI thread.</param>
        public static void InvokeOnUIThread(this DispatcherObject me, Action method)
        {
            me.Dispatcher.Invoke(method);
        }

        /// <summary>
        /// Converts the contents to a list of objects that can be used in LinQ expressions.
        /// The resulting list contains only those elements which are of the indicated type.
        /// </summary>
        public static List<T> ToList<T>(this ItemCollection me) where T : UIElement
        {
            var retVal = new List<T>();
            foreach (var item in me)
            {
                if (item is T) retVal.Add((T)item);
            }
            return retVal;
        }

        /// <summary>
        /// Converts the contents to a list of objects that can be used in LinQ expressions.
        /// The resulting list contains only those elements which are of the indicated type.
        /// </summary>
        public static List<T> ToList<T>(this UIElementCollection me) where T : UIElement
        {
            var retVal = new List<T>();
            foreach (var item in me)
            {
                if (item is T) retVal.Add((T)item);
            }
            return retVal;
        }

        /// <summary>
        /// Adds the specified UIElement to the collection and returns a refrence to the added object.
        /// Useful if you add a UIElement item by constructing it, but you later want to customize it (and you need a reference to it).
        /// </summary>
        /// <typeparam name="T">Type of UIElement that is being added (must derive from UIElement).</typeparam>
        /// <param name="me">UI element collection to which a new child is being added.</param>
        /// <param name="child">Child UIElement being added.</param>
        /// <returns>Reference to <paramref name="child"/>.</returns>
        public static T AddAndReference<T>(this UIElementCollection me, T child) where T : UIElement
        {
            return me[me.Add(child)] as T;
        }

        /// <summary>
        /// This is a macro (convenience) method.
        /// <para/> It adds a new child item to the grid (parent) at the specified row index in the parent grid.
        /// </summary>
        /// <param name="parent">Parent <see cref="Grid"/> view that will contain the specified child item.</param>
        /// <param name="gridRow">Row in the parent grid where the new child item should be placed.</param>
        /// <param name="child">Item to be added as child of the specified grid.</param>
        /// <typeparam name="T">UIElement to add to the grid.</typeparam>
        /// <returns>Reference to the constructed tree view.</returns>
        public static T AddGridItem<T>(this Grid parent, int gridRow, T child) where T : UIElement
        {
            var retVal = parent.Children.AddAndReference(child);
            Grid.SetRow(retVal, gridRow);
            return retVal;
        }

        /// <summary>
        /// This is a macro (convenience) method.
        /// <para/> It adds a new child item to the grid (parent) at the specified row and column index in the parent grid.
        /// </summary>
        /// <param name="parent">Parent <see cref="Grid"/> view that will contain the specified child item.</param>
        /// <param name="gridRow">Row in the parent grid where the new child item should be placed.</param>
        /// <param name="gridColumn">Column in the parent grid where the new child item should be placed.</param>
        /// <param name="child">Item to be added as child of the specified grid.</param>
        /// <typeparam name="T">UIElement to add to the grid.</typeparam>
        /// <returns>Reference to the constructed tree view.</returns>
        public static T AddGridItem<T>(this Grid parent, int gridRow, int gridColumn, T child) where T : UIElement
        {
            var retVal = parent.Children.AddAndReference(child);
            Grid.SetRow(retVal, gridRow);
            Grid.SetColumn(retVal, gridColumn);
            return retVal;
        }
        /// <summary>
        /// Helper to search up the VisualTree
        /// </summary>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <returns>First ancestor with the specified type or null if no such ancestor exists.</returns>
        public static T FindAncestor<T>(this DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

    }
}
