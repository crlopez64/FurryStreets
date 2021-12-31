using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script holding all the particles for object pooling.
/// </summary>
public class ParticlePooler : MonoBehaviour
{
    private List<GameObject> hitMinorPool;

    public GameObject[] hitParticles;

    private void Awake()
    {
        hitMinorPool = new List<GameObject>(5);
        for(int i = 0; i < hitMinorPool.Capacity; i++)
        {
            GameObject particleTemp = Instantiate(hitParticles[0], transform.position, transform.rotation);
            particleTemp.SetActive(false);
            particleTemp.transform.SetParent(gameObject.transform);
            particleTemp.GetComponent<Particle>().SetParticlePooler(this);
            hitMinorPool.Add(particleTemp);
        }
    }

    /// <summary>
    /// Spawn a particle in a pool.
    /// </summary>
    /// <param name="particleIndex"></param>
    public void SpawnParticle(byte particleIndex, Vector3 worldPosition)
    {
        switch(particleIndex)
        {
            case 0:
                foreach(GameObject particle in hitMinorPool)
                {
                    if (particle.activeInHierarchy == false)
                    {
                        particle.transform.SetParent(transform.root);
                        particle.transform.position = worldPosition;
                        particle.SetActive(true);
                        particle.GetComponent<ParticleSystem>().Play();
                        return;
                    }
                }
                GameObject newParticleToPool = GameObject.Instantiate(hitParticles[0], transform.position, transform.rotation);
                hitMinorPool.Add(newParticleToPool);
                hitMinorPool.TrimExcess();
                break;
            default:
                break;
        }
    }
}
