using UnityEngine;
using UnityEngine.UI;

public class HeartAnimation : MonoBehaviour
{
    [SerializeField] private float punchScale = 1.4f;
    // Cat de mult se mareste inima la damage
    
    [SerializeField] private float animationSpeed = 8f;
    // Cat de repede revine la normal

    private Vector3 originalScale;
    private bool isAnimating = false;

    void Awake()
    {
        // Salvam scala originala ca sa revenim la ea
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isAnimating)
        {
            // Revenim smooth la scala originala
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                animationSpeed * Time.deltaTime
            );

            // Daca suntem aproape de scala originala, oprim animatia
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isAnimating = false;
            }
        }
    }

    // Apelata din HealthUI cand playerul ia damage
    public void Punch()
    {
        transform.localScale = originalScale * punchScale;
        isAnimating = true;
    }
}