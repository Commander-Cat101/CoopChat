using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppNinjaKiwi.Common;
using Il2CppNinjaKiwi.LiNK.Transfer;
using Il2CppSystem.Linq;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace CoopChat.UI
{
    public static class UI
    {
        static ModHelperPanel panel;
        public static ModHelperScrollPanel messagescrollpanel;
        static ModHelperInputField inputfield;
        static ModHelperButton button;
        public static void CreatePanel(GameObject screen)
        {
            panel = screen.AddModHelperPanel(new Info("Panel", 0, 0, 3600, 2200), VanillaSprites.MainBGPanelBlue);
            messagescrollpanel = panel.AddScrollPanel(new Info("MessageScrollPanel", 0, 125, 3500, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, 50, 50);
            inputfield = panel.AddInputField(new Info("InputField", -350, -950, 2800, 200), string.Empty, VanillaSprites.BlueInsertPanelRound);
            button = panel.AddButton(new Info("SendButton", 1400, -950, 650, 200), VanillaSprites.YellowBtnLong, (Il2CppSystem.Action)SendMessage);
            button.AddText(new Info("SendText", 0, 10, 600, 150), "Send", 90, Il2CppTMPro.TextAlignmentOptions.Center);
            inputfield.Text.Text.fontSize = 70;


            var animator = panel.AddComponent<Animator>();
            animator.runtimeAnimatorController = Animations.PopupAnim;
            animator.speed = .55f;
        }

        private static void Init()
        {
            var screen = CommonForegroundScreen.instance.transform;
            var ButtonPanel = screen.FindChild("Panel");
            if (ButtonPanel == null)
            {
                CreatePanel(screen.gameObject);

                foreach (var message in CoopChat.ChatMessages)
                {
                    messagescrollpanel.AddScrollContent(ChatMessagePanel.Create(message));
                }
            }
        }


        private static void HideButton()
        {
            panel.GetComponent<Animator>().Play("PopupSlideOut");
            TaskScheduler.ScheduleTask(() => panel.SetActive(false), ScheduleType.WaitForFrames, 25);
        }

        public static void Show()
        {
            Init();
            panel.SetActive(true);
            panel.GetComponent<Animator>().Play("PopupSlideIn");
        }

        public static void Hide()
        {
            var screen = CommonForegroundScreen.instance.transform;
            var ButtonPanel = screen.FindChild("Panel");
            if (ButtonPanel != null)
                HideButton();
        }

        public static void SendMessage()
        {
            string message = inputfield.CurrentValue.Split("ΓÇï")[0];
            

            //Regex.Replace(message, @"[^\u0000-\u007F]+", string.Empty);
            if (message == string.Empty)
                return;
            CoopChat.SendMessageToPeers(message);
            MelonLogger.Msg("Sent message: " + message);
            inputfield.SetText("");
        }

        public static void AddMessage(ModHelperPanel message)
        {
            messagescrollpanel.AddScrollContent(message);

            messagescrollpanel.ScrollRect.verticalNormalizedPosition = -1.0f;
        }
    }
}
