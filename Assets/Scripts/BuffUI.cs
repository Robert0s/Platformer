using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private GameObject buffSlotPrefab;
    

    [SerializeField] private Transform buffContainer;
    

    
    private Dictionary<string, (GameObject slot, TextMeshProUGUI timerText)> activeBuffs 
        = new Dictionary<string, (GameObject, TextMeshProUGUI)>();

    
    public void ShowBuff(string buffName, Sprite icon, float duration)
    {
        if (activeBuffs.ContainsKey(buffName))
        {
            
            StartCoroutine(UpdateTimer(buffName, duration));
            return;
        }

        
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

        
        if (activeBuffs.ContainsKey(buffName))
        {
            Destroy(activeBuffs[buffName].slot);
            activeBuffs.Remove(buffName);
        }
    }
}