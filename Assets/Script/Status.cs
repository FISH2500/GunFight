using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField] private Image HPBar;

    [SerializeField]float MaxHP;
    [SerializeField] float HP;
    [SerializeField] float HealTime;
    [SerializeField] GameObject EnemyCanva;

    private Coroutine Heal;

    bool isHeal=false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (HP < MaxHP&&isHeal) 
        //{
        //    Debug.Log("Heal...");
        //    HP += MaxHP * 0.13f;//HPÇÃ13%Ç∏Ç¬âÒïúÇ∑ÇÈ

        //    if(HP>MaxHP) HP = MaxHP;//è„å¿Çí¥Ç¶ÇƒÇµÇ‹Ç¡ÇΩèÍçá

        //    HPBar.fillAmount = HP / MaxHP;

        //    isHeal = false;

        //    if (Heal != null)
        //    {
        //        StopCoroutine(Heal);
        //    }
        //    Heal = StartCoroutine(ReHeal());

        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullete") 
        {
            Debug.Log("hit");
            DeleteBullete bullte=other.gameObject.GetComponent<DeleteBullete>();
            Debug.Log(bullte);
            HP -= bullte.Damage;

            HPBar.fillAmount = HP / MaxHP;

            Destroy(other.gameObject);

            if (HP <= 0) 
            {
                Destroy(gameObject);
                Destroy(EnemyCanva);
            }

            if (Heal != null)
            {
                StopCoroutine(Heal);
            }
            Heal = StartCoroutine(ReHeal());

        }    
    }
    
    private IEnumerator ReHeal() 
    {
        yield return new WaitForSeconds(HealTime);

        while (HP < MaxHP)
        {
            HP += MaxHP * 0.13f;
            if (HP > MaxHP) HP = MaxHP;

            HPBar.fillAmount = HP / MaxHP;
            Debug.Log($"âÒïúíÜ... åªç›HP: {HP}");

            yield return new WaitForSeconds(1.5f); // 1.5ïbÇ≤Ç∆Ç…âÒïú
        }
    }
        
    
}
