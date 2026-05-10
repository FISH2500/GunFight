using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager instance { private set; get; }

    [SerializeField]
    TextMeshProUGUI[] nameText;

    [SerializeField]
    TextMeshProUGUI[] scoreText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ScoreボードのNameを出力
    public void OutPutScoreName(int playerID,string name) 
    {

        nameText[playerID].text = name;
    }

    public void OutPutScoreText(int playerID,int score) 
    {
        scoreText[playerID].text = score.ToString();
    }

}
