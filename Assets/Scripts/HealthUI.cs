using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private PlayerHealth playerHealth;

    private HeartAnimation[] heartAnimations;
    private int previousHealth;

    void Start()
    {
        // Luam componenta de animatie de pe fiecare inima
        heartAnimations = new HeartAnimation[hearts.Length];
        for (int i = 0; i < hearts.Length; i++)
        {
            heartAnimations[i] = hearts[i].GetComponent<HeartAnimation>();
        }

        previousHealth = playerHealth.CurrentHealth;
    }

    void Update()
    {
        // Daca viata s-a schimbat, animam inima corespunzatoare
        if (playerHealth.CurrentHealth < previousHealth)
        {
            // Inima care tocmai a fost pierduta face punch
            int lostHeartIndex = playerHealth.CurrentHealth;
            if (lostHeartIndex < heartAnimations.Length && heartAnimations[lostHeartIndex] != null)
            {
                heartAnimations[lostHeartIndex].Punch();
            }
        }

        previousHealth = playerHealth.CurrentHealth;

        // Actualizam sprite-urile
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < playerHealth.CurrentHealth ? fullHeart : emptyHeart;
        }
    }
}