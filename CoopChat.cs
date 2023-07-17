using MelonLoader;
using BTD_Mod_Helper;
using CoopChat;
using Il2CppNinjaKiwi.NKMulti;
using Il2CppNinjaKiwi.NKMulti.IO;
using HarmonyLib;
using UnityEngine;
using BTD_Mod_Helper.Api.Coop;
using Il2CppAssets.Scripts.Unity.Utils;
using Il2CppAssets.Scripts.Unity;
using CoopChat.UI;
using System.Collections.Generic;
using BTD_Mod_Helper.Api.Components;
using Il2CppNinjaKiwi.Common;

[assembly: MelonInfo(typeof(CoopChat.CoopChat), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace CoopChat;

public class CoopChat : BloonsTD6Mod
{
    private static NKMultiGameInterface? NkGI;
    public static List<ChatMessage> ChatMessages = new() { };

    public static void SendMessageToPeers(string message)
    {
        ChatMessage chatmessage = new ChatMessage(message, NkGI == null ? 0 : NkGI.PeerID, Game.Player.LiNKAccount.DisplayName);
        ChatMessages.Add(chatmessage);

        UI.UI.AddMessage(ChatMessagePanel.Create(chatmessage));

        if (NkGI != null)
        {
            for (int i = 1; i < 5; i++)
            {
                if (i != NkGI.PeerID)
                {
                    try
                    {
                        NkGI.SendToPeer(i, MessageUtils.CreateMessageEx<ChatMessage>(chatmessage, "Message_CoopChat"));
                        MelonLogger.Msg(chatmessage + " was sent to player " + i);
                    }
                    catch
                    {
                        MelonLogger.Warning("Failed to send message to a player");
                    }
                }
            }
        }
    }
    public override void OnApplicationStart()
    {
        ModHelper.Msg<CoopChat>("CoopChat loaded!");
    }

    public override void OnUpdate()
    {
        if (NkGI != null)
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                UI.UI.Show();
            }
            if (Input.GetKeyUp(KeyCode.F2))
            {
                UI.UI.Hide();

            }
        }
    }
    public override bool ActOnMessage(Message message)
    {
        switch (message.Code)
        {
            case "Message_CoopChat":
                ChatMessage receivedmessage = MessageUtils.ReadMessage<ChatMessage>(message);

                ChatMessages.Add(receivedmessage);

                MelonLogger.Msg(receivedmessage.MessageText + " from " + receivedmessage.SenderName);

                UI.UI.AddMessage(ChatMessagePanel.Create(receivedmessage));
                return true;
            default:
                return false;
        }
    }

    [HarmonyPatch(typeof(NKMultiGameInterface), nameof(NKMultiGameInterface.Connect))]
    private static class NKMultiGameInterface_Connect
    {
        [HarmonyPostfix]
        private static void Postfix(NKMultiGameInterface __instance)
        {
            NkGI = __instance;
        }
    }
    [HarmonyPatch(typeof(NKMultiGameInterface), nameof(NKMultiGameInterface.Disconnect))]
    private static class NKMultiGameInterface_Disconnect
    {
        [HarmonyPostfix]
        private static void Postfix(NKMultiGameInterface __instance)
        {
            NkGI = null;
            ChatMessages.Clear();
            UI.UI.messagescrollpanel.transform.DestroyAllChildren();
        }
    }
}

