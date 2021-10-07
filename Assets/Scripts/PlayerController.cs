using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Collider coll;

    public float walkSpeed = 8f;
    public float jumpSpeed = 7f;

    bool pressedJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkHandler();

        JumpHandler();
    }

    // Gets input from axises, then moves player accordingly.
    void WalkHandler()
    {
        float distance = walkSpeed * Time.deltaTime;

        float hAxis = Input.GetAxis("Horizontal");

        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * distance, 0f, vAxis * distance);

        Vector3 currPosition = transform.position;

        Vector3 newPosition = currPosition + movement;

        rb.MovePosition(newPosition);
    }

    void JumpHandler()
    {
        float jAxis = Input.GetAxis("Jump");

        bool isGrounded = CheckGrounded();

        if (jAxis > 0f)
        {
            // print(isGrounded);
            if (!pressedJump && isGrounded)
            {
                print("Jumping!");
                pressedJump = true;

                Vector3 jumpVector = new Vector3(0f, jumpSpeed, 0f);

                rb.velocity = rb.velocity + jumpVector;
            }
        } else
        {
            pressedJump = false;
        }
    }

    bool CheckGrounded()
    {
        // The size of the object's collider.
        float sizeX = coll.bounds.size.x;
        float sizeZ = coll.bounds.size.z;
        float sizeY = coll.bounds.size.y;

        // The bottom four corners of the object
        // +0.01f to lower the checks by a tad.
        Vector3 corner1 = transform.position + new Vector3(
            sizeX / 3, -sizeY / 2 + 0.1f, sizeZ / 3);
        Vector3 corner2 = transform.position + new Vector3(
            -sizeX / 3, -sizeY / 2 + 0.1f, sizeZ / 3);
        Vector3 corner3 = transform.position + new Vector3(
            sizeX / 3, -sizeY / 2 + 0.1f, -sizeZ / 3);
        Vector3 corner4 = transform.position + new Vector3(
            -sizeX / 3, -sizeY / 2 + 0.1f, -sizeZ / 3);

        RaycastHit hit1;

        // Raycast the ground...
        bool grounded1 = Physics.Raycast(corner1, Vector3.down,
            out hit1, 0.15f);
        bool grounded2 = Physics.Raycast(corner2, Vector3.down, 0.15f);
        bool grounded3 = Physics.Raycast(corner3, Vector3.down, 0.15f);
        bool grounded4 = Physics.Raycast(corner4, Vector3.down, 0.15f);

        // And check to see if the angle is too much.

        //Debug
        print(grounded1);
        Vector3 testPos = corner1;
        testPos.y = testPos.y - 0.15f;
        Debug.DrawLine(corner1, testPos, Color.green);

        return (grounded1 || grounded2 || grounded3 || grounded4);
    }
}
