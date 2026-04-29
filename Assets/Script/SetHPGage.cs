using Unity.Netcode;
using UnityEngine;

public class SetHPGage : NetworkBehaviour
{

    void Start()
    {
        if (!IsLocalPlayer) 
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            rectTransform.rotation=Quaternion.Euler(-30,0,0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
