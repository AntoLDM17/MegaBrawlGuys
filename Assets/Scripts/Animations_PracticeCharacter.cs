using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations_PracticeCharacter : MonoBehaviour
{   
    public KeyCode attack01key;
    public KeyCode attack02key;
    public KeyCode blockingKey;
    public GameObject characterOBJ;
    Animator controllerANIM;

    public bool isBlocking;

    // Start is called before the first frame update
    void Start()
    {
        controllerANIM = characterOBJ.GetComponent<Animator>();
        isBlocking = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(attack01key) || Input.GetButtonDown("Fire1"))
        {
            Attack01();
        }
       if (Input.GetKeyDown(attack02key) || Input.GetButtonDown("Fire2"))
        {
            Attack02();
        }
       if (Input.GetKeyDown(blockingKey) || Input.GetButton("Fire3"))
        {
                isBlocking = true;
                controllerANIM.SetBool("Blocking", isBlocking);

        }
       if (Input.GetKeyUp(blockingKey) || Input.GetButtonUp("Fire3"))
        {
                 isBlocking = false;
                 controllerANIM.SetBool("Blocking", isBlocking);
          }
    }

    [ContextMenu("Attack 01")]
    public void Attack01()
    {
        controllerANIM.SetTrigger("Punch");
        isBlocking = false;
        controllerANIM.SetBool("Blocking", isBlocking);
    }

    [ContextMenu("Attack 02")]
    public void Attack02()
    {
        controllerANIM.SetTrigger("Kick");
        isBlocking = false;
        controllerANIM.SetBool("Blocking", isBlocking);
    }

    [ContextMenu("Block")]
    public void setBlocking()
    {
        if (isBlocking)
        {
            isBlocking = false;
        }
        else
        {
            isBlocking = true;
        }
        controllerANIM.SetBool("Blocking", isBlocking);
    }
}
