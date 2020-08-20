using System.Linq;
using System.Windows.Forms;

namespace NAVService
{
    public partial class ExplorerForm : Form
    {
        private static System.Data.DataTable ParentTable = new System.Data.DataTable { TableName = "parentTable", Locale = UserHelper.culture };
        private static System.Data.DataTable ResultTable = new System.Data.DataTable { TableName = "resultTable", Locale = UserHelper.culture };
        private static System.Data.DataTable ReworkTable = new System.Data.DataTable { TableName = "reworkTable", Locale = UserHelper.culture };

        private static decimal CumulativeVariationsCount { get; set; }
        private static decimal DuplicateRowsCount { get; set; }
        private static decimal ProgressPercentage { get; set; }
        private static string ProgressSummary { get; set; }
        private static bool ParseComparison { get; set; }
        private static bool ParseComplete { get; set; }
        private static bool InitialiseDisplay { get; set; }
        private static int SheetDataGridViewRowCount { get; set; }
        private static int PanelPosition { get; set; }

        private static decimal GetProgressPercentage() { return ProgressPercentage; }
        private static decimal GetCumulativeVariationsCount() { return CumulativeVariationsCount; }
        private static string GetProgressDescription() { return ProgressSummary; }
        private static void SetProgressDescription(string value) => ProgressSummary = value;
        private static void SetCumulativeVariationsCount(decimal value) => CumulativeVariationsCount = value;
        private static void SetReworkTable(System.Data.DataTable value) => ReworkTable = value;
        private static void SetProgressPercentage(decimal value) => ProgressPercentage = value;
        private static void SetSheetDataGridViewRowCount(int value) => SheetDataGridViewRowCount = value;
        private static void SetInitialiseDisplay(bool value) => InitialiseDisplay = value;
        private static void SetParseComplete(bool value) => ParseComplete = value;
        private static void SetProgressBarIndex(int value) => ProgressBar.Value = value;
        private static void SetPanelPosition(int value) => PanelPosition = value;
        private static int GetPanelPosition() { return PanelPosition; }
        private static int ActionPerformed = Constants.ACTION_PARSE;

        private static NAVProgressBar ProgressBar;
        private static Control ProgressCancel;
        private static Control ProgressPanel;
        private static Control ProgressHeader;
        private static Control ProgressDescription;
        private static Control ParseRowsButton;
        private static Control DeleteRowsButton;

        private static void SetParseAbbreviations(bool value) => ParseAbbreviations = value;
        private static void SetResultTable(System.Data.DataTable value) => ResultTable = value;
        private static void SetParentTable(System.Data.DataTable value) => ParentTable = value;
        private static int RecreateParentTable() { return CreateParentTable(); }

        private decimal PercentageInterest = decimal.Parse(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PERCENTAGE_INTEREST), UserHelper.culture);
        private bool MakeCaseInsensitive = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_APPLY_CASE_INSENSITIVITY));
        private bool PadToEqualLength = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PAD_TEXT));
        private bool RemoveWhitespace = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REMOVE_NOISE_CHARACTERS));
        private bool ReverseComparison = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REVERSE_COMPARE));
        private bool MatchAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_MATCH_ABBREVIATIONS));
        private bool PhoneticFilter = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PHONETIC_FILTER));
        private bool WholeWordComparison = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_WHOLE_WORD_MATCH));

        private int MatchingAlgorithm = SetComparisonType(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_MATCHING_ALGORITHM));

        internal static DataGridView GetResultDataGridView() { return ResultDataGridView; }
        internal static void SetComparisonReset(bool value) => ParseComparison = value;
        internal static bool ParseAbbreviations { get; private set; }
        internal static int GetComparisonType(string nvUserPreferenceValue) { return SetComparisonType(nvUserPreferenceValue); }
        internal static void SetDuplicateRowsCount(decimal value) => DuplicateRowsCount = value;

        public ExplorerForm()
        {
            InitializeComponent();
            InitializeExplorerForm();
        }

        internal static void SuspendResultDataGridView()
        {
            ResultDataGridView.SuspendLayout();
            ClassLibraryFramework.DrawingInteropServices.SuspendDrawing(ResultDataGridView);
        }

        internal static void ResumeResultDataGridView()
        {
            ResultDataGridView.ResumeLayout();
            ClassLibraryFramework.DrawingInteropServices.ResumeDrawing(ResultDataGridView);
        }

        internal static void SheetDataGridViewCellValueChanged()
        {
            SetDuplicateRowsCount(0);
            SetComparisonReset(true);
            SetParseComplete(false);
            ReworkTable.Reset();
            ComparisonParametersReset();
        }

        internal static void DeleteSelectedRows(System.Collections.ArrayList keyArray = null)
        {
            System.Collections.ArrayList SelectedRows = keyArray ?? ClassLibraryFramework.DataGridViewMethods.GetSelectedColumnValue(NAVForm.GetSheetDataGridView(), Constants.KEY_COLUMN);

            WorkTables.DeleteDataTableRows(SelectedRows, ResultTable, ParentTable);
            WorkTables.AcceptDataGridViewChanges();

            SetProgressDescription(null);
            SetResultDataGridViewFocus();
        }

        internal static void ResetExplorerForm()
        {
            if (GetResultDataGridView() != null)
            {
                SetInitialiseDisplay(true);
                SetParseComplete(false);

                ReworkTable.Reset();
                ComparisonParametersReset();
                PreferenceDataGridViewDisable();
                PreferenceDescriptionDisable();
            }

        }

        private void InitializeExplorerForm()
        {
            InitialiseAppearance();
            InitialiseDataGridViews();
            InitialiseAbbreviatedLabel();
            InitialiseProgressDelegates();
            InitialiseProgressForm();
            InitialiseFormControls();
            InitialisePreferenceDelegates();
            initialiseToolTips();

            SetInitialiseDisplay(false);

            void InitialiseAppearance()
            {
                BackColor = Constants.COLOR_DEFAULT_BACKCOLOR;
                ProgressBarPanel.BackColor = Constants.COLOR_DEFAULT_BACKCOLOR;
            }

            void InitialiseProgressForm()
            {
                SetPanelPosition(ControlPlaceHolder());
                SetProgressBarIndex(0);

                ProgressBar.CustomText = null;
                ProgressBar.DisplayStyle = ProgressBarDisplayText.CustomText;
                ProgressBar.Style = ProgressBarStyle.Continuous;
                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = 100;
                ProgressBar.Step = 1;

                ProgressCancel.Location = new System.Drawing.Point(375, 295);
                ProgressCancel.Enabled = false;
                ProgressDisplayDisable();
            }

            int ControlPlaceHolder()
            {
                System.Drawing.Point control = ClassLibraryFramework.ScreenAttributes.GetControlLocation(ProgressPanel);
                System.Drawing.Rectangle screen = ClassLibraryFramework.ScreenAttributes.GetScreenDimensions(this);

                return control.X - (screen.Width + (control.Y * 2));
            }

            void InitialiseProgressDelegates()
            {
                ProgressCancel = CancelProgressButton;
                ProgressPanel = ProgressBarPanel;
                ProgressBar = navProgressBar;
            }

            void InitialisePreferenceDelegates()
            {
                ProgressHeader = Controls[Constants.CONTROL_PREFERENCEHEADER_SYMBOL];
                ProgressDescription = Controls[Constants.CONTROL_PREFERENCEDESCRIPTION_SYMBOL];
                ParseRowsButton = Controls[Constants.CONTROL_PARSEBUTTON_SYMBOL];
                DeleteRowsButton = Controls[Constants.CONTROL_DELETEBUTTON_SYMBOL];
            }

            void InitialiseAbbreviatedLabel()
            {
                AbbreviatedLabel.Location = new System.Drawing.Point(375, 313);
                AbbreviatedLabel.MouseEnter += DrawControls.LabelBackColorChange;
                AbbreviatedLabel.MouseLeave += DrawControls.LabelBackColorChange;
            }

            void InitialiseDataGridViews()
            {
                NAVForm.GetSheetDataGridView().Sorted += (object sender, System.EventArgs e) => DrawControls.SetDataGridViewRowBackgroundColour();

                ResultDataGridView.BackgroundColor = Constants.COLOR_DEFAULT_BACKCOLOR;
                ResultDataGridView.Location = new System.Drawing.Point(0, 327);
                ResultDataGridView.Size = new System.Drawing.Size(453, 50);
                ResultDataGridView.Visible = false;
                ResultDataGridView.TabIndex = 1;

                ResultDataGridView.Sorted += (object sender, System.EventArgs e) => DrawControls.SetDataGridViewRowBackgroundColour();
                ResultDataGridView.Enter += (object sender, System.EventArgs e) => ResetOnCompletion();
                ResultDataGridView.VisibleChanged += ResultDataGridViewVisibleChangedEvent;
                ResultDataGridView.CellContentClick += (object sender, DataGridViewCellEventArgs e) => DrawControls.DataGridViewCellContentClick();
                ResultDataGridView.MouseDown += ResultDataGridViewMouseDownEvent;
            }

            void initialiseToolTips()
            {
                ExplorerToolTips.SetToolTip(AbbreviatedLabel, Properties.Resources.TOOLTIP_ABBREVIATIONLABEL);
                ExplorerToolTips.SetToolTip(ProgressCancel, Properties.Resources.TOOLTIP_CANCELPROGRESSBUTTON);

                bDeleteRowFlag.ToolTipText = Properties.Resources.TOOLTIP_DELETEROWFLAGTEXT;
                iCompareValue.ToolTipText = Properties.Resources.TOOLTIP_COMPAREVALUETEXT;
                nvMatchType.ToolTipText = Properties.Resources.TOOLTIP_MATCHTYPETEXT;
                nvExplorerString.ToolTipText = Properties.Resources.TOOLTIP_EXPLORERSTRINGTEXT;
            }

            void InitialiseFormControls()
            {
                SuspendLayout();

                int axisY = 7;

                if (PreferencesForm.UserPreferencesDataTable.Rows.Count > 0)
                {
                    for (int rowIndex = 0; rowIndex < PreferencesForm.UserPreferencesDataTable.Rows.Count; rowIndex++)
                    {
                        if (PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString() == Constants.DB_SEARCH_PREFERENCES)
                        {
                            SetPreferenceAttribute(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString(), GetUserPreferenceValue(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE], PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_FLAG], ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_VALUE_REQUIRED_FLAG].ToString())));

                            if (GroupHeaderRequired(rowIndex))
                            {
                                GroupHeaderInsert(rowIndex, axisY);
                                axisY += 19;
                            }

                            PreferenceNameInsert(rowIndex, axisY);
                            PreferenceValueInsert(rowIndex, axisY);

                            axisY += 19;
                        }
                    }
                }

                CreateParseTableButton(axisY += 10);
                CreateDeleteRowsButton(axisY);
                PreferenceDescriptionHeaderInsert(axisY += 35);
                PreferenceDescriptionInsert(axisY += 19);

                ResumeLayout();
            }

            string GetUserPreferenceValue(object nvUserPreferenceValue, object bUserPreference, bool bClientValueRequired)
            {
                return (bClientValueRequired) ? (string.IsNullOrWhiteSpace(nvUserPreferenceValue.ToString().Trim()) ? null : nvUserPreferenceValue.ToString().Trim()) : (ClassLibraryStandard.HelperMethods.ToBoolean(bUserPreference.ToString()) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO);
            }

            bool GroupHeaderRequired(int rowIndex)
            {
                if (PreferencesForm.UserPreferencesDataTable != null)
                {
                    if (rowIndex == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return PreferencesForm.UserPreferencesDataTable.Rows[rowIndex - 1][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim() != PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim();
                    }
                }

                return false;
            }

            void CreateParseTableButton(int axisY)
            {
                Button ParseWorksheetRowsButton = new Button
                {
                    Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                    Name = Constants.CONTROL_PARSEBUTTON_SYMBOL,
                    Location = new System.Drawing.Point(340, axisY),
                    Size = new System.Drawing.Size(110, 23),
                    Text = Properties.Resources.CONTROL_SEARCHBUTTON_TEXT,
                    UseVisualStyleBackColor = true,
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 100
                };

                ExplorerToolTips.SetToolTip(ParseWorksheetRowsButton, Properties.Resources.TOOLTIP_PARSEWORKSHEETBUTTON);

                ParseWorksheetRowsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
                ParseWorksheetRowsButton.Click += ParseWorksheetClick;
                ParseWorksheetRowsButton.Enter += (object sender, System.EventArgs e) => ActionPerformed = Constants.ACTION_PARSE;

                Controls.Add(ParseWorksheetRowsButton);
            }

            void CreateDeleteRowsButton(int axisY)
            {
                Button DeleteWorksheetRowsButton = new Button
                {
                    Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                    Name = Constants.CONTROL_DELETEBUTTON_SYMBOL,
                    Location = new System.Drawing.Point(225, axisY),
                    Size = new System.Drawing.Size(110, 23),
                    Text = Properties.Resources.CONTROL_REMOVEROWSBUTTON_TEXT,
                    UseVisualStyleBackColor = true,
                    Enabled = false,
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 99
                };

                ExplorerToolTips.SetToolTip(DeleteWorksheetRowsButton, Properties.Resources.TOOLTIP_DELETEWORKSHEETROWSBUTTON);

                DeleteWorksheetRowsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
                DeleteWorksheetRowsButton.Enter += (object sender, System.EventArgs e) => ActionPerformed = Constants.ACTION_REMOVE;
                DeleteWorksheetRowsButton.Click += (object sender, System.EventArgs e) => DeleteWorksheetRowsButtonClick();

                Controls.Add(DeleteWorksheetRowsButton);
            }

            void GroupHeaderInsert(int rowIndex, int axisY)
            {
                string controlName = Constants.CONTROL_DIVIDER_SYMBOL + PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();

                TextBox DividerTextBox = new TextBox
                {
                    Name = controlName,
                    Location = new System.Drawing.Point(0, axisY),
                    Text = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim(),
                    Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right),
                    BackColor = Constants.COLOR_MATCH_ASSOCIATE,
                    ForeColor = Constants.COLOR_DEFAULT_TEXT,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Arrow,
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    RightToLeft = RightToLeft.No,
                    Size = new System.Drawing.Size(451, 20),
                    TabIndex = rowIndex + 500
                };

                DividerTextBox.GotFocus += (s1, e1) => { ClassLibraryStandard.TextBoxInteropServices.HideCaret(DividerTextBox.Handle); };

                DividerTextBox.Enter += PreferenceDescriptionLeave;
                DividerTextBox.Enter += PreferencesForm.TextBoxBackColorChange;
                DividerTextBox.Leave += PreferencesForm.TextBoxBackColorChange;
                DividerTextBox.KeyDown += TextBoxKeyDown;

                Controls.Add(DividerTextBox);
            }

            void PreferenceNameInsert(int rowIndex, int axisY)
            {
                string controlName = Constants.CONTROL_PREFERENCE_SYMBOL + PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();

                TextBox PreferenceTextBox = new TextBox
                {
                    Name = controlName,
                    Location = new System.Drawing.Point(0, axisY),
                    Text = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim(),
                    Anchor = ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left))),
                    BackColor = Constants.COLOR_MATCH_ASSOCIATE,
                    ForeColor = Constants.COLOR_DEFAULT_TEXT,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Arrow,
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    RightToLeft = RightToLeft.No,
                    Size = new System.Drawing.Size(135, 20),
                    AccessibleDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                    AccessibleName = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim(),
                    TabIndex = rowIndex + 500
                };

                PreferenceTextBox.GotFocus += (s1, e1) => { ClassLibraryStandard.TextBoxInteropServices.HideCaret(PreferenceTextBox.Handle); };

                PreferenceTextBox.Enter += PreferencesForm.TextBoxBackColorChange;
                PreferenceTextBox.Enter += PreferenceDescriptionEnter;
                PreferenceTextBox.Leave += PreferencesForm.TextBoxBackColorChange;
                PreferenceTextBox.KeyDown += TextBoxKeyDown;

                Controls.Add(PreferenceTextBox);
            }

            void PreferenceValueInsert(int rowIndex, int axisY)
            {
                string controlName = "navValue" + PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();
                string nvClientPreferenceName = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim();

                bool bClientOverride = ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_OVERRIDE_FLAG].ToString());
                bool bSystemOverride = ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_SYSTEM_OVERRIDE_FLAG].ToString());

                if (ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_VALUE_REQUIRED_FLAG].ToString()))
                {
                    switch (nvClientPreferenceName)
                    {
                        case Constants.DB_MATCHING_ALGORITHM:
                            BuildListValueComboBox();
                            break;

                        default:
                            BuildValueTextBox();
                            break;
                    }
                }
                else
                {
                    BuildValueComboBox();
                }

                void BuildListValueComboBox()
                {
                    ClassLibraryFramework.ComboBoxEx ListValueComboBox = new ClassLibraryFramework.ComboBoxEx
                    {
                        Name = controlName,
                        Location = new System.Drawing.Point(134, axisY - 1),
                        Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        BackColor = Constants.COLOR_DEFAULT,
                        ForeColor = (bClientOverride || bSystemOverride) ? System.Drawing.Color.DimGray : System.Drawing.Color.Black,
                        FormattingEnabled = true,
                        RightToLeft = RightToLeft.No,
                        Size = new System.Drawing.Size(317, 21),
                        AccessibleDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        AccessibleName = nvClientPreferenceName,
                        TabIndex = rowIndex
                    };

                    switch (nvClientPreferenceName)
                    {
                        case Constants.DB_MATCHING_ALGORITHM:
                            ListValueComboBox.Items.AddRange(new object[] { Constants.METHOD_RATCLIFF_OBERSHELP, Constants.METHOD_LEVENSHTEIN_DISTANCE, Constants.METHOD_HAMMING_DISTANCE });
                            ListValueComboBox.SelectedIndex = MatchingAlgorithm;
                            break;

                        default:
                            ListValueComboBox.Items.AddRange(new object[] { Properties.Resources.FIELD_VALUE_YES, Properties.Resources.FIELD_VALUE_NO });
                            ListValueComboBox.SelectedIndex = ListValueComboBox.Items.IndexOf(ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_FLAG].ToString()) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO);
                            break;
                    }

                    ListValueComboBox.Enter += PreferenceDescriptionEnter;
                    if (bClientOverride || bSystemOverride) ListValueComboBox.Enabled = false;
                    if (ListValueComboBox.Enabled) ListValueComboBox.TextChanged += UpdatePreferenceAttribute;

                    Controls.Add(ListValueComboBox);
                }

                void BuildValueTextBox()
                {
                    TextBox ValueTextBox = new TextBox
                    {
                        Name = controlName,
                        Location = new System.Drawing.Point(134, axisY),
                        Text = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        BackColor = Constants.COLOR_DEFAULT,
                        ForeColor = (bClientOverride || bSystemOverride) ? System.Drawing.Color.DimGray : System.Drawing.Color.Black,
                        MaxLength = 1000,
                        RightToLeft = RightToLeft.No,
                        Size = new System.Drawing.Size(317, 20),
                        AccessibleDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        AccessibleName = nvClientPreferenceName,
                        TabIndex = rowIndex
                    };

                    ValueTextBox.Enter += PreferenceDescriptionEnter;
                    if (bClientOverride || bSystemOverride) ValueTextBox.Enabled = false;
                    if (ValueTextBox.Enabled) ValueTextBox.Leave += UpdatePreferenceAttribute;

                    Controls.Add(ValueTextBox);
                }

                void BuildValueComboBox()
                {
                    ClassLibraryFramework.ComboBoxEx ValueComboBox = new ClassLibraryFramework.ComboBoxEx
                    {
                        Name = controlName,
                        Location = new System.Drawing.Point(134, axisY - 1),
                        Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        BackColor = Constants.COLOR_DEFAULT,
                        ForeColor = (bClientOverride || bSystemOverride) ? System.Drawing.Color.DimGray : System.Drawing.Color.Black,
                        FormattingEnabled = true,
                        RightToLeft = RightToLeft.No,
                        Size = new System.Drawing.Size(317, 21),
                        AccessibleDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        AccessibleName = nvClientPreferenceName,
                        TabIndex = rowIndex
                    };

                    ValueComboBox.Items.AddRange(new object[] { Properties.Resources.FIELD_VALUE_YES, Properties.Resources.FIELD_VALUE_NO });
                    ValueComboBox.SelectedIndex = ValueComboBox.Items.IndexOf(ClassLibraryStandard.HelperMethods.ToBoolean(PreferencesForm.UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_FLAG].ToString()) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO);
                    ValueComboBox.Enter += PreferenceDescriptionEnter;
                    if (bClientOverride || bSystemOverride) ValueComboBox.Enabled = false;
                    if (ValueComboBox.Enabled) ValueComboBox.TextChanged += UpdatePreferenceAttribute;

                    Controls.Add(ValueComboBox);
                }

            }

            void PreferenceDescriptionInsert(int axisY)
            {
                TextBox PreferenceDescription = new TextBox
                {
                    Visible = true,
                    Name = Constants.CONTROL_PREFERENCEDESCRIPTION_SYMBOL,
                    Text = null,
                    Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right),
                    BackColor = Constants.COLOR_DEFAULT_BACKCOLOR,
                    BorderStyle = BorderStyle.None,
                    Cursor = Cursors.Arrow,
                    ReadOnly = true,
                    Location = new System.Drawing.Point(0, axisY),
                    Margin = new Padding(3, 3, 3, 3),
                    Multiline = true,
                    Size = new System.Drawing.Size(451, 85),
                    TabIndex = 0,
                    TabStop = false
                };

                PreferenceDescription.GotFocus += (s1, e1) => { ClassLibraryStandard.TextBoxInteropServices.HideCaret(PreferenceDescription.Handle); };

                Controls.Add(PreferenceDescription);
            }


            void PreferenceDescriptionHeaderInsert(int axisY)
            {
                TextBox PreferenceDescriptionHeader = new TextBox
                {
                    Visible = true,
                    Name = Constants.CONTROL_PREFERENCEHEADER_SYMBOL,
                    Text = null,
                    Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right),
                    BackColor = Constants.COLOR_DEFAULT_BACKCOLOR,
                    BorderStyle = BorderStyle.None,
                    Cursor = Cursors.Arrow,
                    ReadOnly = true,
                    Location = new System.Drawing.Point(0, axisY),
                    Margin = new Padding(3, 3, 3, 3),
                    Multiline = false,
                    Size = new System.Drawing.Size(451, 85),
                    TabIndex = 0,
                    TabStop = false
                };

                PreferenceDescriptionHeader.GotFocus += (s1, e1) => { ClassLibraryStandard.TextBoxInteropServices.HideCaret(PreferenceDescriptionHeader.Handle); };

                Controls.Add(PreferenceDescriptionHeader);
            }

            void DeleteWorksheetRowsButtonClick()
            {
                RemoveWorksheetRowsButtonClick();
                //RemoveRowsFlaggedForDeletion(true);
                ResetOnCompletion(true);
            }

        }

        private static void ComparisonParametersReset()
        {
            SetComparisonReset(true);
            SetSheetDataGridViewRowCount(0);
            SetDuplicateRowsCount(0);
            SetProgressIndices();
        }

        private static void SetProgressIndices()
        {
            SetProgressPercentage(0);
            SetProgressBarIndex(0);
        }

        private static void PreferenceDataGridViewDisable()
        {
            AbbreviatedLabel.Visible = false;
            ResultDataGridView.Visible = false;

            ClassLibraryFramework.DataGridViewMethods.SetBackgroundColour(NAVForm.GetSheetDataGridView(), Constants.COLOR_DEFAULT);
        }

        private static void PreferenceDescriptionDisable()
        {
            ProgressHeader.Text = null;
            ProgressDescription.Text = null;
            ProgressHeader.Visible = false;
            ProgressDescription.Visible = false;
            ProgressHeader.Refresh();
            ProgressDescription.Refresh();
        }

        private static void PreferenceDescriptionEnable()
        {
            ProgressHeader.Visible = true;
            ProgressHeader.Refresh();
            ProgressDescription.Visible = true;
            ProgressDescription.Refresh();
        }

        private static void SetResultDataGridViewFocus()
        {
            DrawControls.SetDataGridViewRowBackgroundColour();
            ResultDataGridView.CurrentCell = ResultDataGridView.FirstDisplayedCell;
            ResultDataGridView.Focus();
        }

        private static void InitializeProgressDisplay(string header = null, string description = null)
        {
            ProgressHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ProgressHeader.Text = header;
            ProgressDescription.Text = description;

            PreferenceDescriptionEnable();
        }

        private static void ResultDisplayDisableAll(bool bDisableDescription = true)
        {
            if (bDisableDescription) PreferenceDescriptionDisable();

            PreferenceDataGridViewDisable();
            ProgressDisplayDisable();
            SetProgressIndices();
        }

        private static void ProgressDisplayEnable(bool bPermitCancel)
        {
            if (bPermitCancel) ProgressCancel.Size = new System.Drawing.Size(75, 23);
            ProgressCancel.Refresh();
            ProgressPanel.Location = new System.Drawing.Point(0, 283);
            ProgressPanel.Refresh();
        }

        private static void ProgressDisplayDisable()
        {
            ProgressPanel.Left = GetPanelPosition();
            ProgressCancel.Size = new System.Drawing.Size(0, 0);
        }

        private static int CreateParentTable()
        {
            if (!ParseComplete)
            {
                ParentTable = WorkTables.BuildParentTable();
                SetSheetDataGridViewRowCount(ParentTable.Rows.Count);
            }

            return SheetDataGridViewRowCount;
        }

        private static void EnableWorksheetButtons()
        {
            EnableDeleteRowsButton();
            EnableParseRowsButton();
        }

        private static void EnableDeleteRowsButton()
        {
            DeleteRowsButton.Enabled = (ResultDataGridView.Visible && ResultDataGridView.Rows.Count > 0);
        }

        private static void EnableParseRowsButton()
        {
            ParseRowsButton.Enabled = true;
        }

        private static void DisplayResultDataGridView()
        {
            ResultDataGridView.DataSource = ResultTable;
            ResultDataGridView.Visible = true;

            SetResultDataGridViewFocus();
            EnableWorksheetButtons();

            if (ParseAbbreviations) AbbreviatedLabel.Visible = true;
        }

        private static void RefreshWorkspace()
        {
            SetParseComplete(true);
            ResultTable.AcceptChanges();
            SetReworkTable(ParentTable);

            ComparisonParametersReset();
            DisplayResultDataGridView();
        }

        private static bool DisplayProgressForm(int index, int rowsLength, bool bPermitCancel = false, bool bCancel = false, bool bProgressDisable = true, bool bPreferenceDisable = true)
        {
            if (bCancel)
            {
                ProgressDescription.Text = string.Format(UserHelper.culture, Properties.Resources.NOTIFY_DISPLAYPROGRESS_CANCEL, ProgressDescription.Text);
                ResetProgressForm();
                return false;
            }

            if (index == 0)
            {
                SetProgressForm();
            }
            else if (index + 1 == rowsLength)
            {
                ResetProgressForm(bProgressDisable, bPreferenceDisable);
            }
            else
            {
                PerformProgressStep();
            }

            void SetProgressForm()
            {
                ProgressDisplayEnable(bPermitCancel);
                PreferenceDescriptionEnable();
            }

            void PerformProgressStep()
            {
                decimal progress = System.Math.Round((decimal)index / rowsLength * 100);

                if (progress != GetProgressPercentage())
                {
                    SetProgressPercentage(progress);
                    ProgressDescription.Text = string.Format(UserHelper.culture, Properties.Resources.NOTIFY_DISPLAYPROGRESS_PERCENTAGE, GetProgressPercentage(), index, rowsLength, NAVForm.GetSheetDataGridViewInitialTotal());
                    ProgressDescription.Refresh();
                    ProgressBar.PerformStep();
                }

            }

            return true;
        }

        private static void RemoveWorksheetRowsButtonClick()
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (DataGridViewRow row in ResultDataGridView.Rows)
            {
                if ((int)row.Cells[Constants.COLUMN_DELETE_FLAG].Value == 1) cellArray.Add(row.Cells[Constants.COLUMN_ROW_ID].Value);
            }

            DeleteSelectedRows(cellArray);
        }

        private static void RemoveRowsFlaggedForDeletion()
        {
            WorkTables.SetSheetDataGridViewFocus();

            RemoveDataGridViewEmptyRows();
            RemoveDuplicateRows();
            SetResultDataGridViewFocus();

            void RemoveDataGridViewEmptyRows()
            {
                int[] emptyrows = ParentTable.Rows.Cast<System.Data.DataRow>()
                    .Where(x => string.IsNullOrWhiteSpace(x[Constants.COLUMN_DATA].ToString()))
                    .Select(x => (int)x[Constants.COLUMN_ROW_ID])
                    .Distinct()
                    .ToArray();

                if (emptyrows.Any())
                {
                    for (int i = 0; i < emptyrows.Length; i++)
                    {
                        foreach (DataGridViewRow row in NAVForm.GetSheetDataGridView().Rows)
                        {
                            if (row.IsNewRow) break;

                            if ((int)row.Cells[Constants.KEY_COLUMN].Value == emptyrows[i])
                            {
                                NAVForm.GetSheetDataGridView().Rows.Remove(row);
                            }

                        }

                    }

                }

                ParentTable.AcceptChanges();
            }

            void RemoveDuplicateRows()
            {
                object[] rows = GetRows();

                if (!rows.Any()) return;

                bool bDisplayProgress = rows.Length > (Constants.ACTION_PROGRESS_ESTIMATE / 4);

                System.Threading.Tasks.Task DeleteThread = System.Threading.Tasks.Task.Run(() => { DeleteDuplicates(); });
                DeleteThread.Wait();

                void DeleteDuplicates()
                {
                    ParentTable.AcceptChanges();

                    using (System.Data.DataTable table = NAVForm.DataTableCollection[UserHelper.UserStateModel.nvLastTableFocus])
                    {
                        if (bDisplayProgress) InitializeProgressDisplay(Properties.Resources.CAPTION_DUPLCATE, Properties.Resources.NOTIFY_INITIALISE_PROGRESS + "  [ 0 ] - " + rows.Length);

                        table.AcceptChanges();

                        int k = 0;

                        NAVForm.SuspendSheetDataGridView();

                        for (int i = rows.Length - 1; i > -1; i--)
                        {
                            int j = 0;

                            while (j < table.Rows.Count)
                            {
                                System.Data.DataRow row = table.Rows[j];

                                if (row.HasVersion(System.Data.DataRowVersion.Current))
                                {
                                    if (row[Constants.KEY_COLUMN].Equals(rows[i]))
                                    {
                                        table.Rows.RemoveAt(j);
                                        break;
                                    }
                                }

                                j++;
                            }

                            if (bDisplayProgress) DisplayProgressForm(k, rows.Length, true, false, true, false);

                            if (ParseAbbreviations)
                            {
                                int m = 0;

                                while (m < ParentTable.Rows.Count)
                                {
                                    System.Data.DataRow row = ParentTable.Rows[m];

                                    if (row.HasVersion(System.Data.DataRowVersion.Current))
                                    {
                                        if (row[Constants.COLUMN_ROW_ID].Equals(rows[i]))
                                        {
                                            ParentTable.Rows.RemoveAt(m);
                                            break;
                                        }
                                    }

                                    m++;
                                }

                            }

                            k++;
                        }

                        if (bDisplayProgress) InitializeProgressDisplay(Properties.Resources.CAPTION_COMPARE, Properties.Resources.NOTIFY_ABBREVIATIONPROGRESS);

                        table.AcceptChanges();
                        NAVForm.SetSpreadsheetChanges(true);
                        WorkTables.DeleteDuplicateRows(ResultTable);
                    }

                }

                object[] GetRows()
                {
                    if (NAVForm.GetSheetDataGridView().RowCount == 0) return null;

                    ResultTable.AcceptChanges();

                    rows = ResultTable.Rows.Cast<System.Data.DataRow>()
                       .Where(x => x[Constants.COLUMN_DELETE_FLAG].Equals(1))
                       .Select(x => x[Constants.COLUMN_ROW_ID])
                       .ToArray();

                    return rows;
                }

            }

        }

        public static int SetComparisonType(string nvUserPreferenceValue)
        {
            switch (nvUserPreferenceValue)
            {
                case Constants.METHOD_RATCLIFF_OBERSHELP:
                    return Constants.METHOD_RATCLIFF_OBERSHELP_VALUE;

                case Constants.METHOD_LEVENSHTEIN_DISTANCE:
                    return Constants.METHOD_LEVENSHTEIN_DISTANCE_VALUE;

                case Constants.METHOD_HAMMING_DISTANCE:
                    return Constants.METHOD_HAMMING_DISTANCE_VALUE;

                default:
                    return Constants.METHOD_RATCLIFF_OBERSHELP_VALUE;

            }

        }

        private static void ResetProgressForm(bool bProgressDisable = true, bool bPreferenceDisable = true)
        {
            if (bProgressDisable) ProgressDisplayDisable();
            if (bPreferenceDisable) PreferenceDescriptionDisable();
        }

        private static void RemoveDuplicates()
        {
            SetDuplicateRowsCount(WorkTables.FindParentTableDuplicates(ResultTable, ParentTable));
            
            if (DuplicateRowsCount == 0)
            {
                ClassLibraryStandard.HelperMethods.ProcessSleep(1000);
            }
            else
            {
                RemoveRowsFlaggedForDeletion();
            }

        }

        private void CancelProgressButtonClick(object sender, System.EventArgs e)
        {
            MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        private void PreferenceDescriptionEnter(object sender, System.EventArgs e)
        {
            Control activeControl = (Control)sender;

            ProgressHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ProgressHeader.Text = activeControl.AccessibleName;

            ProgressDescription.Text = string.Format(UserHelper.culture, activeControl.AccessibleDescription, System.Environment.NewLine);

            PreferenceDescriptionEnable();
        }

        private void PreferenceDescriptionLeave(object sender, System.EventArgs e)
        {
            PreferenceDescriptionDisable();
        }

        private void ResultDataGridViewVisibleChangedEvent(object sender, System.EventArgs e)
        {
            if (!ResultDataGridView.Visible)
            {
                PreferenceDescriptionDisable();
                EnableWorksheetButtons();
            }

        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            Control activeControl = (Control)sender;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    SendKeys.Send("{TAB}");
                    break;

                case Keys.Right:

                    if (ClassLibraryStandard.StringMethods.InString(activeControl.Name, Constants.CONTROL_DIVIDER_SYMBOL))
                    {
                        SendKeys.Send("{TAB}");
                    }
                    else if (ClassLibraryStandard.StringMethods.InString(activeControl.Name, Constants.CONTROL_PREFERENCE_SYMBOL))
                    {
                        string[] controlName = activeControl.Name.Split(new string[] { Constants.CONTROL_PREFERENCE_SYMBOL }, System.StringSplitOptions.None);
                        int iUserPreferenceID = int.Parse(controlName[1], UserHelper.culture);

                        foreach (Control control in Controls)
                        {
                            if ((control is TextBox) || (control is ComboBox))
                            {
                                if (control.Name == "navValue" + iUserPreferenceID && control.Enabled)
                                {
                                    control.Focus();
                                    return;
                                }
                            }
                        }

                    }

                    break;

                case Keys.Up:
                    SelectNextControl(activeControl, false, true, true, true);
                    break;

                case Keys.Left:
                    SelectNextControl(activeControl, false, true, true, true);
                    break;

                default:
                    break;
            }

        }

        private void UpdatePreferenceAttribute(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;

            if (control != null)
            {
                if (ClassLibraryStandard.StringMethods.InString(control.Name, "navValue"))
                {
                    bool bUserPreference = ClassLibraryStandard.HelperMethods.ToBoolean(control.Text);
                    string nvUserPreferenceValue = (string.IsNullOrWhiteSpace(control.Text)) ? null : control.Text.Trim();

                    if (control is TextBox)
                    {
                        SetTextBoxPreferenceAttribute();
                    }
                    else
                    {
                        SetPreferenceAttribute(control.AccessibleName, control.Text);
                    }

                    void SetTextBoxPreferenceAttribute()
                    {
                        if (!string.IsNullOrWhiteSpace(nvUserPreferenceValue))
                        {
                            if (nvUserPreferenceValue != control.AccessibleDefaultActionDescription)
                            {
                                if (PreferencesForm.ValidAttribute(control, bUserPreference, nvUserPreferenceValue, Properties.Resources.CAPTION_SIMILARITY))
                                {
                                    SetPreferenceAttribute(control.AccessibleName, control.Text);
                                }
                                else
                                {
                                    control.Text = control.AccessibleDefaultActionDescription;
                                    control.Focus();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_PREFERENCE_FIELDS + control.AccessibleDescription, System.Environment.NewLine), Properties.Resources.CAPTION_PREFERENCES, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            control.Text = control.AccessibleDefaultActionDescription;
                            ActiveControl = control;
                        }

                    }

                }

            }

        }

        private void SetPreferenceAttribute(string PreferenceName, string PreferenceValue)
        {
            bool bUserPreference = ClassLibraryStandard.HelperMethods.ToBoolean(PreferenceValue);
            string nvUserPreferenceValue = (string.IsNullOrWhiteSpace(PreferenceValue)) ? null : PreferenceValue.Trim();

            SetDuplicateRowsCount(0);
            SetComparisonReset(true);

            switch (PreferenceName)
            {
                case Constants.DB_APPLY_CASE_INSENSITIVITY:
                    MakeCaseInsensitive = bUserPreference;
                    break;

                case Constants.DB_PAD_TEXT:
                    PadToEqualLength = bUserPreference;
                    break;

                case Constants.DB_PERCENTAGE_INTEREST:
                    PercentageInterest = int.Parse(nvUserPreferenceValue, UserHelper.culture);
                    break;

                case Constants.DB_REMOVE_NOISE_CHARACTERS:
                    RemoveWhitespace = bUserPreference;
                    break;
                
                case Constants.DB_REVERSE_COMPARE:
                    ReverseComparison = bUserPreference;
                    break;

                case Constants.DB_MATCH_ABBREVIATIONS:
                    MatchAbbreviations = ResetOnMatchAbbreviationsChange(bUserPreference);
                    break;

                case Constants.DB_PHONETIC_FILTER:
                    PhoneticFilter = bUserPreference;
                    break;

                case Constants.DB_WHOLE_WORD_MATCH:
                    WholeWordComparison = bUserPreference;
                    break;

                case Constants.DB_MATCHING_ALGORITHM:
                    MatchingAlgorithm = GetComparisonType(nvUserPreferenceValue);
                    break;

                default:
                    break;
            }

        }

        private bool ResetOnMatchAbbreviationsChange(bool bUserPreference)
        {
            if (MatchAbbreviations != bUserPreference)
            {
                SetParseComplete(false);
                ReworkTable.Reset();
                ComparisonParametersReset();
            }

            return bUserPreference;
        }

        private void ParseWorksheetClick(object sender, System.EventArgs e)
        {
            if (ContinueComparison())
            {
                if (InitializeWorksheet())
                {
                    ResultDisplayDisableAll(false);
                    SetResultTable(System.Threading.Tasks.Task.Run(() => { return ParseWorksheetThread(); }).Result);
                    RefreshWorkspace();
                }

            }

            bool InitializeWorksheet()
            {
                if (DuplicateRowsCount == 0 && CreateParentTable() == 0) return false;

                System.DateTime logHelperStartTime = System.DateTime.Now;

                ParseDuplicateRows();

                LogHelper.TraceTimeElapsedWriteLine(System.DateTime.Now, logHelperStartTime, "TRACE - Parse duplicate rows (internal .NET) Time Elapsed: ");

                if (DiscontinueComparison()) return false;

                return true;

                bool ParseDuplicateRows()
                {
                    if (SheetDataGridViewRowCount == 0 || DuplicateRowsCount > 0) return false;

                    ClassLibraryFramework.DataGridViewMethods.SetBackgroundColour(NAVForm.GetSheetDataGridView(), Constants.COLOR_DEFAULT);

                    InitializeResultTable();
                    NAVForm.GetSheetDataGridView().Refresh();

                    RemoveDuplicates();

                    NAVForm.GetSheetDataGridView().SuspendLayout();

                    RecreateParentTable();

                    void InitializeResultTable()
                    {
                        if (ResultTable.Columns.Contains(Constants.COLUMN_ROW_ID))
                        {
                            ResultTable.Clear();
                        }
                        else
                        {
                            ResultTable = WorkTables.BuildResultTable();
                        }

                        DrawControls.SuspendDataGridViews();
                    }
                    
                    return true;
                }

            }

            bool ContinueComparison()
            {
                WorkTables.SetSheetDataGridViewFocus();

                if (NAVForm.GetSheetDataGridView().RowCount == 0 || !ParseComparison)
                {
                    if (!ParseComparison)
                    {
                        BuildResultDescription();
                    }
                    else
                    {
                        ResetOnCompletion();
                    }

                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;

                ResultDisplayDisableAll();

                if (GetParseAbbreviations() || (NAVForm.GetSheetDataGridViewInitialTotal() > Constants.ACTION_INITIALISE_DISPLAY))
                {
                    InitializeProgressDisplay(Properties.Resources.CAPTION_COMPARE, string.Format(UserHelper.culture, Properties.Resources.NOTIFY_INITIALISE_DUPICATE_DISPLAY, NAVForm.GetSheetDataGridViewInitialTotal()));
                }

                return true;
            }

            bool DiscontinueComparison()
            {
                int columns = NAVForm.GetSheetDataGridView().Columns.GetColumnCount(DataGridViewElementStates.Visible);
                int ParentTableRowsCount = ParentTable.Rows.Count;

                if (ParentTableRowsCount == 0)
                {
                    NAVForm.ResumeSheetDataGridView();
                    return true;
                }

                if (ParentTableRowsCount > Constants.ACTION_PROGRESS_WARNING || (columns > 6 && ParentTableRowsCount > Constants.ACTION_PROGRESS_WARNING / 2))
                {
                    switch (MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_LARGEFILE_WARNING, System.Environment.NewLine, ParentTableRowsCount, columns, DuplicateRowsCount), Properties.Resources.CAPTION_COMPARE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3))
                    {
                        case DialogResult.Yes:

                            return false;

                        default:
                            
                            NAVForm.ResumeSheetDataGridView();
                            InitializeProgressDisplay(Properties.Resources.NOTIFY_PROGRESS_CANCELLED, null);

                            return true;
                    }

                }

                return false;
            }

        }

        private System.Data.DataTable ParseWorksheetThread()
        {
            bool bRefreshParentString = true;
            object ParentTableRowID = null;
            string ParentString = null;
            string ChildString = null;

            SetProgressDescription(null);
            SetInitialiseDisplay(false);

            SetParseAbbreviations(GetParseAbbreviations());
            SimilarityComparison();

            if (ResultTable.Rows.Count > 0) WorkTables.DeleteDuplicateRows(ResultTable);

            return ResultTable;

            void InitialiseParentTable()
            {
                if (ParseComplete)
                {
                    if (ReworkTable.Rows.Count != 0) SetParentTable(ReworkTable.Copy());
                }
                else
                {
                    if (ParseAbbreviations)
                    {
                        SetReworkTable(WorkTables.AbbreviateTable(ParentTable));
                        SetParentTable(ReworkTable.Copy());
                    }

                }

            }

            void SimilarityComparison()
            {
                InitialiseParentTable();

                System.DateTime logHelperStartTime = System.DateTime.Now;
                System.Data.DataTable ChildTable = ParentTable.Copy();

                int iPercentageInterest = (int)PercentageInterest - 1;
                int iParentTableRows = ParentTable.Rows.Count;
                int iComparisonCount = 0;

                SetCumulativeVariationsCount(ParseTables());

                decimal ParseTables()
                {
                    bool bDisplayProgress = CumulativeThreshholdReached();

                    if (bDisplayProgress) InitializeProgressDisplay(Properties.Resources.CAPTION_COMPARE, Properties.Resources.NOTIFY_INITIALISE_PROGRESS);

                    for (int index = 0; index < iParentTableRows; index++)
                    {
                        bRefreshParentString = true;

                        string expression = Constants.COLUMN_ROW_ID + " = " + ParentTable.Rows[index][Constants.COLUMN_ROW_ID].ToString();

                        if (ClassLibraryStandard.DataTableMethods.DeleteRowByID(ChildTable, expression))
                        {
                            if (bRefreshParentString)
                            {
                                ParentTableRowID = ParentTable.Rows[index][Constants.COLUMN_ROW_ID];
                                ParentString = ParentTable.Rows[index][Constants.COLUMN_DATA].ToString();
                            }

                            for (int childIndex = 0; childIndex < ChildTable.Rows.Count; childIndex++)
                            {
                                Compare(childIndex, expression);
                                iComparisonCount++;
                            }

                        }

                        if (bDisplayProgress) DisplayProgressForm(index, iParentTableRows, true, false);
                    }

                    RemoveRowsFlaggedForDeletion();

                    ChildTable.Dispose();

                    LogHelper.TraceTimeElapsedWriteLine(System.DateTime.Now, logHelperStartTime, "TRACE - Comparison (internal .NET) Time Elapsed: ");

                    return iComparisonCount;
                }

                void Compare(int childIndex, string expression)
                {
                    ChildString = ChildTable.Rows[childIndex][Constants.COLUMN_DATA].ToString();

                    int iComparisonPercentage = GetComparisonPercentage(MatchingAlgorithm, ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison); ;

                    if (iComparisonPercentage > iPercentageInterest) PopulateResultTable();

                    void PopulateResultTable()
                    {
                        object ChildTableRowID = ChildTable.Rows[childIndex][Constants.COLUMN_ROW_ID];

                        System.Data.DataRow tableRow;

                        if (!ResultTable.Select(expression).Any())
                        {

                            tableRow = ResultTable.NewRow();
                            tableRow[Constants.COLUMN_DELETE_FLAG] = 0;
                            tableRow[Constants.COLUMN_ROW_ID] = ParentTableRowID;
                            tableRow[Constants.COLUMN_PARENT_ID] = ParentTableRowID;
                            tableRow[Constants.COLUMN_COMPARISON] = iComparisonPercentage;
                            tableRow[Constants.COLUMN_MATCHTYPE] = Constants.MATCH_PRINCIPLE;
                            tableRow[Constants.COLUMN_DATA] = ParentString;

                            ResultTable.Rows.Add(tableRow);
                        }

                        tableRow = ResultTable.NewRow();
                        tableRow[Constants.COLUMN_DELETE_FLAG] = (iComparisonPercentage == 100) ? 1 : 0;
                        tableRow[Constants.COLUMN_ROW_ID] = ChildTableRowID;
                        tableRow[Constants.COLUMN_PARENT_ID] = ParentTableRowID;
                        tableRow[Constants.COLUMN_COMPARISON] = iComparisonPercentage;
                        tableRow[Constants.COLUMN_MATCHTYPE] = Constants.MATCH_ASSOCIATE;
                        tableRow[Constants.COLUMN_DATA] = ChildString;

                        ResultTable.Rows.Add(tableRow);
                    }

                }

                bool CumulativeThreshholdReached()
                {
                    SetSheetDataGridViewRowCount(NAVForm.GetSheetDataGridView().RowCount);
                    if (SheetDataGridViewRowCount == 0) return false;

                    return CumulativeThreshhold() > Constants.ACTION_PROGRESS_ESTIMATE;

                    decimal CumulativeThreshhold()
                    {
                        int i = System.Threading.Tasks.Task.Run(() => { return WorkTables.GetCumulativeThreshholdThread(ParentTable); }).Result;
                        return (i > SheetDataGridViewRowCount) ? i++ : (DuplicateRowsCount > SheetDataGridViewRowCount) ? DuplicateRowsCount : SheetDataGridViewRowCount;
                    }

                }

            }

        }

        public static int GetComparisonPercentage(int MatchingAlgorithm, string ParentString, string ChildString, bool RemoveWhitespace = true, bool MakeCaseInsensitive = true, bool PadToEqualLength = true, bool ReverseComparison = true, bool PhoneticFilter = true, bool WholeWordComparison = true)
        {
            switch (MatchingAlgorithm)
            {
                case Constants.METHOD_RATCLIFF_OBERSHELP_VALUE:
                    return PatternMatching.ComparisonMethods.RatcliffObershelp.GetWeightedComparison(ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison);

                case Constants.METHOD_LEVENSHTEIN_DISTANCE_VALUE:
                    return PatternMatching.ComparisonMethods.LevenshteinDistance.GetWeightedComparison(ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison);

                case Constants.METHOD_HAMMING_DISTANCE_VALUE:
                    return PatternMatching.ComparisonMethods.HammingDistance.GetWeightedComparison(ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison);

                default:
                    return PatternMatching.ComparisonMethods.RatcliffObershelp.GetWeightedComparison(ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison);
            }

        }

        private void ResetOnCompletion(bool bForceRebuild = false)
        {
            DrawControls.ResumeDataGridViews();

            if (InitialiseDisplay)
            {
                ResultDisplayDisableAll();
            }
            else
            {
                if (string.IsNullOrEmpty(GetProgressDescription()) || bForceRebuild) BuildResultDescription();
            }

        }

        private void BuildResultDescription()
        {
            SetComparisonReset(false);

            SetSheetDataGridViewRowCount(NAVForm.GetSheetDataGridView().RowCount);
            SetCumulativeVariationsCount((SheetDataGridViewRowCount == 0) ? 0 : GetCumulativeVariationsCount());
            NAVForm.SetSheetCurrentTotal(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_CURRENT_TOTAL, SheetDataGridViewRowCount - 1));

            string actionType = (ActionPerformed == Constants.ACTION_REMOVE) ? Properties.Resources.NOTIFY_REMAINDER : Properties.Resources.NOTIFY_PROCESSED;
            string matchType = (DuplicateRowsCount == 0) ? Properties.Resources.NOTIFY_PARTIAL : Properties.Resources.NOTIFY_POTENTIAL;

            ProgressHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ProgressHeader.Text = Properties.Resources.CAPTION_COMPARE_RESULT;
            ProgressDescription.Text = string.Format(UserHelper.culture, Properties.Resources.NOTIFY_COMPARISON_RESULT, System.Environment.NewLine, actionType, SheetDataGridViewRowCount - 1, NAVForm.GetSheetDataGridViewInitialTotal(), GetComparisonMeasuresCount(), matchType, ResultDataGridView.Rows.Count, NAVForm.GetSheetDataGridViewInitialTotal() - (SheetDataGridViewRowCount - 1));

            SetProgressDescription(ProgressDescription.Text);
            PreferenceDescriptionEnable();
        }

        private decimal GetComparisonMeasuresCount()
        {
            bool NoiseCharactersRemoved = MatchAbbreviations;

            int i = 1;

            i = MakeCaseInsensitive ? i + 1 : i;
            i = PadToEqualLength ? i + 1 : i;
            i = RemoveWhitespace ? i + 1 : i;
            i = ReverseComparison ? i + 1 : i;
            i = MatchAbbreviations ? i + 1 : i;
            i = PhoneticFilter ? i + 1 : i;
            i = WholeWordComparison ? i + 1 : i;
            i = NoiseCharactersRemoved ? i + 1 : i;

            int iInitialTotal = NAVForm.GetSheetDataGridViewInitialTotal();
            int iFinalRowCount = SheetDataGridViewRowCount - 1;
            int iDuplicates = iInitialTotal - iFinalRowCount;
            int iParsedRows = iFinalRowCount * i;
            int iVariationsUsed = (int)GetCumulativeVariationsCount() * i;
            int iDuplicatesParsed = iInitialTotal + iDuplicates;
            int iAbbreviations = MatchAbbreviations ? iFinalRowCount : 0;

            return iInitialTotal + iVariationsUsed + iDuplicatesParsed + iAbbreviations + iParsedRows;
        }

        private void ResultDataGridViewMouseDownEvent(object sender, MouseEventArgs e)
        {
            ProgressHeader.Text = Properties.Resources.CAPTION_COMPARE_RESULT;
            ProgressDescription.Text = GetProgressDescription();

            if (e.Button == MouseButtons.Right)
            {
                ResultMenuItemDeleteRow.Enabled = ResultDataGridView.RowCount != 0;
                ResultMenuItemRemoveRows.Enabled = ResultDataGridView.RowCount != 0;

                DataGridView.HitTestInfo hitTest = ResultDataGridView.HitTest(e.X, e.Y);

                switch (hitTest.Type)
                {
                    case DataGridViewHitTestType.ColumnHeader:

                        ResultDataGridView.ClearSelection();
                        ResultDataGridView.CurrentCell = ResultDataGridView[hitTest.ColumnIndex, 0];
                        break;

                    case DataGridViewHitTestType.RowHeader:

                        break;

                    case DataGridViewHitTestType.Cell:

                        if (hitTest.RowIndex >= 0)
                        {
                            ResultDataGridView.ClearSelection();
                            ResultDataGridView.CurrentCell = ResultDataGridView[hitTest.ColumnIndex, hitTest.RowIndex];
                        }
                        break;

                    default:

                        break;

                }

            }

        }

        private bool GetParseAbbreviations()
        {
            if (MatchAbbreviations)
            {
                if (UserHelper.GetReplaceAllWords() || UserHelper.GetReplaceDefaultWordsOnly()) return true;
            }

            return false;
        }

        private void ResultMenuItemDeleteRowClick(object sender, System.EventArgs e)
        {
            DeleteSelectedRows(ClassLibraryFramework.DataGridViewMethods.GetSelectedRowCellValue(ResultDataGridView, Constants.COLUMN_ROW_ID));
        }

        private void ResultMenuItemRemoveRowsClick(object sender, System.EventArgs e)
        {
            WorkTables.DeleteRowsMenuItemClick();
        }

    }

}
