using System;
using System.Windows.Forms;

namespace cwsoft.Textblocks.Helper;

// Enable WaitCursor and set optional control text when entering using block. 
// Reset cursor and control text to initial values at exit.
internal class BlockingTask: IDisposable
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private readonly string? _initialText = null;
   private readonly Cursor? _initialCursor = null;

   // Optional form control gets disposed when parent form terminates.
   private readonly Control? _infoControl = null;

   // Public properties.
   // Access text property of optional form control element.
   public string InfoText {
      get => _infoControl?.Text ?? string.Empty;
      private set {
         if (_infoControl is not null) {
            _infoControl.Text = value;
         }
      }
   }

   // Constructors.
   public BlockingTask(Control? infoControl = null, string? message = null)
   {
      (_infoControl, _initialCursor, Cursor.Current) = (infoControl, Cursor.Current, Cursors.WaitCursor);

      if (infoControl is not null && message is not null) {
         (_initialText, InfoText) = (InfoText, message);
      }
   }
   #endregion

   #region // IDisposable Support
   // Reset cursor and control text to initial values.
   public void Dispose()
   {
      if (_initialText is not null) {
         InfoText = _initialText;
      }

      Cursor.Current = _initialCursor;
      _initialCursor?.Dispose();
   }
   #endregion
}
