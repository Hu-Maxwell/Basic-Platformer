using Unity.VisualScripting;
using UnityEngine;

public class sprite_manager : MonoBehaviour
{
    public GameObject baseSprite;
    public GameObject walkingSprite;

    void Start()
    {
        baseSprite.SetActive(true);
        walkingSprite.SetActive(false);
    }


    void Update()
    {
        
    }
}
