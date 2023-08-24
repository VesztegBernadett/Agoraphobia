namespace Agoraphobia.Entity
{
    internal class NPC : INPC
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public string Art { get; private set; }
        public string Description { get => description; }
        public List<int> Inventory { get; set; }
        public string Intro { get; private set; }
        public void Interact()
        {
            Viewport.Shop(id);
        }
        public NPC(int id, string name, string desc, List<int> items, string intro)
        {
            this.id = id;
            this.name = name;
            description = desc;
            INPC.NPCs.Add(this);
            Inventory = items;
            Intro = intro;
            Art = File.ReadAllText($"{Program.PATH}/Arts/NArt{id}.txt");
        }
    }
}
