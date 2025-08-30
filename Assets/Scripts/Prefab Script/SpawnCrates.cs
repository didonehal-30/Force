using UnityEngine;

public class SpawnCrates : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] crates;
    private float cooldownTimer = Mathf.Infinity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > spawnCooldown)
            Spawn();

        cooldownTimer += Time.deltaTime;
    }

    void Spawn()
    {
        cooldownTimer = 0;

        int disabledCrateIndex = FindCrate();
        crates[disabledCrateIndex].transform.position = spawnPoint.position;
        crates[disabledCrateIndex].transform.localScale = new Vector3(2f, 2f, 1f);
        crates[disabledCrateIndex].GetComponent<KineticObject>().enableRespawnCrate();
       // Rigidbody2D rb = crates[disabledCrateIndex].GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0.3f;

    }

    private int FindCrate()
    {
        for (int i = 0; i < crates.Length; i++)
        {
            if (!crates[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
