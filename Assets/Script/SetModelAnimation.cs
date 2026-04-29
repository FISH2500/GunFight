using UnityEngine;
using UnityEngine.UI;

public class SetSelectModel : MonoBehaviour
{
    [SerializeField]
    Button[] selectCharButton;//キャラクター選択ボタン

    [SerializeField]
    Button enterButton;//決定ボタン

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var b in selectCharButton) 
        {
  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
