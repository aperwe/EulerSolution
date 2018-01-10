using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace QBits.Intuition.UI
{
    /// <summary>
    /// Helper methods for working with Windows Forms applications.
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Updates the form's icon to the application icon.
        /// </summary>
        /// <param name="uiWindow">Windows Form, on which to set the icon.</param>
        /// <param name="resourceAssembly">Assembly contianing the icon resource to load.</param>
        /// <param name="resourceName">Name of the icon resource. <para/>This is typically filename preceeded by assembly's namespace.</param>
        public static void UpdateIcon(Form uiWindow, Assembly resourceAssembly, string resourceName)
        {
            //Get icon from assembly.
            var resStream = resourceAssembly.GetManifestResourceStream(resourceName);
            Icon resIcon = new Icon(resStream);
            uiWindow.Icon = resIcon;
        }
        /// <summary>
        /// Sets the parameters of the window so that it looks as typical dialog window.
        /// <para/>This means:
        /// <para/>Window is not resizable.
        /// <para/>Sizing cursors are not shown when mouse pointer hovers on window border.
        /// <para/>Window does not show on Windows taskbar.
        /// </summary>
        /// <param name="uiWindow">Window to set as dialog.</param>
        public static void SetWindowStateToDialog(Form uiWindow)
        {
            uiWindow.MaximizeBox = false;
            uiWindow.MaximumSize = uiWindow.Size;
            uiWindow.MinimumSize = uiWindow.Size;
            uiWindow.FormBorderStyle = FormBorderStyle.FixedDialog;
            uiWindow.ShowInTaskbar = false;
        }
    }
}
