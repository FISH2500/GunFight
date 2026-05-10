using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager instance { private set; get; }



    NetworkVariable<int> player1Score=new NetworkVariable<int>();

    NetworkVariable<int> player2Score = new NetworkVariable<int>();

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        //if (instance != null) 
        //{
        //    Destroy(gameObject);
        //}

        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1Score.OnValueChanged += Player1ScoreChange;
        player2Score.OnValueChanged += Player2ScoreChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Player1ScoreChange(int oldValue ,int newValue) 
    {
        BattleUIManager.instance.OutPutScoreText(0, player1Score.Value);
    }

    void Player2ScoreChange(int oldValue, int newValue)
    {
        BattleUIManager.instance.OutPutScoreText(1, player2Score.Value);
    }

    /// <summary>
    /// スコアを加算する関数
    /// </summary>
    /// <param name="addPlayerID">スコアを追加するPlayer</param>
    [ServerRpc]
    public void AddScoreServerRpc(ulong addPlayerID) 
    {

        if (addPlayerID == 0)
        {
            player1Score.Value++;
            
            Debug.Log("Player1にスコア加算" + player1Score.Value);
        }
        else
        {
            player2Score.Value++;
            Debug.Log("Player2にスコア加算" + player2Score.Value);
        }

    }


}
