using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    private float velocity;

    [SerializeField]
    private Vector3 direction;

    private Vector3 initialPosition;
    private float backgroundWidth;

    void Start()
    {
        initialPosition = transform.position;
        InitBackgroundWidth();
    }

    void Update()
    {
        float displacement = velocity * Time.time;
        float cycle = displacement % backgroundWidth;
        transform.position = initialPosition + cycle * direction;
    }

    private void InitBackgroundWidth()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.sprite.bounds.max.x * 2;
        backgroundWidth = spriteWidth * transform.localScale.x;
    }
}
