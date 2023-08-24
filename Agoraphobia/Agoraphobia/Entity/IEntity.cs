namespace Agoraphobia.Character
{
    internal interface IEntity : IElement
    {
        List<int> Inventory { get; } // Inventory size is capped, Inventory contains the Id of the items.
    }
}
