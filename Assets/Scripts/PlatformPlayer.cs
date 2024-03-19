using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpForce = 12.0f;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;
        float minY = box.bounds.min.y;
        float spanX = box.bounds.extents.x - 0.01f;
        float centerX = box.bounds.center.x;
        Vector2 corner1 = new Vector2(centerX - spanX, minY - 0.1f);
        Vector2 corner2 = new Vector2(centerX + spanX, minY - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool grounded = false;
        if (hit != null)
        {
            grounded = true;
        }
        bool isIdle = Mathf.Approximately(deltaX, 0);
        body.gravityScale = (grounded && isIdle) ? 0 : 1;
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        MovingPlatform platform = null;
        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }
        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }
        anim.SetFloat("speed", Mathf.Abs(deltaX));
        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }
    }
}
