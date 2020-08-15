/*
 This work (Modern Encryption of a String C#, by James Tuley), 
 identified by James Tuley, is free of known copyright restrictions.

 https://gist.github.com/4336842
 http://creativecommons.org/publicdomain/mark/1.0/ 
 
 This code has been modified here from its original 

    1. The use of public, private, and static has been modified
    2. Constants and readonly runtime variables have been localised to the class
    3. Strict datatyping has replaced generic vars 
    
    Remarks    
 
    It's easy to forget. Encryption is just a form of obfuscation.  Time is the factor

*/

namespace Encryption
{
    public static class AESThenHMAC
    {
        private static readonly System.Security.Cryptography.RandomNumberGenerator Random = System.Security.Cryptography.RandomNumberGenerator.Create();

        //Preconfigured Encryption Parameters
        private const int BlockBitSize = 128;
        private const int KeyBitSize = 256;

        //Preconfigured Password Key Derivation Parameters
        private const int SaltBitSize = 64;
        private const int Iterations = 10000;
        private const int MinPasswordLength = 12;

        /// <summary>
        /// Helper that generates a random key on each call.
        /// </summary>
        /// <returns></returns>

        public static byte[] NewKey()
        {
            byte[] key = new byte[KeyBitSize / 8];
            Random.GetBytes(key);
            return key;
        }

        /// <summary>
        /// Simple Encryption (AES) then Authentication (HMAC) for a UTF8 Message.
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="cryptKey">The crypt key.</param>
        /// <param name="authKey">The auth key.</param>
        /// <param name="nonSecretPayload">(Optional) Non-Secret Payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Secret Message Required!;secretMessage</exception>
        /// <remarks>
        /// Adds overhead of (Optional-Payload + BlockSize(16) + Message-Padded-To-Blocksize +  HMac-Tag(32)) * 1.33 Base64
        /// </remarks>

        public static string SimpleEncrypt(string secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage))
                throw new System.ArgumentException("Secret Message Required!", "secretMessage");

            byte[] plainText = System.Text.Encoding.UTF8.GetBytes(secretMessage);
            byte[] cipherText = SimpleEncrypt(plainText, cryptKey, authKey, nonSecretPayload);

            return System.Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Simple Authentication (HMAC) then Decryption (AES) for a secrets UTF8 Message.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="cryptKey">The crypt key.</param>
        /// <param name="authKey">The auth key.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>

        public static string SimpleDecrypt(string encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage)) throw new System.ArgumentException("Encrypted Message Required!", "encryptedMessage");

            byte[] cipherText = System.Convert.FromBase64String(encryptedMessage);
            byte[] plainText = SimpleDecrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength);

            return plainText == null ? null : System.Text.Encoding.UTF8.GetString(plainText);
        }

        /// <summary>
        /// Simple Encryption (AES) then Authentication (HMAC) of a UTF8 message
        /// using Keys derived from a Password (PBKDF2).
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayload">The non secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">password</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// Adds additional non secret payload for key generation parameters.
        /// </remarks>

        public static string SimpleEncryptWithPassword(string secretMessage, string password, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage)) throw new System.ArgumentException("Secret Message Required!", "secretMessage");

            byte[] plainText = System.Text.Encoding.UTF8.GetBytes(secretMessage);
            byte[] cipherText = SimpleEncryptWithPassword(plainText, password, nonSecretPayload);

            return System.Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
        /// using keys derived from a password (PBKDF2). 
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>

        public static string SimpleDecryptWithPassword(string encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage)) throw new System.ArgumentException("Encrypted Message Required!", "encryptedMessage");

            byte[] cipherText = System.Convert.FromBase64String(encryptedMessage);
            byte[] plainText = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength);

            return plainText == null ? null : System.Text.Encoding.UTF8.GetString(plainText);
        }

        public static byte[] SimpleEncrypt(byte[] secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            //User Error Checks
            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8) throw new System.ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "cryptKey");

            if (authKey == null || authKey.Length != KeyBitSize / 8) throw new System.ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "authKey");

            if (secretMessage == null || secretMessage.Length < 1) throw new System.ArgumentException("Secret Message Required!", "secretMessage");

            //non-secret payload optional
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            byte[] cipherText;
            byte[] iv;

            using (System.Security.Cryptography.AesManaged aes = new System.Security.Cryptography.AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            })
            {
                //Use random IV
                aes.GenerateIV();
                iv = aes.IV;

                using (System.Security.Cryptography.ICryptoTransform encrypter = aes.CreateEncryptor(cryptKey, iv))

                using (System.IO.MemoryStream cipherStream = new System.IO.MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(cipherStream, encrypter, System.Security.Cryptography.CryptoStreamMode.Write))

                    using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(cryptoStream))
                    {
                        //Encrypt Data
                        binaryWriter.Write(secretMessage);
                    }

                    cipherText = cipherStream.ToArray();
                }

            }

            //Assemble encrypted message and add authentication
            using (System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256(authKey))

            using (System.IO.MemoryStream encryptedStream = new System.IO.MemoryStream())
            {
                using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(encryptedStream))
                {
                    //Prepend non-secret payload if any
                    binaryWriter.Write(nonSecretPayload);

                    //Prepend IV
                    binaryWriter.Write(iv);

                    //Write Ciphertext
                    binaryWriter.Write(cipherText);

                    binaryWriter.Flush();

                    //Authenticate all data
                    byte[] tag = hmac.ComputeHash(encryptedStream.ToArray());

                    //Postpend tag
                    binaryWriter.Write(tag);
                }

                return encryptedStream.ToArray();
            }

        }

        public static byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            //Basic Usage Error Checks
            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8) throw new System.ArgumentException(string.Format("CryptKey needs to be {0} bit!", KeyBitSize), "cryptKey");

            if (authKey == null || authKey.Length != KeyBitSize / 8) throw new System.ArgumentException(string.Format("AuthKey needs to be {0} bit!", KeyBitSize), "authKey");

            if (encryptedMessage == null || encryptedMessage.Length == 0) throw new System.ArgumentException("Encrypted Message Required!", "encryptedMessage");

            using (System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256(authKey))
            {
                byte[] sentTag = new byte[hmac.HashSize / 8];

                //Calculate Tag
                byte[] calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length);

                int ivLength = (BlockBitSize / 8);

                //if message length is to small just return null
                if (encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength) return null;

                //Grab Sent Tag
                System.Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length);

                //Compare Tag with constant time comparison
                int compare = 0;

                for (int i = 0; i < sentTag.Length; i++) compare |= sentTag[i] ^ calcTag[i];

                //if message doesn't authenticate return null
                if (compare != 0) return null;

                using (System.Security.Cryptography.AesManaged aes = new System.Security.Cryptography.AesManaged
                {
                    KeySize = KeyBitSize,
                    BlockSize = BlockBitSize,
                    Mode = System.Security.Cryptography.CipherMode.CBC,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                })
                {

                    //Grab IV from message
                    byte[] iv = new byte[ivLength];

                    System.Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length);

                    using (System.Security.Cryptography.ICryptoTransform decrypter = aes.CreateDecryptor(cryptKey, iv))

                    using (System.IO.MemoryStream plainTextStream = new System.IO.MemoryStream())
                    {
                        using (System.Security.Cryptography.CryptoStream decrypterStream = new System.Security.Cryptography.CryptoStream(plainTextStream, decrypter, System.Security.Cryptography.CryptoStreamMode.Write))

                        using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(decrypterStream))
                        {
                            //Decrypt Cipher Text from Message
                            binaryWriter.Write(
                              encryptedMessage,
                              nonSecretPayloadLength + iv.Length,
                              encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length
                            );
                        }

                        //Return Plain Text
                        return plainTextStream.ToArray();
                    }

                }

            }

        }

        public static byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
        {
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength) throw new System.ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (secretMessage == null || secretMessage.Length == 0) throw new System.ArgumentException("Secret Message Required!", "secretMessage");

            byte[] payload = new byte[((SaltBitSize / 8) * 2) + nonSecretPayload.Length];

            System.Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            int payloadIndex = nonSecretPayload.Length;

            byte[] cryptKey;
            byte[] authKey;

            //Use Random Salt to prevent pre-generated weak password attacks.
            using (System.Security.Cryptography.Rfc2898DeriveBytes generator = new System.Security.Cryptography.Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
            {
                byte[] salt = generator.Salt;

                //Generate Keys
                cryptKey = generator.GetBytes(KeyBitSize / 8);

                //Create Non Secret Payload
                System.Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                payloadIndex += salt.Length;
            }

            //Deriving separate key, might be less efficient than using HKDF, 
            //but now compatible with RNEncryptor which had a very similar wireformat and requires less code than HKDF.
            using (System.Security.Cryptography.Rfc2898DeriveBytes generator = new System.Security.Cryptography.Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
            {
                byte[] salt = generator.Salt;

                //Generate Keys
                authKey = generator.GetBytes(KeyBitSize / 8);

                //Create Rest of Non Secret Payload
                System.Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
            }

            return SimpleEncrypt(secretMessage, cryptKey, authKey, payload);
        }

        public static byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength) throw new System.ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (encryptedMessage == null || encryptedMessage.Length == 0) throw new System.ArgumentException("Encrypted Message Required!", "encryptedMessage");

            byte[] cryptSalt = new byte[SaltBitSize / 8];
            byte[] authSalt = new byte[SaltBitSize / 8];

            //Grab Salt from Non-Secret Payload
            System.Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length);
            System.Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length);

            byte[] cryptKey;
            byte[] authKey;

            //Generate crypt key
            using (System.Security.Cryptography.Rfc2898DeriveBytes generator = new System.Security.Cryptography.Rfc2898DeriveBytes(password, cryptSalt, Iterations))
            {
                cryptKey = generator.GetBytes(KeyBitSize / 8);
            }

            //Generate auth key
            using (System.Security.Cryptography.Rfc2898DeriveBytes generator = new System.Security.Cryptography.Rfc2898DeriveBytes(password, authSalt, Iterations))
            {
                authKey = generator.GetBytes(KeyBitSize / 8);
            }

            return SimpleDecrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength);
        }

    }

}