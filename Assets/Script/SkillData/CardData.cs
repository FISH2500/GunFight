using UnityEngine;
using UnityEngine.UI;


public enum CARDTYPE
{
    HPBUFF,
    ATKBUFF,
    SKILLBUFF,
}

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData:ScriptableObject
{
    public string cardName;

    public Sprite image;

    public CARDTYPE cardType;

    public bool isUse;

    public void Init() 
    {
        isUse = false;
    }

}
