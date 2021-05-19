using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Resource;
using EstateAgency.Entities;

namespace EstateAgency.Entities
{
    class Matcher: ICacheEntryFilter<int, EstateObject>
    {
        public int LocationID;
        public byte Variant;
        public int Price;
        public byte NeededState;

        public Matcher (ClientWish w)
        {
            LocationID = w.LocationID;
            Variant = w.Variant;
            Price = w.Price;
            NeededState = w.NeededState;
        }

        public bool Invoke (ICacheEntry<int, EstateObject> obj)
        {
            return (
                obj.Value.LocationID == LocationID &&
                obj.Value.Variant == Variant &&
                obj.Value.State >= NeededState &&
                (obj.Value.Price - Price < 1000)
            );
        }
    }
}
