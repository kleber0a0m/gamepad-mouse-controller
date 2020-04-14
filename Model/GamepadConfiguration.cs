﻿using System.Collections.Generic;
using System.IO;
using gamepad_mouse_controller.Actions;
using Newtonsoft.Json;

namespace gamepad_mouse_controller.Model
{
    class GamepadConfiguration
    {
        private const string FILE_EXTENTION = ".gpconf";
        private readonly string name;
        private Dictionary<int, string> config;

        public IAction[] action;

        public GamepadConfiguration(string name, int length)
        {
            this.name = name;
            var actionMap = ActionMap.GetActionMap();

            try
            {
                Load();
            }
            catch(FileNotFoundException)
            {
                config = new Dictionary<int, string>()
                {
                    { 0, "MouseLeftClickAction" },
                    { 1, "MouseRightClickAction" },
                    { 4, "BrowserBackAction" },
                    { 5, "BrowserForwardAction" },
                    { 6, "ShowSettingsAction" },
                    { 7, "WindowsKeyAction" },
                    { length++, "MouseMoveAction" },
                    { length++, "MouseScrollAction" }
                };
                //Save();
            }

            action = new IAction[12];
            for(int i = 0; i < action.Length; i++)
            {
                if(config.ContainsKey(i))
                {
                    action[i] = actionMap[config[i]];
                }
            }
        }

        public void Save()
        {
            StreamWriter file = new StreamWriter(name + FILE_EXTENTION);
            file.Write(JsonConvert.SerializeObject(config));
            file.Flush();
            file.Close();
        }

        public void Load()
        {
            StreamReader file = new StreamReader(name + FILE_EXTENTION);
            config = JsonConvert.DeserializeObject<Dictionary<int, string>>(file.ReadToEnd());
            file.Close();
        }
    }
}
