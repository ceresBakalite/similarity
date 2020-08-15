/*

    A suite of routines that create encrypted text strings and their associated keys

*/

namespace NAVService
{
    public static class EncryptionHelper
    {
        private static string hexCryptKey = Properties.Settings.Default.HashKey.Substring(0, 64);
        private static string hexAuthKey = Properties.Settings.Default.HashKey.Substring(64, 64);
        private static string hashKey = hexCryptKey + hexAuthKey;
        private static byte[] byteCryptKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(hexCryptKey);
        private static byte[] byteAuthKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(hexAuthKey);

        private static bool bKeysCreated;
        private static bool bKeysSaved;
        private static string cipher;

        [System.Diagnostics.Conditional("TRACE")]
        private static void CreateKeys(bool bSaveKeys = false)
        {
            byteCryptKey = Encryption.AESThenHMAC.NewKey();
            byteAuthKey = Encryption.AESThenHMAC.NewKey();

            hexCryptKey = ClassLibraryFramework.GenericStringMethods.GetBytesToString(byteCryptKey);
            hexAuthKey = ClassLibraryFramework.GenericStringMethods.GetBytesToString(byteAuthKey);

            hashKey = hexCryptKey + hexAuthKey;

            if (bSaveKeys) SaveCipherKeys();

            bKeysCreated = true;
        }

        [System.Diagnostics.Conditional("TRACE")]
        public static void SignatureKeyToConsole(object value)
        {
            if (value == null) return;

            System.Diagnostics.Trace.WriteLine($"current signature: " + Properties.Settings.Default.Signature);

            System.TypeCode type = System.Type.GetTypeCode(value.GetType());

            string plainText = null;

            switch (type)
            {
                case System.TypeCode.Int32:

                    plainText = value.ToString();
                    
                    break;

                case System.TypeCode.String:

                    plainText = value.ToString();

                    break;

                default:

                    System.Diagnostics.Trace.WriteLine($"Invalid object:" + plainText);

                    break;

            }


            if (plainText == null) return;

            string encryptString = Encryption.AESThenHMAC.SimpleEncrypt(plainText, byteCryptKey, byteAuthKey);
            byte[] byteSignature = System.Text.Encoding.Default.GetBytes(encryptString);
            string hexSignature = ClassLibraryFramework.GenericStringMethods.GetBytesToString(byteSignature);

            string encryptedString = System.Text.Encoding.Default.GetString(byteSignature);
            string decryptedString = Encryption.AESThenHMAC.SimpleDecrypt(encryptString, byteCryptKey, byteAuthKey);

            System.Diagnostics.Trace.WriteLine($"encryptString: " + encryptString);
            System.Diagnostics.Trace.WriteLine($"hexSignature: " + hexSignature);
            System.Diagnostics.Trace.WriteLine($"encryptedString: " + encryptedString);
            System.Diagnostics.Trace.WriteLine($"decryptedString: " + decryptedString);
        }

        [System.Diagnostics.Conditional("TRACE")]
        private static void SaveCipherKeys()
        {
            switch (System.Windows.Forms.MessageBox.Show(string.Format(UserHelper.culture, "Save Cipher Keys{0}{0}This action will destroy the existing encryption and authentication key pair and cannot be undone.  All existing encrytped values held in the application will need to be recreated{0}{0}Are you sure you wish to continue?", System.Environment.NewLine), ConnectionHelper.ApplicationName(false, false), System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.MessageBoxDefaultButton.Button3))
            {
                case System.Windows.Forms.DialogResult.Yes:

                    Properties.Settings.Default.HashKey = hashKey;
                    Properties.Settings.Default.Save();
                    bKeysSaved = true;

                    break;

                default:

                    break;

            }

        }

        [System.Diagnostics.Conditional("TRACE")]
        public static void EncryptionExamplesToConsole(string plainText = @"The example secret message reads: ""It was that cow that jumped over the //<>C:\#@! moon.""", bool bCreateKeys = false, bool bSaveKeys = false, bool bDisplayCipher = true)
        {
            if (bCreateKeys) CreateKeys(bSaveKeys);

            cipher = Encryption.AESThenHMAC.SimpleEncrypt(plainText, byteCryptKey, byteAuthKey);

            if (bDisplayCipher) DisplayCiphers(plainText);
        }

        [System.Diagnostics.Conditional("TRACE")]
        private static void DisplayCiphers(string plainText = "Unknown")
        {
            string display = "{0}The plain text string to be encrypted: {0}{0}" + plainText;

            display += "{0}{0}The encrypted plain text: {0}{0}" + cipher;
            display += "{0}{0}The hexidecimal encryption and authentication string pair used in the hash key: {0}";
            display += bKeysCreated ? (bKeysSaved ? "This is a new key pair that has been SAVED{0}" : "This is a new key pair that has NOT been saved{0}") : "This is the currently held key pair{0}";
            display += "{0}" + hexCryptKey + "{0}" + hexAuthKey;
            display += "{0}{0}The hexidecimal hash key required in the Properties.Settings.Default.HashKey: {0}";
            display += bKeysSaved ? "This hash value has been SAVED to the \"Properties.Settings.Default.HashKey\"{0}{0}" + hashKey : "{0}" + hashKey;
            display += "{0}{0}The decrypted cipher string: {0}{0}" + Encryption.AESThenHMAC.SimpleDecrypt(cipher, ClassLibraryFramework.GenericStringMethods.GetStringToBytes(hexCryptKey), ClassLibraryFramework.GenericStringMethods.GetStringToBytes(hexAuthKey)) + "{0}";

            System.Diagnostics.Trace.WriteLine(string.Format(UserHelper.culture, display, System.Environment.NewLine));
        }

    }

}
