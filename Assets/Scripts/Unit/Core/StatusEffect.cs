
/// <summary>
/// Script in charge of status effects to Units.
/// </summary>
public class StatusEffect
{
    private readonly string statusName;
    private readonly string statusSpritePath;
    private readonly byte statusEffectID;
    private readonly byte[] timerRange;

    public StatusEffect(string statusName, string statusSpritePath, byte statusEffectID, byte[] timerRange)
    {
        this.statusName = statusName;
        this.statusSpritePath = statusSpritePath;
        this.statusEffectID = statusEffectID;
        this.timerRange = timerRange;
    }

    /// <summary>
    /// Is this Unit stunned? Cannot move at all. MAY REMOVE
    /// </summary>
    /// <returns></returns>
    public bool Stunned()
    {
        return statusEffectID == 1;
    }
    /// <summary>
    /// Is this Unit running a cold? Null certain attacks.
    /// </summary>
    /// <returns></returns>
    public bool RunningCold()
    {
        return statusEffectID == 2;
    }
    /// <summary>
    /// Is this Unit burnt? Null certain attacks.
    /// </summary>
    /// <returns></returns>
    public bool Burned()
    {
        return statusEffectID == 3;
    }
    public bool InHeat()
    {
        return statusEffectID == 4;
    }
    public bool Depressed()
    {
        return statusEffectID == 5;
    }
    public bool Paralyzed()
    {
        return statusEffectID == 6;
    }
    public bool Finished()
    {
        return statusEffectID == 7;
    }
    public string StatusName()
    {
        return statusName;
    }
    public string StatusSpritePath()
    {
        return statusSpritePath;
    }
    public byte[] TimerRange()
    {
        return timerRange;
    }
}
