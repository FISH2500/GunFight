using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Status : NetworkBehaviour
{
    [SerializeField] private GameObject playerHpBar;

    [SerializeField] private GameObject enemyHpBar;

    [SerializeField] private Image HPBar;

    [SerializeField] private Image EnemyHpBar;

    [SerializeField]float MaxHP;
    //[SerializeField] float HP;
    [SerializeField] float HealTime;
    [SerializeField] GameObject EnemyCanva;

    private NetworkVariable<float> HP = new NetworkVariable<float>(
       4000f,
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
   );

    private Coroutine Heal;


    private void Start()
    {
        if (IsLocalPlayer) 
        {
            playerHpBar.gameObject.SetActive( true );
            enemyHpBar.gameObject.SetActive( false );
        }
        else 
        {
            playerHpBar.gameObject.SetActive(false);
            enemyHpBar.gameObject.SetActive(true);
        }
    }
    public override void OnNetworkSpawn()
    {
        // 変更があったら HPBar を更新
        HP.OnValueChanged += OnHPChanged;

        // スポーン時にも UI を更新
        OnHPChanged(HP.Value, HP.Value);
    }

    private void OnHPChanged(float oldValue, float newValue)
    {
        // HPバー更新（全クライアント）
        if (IsLocalPlayer)
        {
            HPBar.fillAmount = newValue / MaxHP;
        } 
        else 
        {
            EnemyHpBar.fillAmount = newValue / MaxHP;
        }
        // HP 0 のときはクライアントでもキャンバス削除
        if (newValue <= 0)
        {
            if (EnemyCanva != null)
                Destroy(EnemyCanva);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        DeleteBullete bullete = other.GetComponent<DeleteBullete>();
        //
        Debug.Log("clientID:bullete.owner"+ OwnerClientId + ":" + bullete.OwnerID.Value);

        

        if (other.CompareTag("Bullete") &&OwnerClientId!=bullete.OwnerID.Value)//弾を打った人物じゃない場合
        {
            if (IsServer)
            {
                // ダメージの反映はサーバー
                ApplyDamage(bullete.Damage,bullete.OwnerID.Value);
                // 弾を消す
                other.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                // サーバーにリクエストを送る
                ApplyDamageServerRpc(bullete.Damage, bullete.OwnerID.Value);
            }

            Debug.Log("弾をうったのは" + bullete.OwnerClientId);

            ShowPlayerClientRpc(bullete.OwnerClientId);
            

        }
    }

    [ClientRpc]
    void ShowPlayerClientRpc(ulong id) 
    {
        Debug.Log("弾をうったのは" + id);
    }

    // サーバー側で HP を減らす
    [ServerRpc]
    private void ApplyDamageServerRpc(float dmg,ulong shooterID)
    {
        ApplyDamage(dmg,shooterID);
    }

    private void ApplyDamage(float dmg,ulong shooterID)
    {
        HP.Value -= dmg;

        if (HP.Value <= 0)//死亡処理
        {
            HP.Value = 0;
            Die(shooterID);
        }

        // 回復ループ開始
        if (Heal != null) StopCoroutine(Heal);
        Heal = StartCoroutine(ReHeal());
    }

    private IEnumerator ReHeal()
    {
        yield return new WaitForSeconds(HealTime);

        while (HP.Value < MaxHP)
        {
            HP.Value += MaxHP * 0.13f;
            if (HP.Value > MaxHP) HP.Value = MaxHP;

            yield return new WaitForSeconds(HealTime);
        }
    }

    // 死亡処理（サーバーのみ）
    private void Die(ulong shooterID)
    {
        Debug.Log("キルをしたのは" + shooterID);
        Debug.Log("敗北者ID"+OwnerClientId);
        BattleManager.instance.ResultServerRpc(OwnerClientId);

        // 破壊はサーバー管理
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }

        

        GameManager.instance.FinishServerRpc();

        ScoreManager.instance.AddScoreServerRpc(shooterID);

        //NetworkObject.OwnerClientId

        
    }
}
