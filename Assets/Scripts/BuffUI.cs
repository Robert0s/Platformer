using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private GameObject buffSlotPrefab;
    // Prefabul unui slot de buff (icon + timer)

    [SerializeField] private Transform buffContainer;
    // Containerul unde apar buff-urile

    // Dictionar: buffName → (slotGameObject, timeRemaining)
    private Dictionary<string, (GameObject slot, TextMeshProUGUI timerText)> activeBuffs 
        = new Dictionary<string, (GameObject, TextMeshProUGUI)>();

    // Apelata din PowerupEffect cand porneste un buff
    public void ShowBuff(string buffName, Sprite icon, float duration)
    {
        if (activeBuffs.ContainsKey(buffName))
        {
            // Buff-ul exista deja, doar resetam timerul
            StartCoroutine(UpdateTimer(buffName, duration));
            return;
        }

        // Cream un slot nou
        GameObject slot = Instantiate(buffSlotPrefab, buffContainer);
        slot.transform.Find("Icon").GetComponent<Image>().sprite = icon;
        TextMeshProUGUI timerText = slot.transform.Find("Timer").GetComponent<TextMeshProUGUI>();

        activeBuffs[buffName] = (slot, timerText);
        StartCoroutine(UpdateTimer(buffName, duration));
    }

    private System.Collections.IEnumerator UpdateTimer(string buffName, float duration)
    {
        float remaining = duration;

        while (remaining > 0)
        {
            if (activeBuffs.ContainsKey(buffName))
                activeBuffs[buffName].timerText.text = Mathf.CeilToInt(remaining).ToString() + "s";

            remaining -= Time.deltaTime;
            yield return null;
        }

        // Timerul a expirat, stergem buff-ul din UI
        if (activeBuffs.ContainsKey(buffName))
        {
            Destroy(activeBuffs[buffName].slot);
            activeBuffs.Remove(buffName);
        }
    }
}