﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HyperResearch.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace HyperResearch.Common.Systems;

[Autoload(Side = ModSide.Client)]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
internal class UISystem : ModSystem
{
    public static Asset<Texture2D>? ResearchButtonTexture;
    public static Asset<Texture2D>? ClearButtonTexture;
    public static Asset<Texture2D>? AutoCraftButtonTexture;
    public static Asset<Texture2D>? ResearchShopButtonTexture;
    private UserInterface? _duplicationMenu;
    private UserInterface? _inventoryButtons;
    private UserInterface? _prefixWindow;
    private UserInterface? _shopButtons;

    internal DuplicationMenu? DuplicationMenu;
    internal InventoryButtons? InventoryButtons;
    internal PrefixWindow? PrefixWindow;
    internal ShopButtons? ShopButtons;

    internal static event Action? WorldLoaded;
    internal static event Action? WorldUnloaded;

    public override void Load()
    {
        if (Main.dedServ) return;

        ResearchButtonTexture = Mod.Assets.Request<Texture2D>("Assets/Images/UI/ResearchButton");
        ClearButtonTexture = Mod.Assets.Request<Texture2D>("Assets/Images/UI/ClearButton");
        AutoCraftButtonTexture = Mod.Assets.Request<Texture2D>("Assets/Images/UI/AutoCraftButton");
        ResearchShopButtonTexture = Mod.Assets.Request<Texture2D>("Assets/Images/UI/ResearchShopButton");

        DuplicationMenu = new DuplicationMenu();
        DuplicationMenu.Activate();
        _duplicationMenu = new UserInterface();
        _duplicationMenu.SetState(DuplicationMenu);

        InventoryButtons = new InventoryButtons();
        InventoryButtons.Activate();
        _inventoryButtons = new UserInterface();
        _inventoryButtons.SetState(InventoryButtons);

        ShopButtons = new ShopButtons();
        ShopButtons.Activate();
        _shopButtons = new UserInterface();
        _shopButtons.SetState(ShopButtons);

        PrefixWindow = new PrefixWindow();
        PrefixWindow.Activate();
        _prefixWindow = new UserInterface();
        _prefixWindow.SetState(PrefixWindow);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _duplicationMenu?.Update(gameTime);
        _inventoryButtons?.Update(gameTime);
        _shopButtons?.Update(gameTime);
        _prefixWindow?.Update(gameTime);
    }

    public override void OnWorldLoad() => WorldLoaded?.Invoke();

    public override void OnWorldUnload() => WorldUnloaded?.Invoke();

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex == -1) return;

        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
            "HyperResearch: Prefix Window",
            delegate
            {
                if (PrefixWindow is { Enabled: true })
                    _prefixWindow!.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI)
        );
        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
            "HyperResearch: Duplication Menu Text",
            delegate
            {
                _duplicationMenu!.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI)
        );
        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
            "HyperResearch: Inventory Buttons",
            delegate
            {
                _inventoryButtons!.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI)
        );
        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
            "HyperResearch: Shop Buttons",
            delegate
            {
                _shopButtons!.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI)
        );
    }

    public override void Unload()
    {
        ResearchButtonTexture = null;
        ClearButtonTexture = null;
        AutoCraftButtonTexture = null;
        ResearchShopButtonTexture = null;
    }
}