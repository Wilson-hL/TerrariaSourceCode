// Decompiled with JetBrains decompiler
// Type: Terraria.Cinematics.CinematicManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
    public class CinematicManager
    {
        public static CinematicManager Instance = new CinematicManager();
        private readonly List<Film> _films = new List<Film>();

        public void Update(GameTime gameTime)
        {
            if (_films.Count <= 0)
                return;
            if (!_films[0].IsActive)
                _films[0].OnBegin();
            if (!Main.hasFocus || Main.gamePaused || _films[0].OnUpdate(gameTime))
                return;
            _films[0].OnEnd();
            _films.RemoveAt(0);
        }

        public void PlayFilm(Film film)
        {
            _films.Add(film);
        }

        public void StopAll()
        {
        }
    }
}