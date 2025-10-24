using UnityEngine;

public class ReadableNote : Interactable
{
    [TextArea(5, 10)]
    public string noteText;
    public override void OnInteract()
    {
        if (SubtitleManager.instance == null)
        {
            Debug.LogError(" Cant find SubtitleManager£¡");
            return;
        }

        SubtitleManager.instance.ShowSubtitles(noteText);
  
    }
}