using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { private set; get; }

    public NetworkVariable<bool> isStart=new NetworkVariable<bool>(readPerm:NetworkVariableReadPermission.Everyone,
        writePerm:NetworkVariableWritePermission.Server);

    [SerializeField]
    FloatingJoystick attack_Joystick;

    [SerializeField]
    DynamicJoystick move_Joystick;

    [SerializeField]
    GameObject startButtonObj;

    [SerializeField]
    GameObject leaveButtonObj;

    [SerializeField]
    GameObject winner;

    [SerializeField]
    GameObject loser;

    bool hasStartButton;
    bool setStart;
    private void Awake()
    {
        instance = this;
        //isStart.Value = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    // Update is called once per frame
    void Update()
    {
        //SetStartButton();

        StartCheck();
    }
    private void OnClientConnected(ulong clientID) 
    {
        
        int count = NetworkManager.Singleton.ConnectedClientsList.Count;

        if (!hasStartButton && count >= 2&&IsHost)//接続数が2以上になったらスタートボタンの有効化 
        {
            Debug.Log("スタートボタンセット...");
            hasStartButton = true;//スタートボタンが表示されたことを示すフラグ
            startButtonObj.SetActive(true);//スタートボタンを有効
        }
    }
    void SetStartButton() 
    {

    }

    void StartCheck() 
    {
        if (isStart.Value&&!setStart)//スタートボタンが押された場合 
        {
            Debug.Log("スタートボタンチェック...");
            SetJoyStick();



            if (IsHost)//ホスト側に表示されていたスタートボタンの非表示
            startButtonObj.SetActive(false);

            setStart = true;

        }



    }

    void SetJoyStick() //スティックの有効
    {
        move_Joystick.enabled = true;
        attack_Joystick.enabled = true;
    }

    public void StartButton() 
    {
        isStart.Value = true;
    }

    public void Finish() 
    {
        SetLeaveServerRpc();
    }


    [ServerRpc]

    void SetLeaveServerRpc() 
    {
        leaveButtonObj.SetActive(true);
    }
}
