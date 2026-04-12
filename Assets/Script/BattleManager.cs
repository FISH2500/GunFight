using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BattleManager : NetworkBehaviour
{
    public static BattleManager instance { private set; get; }

    [SerializeField]
    TextMeshProUGUI resultText;


    void Awake() 
    {
        instance = this;
    }
    [ServerRpc]
    public void ResultServerRpc(ulong loser)//”s–k‚µ‚½ID‚ğˆø”‚Æ‚µ‚Äó‚¯æ‚é
    {

        ResultClientRpc(loser);


    }

    [ClientRpc]
    private void ResultClientRpc(ulong loser) 
    {
        Debug.Log("Loser:LoacalCLientID" + loser + ":" + NetworkManager.Singleton.LocalClientId);
        if (loser == NetworkManager.Singleton.LocalClientId)//”s–k‚µ‚½Player‚Ìê‡
        {
            SetLose();
        }
        else
        {
            SetWin();
        }
    }


    void SetWin()//ŸÒ‘¤‚ÌUI 
    {
        resultText.text = "WIN";
        Debug.Log("WIN");
    }

    void SetLose()//”s–k‘¤‚ÌUI 
    {
        resultText.text = "Lose";
        Debug.Log("Lose");
    }

}
