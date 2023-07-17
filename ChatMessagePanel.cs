using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopChat
{
    public static class ChatMessagePanel
    {
        public static ModHelperPanel Create(ChatMessage message)
        {
            var modpanel = ModHelperPanel.Create(new Info("Message", 0, 0, 3400, 300), message.SenderName == Game.Player.LiNKAccount.DisplayName ? VanillaSprites.MainBgPanelHematite : VanillaSprites.MainBGPanelYellow);
            modpanel.AddText(new Info("MessageSenderText", 0, 75, 3300, 150), string.Format("{0} - Player {1}", message.SenderName, message.SenderID), 100, Il2CppTMPro.TextAlignmentOptions.Left);
            modpanel.AddText(new Info("MessageText", 0, -75, 3300, 150), message.MessageText, 60, Il2CppTMPro.TextAlignmentOptions.Left);
            return modpanel;
        }
    }
}
