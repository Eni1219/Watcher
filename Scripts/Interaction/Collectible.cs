using UnityEngine;

public enum ItemType
{ 
 key,
 USB
}
public class Collectible : Interactable
{
    public ItemType itemType;

    public override void OnInteract()
    {
        switch (itemType)
        { 
            case ItemType.key:
                CollectionManager.instance.hasKey = true;
                AudioManager.instance.Play("KeyPickUp");
                Debug.Log("Got Key");
                break;
                case ItemType.USB:
                CollectionManager.instance.hasUSB = true;
                AudioManager.instance.Play("USBPickUp");
                if (SubtitleManager.instance != null)
                {
                    SubtitleManager.instance.ShowSubtitle("仇木勻化復井切中切扎氏及支勻仃﹜由末戊件匹��化心方井", 2f);
                }
                Debug.Log("Got USB");
                break;
        
        }
        gameObject.SetActive(false);
    }
}
