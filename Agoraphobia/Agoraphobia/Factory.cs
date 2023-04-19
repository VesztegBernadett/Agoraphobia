using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agoraphobia.Items;
using System.Collections;

namespace Agoraphobia
{
    internal class Factory
    {
        public static IElement Create(string filename)
        {
            int id = 0;
            string name = "";
            string desc = "";

            foreach (var line in File.ReadLines(filename, Encoding.UTF8))
            {
                string[] data = line.Split('#');
                switch (data[1])
                {
                    case "Id":
                        id = int.Parse(data[0]);
                        break;
                    case "Name":
                        name = data[0];
                        break;
                    case "Description":
                        desc = data[0];
                        break;
                }
            }
            switch (filename.Substring(0, 3))
            {
                case "Arm":
                    int def = 0;
                    int attack = 0;
                    int rarity = 0;
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Defense":
                                def = int.Parse(data[0]);
                                break;
                            case "Attack":
                                attack = int.Parse(data[0]);
                                break;
                            case "Rarity":
                                rarity = int.Parse(data[0]);
                                break;
                        }
                    }
                    return new Armor(id, name, desc, def, attack, rarity);
                    
            }
        }
    }
}
