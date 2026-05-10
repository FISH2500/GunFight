using UnityEngine;
using UnityEngine.UI;

public class SetJoinCodeUI : MonoBehaviour
{

    [SerializeField]
    private GameObject joinCodeUIBack;

    [SerializeField]
    Button joinCancel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joinCancel.onClick.AddListener(HideJoinCodeUI);
        gameObject.GetComponent<Button>().onClick.AddListener(ShowJoinCodeUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowJoinCodeUI() 
    {
        joinCodeUIBack.SetActive(true);
    }

    void HideJoinCodeUI()
    {
        joinCodeUIBack.SetActive(false);
    }
}
