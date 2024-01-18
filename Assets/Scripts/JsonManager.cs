using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance;
    [HideInInspector] public UserData userData;
    private string jsonpath;

    // Start is called before the first frame update
    void Start()
    {
        // Only one JsonManager on scene.
        if (!Instance) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        // Default status
        jsonpath = $"{Application.persistentDataPath}/userdata.json";
        UserData defaultData = JsonUtility.FromJson<UserData>(Resources.Load<TextAsset>("MainData").text);

        // Create user json if the file doesnt exist
        if (!File.Exists(jsonpath)) SaveDataJSON(defaultData);

        // Load the user's json file
        userData = JsonUtility.FromJson<UserData>(File.ReadAllText(jsonpath));
        GameManager.Instance.gameUI.UpdateCoinsUI();
    }

    // Saves before disabling
    void OnDisable() { SaveDataJSON(userData); }

    // Saves the user data with new values
    public void SaveDataJSON(UserData save) { File.WriteAllText(jsonpath, JsonUtility.ToJson(save)); }
}

// Update coins


// User data json serializable class
[Serializable]
public class UserData
{
    public int coins;
}