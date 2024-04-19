using UnityEngine;

public class SpriteDirectionalController : MonoBehaviour
{
    public float backAngle = 65f;
    public float sideAngle = 155f;
    public Transform mainTransform;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Camera is updated first
    void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Debug.DrawRay(Camera.main.transform.position, camForwardVector, Color.magenta);

        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);

        Vector2 animationDirection = new Vector2(0f, -1f);

        float angle = Mathf.Abs(signedAngle);

        if (angle < backAngle){
            // back animation
            animationDirection = new Vector2(0f, -1f);
        }
        else if (angle < sideAngle){
            // this changes the side animation based on what side
            // the camera is viewing the sprite from
            if (signedAngle < 0){
                animationDirection = new Vector2(-1f, 0f);
            }
            else{
                animationDirection = new Vector2(1f, 0f);
            }
        }
        else{
            // front animation
            animationDirection = new Vector2(0f, 1f);
        }

        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
