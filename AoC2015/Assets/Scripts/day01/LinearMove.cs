using UnityEngine;

public class LinearMove : MonoBehaviour
{
    [SerializeField] Vector3 movePerSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movePerSecond * Time.deltaTime);
    }
}
