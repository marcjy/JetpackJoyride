using UnityEngine;

public class BackgroundSlider : MonoBehaviour
{
    public Vector2 LimitPositionX;
    public Vector2 ResetPositionX;
    public float SlideSpeed = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, LimitPositionX) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, LimitPositionX, SlideSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = ResetPositionX;
        }
    }
}
