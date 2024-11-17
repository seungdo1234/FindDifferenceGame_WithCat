
using UnityEngine;
using System.IO;


public class SaveLoadSystem 
{
    private AESCrypto crypto = new AESCrypto();
    private string path = Path.Combine(Application.dataPath, "PlayerData.json");


    public void DataSave(PlayerSaveData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true); // 데이터 직렬화
        string encryptedJson = crypto.EncryptString(json); // 직렬화 된 데이터 암호화

        File.WriteAllText(path, encryptedJson);
    }

    public PlayerSaveData DataLoad()
    {
        if (!File.Exists(path))
        {
            Debug.Log("데이터가 존재하지 않습니다 !");
            return new PlayerSaveData();
        }

        string encryptedJson = File.ReadAllText(path); // 암호화된 데이터 읽어옴
        string json = crypto.DecryptString(encryptedJson); // 복호화 및 역직렬화

        return JsonUtility.FromJson<PlayerSaveData>(json);
    }
}

