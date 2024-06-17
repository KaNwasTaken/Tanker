using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankMotor : MonoBehaviour
{
    public int speed = 20;
    public int rotationSpeed = 50;
    public float movementSmoothTime = 0.2f;
    public float rotationSmoothTime = 0.2f;
    public float maxVelocity = 20;

    public Vector3 currentVelocity;
    Vector3 currentMovement;
    Vector2 currentRotation;
    Vector2 currentRotationVelocity;

    Rigidbody tankRigidbody;
    bool isGrounded;

    [SerializeField] GameObject flipButton;

    private void Start()
    {
        tankRigidbody = GetComponent<Rigidbody>();

    }

    private void Update()
    {

        flipButton.SetActive(transform.up.y < -0.8f);
       

        Physics.Raycast(transform.position, new Vector3(0, -1, 0), out RaycastHit hitInfo);
        if (Vector3.Magnitude(hitInfo.point - transform.position) < 5)
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    public void MoveTank(Vector2 inputVector)
    {
        if (!isGrounded) return;

        Vector3 movementVector = new Vector3(0, 0, inputVector.y);
        currentMovement = Vector3.SmoothDamp(currentMovement, movementVector, ref currentVelocity,  movementSmoothTime);

        if (Vector3.Magnitude(tankRigidbody.velocity) < maxVelocity)
        {
            tankRigidbody.velocity += (transform.TransformDirection(currentMovement) * speed * Time.deltaTime);
        }

        currentRotation = Vector2.SmoothDamp(currentRotation, inputVector, ref currentRotationVelocity, rotationSmoothTime);
        transform.rotation = transform.rotation * Quaternion.Euler(0, currentRotation.x * rotationSpeed * Time.deltaTime, 0);
    }

    public void FlipUpTank()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
