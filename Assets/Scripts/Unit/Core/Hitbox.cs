using UnityEngine;

/// <summary>
/// Script used to reference the Hitbox object.
/// </summary>
public class Hitbox : MonoBehaviour
{
    private bool forAirborne;

    private void Awake()
    {
        if (gameObject.name.Contains("Airborne"))
        {
            forAirborne = true;
        }
    }

    /// <summary>
    /// Is this hitbox only meant for Units in the air?
    /// </summary>
    /// <returns></returns>
    public bool ForAirborne()
    {
        return forAirborne;
    }
}
