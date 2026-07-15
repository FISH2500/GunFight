using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Button nextRound;

    [SerializeField] private GameObject skillCard;

    [SerializeField] private CardData[] skill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextRound.onClick.AddListener(SetSkillCard);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// スキルのカードを生成する関数
    /// </summary>
    public void SetSkillCard() 
    {
        
    }

}
