using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light light;
    private bool initialEnabled;
    private AudioSource audioSource;
    [SerializeField] private AudioClip flickerSound;
    [SerializeField] private Vector2 flickerSpeedRange = new Vector2(0.05f, 0.1f);
    [SerializeField] private Vector2 intervalRange = new Vector2(1, 5);
    [SerializeField] private Vector2 numberOfFlickerRange = new Vector2(2, 5);

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        light = GetComponent<Light>();
        initialEnabled = light.enabled;

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervalRange.x, intervalRange.y));

            for (int i = 0; i < Random.Range(numberOfFlickerRange.x, numberOfFlickerRange.y); i++)
            {
                yield return new WaitForSeconds(Random.Range(flickerSpeedRange.x, flickerSpeedRange.y));
                light.enabled = !light.enabled;
                if (light.enabled)
                {
                    audioSource.PlayOneShot(flickerSound);
                }
            }

            yield return new WaitForSeconds(Random.Range(flickerSpeedRange.x, flickerSpeedRange.y));
            light.enabled = initialEnabled;
            if (light.enabled)
            {
                audioSource.PlayOneShot(flickerSound);
            }
        }
    }
}
