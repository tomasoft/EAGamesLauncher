using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace EAGamesLauncher
{
    internal class EncryptDecrypt
    {
        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr destination, int length);

        private GCHandle _gcHandle;

        public EncryptDecrypt()
        {
            // For additional security Pin the key.
            _gcHandle = new GCHandle();
        }

        // Function to Generate a 64 bits Key.
        public string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            var desCrypto = (DESCryptoServiceProvider)DES.Create();

            // Use the Automatically generated key for Encryption. 
            var key = Encoding.ASCII.GetString(desCrypto.Key);

            _gcHandle = GCHandle.Alloc(key, GCHandleType.Pinned);

            return key;
        }

        public void CleanUp(string secretKey)
        {
            ZeroMemory(_gcHandle.AddrOfPinnedObject(), secretKey.Length * 2);
            _gcHandle.Free();
        }

        /// <summary>
        ///  Steve Lydford - 12/05/2008.
        /// 
        ///  Encrypts a file using Rijndael algorithm.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="secretKey">The encryption key.</param>
        public void EncryptFile(string inputFile, string outputFile, string secretKey)
        {
            try
            {
                var ue = new UnicodeEncoding();
                var key = ue.GetBytes(secretKey);
                var rmCrypto = new RijndaelManaged();
                var cryptFile = outputFile;

                using (var fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {
                    using (var cs = new CryptoStream(fsCrypt, rmCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write))
                    {
                        using (var fsIn = new FileStream(inputFile, FileMode.Open))
                        {
                            int data;
                            while ((data = fsIn.ReadByte()) != -1)
                                cs.WriteByte((byte) data);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine(@"Error");
            }
        }

        /// <summary>
        ///  Steve Lydford - 12/05/2008.
        /// 
        ///  Decrypts a file using Rijndael algorithm.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="secretKey">The decruption key.</param>
        public void DecryptFile(string inputFile, string outputFile, string secretKey)
        {
            try
            {
                var ue = new UnicodeEncoding();
                var key = ue.GetBytes(secretKey);
                var rmCrypto = new RijndaelManaged();

                using (var fsCrypt = new FileStream(inputFile, FileMode.Open))
                {
                    using (var cs = new CryptoStream(fsCrypt, rmCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read))
                    {
                        using (var fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                                fsOut.WriteByte((byte)data);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine(@"Error");
            }
        }
    }
}