import Axios from "axios";

const initState = () => ({
  cards: []
})

export const state = initState

export const mutations = {
  setCards(state, { cards }) {
    state.cards = cards
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchCards({ commit }) {
    const cards = (await Axios.get("http://localhost:5000/api/cards")).data;
    console.log("Cards:", cards);
    commit("setCards", { cards })
  },
  async createCard({ commit, dispatch }, { card }) {
    await Axios.post("http://localhost:5000/api/cards", card)
    await dispatch('fetchCards')
  }
}
