using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        lineRenderer.startWidth = 1.5f;
        lineRenderer.endWidth = 1.5f;
    }

    public void HideHandGunAimLine() 
    {
        lineRenderer.enabled = false;
    }
}
