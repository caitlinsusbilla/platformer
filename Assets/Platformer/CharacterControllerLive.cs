using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterControllerLive : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed =  10f;
    public float jumpImpulse = 50f;
    public float jumpBoost = 3f;
    public bool isGrounded;

    private bool gameOver = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // void FixedUpdate(){
        
    // }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float sprintMultiply = isSprinting ? 1.5f : 1f;
        
        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.velocity += Vector3.right * horizontalMovement * Time.deltaTime * acceleration * sprintMultiply;

        
        Collider col = GetComponent<Collider>();
        float halfHeight = col.bounds.extents.y + 0.03f;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + Vector3.down * halfHeight;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, halfHeight);
        Color lineColor = (isGrounded) ? Color.red : Color.blue;

        

        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){

            rbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);

        }else if(!isGrounded && Input.GetKey(KeyCode.Space)){

            if (rbody.velocity.y > 0){
                rbody.AddForce(Vector3.up * jumpBoost, ForceMode.Force);
            }
            
        }

        if (Math.Abs(rbody.velocity.x) > maxSpeed){
            Vector3 newVel = rbody.velocity;
            newVel.x = Math.Clamp(newVel.x, -maxSpeed, maxSpeed);
            rbody.velocity = newVel;
        }
        if(rbody.velocity.x > maxSpeed){
            rbody.velocity = rbody.velocity.normalized;
        }
        if(isGrounded && Math.Abs(horizontalMovement) < 0.5f){
            Vector3 newVel = rbody.velocity;
            newVel.x *= 1f - Time.deltaTime;
            rbody.velocity = newVel;
        }
        rbody.velocity *= Math.Abs(horizontalMovement);

        float yaw = (rbody.velocity.x > 0) ? 90 : -90;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        float speed = rbody.velocity.x;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("Speed", Math.Abs(speed));
        anim.SetBool("In Air", !isGrounded);
        anim.SetBool("Shift", isSprinting);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            gameOver = true;
            GameOver(gameOver);
        }
    }

    public static void GameOver(bool whatHappened)
    {
        if (!whatHappened){
            Debug.Log("Game over, you ran out of time");
        }else{
            Debug.Log("Congratulations! You completed the level");
        }

       UnityEditor.EditorApplication.isPlaying = false;
    }
}
