using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f; // How long the effect lasts

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}