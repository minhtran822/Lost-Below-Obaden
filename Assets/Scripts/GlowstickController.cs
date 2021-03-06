using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowstickController : MonoBehaviour
{
    public GameObject glowstick;
    public GameObject glowInstance;

    //number of glowsticks
    public int nglow = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && nglow > 0)
        {
            DropGlowstick();
        }
    }

    public void DropGlowstick()
    {
        glowInstance = Instantiate(glowstick, transform.position + new Vector3(5,-1.2f,0), Quaternion.identity);
        nglow--;
        Destroy(glowInstance, 30);
    }

}
