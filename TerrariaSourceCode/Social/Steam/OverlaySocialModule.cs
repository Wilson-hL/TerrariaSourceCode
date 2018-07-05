// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Steam.OverlaySocialModule
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Steamworks;

namespace Terraria.Social.Steam
{
    public class OverlaySocialModule : Terraria.Social.Base.OverlaySocialModule
    {
        private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissed;
        private bool _gamepadTextInputActive;

        public override void Initialize()
        {
            this._gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(
                new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(OnGamepadTextInputDismissed));
        }

        public override void Shutdown()
        {
        }

        public override bool IsGamepadTextInputActive()
        {
            return this._gamepadTextInputActive;
        }

        public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false,
            string existingText = "", bool password = false)
        {
            if (this._gamepadTextInputActive)
                return false;
            var flag = SteamUtils.ShowGamepadTextInput(
                password ? (EGamepadTextInputMode) 1 : (EGamepadTextInputMode) 0,
                multiLine ? (EGamepadTextInputLineMode) 1 : (EGamepadTextInputLineMode) 0, description, maxLength,
                existingText);
            if (flag)
                this._gamepadTextInputActive = true;
            return flag;
        }

        public override string GetGamepadText()
        {
            var gamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
            string str;
            SteamUtils.GetEnteredGamepadTextInput(out str, gamepadTextLength);
            return str;
        }

        private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t result)
        {
            this._gamepadTextInputActive = false;
        }
    }
}