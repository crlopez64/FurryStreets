using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of all generic particles.
/// </summary>
public class ParticleManager : ParticlePooler
{
    private static ParticleManager instance;
    private List<GameObject> hitParticles0;
    private List<GameObject> hitParticles1;
    private List<GameObject> parryParticles;
    private List<GameObject> blockParticles;
    private List<GameObject> hitParticlesAir;

    private void Awake()
    {
        instance = this;
        PrepareHitParticles();
        PrepareBlockParticles();
        PrepareParryParticles();
    }

    public static ParticleManager Instance()
    {
        return instance;
    }

    /// <summary>
    /// Spawn a random Normal Hit particle. If all are used, create a new Normal Hit particle and make in list again.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="facingDirection"></param>
    public void SpawnHitParticle(Vector3 worldPosition, float facingDirection)
    {
        int selected = Random.Range(0, 2);
        List<GameObject> selectedList;
        switch(selected)
        {
            case 0:
                selectedList = hitParticles0;
                break;
            case 1:
                selectedList = hitParticles1;
                break;
            default:
                Debug.LogError("ERROR: Value greater than 2 items!! " + selected);
                return;
        }
        foreach (GameObject particle in selectedList)
        {
            if (particle.activeInHierarchy == false)
            {
                particle.transform.SetParent(null);
                particle.transform.position = worldPosition;
                Vector3 currentFlip = new Vector3(Mathf.Sign(facingDirection), Mathf.Sign(facingDirection), Mathf.Sign(facingDirection));
                particle.transform.localScale = currentFlip;
                particle.SetActive(true);
                particle.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
        InstantiateHitParticles((byte)selected);
        SpawnHitParticle(worldPosition, facingDirection);
    }
    /// <summary>
    /// Spawn a random Normal Hit Air particle. If all are used, create a new Normal Hit Air particle and make in list again.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="facingDirection"></param>
    public void SpawnHitAirParticle(Vector3 worldPosition, float facingDirection)
    {
        foreach (GameObject particle in hitParticlesAir)
        {
            if (particle.activeInHierarchy == false)
            {
                particle.transform.SetParent(null);
                particle.transform.position = worldPosition;
                Vector3 currentFlip = new Vector3(Mathf.Sign(facingDirection), Mathf.Sign(facingDirection), Mathf.Sign(facingDirection));
                particle.transform.localScale = currentFlip;
                particle.SetActive(true);
                particle.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
        InstantiateHitAirParticles();
        SpawnHitParticle(worldPosition, facingDirection);
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
        parryParticles = new List<GameObject>(2);
        for (int i = 0; i < parryParticles.Capacity; i++)
        {
            InstantiateParryParticle();
        }
    }
    private void PrepareBlockParticles()
    {
        blockParticles = new List<GameObject>(2);
        for (int i = 0; i < blockParticles.Capacity; i++)
        {
            InstantiateBlockParticle();
        }
    }
    private void PrepareHitParticles()
    {
        hitParticles0 = new List<GameObject>(2);
        hitParticles1 = new List<GameObject>(2);
        hitParticlesAir = new List<GameObject>(2);
        for(int i = 0; i < hitParticles0.Capacity; i++)
        {
            InstantiateHitParticles(0);
            InstantiateHitParticles(1);
            InstantiateHitAirParticles();
        }
    }
    private void InstantiateHitParticles(byte index)
    {
        GameObject particleTemp = Instantiate(Resources.Load<GameObject>("Particles/NormalHit/Particles_NormalHit" + index));
        particleTemp.SetActive(false);
        particleTemp.transform.SetParent(gameObject.transform);
        particleTemp.GetComponent<Particle>().SetParticlePooler(this);
        switch (index)
        {
            case 0:
                hitParticles0.Add(particleTemp);
                break;
            case 1:
                hitParticles1.Add(particleTemp);
                break;
            default:
                hitParticles0.Add(particleTemp);
                break;
        }
    }
    private void InstantiateHitAirParticles()
    {
        GameObject particleTemp = Instantiate(Resources.Load<GameObject>("Particles/NormalHit/Particles_NormalHit" + 0));
        particleTemp.SetActive(false);
        particleTemp.transform.SetParent(gameObject.transform);
        particleTemp.GetComponent<Particle>().SetParticlePooler(this);
        hitParticlesAir.Add(particleTemp);
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
