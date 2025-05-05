using UnityEngine;

public class DestroyOnAnimationEvent : MonoBehaviour
{
    private void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * 2;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
