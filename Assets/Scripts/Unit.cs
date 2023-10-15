using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Unit : MonoBehaviour
{
    public enum Stats
    {
        DamageMultiplayer,
        Armor,
        Vampirism
    }

    [Header("Вражеский юнит")]
    [SerializeField] private Unit enemy;

    [Header("Стартовые статы")]
    [SerializeField] private int startHealth;
    [SerializeField] private int damage;
    [SerializeField] private int startArmor;
    [SerializeField] private int startVampirism;
    
    public Dictionary<BuffController.Buffs, int> activeBuffs = new Dictionary<BuffController.Buffs, int>();
    public List<(Stats, char, int)> activeBuffStats = new List<(Stats, char, int)>();

    public int Health
    {
        get
        {
            if (health > 100)
                health = 100;
            return health;
        } 
    }
    public int Damage => damage * DamageMultiplayer;
    public int DamageMultiplayer
    {
        get
        {
            damageMultiplayer = 1;
            foreach (var buff in activeBuffStats)
            {
                if (buff.Item1 == Stats.DamageMultiplayer)
                    damageMultiplayer = MathOperators.Calculate(buff.Item2, damageMultiplayer, buff.Item3);
            }
            return damageMultiplayer;
        }
    }
    public int Vampirism 
    { 
        get
        {
            vampirismBuff = 0;
            foreach(var buff in activeBuffStats)
            {
                if (buff.Item1 == Stats.Vampirism)
                    vampirismBuff = MathOperators.Calculate(buff.Item2, vampirismBuff, buff.Item3);
            }
            var vampirism = startVampirism + vampirismBuff;
            if (vampirism > 100)
                return 100;
            else if (vampirism < 0)
                return 0;
            else 
                return vampirism;
        } 
    }
    public int Armor
    {
        get
        {
            armorBuff = 0;
            foreach (var buff in activeBuffStats)
            {
                if (buff.Item1 == Stats.Armor)
                    armorBuff = MathOperators.Calculate(buff.Item2, armorBuff, buff.Item3);
            }
            var armor = startArmor + armorBuff;
            if (armor > 100)
                return 100;
            else if (armor < 0)
                return 0;
            else
                return armor;
        }
    }
    public Unit Enemy => enemy;


    private int health;
    private int armorBuff;
    private int vampirismBuff;
    private int damageMultiplayer = 1;
    private Random random = new Random();


    public Action OnHealthChanged;
    public static Action OnBuffChanged;
    public static Action OnUnitDied;

    private void OnEnable()
    {
        GameContoller.OnRoundEnded += OnRoundEndedEvent;
    }

    private void Awake()
    {
        health = startHealth;
    }

    public void Attack()
    {
        var vamp = (int)Math.Round((Damage - (0.01f * enemy.Armor * Damage)) * 0.01f * Vampirism, MidpointRounding.AwayFromZero);
        if(vamp != 0)
        {
            health += vamp;
            OnHealthChanged?.Invoke();
        }
        enemy.SetDamage(Damage);
    }

    private void SetDamage(int incomingDamage) //получение урона
    {
        incomingDamage -= (int)Math.Round(0.01f * Armor * incomingDamage, MidpointRounding.AwayFromZero);
        if (incomingDamage > 0)
        {
            health -= incomingDamage;
            if (health < 0)
            {
                OnUnitDied.Invoke();
                return;
            }
            else
                OnHealthChanged?.Invoke();
        }
    }

    public void SetBuff() //добавление баффа
    {
        if (activeBuffs.Count > 1) return;

        BuffController.Buffs newBuff;
        do
        {
            newBuff = (BuffController.Buffs)BuffController.BuffsArray.GetValue(random.Next(BuffController.BuffsArray.Length));
        }
        while (activeBuffs.ContainsKey(newBuff)); //проверка на добавление неповторяющихся баффов

        BuffController.AddBuffEffects(this, newBuff);
        activeBuffs.Add(newBuff, random.Next(1, 4));

        OnBuffChanged?.Invoke();
    }

    private void OnRoundEndedEvent() //событие при окончании раунда
    {
        foreach (var buff in activeBuffs.ToList()) //уменьшение длительности баффов и удаление лишних
        {
            activeBuffs[buff.Key] -= 1;
            if (activeBuffs[buff.Key] == 0)
            {
                BuffController.RemoveBuffEffects(this, buff.Key);
                activeBuffs.Remove(buff.Key);
            }
        }
        OnBuffChanged?.Invoke();
    }

    private void OnDisable()
    {
        GameContoller.OnRoundEnded -= OnRoundEndedEvent;
    }
}
