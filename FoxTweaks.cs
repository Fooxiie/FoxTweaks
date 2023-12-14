using System.Diagnostics;
using System.IO;
using FoxTweaks.config;
using FoxTweaks.TweakClass;
using InsaneSystems.RoadNavigator;
using Life;
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Path = System.IO.Path;

namespace FoxTweaks
{
    public class FoxTweaks : Plugin
    {
        private bool _mecanoTweak;
        private bool _citizenTweak;
        private GiveMeTheKeyToo _giveMeTheKeyToo;
        private SellEnterpriseCarFox _sellEnterpriseCarFox;

        public FoxTweaks(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();

            Debug.Log("Init FoxTweaks");

            var configFilePath = Path.Combine(pluginsPath, "FoxTweaks/config.json");
            var globalConfiguration = ChargerConfiguration(configFilePath);

            _mecanoTweak = globalConfiguration.activeMecanoTweak == 1;
            _citizenTweak = globalConfiguration.activeCitoyenTweak == 1;

            if (!_mecanoTweak) return;
            Debug.Log("TweakMecano : Enable");
            _giveMeTheKeyToo = new GiveMeTheKeyToo();

            if (!_citizenTweak) return;
            Debug.Log("TweakCitizen : Enable");
            _sellEnterpriseCarFox = new SellEnterpriseCarFox();
        }

        private static MainConfig ChargerConfiguration(string configFilePath)
        {
            var jsonConfig = File.ReadAllText(configFilePath);
            return JsonConvert.DeserializeObject<MainConfig>(jsonConfig);
        }
    }
}