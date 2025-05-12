using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс сущностей, на которые можно наложить эффект
/// </summary>
public class Effectable : MonoBehaviour
{
    /// <summary>Общее количество типов эффектов</summary>
    private const int EFFECT_COUNT = 12;

    /// <summary>Количество эффектов на сущности</summary>
    protected List<int> effectCount = new List<int>();
    
    /// <summary>Оставшееся время действия эффекта</summary>
    protected List<float> effectRemainingTime = new List<float>(); 

    /// <summary>Длительность эффекта (-inf для постоянных эффектов)</summary>
    private static List<float> effectDuration = new List<float>(); 

    /// <summary>Максимальное количество эффектов</summary>
    private static List<int> maxEffectCount = new List<int>();

    void Awake()
    {
        InitializeEffects();
    }

    /// <summary>
    /// Метод, инициализалирующий эффекты.
    /// </summary>
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
        maxEffectCount[4] = 15;
        maxEffectCount[5] = 2;
        maxEffectCount[6] = 2;
        maxEffectCount[7] = 1;
        maxEffectCount[8] = 5; //вызвано особенностями наложения эффекта в ловушке
        maxEffectCount[9] = 5;
        maxEffectCount[10] = 1;
        maxEffectCount[11] = 1;

        effectDuration[0] = 3f;
        effectDuration[1] = 1.5f;
        effectDuration[2] = 0.25f;
        effectDuration[3] = 5f;
        effectDuration[10] = 0.25f;
        effectDuration[11] = 1f;
    }


    /// <summary>
    /// Добавляет эффект с заданным id к сущности
    /// </summary>
    /// <param name="effectId">ID эффекта</param>
    /// <param name="diff">Количество добавляемых эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>

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


    /// <summary>
    /// Добавляет эффект с заданным <see cref="EffectNames">названием</see> к сущности
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <param name="diff">Количество удаляемых эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>
    public void AddEffect(EffectNames effect, int diff = 1, bool resetRemainingTime = true)
    {
        AddEffect((int)effect, diff, resetRemainingTime);
    }

    
    /// <summary>
    /// Удаляет эффект с заданным id у сущности
    /// </summary>
    /// <param name="effectId">ID эффекта</param>
    /// <param name="diff">Количество удаляемых эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>
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


    /// <summary>
    /// Удаляет эффект с заданным <see cref="EffectNames">названием</see> у сущности
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <param name="diff">Количество удаляемых эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>
    public void RemoveEffect(EffectNames effect, int diff = 1, bool resetRemainingTime = true)
    {
        RemoveEffect((int)effect, diff, resetRemainingTime);
    }


    /// <summary>
    /// Устанавливает уровень эффекта с заданным id у сущности
    /// </summary>
    /// <param name="effectId">ID эффекта</param>
    /// <param name="count">Количество эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>
    public void SetEffect(int effectId, int count, bool resetRemainingTime = true)
    {
        if ((effectId < 0) || (effectId > effectCount.Count))
        {
            Debug.LogWarning(gameObject.name + ": unable to set effect with id=" + effectId);
        }
        else
        {
            effectCount[effectId] = (int)Mathf.Clamp(count, 0, maxEffectCount[effectId]);
            if ((count < 0) || (count > maxEffectCount[effectId]))
            {
                Debug.Log(gameObject.name + ": setted effect with id=" + effectId +
                    " count to " + effectCount[effectId] + " instead of " + count);
            }
            if (resetRemainingTime) ResetEffectRemainingTime(effectId);
        }
    }


    /// <summary>
    /// Устанавливает уровень эффекта с заданным <see cref="EffectNames">названием</see> у сущности
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <param name="count">Количество эффектов</param>
    /// <param name="resetRemainingTime">Сбросить время действия существующего эффекта?</param>
    public void SetEffect(EffectNames effect, int count, bool resetRemainingTime = true)
    {
        SetEffect((int)effect, count, resetRemainingTime);
    }


    /// <summary>
    /// Сбрасывает время действия эффекта с заданным id
    /// </summary>
    /// <param name="effectId">ID эффекта</param>
    void ResetEffectRemainingTime(int effectId)
    {
        if (effectCount[effectId] > 0)
        {
            effectRemainingTime[effectId] = effectDuration[effectId];
        }
    }

    /// <summary>
    /// Обновление оставшегося времени действия эффектов. Вызывается каждый кадр в функции Update
    /// </summary>
    protected void UpdateEffectRemainingTime()
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

    /// <summary>
    /// Определяет, есть ли эффект с заданным ID на сущности.
    /// </summary>
    /// <returns>bool - имеет ли сущность данный эффект.</returns>
    public bool hasEffect(int effectId)
    {
        if ((effectId < 0) || (effectId >= EFFECT_COUNT)) return false;
        return effectCount[effectId] > 0;
    }

    /// <summary>
    /// Определяет, есть ли эффект с заданным <see cref="EffectNames">названием</see> на сущности.
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <returns>bool - имеет ли сущность данный эффект.</returns>
    public bool hasEffect(EffectNames effect)
    {
        return hasEffect((int)effect);
    }


    /// <summary>
    /// Возвращает количество эффектов с заданным ID на сущности.
    /// </summary>
    /// <returns>int - количество искомых эффектов на сущности.</returns>
    public int getEffectCount(int effectId)
    {
        if ((effectId < 0) || (effectId >= EFFECT_COUNT)) return 0;
        return effectCount[effectId];
    }


    /// <summary>
    /// Возвращает количество эффектов с заданным <see cref="EffectNames">названием</see> на сущности.
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <returns>int - количество искомых эффектов на сущности.</returns>
    public int getEffectCount(EffectNames effect)
    {
        return getEffectCount((int)effect);
    }

    /// <summary>
    /// Возвращает длительность эффекта с заданным ID.
    /// </summary>
    /// <returns>float - длительность эффекта.</returns>
    public float getEffectDuration(int effectId)
    {
        if ((effectId < 0) || (effectId >= EFFECT_COUNT)) return 0;
        return effectDuration[effectId];
    }

    /// <summary>
    /// Возвращает длительность эффекта с заданным <see cref="EffectNames">названием</see>.
    /// </summary>
    /// <param name="effect"><see cref="EffectNames">Название</see> эффекта</param>
    /// <returns>float - длительность эффекта.</returns>
    public float getEffectDuration(EffectNames effect)
    {
        return getEffectDuration((int)effect);
    }
}
