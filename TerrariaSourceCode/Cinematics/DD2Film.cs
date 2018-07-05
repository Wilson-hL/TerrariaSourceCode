// Decompiled with JetBrains decompiler
// Type: Terraria.Cinematics.DD2Film
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace Terraria.Cinematics
{
    public class DD2Film : Film
    {
        private readonly List<NPC> _army = new List<NPC>();
        private readonly List<NPC> _critters = new List<NPC>();
        private NPC _dryad;
        private NPC _ogre;
        private NPC _portal;
        private Vector2 _startPoint;

        public DD2Film()
        {
            AppendKeyFrames(CreateDryad, CreateCritters);
            AppendSequences(120, DryadStand, DryadLookRight);
            AppendSequences(100, DryadLookRight, DryadInteract);
            AddKeyFrame(AppendPoint - 20, CreatePortal);
            AppendSequences(30, DryadLookLeft, DryadStand);
            AppendSequences(40, DryadConfusedEmote, DryadStand,
                DryadLookLeft);
            AppendKeyFrame(CreateOgre);
            AddKeyFrame(AppendPoint + 60, SpawnJavalinThrower);
            AddKeyFrame(AppendPoint + 120, SpawnGoblin);
            AddKeyFrame(AppendPoint + 180, SpawnGoblin);
            AddKeyFrame(AppendPoint + 240, SpawnWitherBeast);
            AppendSequences(30, DryadStand, DryadLookLeft);
            AppendSequences(30, DryadLookRight, DryadWalk);
            AppendSequences(300, DryadAttack, DryadLookLeft);
            AppendKeyFrame(RemoveEnemyDamage);
            AppendSequences(60, DryadLookRight, DryadStand,
                DryadAlertEmote);
            AddSequences(AppendPoint - 90, 60, OgreLookLeft,
                OgreStand);
            AddKeyFrame(AppendPoint - 12, OgreSwingSound);
            AddSequences(AppendPoint - 30, 50, DryadPortalKnock,
                DryadStand);
            AppendKeyFrame(RestoreEnemyDamage);
            AppendSequences(40, DryadPortalFade, DryadStand);
            AppendSequence(180, DryadStand);
            AddSequence(0, AppendPoint, PerFrameSettings);
        }

        private void PerFrameSettings(FrameEventData evt)
        {
            CombatText.clearAll();
        }

        private void CreateDryad(FrameEventData evt)
        {
            _dryad = PlaceNPCOnGround(20, _startPoint);
            _dryad.knockBackResist = 0.0f;
            _dryad.immortal = true;
            _dryad.dontTakeDamage = true;
            _dryad.takenDamageMultiplier = 0.0f;
            _dryad.immune[byte.MaxValue] = 100000;
        }

        private void DryadInteract(FrameEventData evt)
        {
            if (_dryad == null)
                return;
            _dryad.ai[0] = 9f;
            if (evt.IsFirstFrame)
                _dryad.ai[1] = evt.Duration;
            _dryad.localAI[0] = 0.0f;
        }

        private void SpawnWitherBeast(FrameEventData evt)
        {
            var index = NPC.NewNPC((int) _portal.Center.X, (int) _portal.Bottom.Y, 568, 0, 0.0f, 0.0f, 0.0f,
                0.0f, byte.MaxValue);
            var npc = Main.npc[index];
            npc.knockBackResist = 0.0f;
            npc.immortal = true;
            npc.dontTakeDamage = true;
            npc.takenDamageMultiplier = 0.0f;
            npc.immune[byte.MaxValue] = 100000;
            npc.friendly = _ogre.friendly;
            _army.Add(npc);
        }

        private void SpawnJavalinThrower(FrameEventData evt)
        {
            var index = NPC.NewNPC((int) _portal.Center.X, (int) _portal.Bottom.Y, 561, 0, 0.0f, 0.0f, 0.0f,
                0.0f, byte.MaxValue);
            var npc = Main.npc[index];
            npc.knockBackResist = 0.0f;
            npc.immortal = true;
            npc.dontTakeDamage = true;
            npc.takenDamageMultiplier = 0.0f;
            npc.immune[byte.MaxValue] = 100000;
            npc.friendly = _ogre.friendly;
            _army.Add(npc);
        }

        private void SpawnGoblin(FrameEventData evt)
        {
            var index = NPC.NewNPC((int) _portal.Center.X, (int) _portal.Bottom.Y, 552, 0, 0.0f, 0.0f, 0.0f,
                0.0f, byte.MaxValue);
            var npc = Main.npc[index];
            npc.knockBackResist = 0.0f;
            npc.immortal = true;
            npc.dontTakeDamage = true;
            npc.takenDamageMultiplier = 0.0f;
            npc.immune[byte.MaxValue] = 100000;
            npc.friendly = _ogre.friendly;
            _army.Add(npc);
        }

        private void CreateCritters(FrameEventData evt)
        {
            for (var index = 0; index < 5; ++index)
            {
                var num = index / 5f;
                var npc = PlaceNPCOnGround(
                    Utils.SelectRandom(Main.rand, (short) 46, (short) 46, (short) 299, (short) 538),
                    _startPoint +
                    new Vector2((float) ((num - 0.25) * 400.0 + Main.rand.NextFloat() * 50.0 - 25.0),
                        0.0f));
                npc.ai[0] = 0.0f;
                npc.ai[1] = 600f;
                _critters.Add(npc);
            }

            if (_dryad == null)
                return;
            for (var index1 = 0; index1 < 10; ++index1)
            {
                var num = index1 / 10.0;
                var index2 = NPC.NewNPC((int) _dryad.position.X + Main.rand.Next(-1000, 800),
                    (int) _dryad.position.Y - Main.rand.Next(-50, 300), 356, 0, 0.0f, 0.0f, 0.0f, 0.0f,
                    byte.MaxValue);
                var npc = Main.npc[index2];
                npc.ai[0] = (float) (Main.rand.NextFloat() * 4.0 - 2.0);
                npc.ai[1] = (float) (Main.rand.NextFloat() * 4.0 - 2.0);
                npc.velocity.X = (float) (Main.rand.NextFloat() * 4.0 - 2.0);
                _critters.Add(npc);
            }
        }

        private void OgreSwingSound(FrameEventData evt)
        {
            Main.PlaySound(SoundID.DD2_OgreAttack, _ogre.Center);
        }

        private void DryadPortalKnock(FrameEventData evt)
        {
            if (_dryad != null)
            {
                if (evt.Frame == 20)
                {
                    _dryad.velocity.Y -= 7f;
                    _dryad.velocity.X -= 8f;
                    Main.PlaySound(3, (int) _dryad.Center.X, (int) _dryad.Center.Y, 1, 1f, 0.0f);
                }

                if (evt.Frame >= 20)
                {
                    _dryad.ai[0] = 1f;
                    _dryad.ai[1] = evt.Remaining;
                    _dryad.rotation += 0.05f;
                }
            }

            if (_ogre == null)
                return;
            if (evt.Frame > 40)
            {
                _ogre.target = Main.myPlayer;
                _ogre.direction = 1;
            }
            else
            {
                _ogre.direction = -1;
                _ogre.ai[1] = 0.0f;
                _ogre.ai[0] = Math.Min(40f, _ogre.ai[0]);
                _ogre.target = 300 + _dryad.whoAmI;
            }
        }

        private void RemoveEnemyDamage(FrameEventData evt)
        {
            _ogre.friendly = true;
            foreach (var npc in _army)
                npc.friendly = true;
        }

        private void RestoreEnemyDamage(FrameEventData evt)
        {
            _ogre.friendly = false;
            foreach (var npc in _army)
                npc.friendly = false;
        }

        private void DryadPortalFade(FrameEventData evt)
        {
            if (_dryad == null || _portal == null)
                return;
            if (evt.IsFirstFrame)
                Main.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, _dryad.Center);
            var amount = Math.Max(0.0f, (evt.Frame - 7) / (float) (evt.Duration - 7));
            _dryad.color = new Color(Vector3.Lerp(Vector3.One, new Vector3(0.5f, 0.0f, 0.8f), amount));
            _dryad.Opacity = 1f - amount;
            _dryad.rotation += (float) (0.0500000007450581 * (amount * 4.0 + 1.0));
            _dryad.scale = 1f - amount;
            if (_dryad.position.X < (double) _portal.Right.X)
            {
                _dryad.velocity.X *= 0.95f;
                _dryad.velocity.Y *= 0.55f;
            }

            var num1 = (int) (6.0 * amount);
            var num2 = _dryad.Size.Length() / 2f / 20f;
            for (var index = 0; index < num1; ++index)
                if (Main.rand.Next(5) == 0)
                {
                    var dust = Dust.NewDustDirect(_dryad.position, _dryad.width, _dryad.height, 27,
                        _dryad.velocity.X * 1f, 0.0f, 100, new Color(), 1f);
                    dust.scale = 0.55f;
                    dust.fadeIn = 0.7f;
                    dust.velocity *= 0.1f * num2;
                    dust.velocity += _dryad.velocity;
                }
        }

        private void CreatePortal(FrameEventData evt)
        {
            _portal = PlaceNPCOnGround(549, _startPoint + new Vector2(-240f, 0.0f));
            _portal.immortal = true;
        }

        private void DryadStand(FrameEventData evt)
        {
            if (_dryad == null)
                return;
            _dryad.ai[0] = 0.0f;
            _dryad.ai[1] = evt.Remaining;
        }

        private void DryadLookRight(FrameEventData evt)
        {
            if (_dryad == null)
                return;
            _dryad.direction = 1;
            _dryad.spriteDirection = 1;
        }

        private void DryadLookLeft(FrameEventData evt)
        {
            if (_dryad == null)
                return;
            _dryad.direction = -1;
            _dryad.spriteDirection = -1;
        }

        private void DryadWalk(FrameEventData evt)
        {
            _dryad.ai[0] = 1f;
            _dryad.ai[1] = 2f;
        }

        private void DryadConfusedEmote(FrameEventData evt)
        {
            if (_dryad == null || !evt.IsFirstFrame)
                return;
            EmoteBubble.NewBubble(87, new WorldUIAnchor(_dryad), evt.Duration);
        }

        private void DryadAlertEmote(FrameEventData evt)
        {
            if (_dryad == null || !evt.IsFirstFrame)
                return;
            EmoteBubble.NewBubble(3, new WorldUIAnchor(_dryad), evt.Duration);
        }

        private void CreateOgre(FrameEventData evt)
        {
            var index = NPC.NewNPC((int) _portal.Center.X, (int) _portal.Bottom.Y, 576, 0, 0.0f, 0.0f, 0.0f,
                0.0f, byte.MaxValue);
            _ogre = Main.npc[index];
            _ogre.knockBackResist = 0.0f;
            _ogre.immortal = true;
            _ogre.dontTakeDamage = true;
            _ogre.takenDamageMultiplier = 0.0f;
            _ogre.immune[byte.MaxValue] = 100000;
        }

        private void OgreStand(FrameEventData evt)
        {
            if (_ogre == null)
                return;
            _ogre.ai[0] = 0.0f;
            _ogre.ai[1] = 0.0f;
            _ogre.velocity = Vector2.Zero;
        }

        private void DryadAttack(FrameEventData evt)
        {
            if (_dryad == null)
                return;
            _dryad.ai[0] = 14f;
            _dryad.ai[1] = evt.Remaining;
            _dryad.dryadWard = false;
        }

        private void OgreLookRight(FrameEventData evt)
        {
            if (_ogre == null)
                return;
            _ogre.direction = 1;
            _ogre.spriteDirection = 1;
        }

        private void OgreLookLeft(FrameEventData evt)
        {
            if (_ogre == null)
                return;
            _ogre.direction = -1;
            _ogre.spriteDirection = -1;
        }

        public override void OnBegin()
        {
            Main.NewText("DD2Film: Begin", byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
            Main.dayTime = true;
            Main.time = 27000.0;
            _startPoint = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY - 32f);
            base.OnBegin();
        }

        private NPC PlaceNPCOnGround(int type, Vector2 position)
        {
            var x = (int) position.X;
            var y = (int) position.Y;
            var i = x / 16;
            var j = y / 16;
            while (!WorldGen.SolidTile(i, j))
                ++j;
            var Y = j * 16;
            var Start = 100;
            switch (type)
            {
                case 20:
                    Start = 1;
                    break;
                case 576:
                    Start = 50;
                    break;
            }

            var index = NPC.NewNPC(x, Y, type, Start, 0.0f, 0.0f, 0.0f, 0.0f, byte.MaxValue);
            return Main.npc[index];
        }

        public override void OnEnd()
        {
            if (_dryad != null)
                _dryad.active = false;
            if (_portal != null)
                _portal.active = false;
            if (_ogre != null)
                _ogre.active = false;
            foreach (Entity critter in _critters)
                critter.active = false;
            foreach (Entity entity in _army)
                entity.active = false;
            Main.NewText("DD2Film: End", byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
            base.OnEnd();
        }
    }
}