using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;
    public float MovementSpeed = 1;
    public float MoveX;
    public float MoveY;
    public float LookSpeed = 1;
    public float LookX;
    public float LookY;
    public Transform Head;
    public float distance;
    public bool IsGrounded;
    public float GravityVelocity;
    public float gravity = 9.81f;
    public float JumpPower = 1;
    public float GroundCheckTime = 0.1f;
    private float NextCheckTime;
    private bool CanMove = true;
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CanMove = menu.activeSelf;
        if(CanMove == false)
        {
            return;
        }
        IsGrounded = CheckGrounded();
        GetInput();
        transform.Rotate(transform.up*LookX*LookSpeed*Time.deltaTime);
        Head.Rotate(-Vector3.right*LookY*LookSpeed*Time.deltaTime);
        Vector3 MovementVector = new Vector3(MoveX, 0, MoveY);
        controller.Move(transform.TransformVector(MovementVector)*MovementSpeed*Time.deltaTime+Vector3.up*GravityVelocity*Time.deltaTime);
        if(Input.GetButtonDown("Jump")&IsGrounded)
        {
            GravityVelocity = JumpPower;
            NextCheckTime = Time.time + GroundCheckTime;
        }
        
    }

    void FixedUpdate() 
    {
        if(IsGrounded == false)
        {
            GravityVelocity -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            GravityVelocity = 0;
        }
    }

    void GetInput()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveY = Input.GetAxis("Vertical");
        LookX = Input.GetAxis("Look X");
        LookY = Input.GetAxis("Look Y");
    }

    bool CheckGrounded()
    {
        if(Time.time > NextCheckTime)
        {
            RaycastHit hit;
            return Physics.Raycast(transform.position, Vector3.down, out hit, distance);
        }
        return false;
    }

    public void DisableMovement()
    {
        CanMove = false;
    }

    public void EnableMovement()
    {
        CanMove = true;
    }
}
