﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using ResoniteModLoader;

namespace ComponentSelectorSearch
{
    public partial class ComponentSelectorSearch : ResoniteMod
    {
        internal static ModConfiguration Config;

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> AlwaysShowFullPath = new("AlwaysShowFullPath", "Whether to always show the full category path on component results, rather than only on hover.", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<HashSet<string>> ExcludedCategoriesKey = new("ExcludedCategories", "Exclude specific categories. Discarded while loading.", internalAccessOnly: true);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> SearchRefreshDelay = new("SearchRefreshDelay", "Time in ms to wait after search input change before refreshing the results. 0 to always refresh.", () => 0, valueValidator: value => value >= 0);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<string> UserExcludedCategories = new("UserExcludedCategories", "Exclude specific categories by path (case sensitive). Separate entries by semicolon. Search will work inside them anyways.", () => "/ProtoFlux; /ProtoFlux/FrooxEngine/ProtoFlux/CoreNodes; /ProtoFlux/Runtimes/DSP");

        private static readonly char[] UserExclusionSeparator = new[] { ';' };
        private static string lastUserExcludedCategories = "";
        public override string Author => "Banane9/Delta/sctanf";
        public override string Link => "https://github.com/sctanf/ResoniteComponentSelectorSearch";
        public override string Name => "ComponentSelectorSearch";
        public override string Version => "1.2.1";
        private static HashSet<string> ExcludedCategories => Config.GetValue(ExcludedCategoriesKey);

        public override void OnEngineInit()
        {
            var harmony = new Harmony($"{Author}.{Name}");

            Config = GetConfiguration();

            Config.Set(ExcludedCategoriesKey, new HashSet<string>());
            updateExcludedCategories();

            Config.OnThisConfigurationChanged += Config_OnThisConfigurationChanged;
            Config.Save(true);

            harmony.PatchAll();
        }

        private void Config_OnThisConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
            if (configurationChangedEvent.Key == UserExcludedCategories)
                updateExcludedCategories();
        }

        private void updateExcludedCategories()
        {
            var previousValues = lastUserExcludedCategories.Split(UserExclusionSeparator, StringSplitOptions.RemoveEmptyEntries);
            var newValues = Config.GetValue(UserExcludedCategories).Split(UserExclusionSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in previousValues)
                ExcludedCategories.Remove(value.Trim());

            foreach (var value in newValues)
                ExcludedCategories.Add(value.Trim());

            lastUserExcludedCategories = Config.GetValue(UserExcludedCategories);
        }
    }
}