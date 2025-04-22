using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject contentParrent;

    public static PlayFabManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "1918B0";
        }
        var request = new LoginWithCustomIDRequest 
        { 
            CustomId = System.Guid.NewGuid().ToString(),
            CreateAccount = true ,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = LocalData.name,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisPlayName, OnError);

    }
    private void OnDisPlayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update Display name: " + result.DisplayName);
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>()
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatics, OnError);
    }

    public void OnUpdateStatics(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesfull sent Leader Board");
        GetLeaderBoard();
    }


    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderBoard, OnError);
    }

    public void OnGetLeaderBoard(GetLeaderboardResult result)
    {
        if(contentParrent.transform.parent.childCount > 0)
        {
            for(int i = 0; i < contentParrent.transform.childCount; i++)
            {
                Destroy(contentParrent.transform.GetChild(i).gameObject);
            }
        }



        foreach(var i in result.Leaderboard)
        {
            Debug.Log(i.Position+ " " + i.DisplayName + " " +i.StatValue);
            GameObject tmp = Instantiate(content, contentParrent.transform);
            if(tmp != null)
            {
                Text[] txt = tmp.GetComponentsInChildren<Text>();
                txt[0].text = i.Position.ToString();
                txt[1].text = i.DisplayName;
                txt[2].text = i.StatValue.ToString();
            }
               
        }
    }

}
