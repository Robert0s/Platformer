using UnityEngine;
using UnityEngine.UI;

public class HeartAnimation : MonoBehaviour
{
    [SerializeField] private float punchScale = 1.4f;
    
    
    [SerializeField] private float animationSpeed = 8f;
    

    private Vector3 originalScale;
    private bool isAnimating = false;

    void Awake()
    {
        
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isAnimating)
        {
            
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                animationSpeed * Time.deltaTime
            );

            
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isAnimating = false;
            }
        }
    }

    
    public void Punch()
    {
        transform.localScale = originalScale * punchScale;
        isAnimating = true;
    }
}