using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moove : MonoBehaviour
{
    public bool moove = true;
    public float aspeed = 10.0f;
    public float arotationSpeed = 100.0f;
    private void FixedUpdate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * aspeed;
        float rotation = Input.GetAxis("Horizontal") * arotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // true je vais en arrière
        if (translation < 0)
        {
            moove = true;
        } else
        {
            moove = false;
        }
        // Move translation along the object's z-axis
        //transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        //transform.Rotate(0, rotation, 0);
    }
}
