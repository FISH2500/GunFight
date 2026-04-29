using UnityEngine;
using UnityEngine.UI;

public class SetJoinCodeUI : MonoBehaviour
{

    [SerializeField]
    private GameObject joinCodeUIBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
