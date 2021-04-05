using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Structs {
    public struct HotbarSlot {
        public HotbarSlot(int slot, GameObject slotUI) {
            SlotNumber = slot;
            UIGameObj = slotUI;
            UIImageComponent = slotUI.GetComponent<Image>();
            UIRawImageComponent = slotUI.transform.Find("Item").gameObject.GetComponent<RawImage>();
            Ability = null;
        }

        public bool IsOccupied {
            get { return Ability != null; }
        }

        public int SlotNumber { get; }
        public GameObject UIGameObj { get; }
        public Image UIImageComponent { get; }
        public RawImage UIRawImageComponent { get; }
        public Spell Ability { get; set; }

        public override string ToString() => $"Slot {SlotNumber} - {Ability}";
    }
}
