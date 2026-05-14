using Unity.Netcode;
using UnityEngine;

public class AimLine : NetworkBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    Material material;

    [SerializeField]
    Transform end;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsLocalPlayer) lineRenderer.gameObject.SetActive(false);
        //lineRenderer.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ハンドガンのエイム
    /// </summary>
    /// <param name="startPos">開始地点</param>
    /// <param name="endPos">到達点</param>
    /// <param name="dir">方向</param>
    public void ShowHandGunAimLine(Vector3 startPos,Vector3 endPos) 
    {
        lineRenderer.enabled = true;

        lineRenderer.material = material;

        lineRenderer.positionCount = 2;

        endPos.y = startPos.y;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);


        lineRenderer.startWidth = 7.5f;
        lineRenderer.endWidth = 7.5f;

        Debug.Log("Start:" + startPos + "END:" + endPos);

    }

    public void HideHandGunAimLine() 
    {
        lineRenderer.enabled = false;
    }

    /// <summary>
    /// ショットガンのエイム
    /// </summary>
    /// <param name="startPos">開始地点</param>
    /// <param name="endPos">到達点</param>
    /// <param name="dir">方向</param>
    public void ShowShotGunAimLine(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.enabled = true;

        lineRenderer.material = material;

        lineRenderer.positionCount = 2;

        endPos.y = startPos.y;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);


        lineRenderer.startWidth = 7.5f;
        lineRenderer.endWidth = 7.5f;

        Debug.Log("Start:" + startPos + "END:" + endPos);

    }

    public void HideShotGunAimLine()
    {
        lineRenderer.enabled = false;
    }
}
