using UnityEngine;
using System.Collections;

public class Player5 : MonoBehaviour
{

    private bool myPlayer = false;

    public void SetOwner(bool amOwner)
    {
        myPlayer = amOwner;
    }

    void Update()
    {
        if (myPlayer)
        {
            Vector3 moveDirection = new Vector3(-1 * Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            float speed = 5;
            transform.Translate(speed * moveDirection * Time.deltaTime);//now really move!
        }
    }

}