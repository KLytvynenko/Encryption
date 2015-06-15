using Contracts;
using Starksoft.Aspen.GnuPG;
//using Starksoft.Cryptography.OpenPGP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EncryptionService : IEncryptionService
    {
        Gpg _gpg = new Gpg();
        string _appPath;

        public EncryptionService()
        {
            //TODO: install gpg to your PC and get GPG2.exe local path
            _appPath = @"C:\Program Files (x86)\GNU\GnuPG\pub\gpg2.exe";
        }

        //Encrypt/decrut works with docx,images,xlsx files
        public FileInfo EncryptFile(string keyUserId, Stream fileStream, string encryptedFile)
        {
            // check parameters
            if (string.IsNullOrEmpty(keyUserId))
                throw new ArgumentException("keyUserId parameter is either empty or null", "keyUserId");
            if (string.IsNullOrEmpty(encryptedFile))
                throw new ArgumentException("encryptedFile parameter is either empty or null", "encryptedFile");

                using (Stream encryptedFileStream = new FileStream(encryptedFile, FileMode.Create))
                {
                    _gpg.BinaryPath = _appPath;
                    _gpg.Recipient = keyUserId;
                    //  Perform encryption
                    _gpg.Encrypt(fileStream, encryptedFileStream);
                    return new FileInfo(encryptedFile);
                }
        }

        public MemoryStream DecryptFile(string encryptedSourceFile)
        {
            // check parameters
            if (string.IsNullOrEmpty(encryptedSourceFile))
                throw new ArgumentException("encryptedSourceFile parameter is either empty or null", "encryptedSourceFile");
           
            var ms =new MemoryStream();
            using (FileStream encryptedSourceFileStream = new FileStream(encryptedSourceFile, FileMode.Open))
            {
                //  make sure the stream is at the start.
                encryptedSourceFileStream.Position = 0;             
                _gpg.BinaryPath = _appPath;

                //TODO: write your password
                _gpg.Passphrase = "password";

                 //  Decrypt
                _gpg.Decrypt(encryptedSourceFileStream, ms);
                return ms;
            }
        }
    }
}
