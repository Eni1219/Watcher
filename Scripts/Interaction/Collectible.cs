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
                    SubtitleManager.instance.ShowSubtitle("����äƴ_�����������Τ�ä����ѥ������Ҋ�Ƥߤ褫", 2f);
                }
                Debug.Log("Got USB");
                break;
        
        }
        gameObject.SetActive(false);
    }
}
