using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 90f;

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}