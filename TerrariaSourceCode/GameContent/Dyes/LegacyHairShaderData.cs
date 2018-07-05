// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Dyes.LegacyHairShaderData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
    public class LegacyHairShaderData : HairShaderData
    {
        public delegate Color ColorProcessingMethod(Player player, Color color, ref bool lighting);

        private ColorProcessingMethod _colorProcessor;

        public LegacyHairShaderData()
            : base(null, null)
        {
            _shaderDisabled = true;
        }

        public override Color GetColor(Player player, Color lightColor)
        {
            var lighting = true;
            var color = _colorProcessor(player, player.hairColor, ref lighting);
            if (lighting)
                return new Color(color.ToVector4() * lightColor.ToVector4());
            return color;
        }

        public LegacyHairShaderData UseLegacyMethod(ColorProcessingMethod colorProcessor)
        {
            _colorProcessor = colorProcessor;
            return this;
        }
    }
}