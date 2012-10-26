using System;
using System.Collections.Generic;
using System.Serialization;
using System.Text;
using CommonLibraries;
using CommonShuffleLibrary;
using CommonWebLibraries;
using NodeJSLibrary;

namespace GameServer
{
    public class GameServer
    {
        private int QUEUEPERTICK = 1;
        private JsDictionary<string, GameObject> cachedGames;
        private ChildProcess childProcess;
        private DataManager dataManager;
        private FS fs;
        private GameData gameData;
        private string gameServerIndex;
        private QueueManager qManager;    
        private List<GameRoom> rooms;
        private int skipped__;
        private DateTime startTime = new DateTime();
        private int total__;
        private bool verbose = false;

        public GameServer()
        { 
            fs = Global.Require<FS>("fs");
            childProcess = Global.Require<ChildProcess>("child_process");

             

            dataManager = new DataManager();

            gameServerIndex = "GameServer" + Guid.NewGuid();
            cachedGames = new JsDictionary<string, GameObject>();
            Global.Require<NodeModule>("fibers");
            rooms = new List<GameRoom>();
            gameData = new GameData();
            Global.Process.On("exit", () => Console.Log("exi"));
            /*qManager.AddChannel("Area.Game.Create", (arg1, arg2) =>
                {
                    GameRoom room;
                    rooms.Add(room = new GameRoom());
                });*/

            qManager = new QueueManager(gameServerIndex, new QueueManagerOptions(new[]
                {
                    new QueueWatcher("GameServer", null),
                    new QueueWatcher(gameServerIndex, null),
                }, new[]
                    {
                        "GameServer",
                        "GatewayServer",
                        "Gateway*"
                    }));

            qManager.AddChannel<SomeModel>("SomeNamespaceOrEnum??",
                                                        (user, data) =>
                                                        {
                                                            EmitAll(room, "Area.Game.UpdateState", new Compressor().CompressText(Json.Stringify(room.Game.CardGame.CleanUp())));

                                                            
            var user = getPlayerByUsername(room, answ.User.UserName);
                                                            qManager.SendMessage(user, user.Gateway, "Area.Game.AskQuestion", gameAnswer.CleanUp());
                                                        });


 
             
        }
         

 

        public int[] Range(int start, int length)
        {
            int[] mf = new int[length];
            for (int i = start; i < length; i++)
            {
                mf[i - start] = i;
            }
            return mf;
        }

 
        private UserModel getPlayerByUsername(GameRoom room, string userName)
        {
            foreach (var player in room.Players)
            {
                if (player.UserName == userName)
                {
                    return player;
                }
            }
            return null;
        }


        private void EmitAll(GameRoom room, string message, object val)
        {
            foreach (var player in room.Players)
            {
                qManager.SendMessage(player, player.Gateway, message, val);
            }
        }

     }
}
