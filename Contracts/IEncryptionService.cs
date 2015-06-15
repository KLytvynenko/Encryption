using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEncryptionService
    {
        MemoryStream DecryptFile(string encryptedSourceFile);
        FileInfo EncryptFile(string keyUserId, Stream sourceFile, string encryptedFile);
    }
}
