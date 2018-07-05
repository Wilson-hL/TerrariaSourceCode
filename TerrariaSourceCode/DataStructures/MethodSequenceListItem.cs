// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.MethodSequenceListItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
    public class MethodSequenceListItem
    {
        public Func<bool> Method;
        public string Name;
        public MethodSequenceListItem Parent;
        public bool Skip;

        public MethodSequenceListItem(string name, Func<bool> method, MethodSequenceListItem parent = null)
        {
            Name = name;
            Method = method;
            Parent = parent;
        }

        public bool ShouldAct(List<MethodSequenceListItem> sequence)
        {
            if (Skip || !sequence.Contains(this))
                return false;
            if (Parent != null)
                return Parent.ShouldAct(sequence);
            return true;
        }

        public bool Act()
        {
            return Method();
        }

        public static void ExecuteSequence(List<MethodSequenceListItem> sequence)
        {
            foreach (var sequenceListItem in sequence)
                if (sequenceListItem.ShouldAct(sequence) && !sequenceListItem.Act())
                    break;
        }

        public override string ToString()
        {
            return "name: " + Name + " skip: " + Skip + " parent: " + Parent;
        }
    }
}