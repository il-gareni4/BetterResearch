﻿using HyperResearch.Common.ModPlayers.Interfaces;
using HyperResearch.Common.Systems;
using HyperResearch.Utils;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace HyperResearch.Common.ModPlayers;

public class BuffPlayer : ModPlayer, IResearchPlayer
{
    public DictionaryAnalysisData<int, bool> Buffs { get; private set; } = [];

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!Researcher.IsPlayerInJourneyMode) return;

        if (Main.HoverItem.tooltipContext == ItemSlot.Context.CreativeInfinite
            && BuffUtils.IsBuffPotion(Main.HoverItem)
            && Buffs.TryGetValue(Main.HoverItem.buffType, out bool enabled)
            && KeybindSystem.EnableDisableBuffBind.JustPressed)
        {
            Buffs[Main.HoverItem.buffType] = !enabled;
        }
        if (KeybindSystem.ForgetAllBind.JustPressed)
            Buffs.Clear();
    }

    public override void OnEnterWorld()
    {
        if (!Researcher.IsPlayerInJourneyMode) return;

        for (int itemId = 1; itemId < ItemLoader.ItemCount; itemId++)
        {
            Item item = ContentSamples.ItemsByType[itemId];
            if (Researcher.IsResearched(item.type)) ResearchItem(item, false);
        }
    }

    public override void SaveData(TagCompound tag)
    {
        if (Main.CurrentPlayer.difficulty != 3) return;
        tag["buffsEnabled"] = Buffs.Where(kv => kv.Value).Select(kv => kv.Key).ToArray();
    }

    public override void Unload()
    {
        if (Main.CurrentPlayer.difficulty != 3) return;
        Buffs.Clear();
    }

    public override void LoadData(TagCompound tag)
    {
        if (Main.CurrentPlayer.difficulty != 3) return;

        if (tag.TryGet("buffsEnabled", out int[] enabled))
            foreach (int buffId in enabled)
                Buffs[buffId] = true;
    }

    public override void PostUpdateBuffs()
    {
        if (!Researcher.IsPlayerInJourneyMode || !ConfigOptions.UseResearchedPotionsBuff) return;

        foreach ((int buffId, bool enabled) in Buffs)
            if (enabled) Player.AddBuff(buffId, 1);
    }

    public void OnResearch(Item item) => ResearchItem(item);

    public void ResearchItem(Item item, bool enabledIfNotFound = true)
    {
        if (item.buffType == 0 || Buffs.ContainsKey(item.buffType)) return;

        Buffs[item.buffType] = BuffUtils.IsBuffPotion(item) && enabledIfNotFound;
    }
}
