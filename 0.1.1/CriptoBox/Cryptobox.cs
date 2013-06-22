using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

/// <summary>
/// ToDo List:
/// 7. Шифрование файлов
/// 8. Добавить ГОСТ 28147-89
/// 10. Возможность шаринга контента через соц сети //Отчасти
/// 11. Поиск по приложению //Отчати
/// 12. Хелп по криптографии через app bar //Отчасти
/// 13. В шаринг файлов через SkyDrive
/// 14. Попробовать добавить Threefish
/// последняя правка 28.04.2013
/// </summary>

class Cryptobox
{
    IBuffer EncryptBuffer;
    IBuffer DecryptBuffer;
    IBuffer SourceTextBuffer;
    IBuffer IVBuffer = null;
    public String DecryptTextOutput, EncryptTextOutput;
    CryptographicKey CryptoKey;  
 
    /// <summary>
    /// Увеличение размера пользовательноского ключа до размеров необходимых стандартом шифрования
    /// </summary>
    /// <param name="keySize">размер ключа, соответствующий стандарту шифрования (64, 128, etc)</param>
    /// <param name="InputKey">Принимает пользовательский ключ</param>
    /// <returns>возвращает [CryptographicKey] CryptoKey</returns>
    public CryptographicKey KeyExpansion(UInt32 keySize, string InputKey)
    {
        for (int i = 0; i <= keySize; i++)
        {
            //надо думать
        }
            return CryptoKey;
    }
    

    /// <summary>
    /// Создает случайную последовательность размера lenght бит
    /// </summary>
    /// <param name="lenght">размер последовательности</param>
    /// <returns>Случайная последовательсноть</returns>
    public string Random(UInt32 lenght)
    {
       string RandTextOutput;
       RandTextOutput = CryptographicBuffer.EncodeToBase64String(CryptographicBuffer.GenerateRandom((lenght+7)/8));
       return RandTextOutput;
    }

    /// <summary>
    /// Шифрование текста
    /// </summary>
    /// <param name="SourceText">Исходный (открытый) текст</param>
    /// <param name="InputKey">Ключ шифрование</param>
    /// <param name="AlgorythmName">Имя алгоритма шифрования</param>
    /// <returns>Зашифрованный текст</returns>
    public string EncryptMode(string SourceText, string InputKey, string AlgorythmName, string IV, string KeySize)
    {
        SymmetricKeyAlgorithmProvider Algorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(AlgorythmName);
        IBuffer KeyBuffer = CryptographicBuffer.ConvertStringToBinary(InputKey, BinaryStringEncoding.Utf16LE);
        IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary("ll234hl@kljh5:Annc!6002mz", BinaryStringEncoding.Utf16LE);

        //CryptoKey = Algorithm.CreateSymmetricKey(KeyBuffer);

//#########################
        KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha512);

        KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 10000);

        CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(KeyBuffer);

        IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, Convert.ToUInt32(KeySize)/8);

        //string test = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, keyMaterial);

        CryptoKey = Algorithm.CreateSymmetricKey(keyMaterial);
//###############

       // CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);



        //Надо присваивать IV пользовательское значение или генерированное значение IV
        if (AlgorythmName.Contains("CBC"))
        {
            IVBuffer = CryptographicBuffer.ConvertStringToBinary(IV, BinaryStringEncoding.Utf16LE); 
        }

        // Set the data to encrypt. 
       SourceTextBuffer = CryptographicBuffer.ConvertStringToBinary(SourceText, BinaryStringEncoding.Utf16LE);
       
        // Encrypt
        EncryptBuffer = Windows.Security.Cryptography.Core.CryptographicEngine.Encrypt(CryptoKey, SourceTextBuffer, IVBuffer);
        //Convert to Base64
        EncryptTextOutput = CryptographicBuffer.EncodeToBase64String(EncryptBuffer);
        string test = CryptographicBuffer.EncodeToBase64String(keyMaterial);

        //return test;// 
        return EncryptTextOutput;
    }


    /// <summary>
    /// Дешифрования текст
    /// </summary>
    /// <param name="SourceText">Исходный (шифрованный) текст</param>
    /// <param name="InputKey">Ключ шифрования</param>
    /// <param name="AlgorytmName">Имя алгоритма дешифрования</param>
    /// 
    /// <returns>Расшифрованный (открытый) текст</returns>
    public string DecrypMode(string SourceText, string InputKey, string AlgorytmName, string IV, string KeySize)
    {
        SymmetricKeyAlgorithmProvider Algorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(AlgorytmName);
        IBuffer KeyBuffer = CryptographicBuffer.ConvertStringToBinary(InputKey, BinaryStringEncoding.Utf16LE);
        IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary("ll234hl@kljh5:Annc!6002mz", BinaryStringEncoding.Utf16LE);
        //CryptoKey = Algorithm.CreateSymmetricKey(keymaterial);




        KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha512);

        KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 10000);

        CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(KeyBuffer);

        IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms,Convert.ToUInt32(KeySize)/8);
        
      //  string test= CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, keyMaterial);

        CryptoKey = Algorithm.CreateSymmetricKey(keyMaterial);




        if (AlgorytmName.Contains("CBC"))
        {
            IVBuffer = CryptographicBuffer.ConvertStringToBinary(IV, BinaryStringEncoding.Utf16LE);
        }
        // Set the data to encrypt. 
       SourceTextBuffer = CryptographicBuffer.DecodeFromBase64String(SourceText);

        // Decrypt
        DecryptBuffer = Windows.Security.Cryptography.Core.CryptographicEngine.Decrypt(CryptoKey, SourceTextBuffer, IVBuffer);
        DecryptTextOutput = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, DecryptBuffer);//Надо думать над реализацией Base64
        
    return DecryptTextOutput;
    }


    /// <summary>
    /// Хеширование текста
    /// </summary>
    /// <param name="SourceText">Исходный текст для хэширования</param>
    /// <param name="HashAlgName">Название алгоритма хеширования</param>
    /// <returns>Хэш сообщения</returns>
    public string HashMode(string SourceText, string HashAlgName)
    {
        // Create a HashAlgorithmProvider object.
        HashAlgorithmProvider Hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgName);
        IBuffer SourceTextBuffer = CryptographicBuffer.ConvertStringToBinary(SourceText, BinaryStringEncoding.Utf16LE);
       // IBuffer SourceTextBuffer = CryptographicBuffer.DecodeFromBase64String(SourceText);
        IBuffer HashBuffer = Hash.HashData(SourceTextBuffer);
        return CryptographicBuffer.EncodeToHexString(HashBuffer);
    }
}
