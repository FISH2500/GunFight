using UnityEngine;
using UnityEngine.UI;

public class SetSkill : MonoBehaviour
{
    [SerializeField]
    CardData cardData;

    [SerializeField]
    Sprite icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Image>().sprite = icon;
        //SetCard(cardData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(CardData data) 
    {
        cardData = data;

        icon = cardData.image;

        GetComponent<Image>().sprite = cardData.image;
    }

    public void SetSprite() 
    {
        
    }
}
