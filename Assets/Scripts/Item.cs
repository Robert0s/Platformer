using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // ScriptableObject = asset de date in Unity
    // Cream cate un asset pentru fiecare tip de item (pistol, bomba etc.)
    
    public string itemName;
    public Sprite icon;         // Iconita in inventar
    public int maxStack = 5;    // Cate bucati poti tine
    public GameObject projectilePrefab; // Ce se trage cand folosesti itemul
}