using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager instance;

    public bool hasKey=false;
    public bool hasUSB=false;

    private void Awake()
    {
        if(instance == null )
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
            Destroy(gameObject);
    }
}
