using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class ColorDictionary : IEnumerable<ColorEntry>
    {
        // Database and Navigation Properties.
        public int ID { get; set; }

        [DataMember]
        public virtual ICollection<ColorEntry> Entries { get; set; } = new List<ColorEntry>();

        // RackType.
        [DataMember]
        public RackType EntriesRackType { get; set; }

        // Propeties.
        public int Count
        {
            get
            {
                return Entries.Count;
            }
        }

        public Color this[int universeKey, int dimmerNumberKey]
        {
            get
            {
                return GetColor(universeKey, dimmerNumberKey);
            }

            set
            {
                if (ContainsKey(universeKey, dimmerNumberKey) == true)
                {
                    // Remove before adding incoming Entry.
                    Remove(universeKey, dimmerNumberKey);
                }

                Add(universeKey, dimmerNumberKey, value);
            }
        }


        // Methods.
        public Color GetColor(int universeKey, int dimmerNumberKey)
        {
            var query = from entry in Entries
                        where entry.UniverseKey == universeKey &&
                        entry.DimmerNumberKey == dimmerNumberKey
                        select entry.Value;

            if (query.Count() == 0)
            {
                throw new KeyNotFoundException();
            }

            else
            {
                return query.First();
            }
        }

        public bool TryGetColor(int universeKey, int dimmerNumberKey, out Color value)
        {
            var query = from entry in Entries
                        where entry.UniverseKey == universeKey &&
                        entry.DimmerNumberKey == dimmerNumberKey
                        select entry;

            if (query.Count() == 0)
            {
                value = Colors.White;
                return false;
            }

            else
            {
                value = query.First().Value;
                return true;
            }
        }

        public bool ContainsKey(int universeKey, int dimmerNumberKey)
        {
            Color ignore = Colors.White;

            return TryGetColor(universeKey, dimmerNumberKey, out ignore);
        }

        public void Add(int universeKey, int dimmerNumberKey, Color value)
        {
            if (ContainsKey(universeKey, dimmerNumberKey))
            {
                throw new ArgumentException("An element with the same key already exists in the Dictionary"); 
            }

            else
            {
                Entries.Add(new ColorEntry
                {
                    UniverseKey = universeKey,
                    DimmerNumberKey = dimmerNumberKey,
                    Value = value
                });
            }
        }

        public void Remove(int universeKey, int dimmerNumberKey)
        {
            ColorEntry entry = (from item in Entries
                                where item.UniverseKey == universeKey &&
                                item.DimmerNumberKey == dimmerNumberKey
                                select item).ToList().FirstOrDefault();

            if (entry != null)
            {
                Entries.Remove(entry);
            }
        }

        public IEnumerator<ColorEntry> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [DataContract]
    public class ColorEntry
    {
        // Database and Navigation Properties.
        public int ID { get; set; }
        public int ColorDictionaryID { get; set; }
        public ColorDictionary ColorDictionary { get; set; }

        // Key Value Pairings.
        [DataMember]
        public int UniverseKey { get; set; }
        [DataMember]
        public int DimmerNumberKey { get; set; }


        public Color Value
        {
            get
            {
                return new Color()
                {
                    A = A,
                    R = R,
                    G = G,
                    B = B,
                };
            }

            set
            {
                A = value.A;
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        [DataMember]
        public byte A { get; set; } = 255;
        [DataMember]
        public byte R { get; set; } = 255;
        [DataMember]
        public byte G { get; set; } = 255;
        [DataMember]
        public byte B { get; set; } = 255;


    }

}
