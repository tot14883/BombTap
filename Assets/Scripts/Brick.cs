using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private static Dictionary<string, Sprite> mTileImages;

    public bool mine = false;

    public float radius = 1.42f;

    public SpriteRenderer tile = null;

    public List<Brick> mNeighbors;

    private bool mShowed = false;

    public GameObject bomb;
    public float power = 10.0f;
    public float radiusBomb = 5.0f;
    public float upForce = 1.0f;
    public GameObject bigExplosionPrefab;

    public static void BuildSpritesMap()
    {
        if (mTileImages == null)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/MinesweeperSpritesheet");
            mTileImages = new Dictionary<string, Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                mTileImages.Add(sprites[i].name, (Sprite)sprites[i]);
            }
        }
    }

    void Start()
    {
        BuildSpritesMap();
    }

    void OnValidate()
    {
        FindNeighbors();
    }

    private void FindNeighbors()
    {
        var allBricks = GameObject.FindGameObjectsWithTag("Brick");

        mNeighbors = new List<Brick>();

        for (int i = 0; i < allBricks.Length; i++)
        {
            var brick = allBricks[i];
            var distance = Vector3.Distance(transform.position, brick.transform.position);
            if (0 < distance && distance <= radius)
            {
                mNeighbors.Add(brick.GetComponent<Brick>());
            }
        }

        Debug.Log($"{mNeighbors.Count} neighbors");
    }

    public void ShowSecret()
    {
        if (mShowed) return;

        mShowed = true;

        string name;

        if (mine)
        {
            name = "TileMine";
            Detonate();
        }
        else
        {
            int num = 0;
            mNeighbors.ForEach(brick => {
                if (brick.mine) num += 1;
            });
            name = $"Tile{num}";
        }

        Sprite sprite;
        if (mTileImages.TryGetValue(name, out sprite))
            tile.sprite = sprite;
    }

    void Detonate()
    {
        Instantiate(bigExplosionPrefab, transform.position, transform.rotation);
        Vector3 explosionPosition = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radiusBomb);
        foreach (Collider hit in colliders)
        {

            Limb limb = hit.transform.GetComponent<Limb>();
            if (limb != null)
            {
                limb.GetHit();
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radiusBomb, upForce, ForceMode.Impulse);
            }

            ExplosionDamage explosionDamage = hit.GetComponent<ExplosionDamage>();
            if (explosionDamage != null)
            {
                explosionDamage.Detonate();
            }
        }

        // Destroy(gameObject);
    }
}
