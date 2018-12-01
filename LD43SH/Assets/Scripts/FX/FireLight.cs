using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    public float amplitude = 1.0F;
    public float frequency = 10.0F;

    protected Light light;
    protected float intensity;
    protected float seed;

    private void Start()
    {
        light = GetComponent<Light>();

        intensity = light.intensity;
        seed = Random.value;
	}
	
	void Update()
    {
        light.intensity = intensity + Mathf.PerlinNoise(Time.time * frequency, seed) * amplitude;

    }
}
