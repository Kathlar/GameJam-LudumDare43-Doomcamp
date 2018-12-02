using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    public float snowifingSpeed = 0.1F;
    public float freezingSpeed = 0.3F;
    public bool isBurring = true;
    public float burringSpeed = 0.1F;
    public float destroyHeightTreshold = -3.0F;
    public Animator animator;

    protected bool isEnabled = false;
    protected Renderer renderer;
    protected float timeElapsed = 0.0F;
    protected float timeOffset = 0.0F;

    public void Enable(float timeOffset = 0.0F)
    {
        this.timeOffset = timeOffset;
        isEnabled = true;
    }

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (isEnabled)
        {
            if (timeOffset <= 0.0)
            {
                if (renderer.material.HasProperty("_Frozeninnedeness"))
                {
                    timeElapsed += Time.deltaTime;

                    float frozeninnedeness = Mathf.Clamp01(timeElapsed * snowifingSpeed);

                    renderer.material.SetFloat("_Frozeninnedeness", frozeninnedeness);
                }

                if (animator != null)
                {
                    animator.speed = Mathf.Max(Mathf.Lerp(animator.speed, -0.3F, Time.deltaTime * freezingSpeed), 0.0F);
                }

                if (isBurring)
                {
                    Vector3 position = animator.transform.position;

                    if (position.y > destroyHeightTreshold)
                    {
                        position.y -= Time.deltaTime * burringSpeed;

                        animator.transform.position = position;
                    }
                    else
                    {
                        Destroy(animator.gameObject);
                    }
                }
            }
            else
            {
                timeOffset -= Time.deltaTime;
            }
        }
    }
}
