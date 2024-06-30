using System;
using GXPEngine;
using TiledMapParser;


    internal class Level : GameObject
    {
        Player player;

        public Level(Player newPlayer, string mapName)
        {
            player = newPlayer;
            TiledLoader loader = new TiledLoader(mapName);
            loader.autoInstance = true;
            loader.rootObject = this;
            loader.addColliders = false;
            loader.LoadTileLayers(0);
            loader.addColliders = true;
            loader.LoadTileLayers(1);
            loader.LoadObjectGroups();
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            //makes sure the player is in the middle of the screen
            x = -player.x + game.width / 2;
            y = -player.y + game.height / 2;
        }
    }
