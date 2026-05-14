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


    private bool die=false;

    private NetworkVariable<float> HP = new NetworkVariable<float>(
       4000f,
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
   );

    public NetworkVariable<bool> invincible = new NetworkVariable<bool>(default,
   NetworkVariableReadPermission.Everyone,
   NetworkVariableWritePermission.Server
);//無敵フラグ

    private Coroutine Heal;


    private void Start()
    {


        GameObject standPlayerSetObj = GameObject.Find("StandPlayerSet");

        if (standPlayerSetObj != null)
        {
            StandPlayerSet standPlayerSet = standPlayerSetObj.GetComponent<StandPlayerSet>();

            standPlayerSet.Connect();

        }

        if (IsLocalPlayer)
        {
            playerHpBar.gameObject.SetActive(true);
            enemyHpBar.gameObject.SetActive(false);
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


    /// <summary>
    /// 待機開始
    /// </summary>
    [ServerRpc]
    public void StandStartServerRpc()
    {

        invincible.Value = true;//無敵解除
        Debug.Log("スタンドモード" + invincible.Value);
    }

    /// <summary>
    /// バトル開始
    /// </summary>
    [ServerRpc]
    public void BattleStartServerRpc() 
    {
        
        invincible.Value = false;//無敵解除
        Debug.Log("ばとるモード" + invincible.Value);
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
            if (!IsServer) return;
            {
                if (!invincible.Value)//無敵じゃなければ
                // ダメージの反映はサーバー
                ApplyDamage(bullete.Damage,bullete.OwnerID.Value);
                // 弾を消す
                other.GetComponent<NetworkObject>().Despawn();
            }

            

        }
    }

    private void ApplyDamage(float dmg,ulong shooterID)
    {

        if (die) return;

        HP.Value -= dmg;

        if (HP.Value <= 0)//死亡処理
        {
            die = true;
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
        //BattleManager.instance.ResultServerRpc(OwnerClientId);//勝敗の結果

        GetComponent<NetworkObject>().Despawn();

        GameManager.instance.Finish();//次の行動用ボタンの表示

        ScoreManager.instance.AddScoreServerRpc(shooterID);
        var playerObj = NetworkManager.Singleton.ConnectedClients[shooterID].PlayerObject;//キルしたPlayerを取得

        playerObj.GetComponent<Status>().invincible.Value = true;


        
    }
}
