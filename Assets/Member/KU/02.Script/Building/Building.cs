using UnityEngine;

public class Building : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public BuildingSO buildingSO;
    
    private int level = 1;
    private int minionCount = 0;

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
        boxCollider.size = new Vector2(buildingSO.maxW + 4,  wSize+ 4);
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
}