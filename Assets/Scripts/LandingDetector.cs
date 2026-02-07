using UnityEngine;

public class LandingDetector : MonoBehaviour
{
    public ParticleSystem landingParticle;

    public float rayDistance = 0.6f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool wasGroundedLastFrame;

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer);

        if (!wasGroundedLastFrame && isGrounded)
        {
            PlayLandingEffect();
        }

        wasGroundedLastFrame = isGrounded;
    }

    void PlayLandingEffect()
    {
        if (landingParticle != null)
        {
            landingParticle.Stop();
            landingParticle.Play();
        }
    }
}
