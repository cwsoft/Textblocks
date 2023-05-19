using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace cwsoft.Textblocks.Gui;

// This class contains the logic for the main App UI.
public partial class App: Form
{
   #region // Fields, Properties, Constructors
   // Private constants and fields.
   private const int _minimumRequiredSearchPatternLength = 3;
   private Catalog.CatalogManager? _catalogManager = null;
   private List<Model.Textblock> _activeTextblocks = new();
   private readonly Helper.ListFilter _filter = new();

   // Public properties. 
   public string InfoText { get => Rtb_Preview.Text; set => Rtb_Preview.Text = value; }
   public string StatusBarText { get => Lbl_StatusBar.Text; set => Lbl_StatusBar.Text = value; }

   // Constructor. Blocking tasks moved to App_Shown for progress status on start-up.
   public App()
   {
      InitializeComponent();
      Font = new System.Drawing.Font("Segoe UI", 9f, FontStyle.Regular);
      ApplyFormControlDefaults();
   }
   #endregion

   #region // App: Main Entry and Initialization
   // Blocking tasks moved here to allow display of progress status on start-up.
   // Initialize catalog manager, open last used catalog or reset App into defined state.
   private void App_Shown(object sender, EventArgs e)
   {
      // Initialize catalog manager object.
      StatusBarText = "Initialisiere Textblocks ... bitte warten";
      using (new Helper.BlockingTask(Rtb_Preview, "Initialisiere Textblocks ... bitte warten")) {
         _catalogManager = new(infoControl: Rtb_Preview);
         if (!_catalogManager.IsInitialized) {
            // Shutdown App if MS-Word is not accessible (e.g. not installed).
            ShutdownApp();
         }
      }

      // Load last used catalog or reset App into defined state.
      if (File.Exists(Properties.Settings.Default.LastUsedCatalog)) {
         LoadCatalog(catalogPath: Properties.Settings.Default.LastUsedCatalog);
      }
      else {
         ResetCatalog();
      }
   }
   #endregion

   #region // App: Internal API
   // Shutdown App if MS-Word is not accessible during start-up.
   private void ShutdownApp()
   {
      using (new Helper.BlockingTask(Rtb_Preview, "Textblocks wird beendet ... bitte warten")) {
         _ = MessageBox.Show(
            "Konnte keine Verbindung zu MS-Word aufbauen.\n" +
            "Ohne MS-Word ist Textblocks nicht funktionsfähig.",
            "Textblocks wird beendet",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
         );
         Close();
      }
   }

   // Close actual catalog and update status infos.
   private void ResetCatalog()
   {
      // Close actual catalog and release combobox data binding.
      _catalogManager?.CloseCatalog();
      (Cbx_Categories.DataSource, Cbx_Textblocks.DataSource) = (null, null);

      // Reset active textblocks and filter settings and update form controls.
      _activeTextblocks = new();
      _filter?.ResetPattern();
      UpdateFilterControls(new Model.Category());

      // Update menu entries and status infos.
      Men_CloseCatalog.Enabled = false;
      StatusBarText = "Keine gültige Katalogdatei geladen.";
      InfoText = $"Bitte gültige Katalogdatei laden (Datei -> Katalog öffnen) oder Textblocks beenden.";

      // Don't autoload actual catalog on next App start if catalog was reset.
      Properties.Settings.Default.LastUsedCatalog = string.Empty;
   }

   // Load given catalog, extract data and refresh UI.
   private void LoadCatalog(string catalogPath)
   {
      if (_catalogManager is null) {
         return;
      }

      // Load catalog and extract categories and textblocks.
      InfoText = $"Lade Katalog '{Catalog.CatalogManager.GetFilenameOrDefault(catalogPath)}' ... bitte warten";
      StatusBarText = $"Lade '{catalogPath}'";
      bool isCatalogLoaded = false;
      using (new Helper.BlockingTask()) {
         isCatalogLoaded = _catalogManager.OpenCatalog(catalogPath);
      }

      // Update data bindings for category and textblock combobox data.
      Cbx_Categories.DataSource = _catalogManager.Catalog.Categories.ToList();
      Cbx_Textblocks.DataSource = _catalogManager.Catalog.Textblocks.ToList();

      // Update status infos and textblock data for actual catalog.
      if (isCatalogLoaded) {
         StatusBarText = $"{_catalogManager.Catalog}";
         Men_CloseCatalog.Enabled = true;
         Properties.Settings.Default.LastUsedCatalog = catalogPath;

         // Don't apply previous search filters on newly loaded catalog.
         _filter.ResetPattern();
         RefreshTextblocksUI(reloadActiveTextblocks: true);
      }

      // Actual catalog is empty or invalid so we reset the App into a defined state.
      if (!isCatalogLoaded) {
         ResetCatalog();
         InfoText = $"Katalog ungültig. Bitte gültige Katalogdatei laden oder Textblocks beenden.";
         StatusBarText = $"Ungültiger Katalog: '{catalogPath}'";

         // Show warning to user that the specified catalog is invalid.
         _ = MessageBox.Show(
            "Katalogdatei leer oder fehlerhaft.\n" +
            "Bitte Katalog-Struktur und Formatvorlagen prüfen.",
            "Ungültige Katalogdatei",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation
         );
      }
   }

   // Refresh textblocks UI to match actual selected category and textblock and optional search filters.
   private void RefreshTextblocksUI(bool reloadActiveTextblocks = true)
   {
      try {
         // Update textblocks UI based on actual selected category and filter settings.
         if (Cbx_Categories.SelectedItem is Model.Category category) {
            SetActiveTextblocks(reloadActiveTextblocks, category);
            UpdateActualTextblockPreview();
            UpdateFilterControls(category);
         }
      }
      catch (System.Runtime.InteropServices.COMException) {
         // Connection to MS-Word lost (e.g. killed process).
         // Reset app into defined state and let user decide how to proceed.
         ResetCatalog();
         InfoText = $"Verbindung zu MS-Word wurde unterbrochen. Katalogdatei erneut laden oder Textblocks beenden.";
         StatusBarText = "Verbindung zu MS-Word wurde unterbrochen";
      }
   }

   // Set active textblocks for actual category and specified filters.
   private void SetActiveTextblocks(bool reloadActiveTextblocks, Model.Category category)
   {
      if (_catalogManager is not null && reloadActiveTextblocks) {
         _activeTextblocks = _catalogManager.GetTextblocksByCategoryId(category.Id);
         _activeTextblocks = _filter.GetMatches(_activeTextblocks, propertyName: "Content", operatorAnd: Rbt_And.Checked);
         Cbx_Textblocks.DataSource = _activeTextblocks;
      }

      if (_activeTextblocks.Count == 0) {
         Cbx_Textblocks.Text = string.Empty;
         InfoText = "Für die ausgewählte Kategorie und Suchfilter sind keine Textblöcke vorhanden.";
      }
   }

   // Update textblock preview with content of the actual selected textblock.
   private void UpdateActualTextblockPreview()
   {
      if (Cbx_Textblocks.SelectedItem is Model.Textblock textblock) {
         Lbl_Category.Text = $"Kategorie: {_catalogManager?.GetCategoryById(textblock.CategoryId)?.Heading ?? "N/A"}";

         // Copy MS-Word range of actual textblock to clipboard and extract rich text format for preview.
         if (_catalogManager?.GetTextblockDocumentRange(textblock) is Microsoft.Office.Interop.Word.Range range) {
            range.Copy();

            // HACK: Avoid empty textblock preview e.g. due to timing issues with clipboard.
            string rtfText;
            int trials = 0;
            do {
               rtfText = Clipboard.GetText(TextDataFormat.Rtf);
               trials++;
            } while (trials <= 3 && !string.IsNullOrEmpty(range.Text) && string.IsNullOrEmpty(rtfText));

            // Replace generic textblock Id with current textblock Id.
            Rtb_Preview.Rtf = rtfText.Replace(@"1.\tab}", $@"{textblock.Id}.\tab}}");

            // Highlight all and/or search patterns in the actual textblock preview text.
            if (!string.IsNullOrEmpty(_filter.ValidatedPattern) && Rtb_Preview.Rtf.Length > 0) {
               HighlightSearchPatternsMatchesInActualTextblockPreview();
            }
         }
      }
   }

   // Highlight all search pattern matches in the actual textblock preview.
   private void HighlightSearchPatternsMatchesInActualTextblockPreview()
   {
      foreach (string highlightPattern in _filter.InclusivePatterns) {
         int nextPos = Rtb_Preview.Text.IndexOf(highlightPattern, 0, StringComparison.OrdinalIgnoreCase);
         while (nextPos > -1) {
            Rtb_Preview.Select(nextPos, highlightPattern.Length);
            Rtb_Preview.SelectionBackColor = Color.Yellow;
            nextPos = Rtb_Preview.Text
               .IndexOf(highlightPattern, nextPos + highlightPattern.Length, StringComparison.OrdinalIgnoreCase);
         }
      }
   }
   #endregion

   #region // App: Helper to reflect state of form controls
   // Update filter controls to reflect actual state.
   private void UpdateFilterControls(Model.Category category)
   {
      Grp_Textblocks.Text = $"Verfügbare Textblöcke ({_activeTextblocks.Count}/{category.NbrTextblocksInCategory}):";
      Grp_SearchFilter.Text = string.IsNullOrWhiteSpace(_filter.ValidatedPattern)
         ? "Suchfilter inaktiv:"
         : $"Suchfilter aktiviert (Treffer: {_activeTextblocks.Count}):";
      Grp_SearchFilter.ForeColor = string.IsNullOrWhiteSpace(_filter.ValidatedPattern) ? Color.Black : Color.Blue;
      Tbx_SearchFilter.ForeColor = Grp_SearchFilter.ForeColor;
   }

   // Apply default values for some main App controls.
   private void ApplyFormControlDefaults()
   {
      // Show App version in main window title.
      Text = $"Textblocks {Properties.Resources.AppVersion} - © 2018-{DateTime.Now:yyyy} Christian Sommer";

      // Change defaults of status bar tool strip labels.
      StatusBarText = string.Empty;
      Lbl_StatusBar.Alignment = ToolStripItemAlignment.Left;

      // Configure tool tips for filter controls.
      ToolTip.AutoPopDelay = 10000;
      ToolTip.ShowAlways = true;

      string searchTip = "Suchbegriffe werden mit Leerzeichen getrennt (z.B.: begriff1 begriff2).\n" +
         $"Jeder Suchbegriff muss mindestens {_minimumRequiredSearchPatternLength} Zeichen enthalten.\n\n" +
         "UND/ODER ändert das Suchverhalten (ALLE bzw. min. EIN Begriff).\n" +
         "Suchbegriffe mit - am Anfang werden ausgeschlossen (z. B.: -begriff1).\n\n" +
         "Die Suche unterscheidet nicht zwischen Groß- und Kleinschreibung.";
      ToolTip.SetToolTip(Tbx_SearchFilter, searchTip);
      ToolTip.SetToolTip(Lbl_SearchFilter, searchTip);

      ToolTip.SetToolTip(Btn_ApplyFilter, "Filtereinstellungen anwenden (oder [ENTER] drücken).");
      ToolTip.SetToolTip(Btn_ResetFilter, "Filtereinstellungen löschen (oder [ESC] drücken).");

      ToolTip.SetToolTip(Rbt_And, "Zeigt Textblöcke die ALLE Suchbegriffe enthalten.");
      ToolTip.SetToolTip(Rbt_Or, "Zeigt Textblöcke die min. EINEN Suchbegriff enthalten.");
   }
   #endregion

   #region // App: Control Event Handler
   // Store last opened catalog path and dispose catalog object when form is closed.
   private void App_FormClosing(object sender, FormClosingEventArgs e)
   {
      // Save possible actual loaded catalog for autoload on next startup.
      Properties.Settings.Default.Save();

      // Free managed and unmanaged resources.
      _catalogManager?.Dispose();
   }

   // Don't use _SelectionChange event to avoid updates triggerd by application code.
   private void Cbx_Categories_SelectionChangeCommitted(object sender, EventArgs e) => RefreshTextblocksUI(reloadActiveTextblocks: true);

   // Don't use _SelectionChange event to avoid updates triggerd by application code.
   private void Cbx_Textblocks_SelectionChangeCommitted(object sender, EventArgs e) => RefreshTextblocksUI(reloadActiveTextblocks: false);

   // Re-apply filter, if filter option (AND/OR) changes after filter was enabled.
   private void Rbt_And_CheckedChanged(object sender, EventArgs e)
   {
      if (!string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         Btn_ApplyFilter.PerformClick();
      }
   }

   // Apply filter if ENTER key is pressed inside search filter textbox.
   // Reset filter if filter pattern changes after the filter was enabled.
   private void Tbx_SearchFilter_KeyDown(object sender, KeyEventArgs e)
   {
      // Reset filter if filter pattern was changed.
      if (!string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         _filter.ResetPattern();
         RefreshTextblocksUI(reloadActiveTextblocks: true);
      }

      // Apply filter, if return key is pressed inside search filter textbox.
      if (e.KeyCode == Keys.Enter) {
         Btn_ApplyFilter.PerformClick();

         // Suppress enter for App form to avoid beep sound.
         (e.Handled, e.SuppressKeyPress) = (true, true);
         return;
      }
   }

   // Validate user defined search pattern and refresh form to reflect filter settings.
   private void Btn_ApplyFilter_Click(object sender, EventArgs e)
   {
      // Set new filter and validate search patterns (supporting boolean operators).
      _filter.SetPattern(Tbx_SearchFilter.Text, _minimumRequiredSearchPatternLength);

      // Enable filter mode, if initial and validated search patterns match.
      if (_filter.InputPattern == _filter.ValidatedPattern) {
         RefreshTextblocksUI(reloadActiveTextblocks: true);
         if (_activeTextblocks.Count > 0) {
            _ = Cbx_Textblocks.Focus();
         }
         return;
      }

      // Disable filter mode and show warning, if no or only invalid search patterns were specified.
      if (string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         _ = MessageBox.Show("Die eingegebenen Suchparameter sind ungültig.\n" +
            $"Jeder Suchparameter muss mindestens {_minimumRequiredSearchPatternLength} Zeichen enthalten."
            , "Filter kann nicht angewandt werden"
            , MessageBoxButtons.OK, MessageBoxIcon.Information
         );
         return;
      }

      // If at least one valid sub pattern was specified, let user decide how to proceed.
      if (MessageBox.Show("Einige Suchparameter sind ungültig.\n" +
         $"Validierte Parameter: '{_filter.ValidatedPattern}'.\n\n" +
         "Suche mit den validierten Parametern durchführen?", "Filter mit validierten Parametern anwenden",
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes
      ) {
         Tbx_SearchFilter.Text = _filter.ValidatedPattern;
         RefreshTextblocksUI(reloadActiveTextblocks: true);
         if (_activeTextblocks.Count > 0) {
            _ = Cbx_Textblocks.Focus();
         }
         return;
      }

      // User rejected to apply validated sub pattern, so we invalidate it.
      RefreshTextblocksUI(reloadActiveTextblocks: true);
      return;
   }

   // Reset filter settings and refresh to show all textblocks of selected category.
   private void Btn_ResetFilter_Click(object sender, EventArgs e)
   {
      _filter.ResetPattern();
      Tbx_SearchFilter.Clear();
      Rbt_And.Checked = true;
      RefreshTextblocksUI(reloadActiveTextblocks: true);
   }
   #endregion

   #region // App: Menu Event Handler
   // Open catalog selected via file open dialog.
   private void Men_OpenCatalog_Click(object sender, EventArgs e)
   {
      if (_catalogManager is not null) {
         string documentPath = Catalog.CatalogManager.SelectCatalog();
         if (!string.IsNullOrEmpty(documentPath)) {
            InfoText = $"Lade Katalog '{Catalog.CatalogManager.GetFilenameOrDefault(documentPath)}' ... bitte warten";
            LoadCatalog(documentPath);
         }
      }
   }

   // Close actual catalog.
   private void Men_CloseCatalog_Click(object sender, EventArgs e) => ResetCatalog();

   // Quit App.
   private void Men_Quit_Click(object sender, EventArgs e) => Close();

   // Open App manual if exists.
   private void Men_Help_Click(object sender, EventArgs e)
   {
      // Assume help file "Textblocks.pdf" is located in the application startup folder.
      string helpFile = $@"{System.Windows.Forms.Application.StartupPath}\Textblocks.pdf";
      try {
         _ = System.Diagnostics.Process.Start(helpFile);
      }
      catch (Exception) {
         _ = MessageBox.Show($"Hilfedatei konnte nicht geladen werden.\nPfad: {helpFile}",
            "Hilfedatei kann nicht geladen werden",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation
         );
      }
   }

   // Show App about dialoge.
   private void Men_About_Click(object sender, EventArgs e) => new About().ShowDialog();
   #endregion
}
