using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRandomStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1000f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
