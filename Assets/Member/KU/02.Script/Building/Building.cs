using UnityEngine;


public class Building : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;

    public BuildingSO buildingSO;
    
    private int level = 1;
    private int minionCount = 0;

    [Header("Collider View Settings")]
    public bool showCollider = true;
    [SerializeField] Color colliderColor = Color.green;
    [SerializeField] float lineWidth = 0.05f;

    public int NowLevel
    {
        get
        {
            return level;
        }
        set
        {
            if (buildingSO.maxLevel+1 > value)
                level = value;
        }
    }
    public int NowMinion
    {
        get
        {
            return minionCount;
        }
        set
        {
            if(buildingSO.maxMinion+1 > value)
                minionCount = value;
        }
    }

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        int wSize = Mathf.RoundToInt(buildingSO.width / buildingSO.maxW);
        boxCollider.size = new Vector2(buildingSO.maxW + 2,  wSize + 2);

        InitializeBuilding();
    }

    private void Update()
    {
        UpdateColliderView();
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.green;

            Vector3 boxPos = transform.position + (Vector3)boxCollider.offset;

            Vector3 boxSize = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);

            Gizmos.DrawWireCube(boxPos, boxSize);
        }
    }
    public void BuildUpgrade()
    {
        NowLevel++;
    }
    public void MinionPlus(int plus)
    {
        NowMinion += plus;
    }



    private void InitializeBuilding()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        
        InitializeLineRenderer();
    }
    private void InitializeLineRenderer()
    {
        lineRenderer.positionCount = 4;

        lineRenderer.loop = true;

        lineRenderer.useWorldSpace = false;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = colliderColor;
        lineRenderer.endColor = colliderColor;

        lineRenderer.enabled = showCollider;
    }
    private void UpdateColliderView()
    {
        lineRenderer.enabled = showCollider;

        if (!showCollider || boxCollider == null) return;

        Vector2 size = boxCollider.size;
        Vector2 offset = boxCollider.offset;

        Vector3[] points = new Vector3[5]
        {
            offset + new Vector2(-size.x/2, -size.y/2),
            offset + new Vector2(-size.x/2, size.y/2),
            offset + new Vector2(size.x/2, size.y/2),
            offset + new Vector2(size.x/2, -size.y/2),
            offset + new Vector2(-size.x/2, -size.y/2)
        };

        lineRenderer.SetPositions(points);
    }
}