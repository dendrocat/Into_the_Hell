using UnityEngine;

/// <summary>
/// Абстрактный базовый класс панели магазина, наследующий <see cref="BasePanel"/>.
/// Управляет отображением и состоянием предметов через <see cref="ItemsPanel"/>.
/// </summary>
public abstract class ShopPanel : BasePanel
{
    [Tooltip("Панель с предметами магазина")]
    [SerializeField] ItemsPanel items;

    /// <summary>
    /// Абстрактный метод инициализации панели магазина.
    /// Наследники должны реализовать логику инициализации.
    /// </summary>
    protected abstract void InitPanel();

    void Start()
    {
        InitPanel();
    }

    /// <summary>
    /// Устанавливает количество предмета в панели.
    /// </summary>
    /// <param name="item">Имя предмета.</param>
    /// <param name="count">Количество.</param>
    protected void SetItemCount(string item, int count)
    {
        items.SetItemCount(item, count);
    }

    /// <summary>
    /// Устанавливает стоимость предмета или изменяет его состояние в панели.
    /// </summary>
    /// <param name="item">Имя предмета.</param>
    /// <param name="cost">
    /// Стоимость предмета.
    /// <list type="bullet">
    /// <item><description>-2 - деактивировать предмет (сделать кнопку неактивной).</description></item>
    /// <item><description>-1 - удалить предмет из панели.</description></item>
    /// <item><description>Любое другое значение - установить стоимость.</description></item>
    /// </list>
    /// </param>
    protected void SetItemCost(string item, int cost)
    {
        if (cost == -1)
        {
            items.RemoveItem(item);
            return;
        }
        if (cost == -2)
        {
            items.DeactivateItem(item);
        }
        items.SetItemCost(item, cost);
    }

    /// <summary>
    /// Проверяет, может ли игрок купить предмет с заданной стоимостью.
    /// </summary>
    /// <param name="cost">Стоимость предмета.</param>
    /// <param name="name">Имя предмета (необязательно).</param>
    /// <returns><see langword="true">, если у игрока достаточно денег, иначе <see langword="false"/>.</returns>
    protected virtual bool CheckBuy(int cost, string name = "")
    {
        return _inventory.GetMoney() >= cost;
    }

    /// <summary>
    /// Вычисляет и обновляет состояние доступности предметов в панели на основе проверки <see cref="CheckBuy"/>.
    /// </summary>
    protected void CalcActiveItems()
    {
        items.CalcItemsState(CheckBuy);
    }
}
