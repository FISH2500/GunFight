using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameEnter : MonoBehaviour
{

    [SerializeField]
    TMP_InputField nameInputField;

    [SerializeField]
    GameObject selectChar;

    [SerializeField]
    GameObject setName;

    public static string PlayerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(EnterName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterName() 
    {
        PlayerName = nameInputField.text;

        selectChar.SetActive(true);

        setName.SetActive(false);

        Debug.Log(name);

    }

}
