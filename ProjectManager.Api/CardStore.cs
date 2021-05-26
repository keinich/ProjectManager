using ProjectManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Api {


  public class CardStore {
    private readonly List<Card> _Cards;

    public CardStore() {
      _Cards = new List<Card>();
      _Cards.Add(new Card { Id = 1, Name = "Test Card" });
    }

    public IEnumerable<Card> All => _Cards;

    public void Add(Card card) {
      card.Id = _Cards.Count + 1;
      _Cards.Add(card);
    }

  }
}
