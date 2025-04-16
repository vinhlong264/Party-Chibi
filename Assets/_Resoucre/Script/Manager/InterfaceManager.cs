using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    [SerializeField] private InputField roomName;
    [SerializeField] private InputField userName;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void setNickName(string nickName)
    {
        LocalData.name = nickName;
    }
}

[System.Serializable]
public static class LocalData
{
    public static string name;
}

