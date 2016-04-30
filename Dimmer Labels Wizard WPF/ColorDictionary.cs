using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ColorDictionary : IEnumerable<ColorEntry>
    {
        // Navigation Properties.
        public virtual ICollection<ColorEntry> Entries { get; set; }

        // Propeties.
        public int Count
        {
            get
            {
                return Entries.Count;
            }
        }

        public Color this[int key]
        {
            get
            {
                return GetColor(key);
            }

            set
            {
                if (ContainsKey(key) == true)
                {
                    // Remove before adding incoming Entry.
                    Remove(key);
                }

                Add(key, value);
            }
        }


        // Methods.
        public Color GetColor(int key)
        {
            var query = from entry in Entries
                        where entry.Key == key
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

        public bool TryGetColor(int key, out Color value)
        {
            var query = from entry in Entries
                        where entry.Key == key
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

        public bool ContainsKey(int key)
        {
            Color ignore = Colors.White;

            return TryGetColor(key, out ignore);
        }

        public void Add(int key, Color value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists in the Dictionary"); 
            }

            else
            {
                Entries.Add(new ColorEntry { Key = key, Value = value });
            }
        }

        public void Remove(int key)
        {
            ColorEntry entry = (from item in Entries
                                where item.Key == key
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

    public class ColorEntry
    {
        // Auto Generated ID.
        public int ID { get; set; }

        // Key Value Pair.
        public int Key { get; set; }
        public Color Value { get; set; }

        // Foreign Key.
        public int ColorDictionaryID { get; set; }

    }

}
