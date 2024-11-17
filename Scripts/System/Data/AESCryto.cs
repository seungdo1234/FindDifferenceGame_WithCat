using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AESCrypto
{ 
    private byte[] key; // 암호화에 사용되는 키
    private byte[] iv; // 초기화 벡터 
    private readonly string keyPath = Path.Combine(Application.persistentDataPath, "aesKey.dat");
    private readonly string ivPath = Path.Combine(Application.persistentDataPath, "aesIV.dat");

    public AESCrypto()
    {
        if (File.Exists(keyPath) && File.Exists(ivPath)) // 키와 IV가 존재하는 지 확인
        {
            // 존재한다면 해당 키와 IV를 읽어옴
            key = File.ReadAllBytes(keyPath);
            iv = File.ReadAllBytes(ivPath);
        }
        else
        {
            // 없다면 다시 생성
            key = GenerateRandomBytes(32); // 256-bit key
            iv = GenerateRandomBytes(16);  // 128-bit IV

            File.WriteAllBytes(keyPath, key);
            File.WriteAllBytes(ivPath, iv);
        }
    }
    
    // 지정된 길이의 랜덤 바이트 배열을 생성
    private byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        
        // RNGCryptoServiceProvider : 난수 발생기
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }
    
    public string EncryptString(string plainText)
    {
        
        // AES 객체와 같은 암호화 객체는 네이티브 리소스를 사용하므로 사용후에 반드시 해제해야한다.
        // using을 사용하여 작업이 끝난 후에 자동으로 리소스가 해제되게 설계했다.
        // 네이티브 리소스는 운영 체제나 하드웨어와 직접적으로 상호작용하는 리소스 (GC에 의해 자동으로 관리되지 않기 떄문에 수동으로 해제해줘야 함)
        using (Aes aesAlg = Aes.Create()) // AES 알고리즘 생성
        {
            // 키, IV 설정
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // 암호화 변환기를 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            // 평문 텍스트를 암호화
            byte[] encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
            
            // 암호화된 바이트 배열을 Base64 문자열로 변환 후 반환
            return System.Convert.ToBase64String(encrypted);
        }
    }

    public string DecryptString(string cipherText) // 복호화 함수
    {
        // Base64 문자열을 바이트 배열로 변환
        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create()) // AES 알고리즘 생성
        {
            // 키, IV 설정
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // 복호화 변환기를 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // 암호화된 바이트 배열을 복호화 
            byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

            // 복호화된 바이트 배열을 UTF-8 문자열로 변환 후 반환
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}