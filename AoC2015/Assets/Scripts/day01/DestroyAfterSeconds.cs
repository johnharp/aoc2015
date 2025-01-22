using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float delaySeconds;

    void Start()
    {
        Destroy(gameObject, delaySeconds);
    }

}
