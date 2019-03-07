using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private bool frozen;

    //Character movement
    /// <summary>
    /// Player movement speed.
    /// </summary>
    public float defaultSpeed = 6.0f;
    private float speed;
    /// <summary>
    /// Player jump force.
    /// </summary>
    public float jumpSpeed = 4.0f;
    private bool jumpAllowed = true;
    private Rigidbody rb;

    //Character rotation
    /// <summary>
    /// Used to determine direction.  1 is forward movement, 0 is idle, -1 is backward movement.
    /// </summary>
    private int velocityForward;
    /// <summary>
    /// Used to determine direction.  1 is right movement, 0 is idle, -1 is left movement.
    /// </summary>
    private int velocityRight;
    #endregion

    void Awake()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
        }
        catch (MissingComponentException)
        {
            print("ERROR in PlayerController.cs: The object \"" + name + "\" does not have a Rigidbody component attached to it.");
        }
    }

    void Update()
    {
        if (!frozen)
        {
            Movement();

            if (!Input.GetKey(KeyCode.LeftAlt))
                Orientation();
        }
    }

    void Movement()
    {
        #region Process input
        float inputHoriz = Input.GetAxis("Horizontal"); //Get input from AD and left/right arrows, returns scale of -1 to 1
        float inputVert = Input.GetAxis("Vertical"); //Get input from WS and up/down arrows, returns scale of -1 to 1

        if (inputHoriz != 0 || inputVert != 0)
        {
            if (Math.Abs(inputHoriz) > Math.Abs(inputVert)) //Check if absolute value of horiz input is greater than that of vert
                speed = Math.Abs(defaultSpeed * Math.Abs(inputHoriz) - (defaultSpeed * .35f)); //Set speed based on horiz input and reduce due to strafing
            else if (inputVert < 0)
                speed = Math.Abs(defaultSpeed * Math.Abs(inputVert) - (defaultSpeed * .35f)); //Set speed based on vert input and reduce due to backwards direction
            else
                speed = Math.Abs(defaultSpeed * Math.Abs(inputVert)); //Set speed based on vert input
        }
        #endregion

        #region Physical movement
        Vector3 velocity;

        if (Input.GetKey(KeyCode.LeftAlt))
            velocity = transform.position + ((inputVert * transform.forward) + (inputHoriz * transform.right)) * speed * Time.deltaTime; //Determine velocity based on character rotation
        else
            velocity = transform.position + ((inputVert * Camera.main.transform.forward) + (inputHoriz * Camera.main.transform.right)) * speed * Time.deltaTime; //Determine velocity based on camera rotation

        rb.MovePosition(velocity); //Move

        if (Input.GetKeyDown(KeyCode.Space) && jumpAllowed)
        {
            jumpAllowed = false;
            rb.velocity += jumpSpeed * Vector3.up; //Jump
        }
        #endregion
    }

    void Orientation()
    {
        Quaternion characterRotation = Camera.main.transform.rotation;
        characterRotation.x = 0;
        characterRotation.z = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, characterRotation, 10 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        jumpAllowed = true;
    }

    public void FreezePlayer(bool freezeState)
    {
        frozen = freezeState;
    }
}
