namespace NAVService
{
    public partial class NAVSymbolForm : System.Windows.Forms.Form
    {

#pragma warning disable IDE1006 // for ease of maintenance the camelCase naming convention emulates their TSQL camelCase column name

        
        internal string nvWord { get; private set; }
        internal string nvAbbreviation { get; private set; }
        internal string nvAbbreviationDescription { get; private set; }
        internal int bAlwaysUse { get; private set; }

#pragma warning restore IDE1006 // for conformity the naming convention returns to C# PascalCase

        public NAVSymbolForm(bool bEdit = false)
        {
            InitializeComponent();
            if (bEdit) InitialiseDataComponents();
        }

        private void InitialiseDataComponents()
        {
            System.Windows.Forms.DataGridViewRow row = NAVPanelForm.GetAbbreviationsDataGridView().CurrentRow;

            WordTextBox.Text = (string)row.Cells[Constants.COLUMN_ABBREVIATION_WORD].Value;
            AbbreviationTextBox.Text = (string)row.Cells[Constants.COLUMN_ABBREVIATION].Value;
            AbbreviationDescriptionRichTextBox.Text = (string)row.Cells[Constants.COLUMN_ABBREVIATION_DESCRIPTION].Value;
            AlwaysUseComboBox.Checked = System.Convert.ToBoolean(row.Cells[Constants.COLUMN_ABBREVIATION_FLAG].Value, UserHelper.culture);
        }

        private void OKButtonClick(object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;

            nvWord = string.IsNullOrEmpty(WordTextBox.Text.Trim()) ? "" : WordTextBox.Text.Trim(); 
            nvAbbreviation = string.IsNullOrEmpty(AbbreviationTextBox.Text.Trim()) ? "" : AbbreviationTextBox.Text.Trim();
            nvAbbreviationDescription = string.IsNullOrEmpty(AbbreviationDescriptionRichTextBox.Text.Trim()) ? "" : AbbreviationDescriptionRichTextBox.Text.Trim();
            bAlwaysUse = (AlwaysUseComboBox.Checked) ? 1 : 0;

            bool ErrorDetected = false;

            if (!ErrorDetected && string.IsNullOrWhiteSpace(nvWord)) ErrorDetected = DataValidation(Constants.COLUMN_ABBREVIATION_WORD);
            if (!ErrorDetected && string.IsNullOrWhiteSpace(nvAbbreviation)) ErrorDetected = DataValidation(Constants.COLUMN_ABBREVIATION);
            if (!ErrorDetected && string.IsNullOrWhiteSpace(nvAbbreviationDescription)) ErrorDetected = DataValidation(Constants.COLUMN_ABBREVIATION_DESCRIPTION);

            if (!ErrorDetected) DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool DataValidation(string fieldName)
        {
            string message = Properties.Resources.NOTIFY_FORM_ERROR;

            switch (fieldName) 
            {
                case Constants.COLUMN_ABBREVIATION_WORD:

                    message = Properties.Resources.NOTIFY_FIELD_ERROR;
                    ActiveControl = WordTextBox;

                    break;

                case Constants.COLUMN_ABBREVIATION:

                    message = Properties.Resources.NOTIFY_FIELD_ABBREVIATION_ERROR;
                    ActiveControl = AbbreviationTextBox;

                    break;

                case Constants.COLUMN_ABBREVIATION_DESCRIPTION:

                    message = Properties.Resources.NOTIFY_FIELD_DESCRIPTION_ERROR;
                    ActiveControl = AbbreviationDescriptionRichTextBox;

                    break;

                default:

                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                    break;

            }
            
            System.Windows.Forms.MessageBox.Show(string.Format(UserHelper.culture, message, System.Environment.NewLine), Properties.Resources.CAPTION_ABBREVIATIONS, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            return true;
        }

    }

}
