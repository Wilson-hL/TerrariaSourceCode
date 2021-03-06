﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIList
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    public class UIList : UIElement
    {
        protected List<UIElement> _items = new List<UIElement>();
        private UIElement _innerList = (UIElement) new UIList.UIInnerList();
        public float ListPadding = 5f;
        protected UIScrollbar _scrollbar;
        private float _innerListHeight;

        public int Count
        {
            get { return this._items.Count; }
        }

        public UIList()
        {
            this._innerList.OverflowHidden = false;
            this._innerList.Width.Set(0.0f, 1f);
            this._innerList.Height.Set(0.0f, 1f);
            this.OverflowHidden = true;
            this.Append(this._innerList);
        }

        public float GetTotalHeight()
        {
            return this._innerListHeight;
        }

        public void Goto(UIList.ElementSearchMethod searchMethod)
        {
            for (var index = 0; index < this._items.Count; ++index)
            {
                if (searchMethod(this._items[index]))
                {
                    this._scrollbar.ViewPosition = this._items[index].Top.Pixels;
                    break;
                }
            }
        }

        public virtual void Add(UIElement item)
        {
            this._items.Add(item);
            this._innerList.Append(item);
            this.UpdateOrder();
            this._innerList.Recalculate();
        }

        public virtual bool Remove(UIElement item)
        {
            this._innerList.RemoveChild(item);
            this.UpdateOrder();
            return this._items.Remove(item);
        }

        public virtual void Clear()
        {
            this._innerList.RemoveAllChildren();
            this._items.Clear();
        }

        public override void Recalculate()
        {
            base.Recalculate();
            this.UpdateScrollbar();
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (this._scrollbar == null)
                return;
            this._scrollbar.ViewPosition -= (float) evt.ScrollWheelValue;
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            var pixels = 0.0f;
            for (var index = 0; index < this._items.Count; ++index)
            {
                this._items[index].Top.Set(pixels, 0.0f);
                this._items[index].Recalculate();
                var outerDimensions = this._items[index].GetOuterDimensions();
                pixels += outerDimensions.Height + this.ListPadding;
            }

            this._innerListHeight = pixels;
        }

        private void UpdateScrollbar()
        {
            if (this._scrollbar == null)
                return;
            this._scrollbar.SetView(this.GetInnerDimensions().Height, this._innerListHeight);
        }

        public void SetScrollbar(UIScrollbar scrollbar)
        {
            this._scrollbar = scrollbar;
            this.UpdateScrollbar();
        }

        public void UpdateOrder()
        {
            this._items.Sort(new Comparison<UIElement>(this.SortMethod));
            this.UpdateScrollbar();
        }

        public int SortMethod(UIElement item1, UIElement item2)
        {
            return item1.CompareTo((object) item2);
        }

        public override List<SnapPoint> GetSnapPoints()
        {
            var snapPointList = new List<SnapPoint>();
            SnapPoint point;
            if (this.GetSnapPoint(out point))
                snapPointList.Add(point);
            foreach (var uiElement in this._items)
                snapPointList.AddRange((IEnumerable<SnapPoint>) uiElement.GetSnapPoints());
            return snapPointList;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (this._scrollbar != null)
                this._innerList.Top.Set(-this._scrollbar.GetValue(), 0.0f);
            this.Recalculate();
        }

        public delegate bool ElementSearchMethod(UIElement element);

        private class UIInnerList : UIElement
        {
            public override bool ContainsPoint(Vector2 point)
            {
                return true;
            }

            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                var position1 = this.Parent.GetDimensions().Position();
                var dimensions1 =
                    new Vector2(this.Parent.GetDimensions().Width, this.Parent.GetDimensions().Height);
                foreach (var element in this.Elements)
                {
                    var position2 = element.GetDimensions().Position();
                    var dimensions2 = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height);
                    if (Collision.CheckAABBvAABBCollision(position1, dimensions1, position2, dimensions2))
                        element.Draw(spriteBatch);
                }
            }
        }
    }
}