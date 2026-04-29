using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputCodeField;

    [SerializeField]
    SetRelay setRelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PushJoinButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PushJoinButton() 
    {
        string joinCode= inputCodeField.text;

        setRelay.JoinRelay(joinCode);

    }

}
