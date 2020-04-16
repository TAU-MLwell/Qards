using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qards
{
    public class QardsGame
    {
        Deck main_deck;
        Dictionary<string, Deck> players = new Dictionary<string, Deck>();

        public QardsGame(string deck_description)
        {
            main_deck = Deck.Load(deck_description);
        }

        public Deck GetPlayer(string player_name)
        {
            if (players.ContainsKey(player_name))
                return players[player_name];
            else
            {
                var cards = Enumerable.Range(0,4).Select(i => main_deck.GetRandom());
                var deck = new Deck(cards);
                players[player_name] = deck;
                return deck;
            }
        }

        public bool DrawCard(string player_name)
        {
            var c = main_deck.GetRandom();
            if (c == null) return false;
            GetPlayer(player_name).Add(c);
            return true;
        }

        public bool TransferCard(Deck from, Deck to, string family, string name)
        {
            var c = from.Get(family, name);
            if (c == null) return false;
            to.Add(c);
            return true;
        }


    }
}
