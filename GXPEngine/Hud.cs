using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Hud : GameObject
    {

        EasyDraw textUI;

        EasyDraw canvas;

        MyGame myGame;

        Sprite selection = new Sprite("selection.png");

        public Hud() : base() 
        {
            myGame = (MyGame)game;



            textUI = new EasyDraw(game.width, game.height / 2, false);

            canvas = new EasyDraw(game.width, game.height, false);
            if (myGame.levelIndex == 1)
            {
                AddChild(canvas);
            }
            else
            {
                AddChild(textUI);
            }
        }

        public void UpdateTextUI(String text)
        {
            textUI.ClearTransparent();
            textUI.Text(text);
        }

        public void AddTextUI(String text, int offset)
        {
            textUI.y += 20;
            textUI.Text(text,0,game.height/2 - 20 * offset);
        }

        public void UpdateGameUI(int health, int ammo, int wave, float waveTimer, int enemyCount)
        {
            canvas.ClearTransparent();
            canvas.TextAlign(CenterMode.Min, CenterMode.Min);
            canvas.Text("Health: " + health, 0, 0);
            canvas.Text("Ammo: " + ammo, 0, 24);
            canvas.Text("Wave: " + wave, 0, 48);
            canvas.Text("Wave Time: " + (int)waveTimer / 1000, 0, 72);
            canvas.Text("Enemies Left: " + enemyCount, 0, 96);
        }

        public void DrawInventoryItem(Sprite[] inventory, int selectedSlot) {
            Sprite[] sprites = inventory;
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null) continue;
                Sprite sprite = new Sprite(sprites[i].name);
                sprite.SetOrigin(sprite.width, 0);
                sprite.SetXY(game.width, i * sprite.height);
                if (selectedSlot == i)
                {
                    //canvas.Rect(sprite.x - sprite.width / 2, sprite.y + sprite.height / 2, sprite.width, sprite.height);
                    selection.SetXY(sprite.x - sprite.width,sprite.y);
                    canvas.DrawSprite(selection);
                }
                canvas.DrawSprite(sprite);
            }
        }
    }
}
