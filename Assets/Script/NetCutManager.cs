using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetCutManager : NetworkBehaviour
{
    [SerializeField]
    GameObject netCutUI;//ネットが切断されたことを表すUI

    [SerializeField]
    Button enterButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        enterButton.onClick.AddListener(PushEnterButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientDisconnected(ulong clientID) 
    {
        Debug.Log("Player切断:" + clientID);
        netCutUI.SetActive(true);//接続が切れたことを示すUI
    }

    private void PushEnterButton()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
    }



}
