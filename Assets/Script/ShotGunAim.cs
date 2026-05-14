using UnityEngine;

public class ShotGunAim : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private float angle = 60f;
    [SerializeField] private int segments = 30;

    private Mesh mesh;

    private MeshRenderer meshRenderer;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        meshRenderer=GetComponent<MeshRenderer>();

        //ShowShotGunAim();
    }

    /// <summary>
    /// ショットガンのエイム線を出す
    /// </summary>
    public void ShowShotGunAim(Vector3 dir)
    {
        meshRenderer.enabled = true;

        dir.y = 0.0f;

        transform.rotation=Quaternion.LookRotation(dir);

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // 中心点
        vertices[0] = Vector3.zero;

        float currentAngle = -angle / 2f;
        float angleStep = angle / segments;

        // 円弧部分の頂点生成
        for (int i = 0; i <= segments; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 pos =
                new Vector3(
                    Mathf.Sin(rad),
                    0,
                    Mathf.Cos(rad)
                ) * distance;

            vertices[i + 1] = pos;

            currentAngle += angleStep;
        }

        // 三角形生成
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    public void HideShotGunAim() 
    {
        meshRenderer.enabled = false;
    }
}
