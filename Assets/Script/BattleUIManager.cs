using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager instance { private set; get; }

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

    public void OutPutScoreText(int playerID,int score) 
    {
        scoreText[playerID].text = score.ToString();
    }

}
