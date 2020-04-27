using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qards
{
    public class Card
    {
        public Card(string description)
        {
            var fields = description.Split('\t');
            Family = fields[0];
            Name = fields[1];
            ImageURL = fields[2];
        }

        public string Family { get; set; }
        public string Name { get; set; }
        public string[] FamilyMembers { get; set; }
        public string ImageURL { get; set; }

    }

    public class Deck
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public Deck(IEnumerable<Card> Cards)
        {
            this.Cards = Cards.OrderBy(c => c.Family).ToList();
        }

        public bool Add(Card c)
        {
            if (Cards.Contains(c)) return (false);
            Cards.Add(c);
            Cards = Cards.OrderBy(c => c.Family).ToList();
            return (true);
        }

        public Card Get(string Family, string Name)
        {
            IEnumerable<Card> l = Cards;
            if (Family != null)
            {
                l = l.Where(c => c.Family == Family);
            }
            if (Name != null)
            {
                l = l.Where(c => c.Name == Name);
            }
            var c = l.FirstOrDefault();
            Cards.Remove(c);
            return c;
        }

        public Card GetRandom()
        {
            var l = Cards.Count();
            if (l == 0) return null;
            var r = RandomGen.Next() % l;
            var c = Cards[r];
            Cards.RemoveAt(r);
            return (c);
        }

        static void PopulateFamilyMembers(List<Card> Cards)
        {
            Dictionary<string, List<string>> members = new Dictionary<string, List<string>>();
            foreach (var c in Cards)
            {
                if (!members.ContainsKey(c.Family))
                    members[c.Family] = new List<string>();
                members[c.Family].Add(c.Name);
            }
            foreach (var c in Cards)
            {
                c.FamilyMembers = members[c.Family].Where(n => n != c.Name).ToArray();
            }
        }
        public static Deck Load(string[] description)
        {
            var Cards = description.Select(l => new Card(l)).ToList();
            PopulateFamilyMembers(Cards);
            return new Deck(Cards);
        }

        public static Deck Load(string description)
        {
            return Load(description.Split('\n'));
        }
    }


    public static class RandomGen
    {
        private static Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        public static int Next()
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (_global) seed = _global.Next();
                _local = inst = new Random(seed);
            }
            return inst.Next();
        }
    }
}