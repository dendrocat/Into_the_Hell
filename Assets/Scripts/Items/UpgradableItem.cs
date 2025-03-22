using UnityEngine;

//Класс, описывающий улучшаемый предмет
public class UpgradableItem : MonoBehaviour
{
    protected float scaleCoeff = 0.2f; //Коэффициент масштабирования.
                             //Задается в классах-наследниках в методе Start
    public byte level; //Текущий уровень предмета
    public byte maxLevel; //Максимальный уровень предмета

    public int GetUpgradeCost() //Возвращает стоимость улучшения на следующий уровень
    {
        if (level < maxLevel) return 100 * level;
        else return -1; //возвращаем -1 как знак о том, что предмет максимально улучшен
    }
    
    public float CalcScale(float scalable) //Возвращает значение scalable, умноженное согласно
                                           //текущему уровню предмета
    {
        return scalable * (1f + (level - 1) * scaleCoeff);
    }

    public float CalcScaleDescending(float scalable) //Возвращает значение scalable, уменьшенное
                                                     //согласно текущему уровню предмета
    {
        float coeff = 0.5f / (maxLevel - 1);
        return scalable * (1f - (level - 1) * coeff);
    }

    public void Upgrade(byte diff) //Функция, отвечающая за апгрейд предмета
    {
        level = (byte)Mathf.Clamp(level + diff, 1, maxLevel);
    }
}
