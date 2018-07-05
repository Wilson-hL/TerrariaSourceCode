// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Dyes.ReflectiveArmorShaderData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
    public class ReflectiveArmorShaderData : ArmorShaderData
    {
        public ReflectiveArmorShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            if (entity == null)
            {
                Shader.Parameters["uLightSource"].SetValue(Vector3.Zero);
            }
            else
            {
                var num1 = 0.0f;
                if (drawData.HasValue)
                    num1 = drawData.Value.rotation;
                var position = entity.position;
                var width = entity.width;
                var height = (float) entity.height;
                var vector2 = position + new Vector2(width, height) * 0.1f;
                var x = width * 0.8f;
                var y = height * 0.8f;
                var subLight1 = Lighting.GetSubLight(vector2 + new Vector2(x * 0.5f, 0.0f));
                var subLight2 = Lighting.GetSubLight(vector2 + new Vector2(0.0f, y * 0.5f));
                var subLight3 = Lighting.GetSubLight(vector2 + new Vector2(x, y * 0.5f));
                var subLight4 = Lighting.GetSubLight(vector2 + new Vector2(x * 0.5f, y));
                var num2 = subLight1.X + subLight1.Y + subLight1.Z;
                var num3 = subLight2.X + subLight2.Y + subLight2.Z;
                var num4 = subLight3.X + subLight3.Y + subLight3.Z;
                var num5 = subLight4.X + subLight4.Y + subLight4.Z;
                var spinningpoint = new Vector2(num4 - num3, num5 - num2);
                if (spinningpoint.Length() > 1.0)
                {
                    var num6 = 1f;
                    spinningpoint /= num6;
                }

                if (entity.direction == -1)
                    spinningpoint.X *= -1f;
                spinningpoint = spinningpoint.RotatedBy(-(double) num1, new Vector2());
                var vector3 = new Vector3(spinningpoint,
                    (float) (1.0 - (spinningpoint.X * (double) spinningpoint.X +
                                    spinningpoint.Y * (double) spinningpoint.Y)));
                vector3.X *= 2f;
                vector3.Y -= 0.15f;
                vector3.Y *= 2f;
                vector3.Normalize();
                vector3.Z *= 0.6f;
                Shader.Parameters["uLightSource"].SetValue(vector3);
            }

            base.Apply(entity, drawData);
        }
    }
}