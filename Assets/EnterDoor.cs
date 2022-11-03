using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{

    private bool enterAllowed;
    private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<BlueDoor>())
        {
            sceneToLoad = "Level2";
            enterAllowed = true;
        }
        else if(collision.GetComponent<BrownDoor>())
        {
            sceneToLoad = "SampleScene";
            enterAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.GetComponent<BlueDoor>() || collision.GetComponent<BrownDoor>())
        {
            enterAllowed = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enterAllowed && Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        
    }
}
