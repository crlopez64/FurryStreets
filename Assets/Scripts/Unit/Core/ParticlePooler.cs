using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script holding all the particles for object pooling.
/// </summary>
public class ParticlePooler : MonoBehaviour
{
    private List<GameObject> specialMoveParticles;

    /// <summary>
    /// Set Special Moves particles when playing Mateo (Wolf).
    /// </summary>
    public void SetParticlesSpecialMovesWolf()
    {
        if (specialMoveParticles == null)
        {
            specialMoveParticles = new List<GameObject>(4);
        }
        specialMoveParticles.Clear();
        GameObject particleTemp = Instantiate(Resources.Load<GameObject>("Particles/SpecialMoves/Particles_FireHorizontal"));
        particleTemp.SetActive(false);
        particleTemp.transform.SetParent(gameObject.transform);
        particleTemp.GetComponent<Particle>().SetParticlePooler(this);
        specialMoveParticles.Add(particleTemp);
    }
    /// <summary>
    /// Spawn the Special Move particle
    /// </summary>
    /// <param name="particleIndex"></param>
    /// <param name="worldPosition"></param>
    public void SpawnParticleSpecial(byte particleIndex, Vector3 worldPosition, float facingDirection)
    {
        if (particleIndex >= specialMoveParticles.Count)
        {
            particleIndex = (byte)(specialMoveParticles.Count - 1);
        }
        //If particle is not active, turn it on;
        //Else, restart the particle.
        if (specialMoveParticles[particleIndex].activeInHierarchy == false)
        {
            specialMoveParticles[particleIndex].transform.SetParent(transform.root);
            specialMoveParticles[particleIndex].transform.position = worldPosition;
            Vector3 currentFlip = new Vector3(Mathf.Sign(facingDirection), Mathf.Sign(facingDirection), Mathf.Sign(facingDirection));
            specialMoveParticles[particleIndex].transform.localScale = currentFlip;
            specialMoveParticles[particleIndex].SetActive(true);
            specialMoveParticles[particleIndex].GetComponent<ParticleSystem>().Play();
            return;
        }
        else
        {
            specialMoveParticles[particleIndex].GetComponent<ParticleSystem>().Stop();
            specialMoveParticles[particleIndex].transform.position = worldPosition;
            Vector3 currentFlip = new Vector3(Mathf.Sign(facingDirection), Mathf.Sign(facingDirection), Mathf.Sign(facingDirection));
            specialMoveParticles[particleIndex].transform.localScale = currentFlip;
            specialMoveParticles[particleIndex].GetComponent<ParticleSystem>().Play();
        }
    }
}
