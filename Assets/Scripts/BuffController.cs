using System;

public static class BuffController
{
    public enum Buffs
    {
        DoubleDamage,
        ArmorSelf,
        ArmorDestruction,
        VampirismSelf,
        VampirismDecrease
    }

    public static Array BuffsArray => Enum.GetValues(typeof(Buffs));

    public static void AddBuffEffects(Unit unit, Buffs buff)
    {
        switch (buff)
        {
            case Buffs.DoubleDamage:
                unit.activeBuffStats.Add((Unit.Stats.DamageMultiplayer, '*', 2));
                break;
            case Buffs.ArmorSelf:
                unit.activeBuffStats.Add((Unit.Stats.Armor, '+', 50));
                break;
            case Buffs.ArmorDestruction:
                unit.Enemy.activeBuffStats.Add((Unit.Stats.Armor, '-', 10));
                break;
            case Buffs.VampirismSelf:
                unit.activeBuffStats.Add((Unit.Stats.Vampirism, '+', 50));
                unit.activeBuffStats.Add((Unit.Stats.Armor, '-', 25));
                break;
            case Buffs.VampirismDecrease:
                unit.Enemy.activeBuffStats.Add((Unit.Stats.Vampirism, '-', 25));
                break;
        }
    }

    public static void RemoveBuffEffects(Unit unit, Buffs buff)
    {
        switch (buff)
        {
            case Buffs.DoubleDamage:
                unit.activeBuffStats.Remove((Unit.Stats.DamageMultiplayer, '*', 2));
                break;
            case Buffs.ArmorSelf:
                unit.activeBuffStats.Remove((Unit.Stats.Armor, '+', 50));
                break;
            case Buffs.ArmorDestruction:
                unit.Enemy.activeBuffStats.Remove((Unit.Stats.Armor, '-', 10));
                break;
            case Buffs.VampirismSelf:
                unit.activeBuffStats.Remove((Unit.Stats.Vampirism, '+', 50));
                unit.activeBuffStats.Remove((Unit.Stats.Armor, '-', 25));
                break;
            case Buffs.VampirismDecrease:
                unit.Enemy.activeBuffStats.Remove((Unit.Stats.Vampirism, '-', 25));
                break;
        }
    }
}


