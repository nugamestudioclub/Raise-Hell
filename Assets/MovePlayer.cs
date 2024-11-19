using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
    }

    void Flip() {
        if(Keyboard.current.downArrowKey.wasPressedThisFrame) {
            Debug.Log("Down arrow key pressed");
            transform.Rotate(0, 180, 0);
        }
    }
}
