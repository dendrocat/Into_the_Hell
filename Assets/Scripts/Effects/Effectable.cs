using System.Collections.Generic;
using UnityEngine;

public enum EffectNames { 
    Burning = 0, //горение
    Freezing = 1, //заморозка
    Stun = 2, //оглушение
    AttackChain = 3, //серия атак
    MiniGolem = 4, //мини-голем
    FireResistance = 5, //огнестойкость
    FrostResistance = 6, //морозостойкость
    ExplosionResistance = 7, //взрывостойкость
    IceDrifting = 8, //скольжение по льду
    ShieldBlock = 9, //блок щитом
    Shift = 10 //рывок
};
//Класс сущностей, на которые можно наложить эффект
public class Effectable: MonoBehaviour
{
    private const int EFFECT_COUNT = 11; //общее количество типов эффектов

    protected List<int> effectCount = new List<int>(); //количество эффектов на сущности
    protected List<float> effectRemainingTime = new List<float>(); //оставшееся время действия эффекта

    private static List<float> effectDuration = new List<float>(); //длительность эффекта (-inf для постоянных эффектов)
    private static List<int> maxEffectCount = new List<int>(); //макс. количество эффектов

    void Awake()
    {
        InitializeEffects();
    }
    protected void InitializeEffects()
    {
        for (int i = 0; i < EFFECT_COUNT; i++)
        {
            effectCount.Add(0);
            effectRemainingTime.Add(0.0f);
            effectDuration.Add(Mathf.NegativeInfinity);
            maxEffectCount.Add(0);
        }

        //initialize effects
        maxEffectCount[0] = 1;
        maxEffectCount[1] = 1;
        maxEffectCount[2] = 1;
        maxEffectCount[3] = 10;
        maxEffectCount[4] = 5;
        maxEffectCount[5] = 2;
        maxEffectCount[6] = 2;
        maxEffectCount[7] = 1;
        maxEffectCount[8] = 1;
        maxEffectCount[9] = 5;
        maxEffectCount[10] = 1;

        effectDuration[0] = 3f;
        effectDuration[1] = 1.5f;
        effectDuration[2] = 0.25f;
        effectDuration[3] = 5f;
        effectDuration[10] = 0.25f;
    }

    public void AddEffect(int effectId, int diff = 1, bool resetRemainingTime = true)
    {
        if ((effectId < 0) || (effectId > effectCount.Count))
        {
            Debug.LogWarning(gameObject.name + ": unable to add effect with id=" + effectId);
        }
        else
        {
            effectCount[effectId] += diff;
            if (effectCount[effectId] > maxEffectCount[effectId]) 
                effectCount[effectId] = maxEffectCount[effectId];
            if (resetRemainingTime) ResetEffectRemainingTime(effectId);
        }
    }

    public void AddEffect(EffectNames effect, int diff = 1, bool resetRemainingTime = true)
    {
        AddEffect((int)effect, diff, resetRemainingTime);
    }

    public void RemoveEffect(int effectId, int diff = 1, bool resetRemainingTime = true)
    {
        if ((effectId < 0) || (effectId > effectCount.Count))
        {
            Debug.LogWarning(gameObject.name + ": unable to remove effect with id=" + effectId);
        }
        else
        {
            effectCount[effectId] -= diff;
            if (effectCount[effectId] < 0) 
                effectCount[effectId] = 0;
            if (resetRemainingTime) ResetEffectRemainingTime(effectId);
        }
    }

    public void RemoveEffect(EffectNames effect, int diff = 1, bool resetRemainingTime = true)
    {
        RemoveEffect((int)effect, diff, resetRemainingTime);
    }

    public void SetEffect(int effectId, int count, bool resetRemainingTime = true)
    {
        if ((effectId < 0) || (effectId > effectCount.Count))
        {
            Debug.LogWarning(gameObject.name + ": unable to set effect with id=" + effectId);
        }
        else
        {
            effectCount[effectId] = (int) Mathf.Clamp(count, 0, maxEffectCount[effectId]);
            if ((count < 0) || (count > maxEffectCount[effectId]))
            {
                Debug.Log(gameObject.name + ": setted effect with id=" + effectId +
                    " count to " + effectCount[effectId] + " instead of " + count);
            }
            if (resetRemainingTime) ResetEffectRemainingTime(effectId);
        }
    }

    public void SetEffect(EffectNames effect, int count, bool resetRemainingTime = true)
    {
        SetEffect((int)effect, count, resetRemainingTime);
    }

    void ResetEffectRemainingTime(int effectId)
    {
        if (effectCount[effectId] > 0)
        {
            effectRemainingTime[effectId] = effectDuration[effectId];
        }
    }

    protected void UpdateEffectRemainingTime() //обновление оставшегося времени действия эффектов
    {
        for (int i = 0; i < EFFECT_COUNT; i++) //проверяем каждый тип эффекта
        {
            if (effectCount[i] > 0) //если он наложен
            {
                if (effectDuration[i] != Mathf.NegativeInfinity) //если эффект имеет время действия
                {
                    effectRemainingTime[i] -= Time.deltaTime;
                    if (effectRemainingTime[i] < 0.0f) //если эффект закончился
                    {
                        RemoveEffect(i, 1); //убираем 1 эффект
                    }
                }
            }
        }
    }

    public bool hasEffect(int effectId)
    {
        if ((effectId < 0) || (effectId >= EFFECT_COUNT)) return false; 
        return effectCount[effectId] > 0;
    }

    public bool hasEffect(EffectNames effect)
    {
        return hasEffect((int)effect);
    }

    public int getEffectCount(int effectId)
    {
        if ((effectId < 0) || (effectId >= EFFECT_COUNT)) return 0;
        return effectCount[effectId];
    }

    public int getEffectCount(EffectNames effect)
    {
        return getEffectCount((int)effect);
    }
}
