using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of particles.
/// </summary>
public class Particle : MonoBehaviour
{
    private ParticlePooler particlePooler;
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (!particles.IsAlive())
        {
            ReturnToParent();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set the Particle Pooler to this object.
    /// </summary>
    /// <param name="particlePooler"></param>
    public void SetParticlePooler(ParticlePooler particlePooler)
    {
        this.particlePooler = particlePooler;
    }
    private void ReturnToParent()
    {
        transform.SetParent(particlePooler.transform);
    }
}
