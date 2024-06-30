using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Inventory : GameObject
    {
        Hud hud;

        int selectedSlot = -1;

        List<Gun> inventory;
        Gun[] inventoryArray;
        Gun pistol = new Gun("pistol.png", 2, false, .1f, 25);
        Gun rifle = new Gun("rifle.png", 1, true, .1f, 25);
        Gun sniper = new Gun("sniper.png", 5, false, .75f, 5);
        Gun shotgun = new Gun("shotgun.png", 3, false, .5f, 6, 3, 15);

        public Inventory()
        {
            hud = game.FindObjectOfType<Hud>();
            inventory = new List<Gun>();
            inventory.Add(pistol);
            inventory.Add(rifle);
            inventory.Add(sniper);
            inventoryArray = inventory.ToArray();

            AddItem(shotgun);
        }

        void Update()
        {
            if (hud == null)
            {
                hud = game.FindObjectOfType<Hud>();
            }

            if (Input.GetKeyDown(Key.ONE))
                selectSlot(0);
            if (Input.GetKeyDown(Key.TWO))
                selectSlot(1);
            if (Input.GetKeyDown(Key.THREE))
                selectSlot(2);
            if (Input.GetKeyDown(Key.FOUR))
                selectSlot(3);
            if (Input.GetKeyDown(Key.FIVE))
                selectSlot(4);

            hud.DrawInventoryItem(inventoryArray, selectedSlot);
        }

        void selectSlot(int selection)
        {
            if (selection < inventoryArray.Length)
            {
                selectedSlot = selection;
                //remove old item
                if (game.FindObjectOfType<Player>().FindObjectOfType<Gun>() != null)
                    game.FindObjectOfType<Player>().FindObjectOfType<Gun>().Remove();
                //add new item
                game.FindObjectOfType<Player>().AddChild(inventoryArray[selection]);
            }
        }

        void AddItem(Gun item)
        {
            inventory.Add(shotgun);

            inventoryArray = inventory.ToArray();
        }
    }
}
