using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of all generic particles.
/// </summary>
public class ParticleManager : ParticlePooler
{
    private static ParticleManager instance;
    private List<GameObject> parryParticles;
    private List<GameObject> blockParticles;

    private void Awake()
    {
        instance = this;
        PrepareBlockParticles();
        PrepareParryParticles();
    }

    public static ParticleManager Instance()
    {
        return instance;
    }
    /// <summary>
    /// Spawn a parry particle. If all are used, create a new parry particle and make in list again.
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SpawnParryParticle(Vector3 worldPosition)
    {
        foreach (GameObject particle in parryParticles)
        {
            if (particle.activeInHierarchy == false)
            {
                particle.transform.SetParent(null);
                particle.transform.position = worldPosition;
                particle.SetActive(true);
                particle.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
        InstantiateParryParticle();
        SpawnParryParticle(worldPosition);
    }
    /// <summary>
    /// Spawn a block particle. If all are used, create a new block particle and make in list again.
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SpawnBlockParticle(Vector3 worldPosition)
    {
        foreach (GameObject particle in blockParticles)
        {
            if (particle.activeInHierarchy == false)
            {
                particle.transform.SetParent(null);
                particle.transform.position = worldPosition;
                particle.SetActive(true);
                particle.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
        InstantiateBlockParticle();
        SpawnBlockParticle(worldPosition);
    }
    private void PrepareParryParticles()
    {
        parryParticles = new List<GameObject>(3);
        for (int i = 0; i < parryParticles.Capacity; i++)
        {
            InstantiateParryParticle();
        }
    }
    private void PrepareBlockParticles()
    {
        blockParticles = new List<GameObject>(3);
        for (int i = 0; i < blockParticles.Capacity; i++)
        {
            InstantiateBlockParticle();
        }
    }
    private void InstantiateBlockParticle()
    {
        GameObject particleTemp = Instantiate(Resources.Load<GameObject>("Particles/NormalHit/Particles_Block"));
        particleTemp.SetActive(false);
        particleTemp.transform.SetParent(gameObject.transform);
        particleTemp.GetComponent<Particle>().SetParticlePooler(this);
        blockParticles.Add(particleTemp);
    }
    private void InstantiateParryParticle()
    {
        GameObject particleTemp = Instantiate(Resources.Load<GameObject>("Particles/NormalHit/Particles_Parry"));
        particleTemp.SetActive(false);
        particleTemp.transform.SetParent(gameObject.transform);
        particleTemp.GetComponent<Particle>().SetParticlePooler(this);
        parryParticles.Add(particleTemp);
    }

}
