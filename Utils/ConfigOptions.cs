﻿using HyperResearch.Common.Configs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace HyperResearch.Utils
{
    public static class ConfigOptions
    {
        public static bool UseServerSettings { get => Main.netMode == NetmodeID.MultiplayerClient && ServerConfig.Instance.UseServerSettings; }

        public static bool IgnoreCraftingConditions
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.IgnoreCraftingConditions : HyperConfig.Instance.IgnoreCraftingConditions;
        }

        public static bool ResearchShimmerableItems
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.ResearchShimmerableItems : HyperConfig.Instance.ResearchShimmerableItems;
        }

        public static bool ResearchDecraftItems
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.ResearchDecraftItems : HyperConfig.Instance.ResearchDecraftItems;
        }

        public static bool BalanceShimmerAutoresearch
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.BalanceShimmerAutoresearch : HyperConfig.Instance.BalanceShimmerAutoresearch;
        }

        public static bool BalancePrefixPicker
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.BalancePrefixPicker : HyperConfig.Instance.BalancePrefixPicker;
        }

        public static bool UseResearchedBannersBuff
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.UseResearchedBannersBuff : HyperConfig.Instance.UseResearchedBannersBuff;
        }

        public static bool UseResearchedPotionsBuff
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.UseResearchedPotionsBuff : HyperConfig.Instance.UseResearchedPotionsBuff;
        }

        public static bool OnlyOneItemNeeded
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.OnlyOneItemNeeded : HyperConfig.Instance.OnlyOneItemNeeded;
        }

        public static Dictionary<ItemDefinition, uint> ItemResearchCountOverride
        {
            get =>
                UseServerSettings ? ServerConfig.Instance.ItemResearchCountOverride : HyperConfig.Instance.ItemResearchCountOverride;
        }
    }
}
