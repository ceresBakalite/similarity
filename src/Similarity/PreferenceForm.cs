using System;
using System.Windows.Forms;

namespace NAVService
{
    public partial class PreferencesForm : Form
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        
        private static Control PreferenceHeader;
        private static Control PreferenceDescription;

        protected internal static System.Data.DataTable UserPreferencesDataTable { get; private set; }

        public PreferencesForm()
        {
            InitializeComponent();
            InitializePreferencesForm();
        }

        protected internal static void TextBoxBackColorChange(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            if (control != null)
            {
                control.BackColor = (control.BackColor == System.Drawing.SystemColors.InactiveBorder) ? System.Drawing.SystemColors.Highlight : System.Drawing.SystemColors.InactiveBorder;
                control.ForeColor = (control.ForeColor == Constants.COLOR_DEFAULT_TEXT) ? Constants.COLOR_DEFAULT_WINDOW : System.Drawing.SystemColors.WindowText;
            }

        }

        protected internal static bool ValidAttribute(Control control, bool bUserPreference, string nvUserPreferenceValue, string callerName)
        {
            if (control != null)
            {
                bool bUseBooleanPreference = nvUserPreferenceValue == null;

                if (bUseBooleanPreference)
                {
                    switch (control.AccessibleName)
                    {
                        case Constants.NOT_IMPLEMENTED:

                            control.Text = bUserPreference ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO;
                            control.AccessibleDefaultActionDescription = control.Text;
                            break;

                        default:

                            break;
                    }

                    return true;
                }
                else
                {
                    switch (control.AccessibleName)
                    {
                        case Constants.DB_PERCENTAGE_INTEREST:

                            if (ClassLibraryStandard.GenericHelperMethods.IsInteger(nvUserPreferenceValue))
                            {
                                int value = int.Parse(nvUserPreferenceValue, UserHelper.culture);

                                if (value > -1 && value < 101)
                                {
                                    control.Text = value.ToString(UserHelper.culture);
                                    control.AccessibleDefaultActionDescription = control.Text;
                                    return true;
                                }
                            }

                            MessageBox.Show(Properties.Resources.NOTIFY_PERCENTAGE_VALIDATION, (callerName == Properties.Resources.CAPTION_PREFERENCES) ? Properties.Resources.CAPTION_PREFERENCES : Properties.Resources.CAPTION_SIMILARITY, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return false;

                        default:

                            return true;
                    }
                }
            }

            return true;
        }

        private void InitializePreferencesForm()
        {
            UserPreferencesDataTable = ClassLibraryStandard.DataTableMethods.GetDataTable(DataAccess.GetUserPreferences());

            try
            {
                BuildFormControls();
            }
            catch (NullReferenceException ex)
            {
                LogHelper.FatalNullReferenceException(ex);
            }

            void BuildFormControls()
            {
                SuspendLayout();

                int axisY = 0;

                for (int rowIndex = 0; rowIndex < UserPreferencesDataTable.Rows.Count; rowIndex++)
                {
                    if (GroupHeaderRequired(rowIndex))
                    {
                        GroupHeaderInsert(rowIndex, axisY);
                        axisY += 19;
                    }

                    PreferenceNameInsert(rowIndex, axisY);
                    PreferenceValueInsert(rowIndex, axisY);

                    axisY += 19;
                }

                RestoreDefaultsButton(axisY += 10);

                axisY += 5;

                PreferenceDescriptionHeaderInsert(axisY += 19);
                PreferenceDescriptionInsert(axisY += 19);

                InstantiatePreferenceDelegates();

                ResumeLayout();
            }

            void RestoreDefaultsButton(int axisY)
            {
                Button restoreDefaultsButton = new Button
                {
                    Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                    Name = "RestoreDefaultsButton",
                    Location = new System.Drawing.Point(351, axisY),
                    Size = new System.Drawing.Size(100, 23),
                    Text = "Restore defaults",
                    UseVisualStyleBackColor = true,
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 1000
                };

                PreferenceToolTips.SetToolTip(restoreDefaultsButton, Properties.Resources.TOOLTIP_RESTOREDEFAULTSBUTTON);

                restoreDefaultsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
                restoreDefaultsButton.Enter += PreferenceDescriptionLeave;
                restoreDefaultsButton.Click += RestoreDefaultsButtonClick;

                Controls.Add(restoreDefaultsButton);
            }

            void PreferenceDescriptionHeaderInsert(int axisY)
            {
                TextBox PreferenceDescriptionHeader = new TextBox
                {
                    Name = "preferenceDescriptionHeader",
                    Text = "",
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

            void PreferenceDescriptionInsert(int axisY)
            {
                TextBox PreferenceDescription = new TextBox
                {
                    Name = "preferenceDescription",
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

            void GroupHeaderInsert(int rowIndex, int axisY)
            {
                string controlName = "navDivider" + UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();

                TextBox DividerTextBox = new TextBox
                {
                    Name = controlName,
                    Location = new System.Drawing.Point(0, axisY),
                    Text = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim(),
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
                DividerTextBox.Enter += TextBoxBackColorChange;
                DividerTextBox.Leave += TextBoxBackColorChange;
                DividerTextBox.KeyDown += TextBoxKeyDown;

                Controls.Add(DividerTextBox);
            }

            void PreferenceNameInsert(int rowIndex, int axisY)
            {
                string controlName = "navPreference" + UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();

                TextBox PreferenceTextBox = new TextBox
                {
                    Name = controlName,
                    Location = new System.Drawing.Point(0, axisY),
                    Text = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim(),
                    Anchor = ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left))),
                    BackColor = Constants.COLOR_MATCH_ASSOCIATE,
                    ForeColor = Constants.COLOR_DEFAULT_TEXT,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Arrow,
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    RightToLeft = RightToLeft.No,
                    Size = new System.Drawing.Size(135, 20),
                    AccessibleDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                    AccessibleName = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim(),
                    TabIndex = rowIndex + 500
                };

                PreferenceTextBox.GotFocus += (s1, e1) => { ClassLibraryStandard.TextBoxInteropServices.HideCaret(PreferenceTextBox.Handle); };

                PreferenceTextBox.Enter += TextBoxBackColorChange;
                PreferenceTextBox.Enter += PreferenceDescriptionEnter;
                PreferenceTextBox.Leave += TextBoxBackColorChange;
                PreferenceTextBox.KeyDown += TextBoxKeyDown;

                Controls.Add(PreferenceTextBox);
            }

            void PreferenceValueInsert(int rowIndex, int axisY)
            {
                string controlName = "navValue" + UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim();
                string nvClientPreferenceName = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim();

                bool bClientOverride = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_OVERRIDE_FLAG].ToString());
                bool bSystemOverride = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_SYSTEM_OVERRIDE_FLAG].ToString());

                if (ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_VALUE_REQUIRED_FLAG].ToString()))
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
                        AccessibleDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        AccessibleName = nvClientPreferenceName,
                        TabIndex = rowIndex
                    };

                    switch (nvClientPreferenceName)
                    {
                        case Constants.DB_MATCHING_ALGORITHM:
                            ListValueComboBox.Items.AddRange(new object[] { Constants.METHOD_RATCLIFF_OBERSHELP, Constants.METHOD_LEVENSHTEIN_DISTANCE, Constants.METHOD_HAMMING_DISTANCE });
                            ListValueComboBox.SelectedIndex = ExplorerForm.GetComparisonType(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim());
                            break;

                        default:
                            ListValueComboBox.Items.AddRange(new object[] { Properties.Resources.FIELD_VALUE_YES, Properties.Resources.FIELD_VALUE_NO });
                            ListValueComboBox.SelectedIndex = ListValueComboBox.Items.IndexOf(ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_FLAG].ToString()) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO);
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
                        Text = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                        BackColor = Constants.COLOR_DEFAULT,
                        ForeColor = (bClientOverride || bSystemOverride) ? System.Drawing.Color.DimGray : System.Drawing.Color.Black,
                        MaxLength = 1000,
                        RightToLeft = RightToLeft.No,
                        Size = new System.Drawing.Size(317, 20),
                        AccessibleDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
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
                        AccessibleDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_DESCRIPTION].ToString().Trim(),
                        AccessibleDefaultActionDescription = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_VALUE].ToString().Trim(),
                        AccessibleName = nvClientPreferenceName,
                        TabIndex = rowIndex
                    };

                    ValueComboBox.Items.AddRange(new object[] { Properties.Resources.FIELD_VALUE_YES, Properties.Resources.FIELD_VALUE_NO });
                    ValueComboBox.SelectedIndex = ValueComboBox.Items.IndexOf(ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_FLAG].ToString()) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO);
                    ValueComboBox.Enter += PreferenceDescriptionEnter;
                    if (bClientOverride || bSystemOverride) ValueComboBox.Enabled = false;
                    if (ValueComboBox.Enabled) ValueComboBox.TextChanged += UpdatePreferenceAttribute;

                    Controls.Add(ValueComboBox);
                }

            }

            bool GroupHeaderRequired(int rowIndex)
            {
                if (UserPreferencesDataTable != null)
                {
                    if (rowIndex == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return UserPreferencesDataTable.Rows[rowIndex - 1][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim() != UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim();
                    }
                }

                return false;
            }

        }    

        private static string RestoreDefaults(int iUserPreferenceID)
        {
            for (int rowIndex = 0; rowIndex < UserPreferencesDataTable.Rows.Count; rowIndex++)
            {
                if (int.Parse(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim(), UserHelper.culture) == iUserPreferenceID)
                {
                    bool bClientPreference = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_FLAG].ToString());
                    string bClientValueRequired = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_VALUE_REQUIRED_FLAG].ToString();
                    string nvClientPreferenceValue = string.IsNullOrWhiteSpace(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_VALUE].ToString().Trim()) ? null : UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_VALUE].ToString().Trim();

                    UpdatePreference(iUserPreferenceID, bClientPreference, nvClientPreferenceValue);

                    return ClassLibraryStandard.GenericHelperMethods.ToBoolean(bClientValueRequired) ? nvClientPreferenceValue : bClientPreference.ToString(UserHelper.culture);
                }
            }

            return null;
        }

        private static void UpdatePreference(int iUserPreferenceID, bool bPreference, string nvPreferenceValue)
        {
            try
            {
                int iPreference = (bPreference) ? 1 : 0;

                Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                param.Add("@" + Constants.COLUMN_USER_PREFERENCE_ID, iUserPreferenceID);
                param.Add("@" + Constants.COLUMN_USER_PREFERENCE_FLAG, iPreference);
                param.Add("@" + Constants.COLUMN_USER_PREFERENCE_VALUE, nvPreferenceValue);

                DataAccess.UpdateUserPreference(param);

                for (int rowIndex = 0; rowIndex < UserPreferencesDataTable.Rows.Count; rowIndex++)
                {
                    if (int.Parse(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_USER_PREFERENCE_ID].ToString().Trim(), UserHelper.culture) == iUserPreferenceID)
                    {
                        System.Collections.Generic.Dictionary<int, NAVChangePreferencesModel> map = new System.Collections.Generic.Dictionary<int, NAVChangePreferencesModel>()
                        {
                            { 1, new NAVChangePreferencesModel
                            {
                                iPreferenceID = iUserPreferenceID,
                                iClientPreferenceTypeID = int.Parse(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE_ID].ToString().Trim(), UserHelper.culture),
                                nvClientPreferenceType = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_TYPE].ToString().Trim(),
                                nvClientPreferenceName = UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_PREFERENCE_NAME].ToString().Trim(),
                                nvPreferenceValue = nvPreferenceValue,
                                bPreference = ClassLibraryStandard.GenericHelperMethods.ToBoolean(bPreference.ToString(UserHelper.culture)),
                                bClientValueRequired = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_VALUE_REQUIRED_FLAG].ToString()),
                                bClientOverride = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_CLIENT_OVERRIDE_FLAG].ToString()),
                                bSystemOverride = ClassLibraryStandard.GenericHelperMethods.ToBoolean(UserPreferencesDataTable.Rows[rowIndex][Constants.COLUMN_SYSTEM_OVERRIDE_FLAG].ToString())
                            }
                            }
                        };

                        NAVPanelForm.ApplyPreferenceChange(map);
                        SetRequiredNotificationChange();

                        void SetRequiredNotificationChange()
                        {
                            NAVPanelForm.SetNotifyPreferenceChange(true);

                            string nvClientPreferenceType = map[1].nvClientPreferenceType.Trim();
                            string nvClientPreferenceName = map[1].nvClientPreferenceName.Trim();

                            switch (nvClientPreferenceType)
                            {
                                case Constants.DB_SLIDE_PANEL_TAB_VIEWS:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_HIDE_ABBREVIATIONS: break;
                                        case Constants.DB_HIDE_EXPLORER: break;
                                        case Constants.DB_OPEN_LAST_TAB: break;
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_IMPORT_FILE_PERMISSIONS:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_ORDER_COLUMNS: break;
                                        case Constants.DB_DELETE_ROWS: break;
                                        case Constants.DB_EDIT_CELLS: break;
                                        case Constants.DB_CREATE_ROWS: break;
                                        case Constants.DB_OPEN_LAST_FILE: break;
                                        case Constants.DB_OPEN_LAST_WORKSHEET: break;
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_ABBREVIATION_TAB_PERMISSIONS:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_REPLACE_WORDS:
                                            NAVPanelForm.SetNotifyPreferenceChange(false);
                                            break;

                                        case Constants.DB_PERMIT_DEFAULTS:
                                            NAVPanelForm.SetNotifyPreferenceChange(false);
                                            break;

                                        case Constants.DB_PULL_ABBREVIATIONS:
                                            NAVPanelForm.SetNotifyPreferenceChange(false);
                                            break;

                                        case Constants.DB_DELETE_ABBREVIATIONS: break;
                                        case Constants.DB_ADD_NEW_ABBREVIATIONS: break;
                                        case Constants.DB_EDIT_ABBREVIATIONS: break;
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_ERROR_LOG_PREFERENCES:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_LOG_ERRORS:
                                            NAVPanelForm.SetNotifyPreferenceChange(false);
                                            break;

                                        case Constants.DB_LOG_ERRORS_TO_FILE: break; // TO DO: log4net - disabled (redundant?)
                                        case Constants.DB_ERROR_LOG_FILE_LOCATION: break; // TO DO: log4net - disabled (redundant?)
                                        case Constants.DB_ERROR_LOG_MAXIMUM_SIZE: break; // TO DO: log4net - disabled (redundant?)
                                        case Constants.DB_ERROR_LOG_ARCHIVE_TOTAL: break; // TO DO: log4net - disabled (redundant?)
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_DATA_CONNECTION_ATTRIBUTES:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_CLOUD_CONNECTION_STRING: break; // TO DO: return value in ConnectionHelper.cs - disabled (redundant?)
                                        case Constants.DB_LOCAL_CONNECTION_STRING: break; // TO DO: return value in ConnectionHelper.cs - disabled (redundant?)
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_USER_INTERFACE_THEMES:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_COLOUR_SCHEME: break; // TO DO: future imiplementation - disabled
                                        case Constants.DB_INTERFACE_LANGUAGE: break; // TO DO: future imiplementation - disabled
                                        case Constants.DB_APPLY_RTL_ORIENTATION: break; // TO DO: future imiplementation - disabled
                                        default: break;
                                    }

                                    break;

                                case Constants.DB_SEARCH_PREFERENCES:

                                    switch (nvClientPreferenceName)
                                    {
                                        case Constants.DB_PULL_ABBREVIATIONS: break;
                                        case Constants.DB_REMOVE_NOISE_CHARACTERS: break;
                                        case Constants.DB_APPLY_CASE_INSENSITIVITY: break;
                                        case Constants.DB_PAD_TEXT: break;
                                        case Constants.DB_REVERSE_COMPARE: break;
                                        case Constants.DB_MATCH_ABBREVIATIONS: break;
                                        case Constants.DB_PHONETIC_FILTER: break;
                                        case Constants.DB_WHOLE_WORD_MATCH: break;
                                        case Constants.DB_PERCENTAGE_INTEREST: break;
                                        case Constants.DB_MATCHING_ALGORITHM: break;
                                        default: break;
                                    }

                                    break;

                                default:

                                    break;

                            }

                        }

                        return;
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(Properties.Resources.NOTIFY_PREFERENCE_CELL, Properties.Resources.CAPTION_PREFERENCES, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (log != null) log.Error(Properties.Resources.NOTIFY_PREFERENCE_CELL, ex);
            }

        }

        private void RestoreDefaultsButtonClick(object sender, EventArgs e)
        {
            Button restoreDefaultsButton = (Button)sender;

            if (restoreDefaultsButton != null)
            {
                switch (MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_RESTORE_PREFERENCES, Environment.NewLine), Properties.Resources.CAPTION_PREFERENCES, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
                {
                    case DialogResult.Yes:

                        foreach (Control control in Controls)
                        {
                            if ((control is TextBox) || (control is ComboBox))
                            {
                                if (ClassLibraryStandard.GenericStringMethods.InString(control.Name, "navValue"))
                                {
                                    if (control.Enabled)
                                    {
                                        string[] controlName = control.Name.Split(new string[] { "navValue" }, StringSplitOptions.None);
                                        int iUserPreferenceID = int.Parse(controlName[1], UserHelper.culture);

                                        control.Text = (control is ComboBox) ? (ClassLibraryStandard.GenericHelperMethods.ToBoolean(RestoreDefaults(iUserPreferenceID)) ? Properties.Resources.FIELD_VALUE_YES : Properties.Resources.FIELD_VALUE_NO) : RestoreDefaults(iUserPreferenceID);
                                    }
                                }
                            }
                        }

                        break;

                    default:

                        break;
                }

                NAVPanelForm.SetNotifyPreferenceChange(false);
            }

        }

        private void InstantiatePreferenceDelegates()
        {
            PreferenceHeader = Controls["preferenceDescriptionHeader"];
            PreferenceDescription = Controls["preferenceDescription"];
        }

        private void PreferenceDescriptionLeave(object sender, EventArgs e)
        {
            if (PreferenceHeader != null) PreferenceHeader.Text = null;
            if (PreferenceDescription != null) PreferenceDescription.Text = null;
        }

        private void PreferenceDescriptionEnter(object sender, EventArgs e)
        {
            Control activeControl = (Control)sender;

            if (PreferenceHeader != null)
            {
                PreferenceHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                PreferenceHeader.Text = activeControl.AccessibleName;
            }

            if (PreferenceDescription != null)
            {
                PreferenceDescription.Text = string.Format(UserHelper.culture, activeControl.AccessibleDescription, Environment.NewLine);
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

                    if (ClassLibraryStandard.GenericStringMethods.InString(activeControl.Name, "navDivider"))
                    {
                        SendKeys.Send("{TAB}");
                    }
                    else if(ClassLibraryStandard.GenericStringMethods.InString(activeControl.Name, "navPreference"))
                    {
                        string[] controlName = activeControl.Name.Split(new string[] { "navPreference" }, StringSplitOptions.None);
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

                default: break;
            }

        }

        private void UpdatePreferenceAttribute(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            if (control != null)
            {
                if (ClassLibraryStandard.GenericStringMethods.InString(control.Name, "navValue"))
                {
                    string[] controlName = control.Name.Split(new string[] { "navValue" }, StringSplitOptions.None);

                    int iUserPreferenceID = int.Parse(controlName[1], UserHelper.culture);
                    bool bUserPreference = ClassLibraryStandard.GenericHelperMethods.ToBoolean(control.Text);
                    string nvUserPreferenceValue = (string.IsNullOrWhiteSpace(control.Text)) ? null : control.Text.Trim();

                    if (control is TextBox)
                    {
                        if (!string.IsNullOrWhiteSpace(nvUserPreferenceValue))
                        {
                            if (nvUserPreferenceValue != control.AccessibleDefaultActionDescription)
                            {
                                if (ValidAttribute(control, bUserPreference, nvUserPreferenceValue, Properties.Resources.CAPTION_PREFERENCES))
                                {
                                    UpdatePreference(iUserPreferenceID, bUserPreference, control.Text);
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
                            MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_PREFERENCE_FIELDS + control.AccessibleDescription, Environment.NewLine), Properties.Resources.CAPTION_PREFERENCES, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            control.Text = control.AccessibleDefaultActionDescription;
                            ActiveControl = control;
                        }
                    }
                    else
                    {
                        UpdatePreference(iUserPreferenceID, bUserPreference, control.Text);
                    }

                }

            }
            
        }

    }

}
