using Agoraphobia.Character;

namespace Agoraphobia.Entity
{
    interface INPC : IEntity, IArtist
    {
        static int[] Coordinates = { 10, 4 };
        static List<INPC> NPCs = new List<INPC>(); //Add the instance to this list in the constructor
        void Interact();
        public string Intro { get; }
    }
}
