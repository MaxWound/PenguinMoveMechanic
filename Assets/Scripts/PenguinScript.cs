using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinScript : MonoBehaviour
{
    public static PenguinScript penguinScript;
    [SerializeField, Range(1f, 3f)]
    float BumpForce;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Transform floorTransform;
    [SerializeField]
    float snakePower;
    [SerializeField]
    GameObject massGo;
    Angle angleInstance;
    
    Transform penguinTransform;
    [SerializeField]
    Transform squareTransform;
    Rigidbody2D rb;
    [SerializeField]
    GameObject PenguinSprite;
    private bool Grounded;
    private bool ExittedGround;
    private bool rotSet;
    private void Awake()
    {
        penguinScript = this;
    }
    private void Start()
    {

        rotSet = false;
        snakePower = 10f;
        penguinTransform = gameObject.transform;
        angleInstance = Angle.angleInstance;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        rb.isKinematic = true;

    }
    private void Update()
    {
        

        if (penguinTransform.position.y - floorTransform.position.y >= 6f)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.black;
        }

        if (Grounded != true)
        {
            Rotate();
        }


    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Grounded != true)
        {

            if (other.tag == "Ground")
            {
                Grounded = true;
                SetAngle();
                if (rb.velocity.y <= -5)
                {
                    Bump();
                }
            }
        }

        if (other.tag == "Snake")
        {
            hitSnake();
        }


    }
    private void OnTriggerStay2D(Collider2D other)
    {


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            rotSet = false;
            Grounded = false;
        }
    }

    private void hitSnake()
    {
        rotSet = false;
        Grounded = false;
        rb.AddForce(Vector2.up * snakePower, ForceMode2D.Impulse);
    }
    public void hitPenguin(float angle, float power)
    {
        rotSet = false;
        rb.isKinematic = false;
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

    }
    private void SetGrounded(bool _bool)
    {
        Grounded = _bool;
    }
    void Bump()
    {
        rb.AddForce(Vector2.up * (rb.velocity.y * -1) * BumpForce, ForceMode2D.Impulse);

    }
    void SetAngle()
    {
        if (Grounded == true)
        {
            if (rotSet == false)
            {
                //penguinTransform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                print("rot");
                //squareTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                rb.SetRotation(90f);
                rotSet = true;
                Grounded = true;
            }

            //penguinTransform.localEulerAngles = new Vector3(0f, 0f, -90f);
        }

    }
    void Rotate()
    {
        rotSet = false;
        var direction = rb.velocity;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        //penguinTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        print("r");
        rb.SetRotation(angle);

    }
    IEnumerator SetGroundedTrueForSeconds(float seconds)
    {
        Grounded = true;
        yield return new WaitForSeconds(seconds);
        if (ExittedGround == true)
        {
            Grounded = false;
        }
        else
            Grounded = true;
    }
}
