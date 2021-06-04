<template>
  <v-row>
    <v-col class="text-center">
      <div v-if="cards">
        <p v-for="c in cards">
          {{ c.name }}
        </p>
      </div>
      <v-btn @click="getCards">Get Cards</v-btn>
      <div>
        <v-text-field label="Card Name" v-model="cardName"></v-text-field>
        <v-btn @click="saveCard">Save Card</v-btn>
      </div>
    </v-col>
  </v-row>
</template>
<script>
import { mapState, mapActions, mapMutations } from "vuex";
import https from "https";

export default {
  computed: mapState({}),

  async fetch() {
    this.cards = [{ name: "Dummy Card" }];
    await this.getCards();
  },

  data() {
    return {
      cards: [],
      cardName: "New Card"
    };
  },

  methods: {
    onClick() {
      console.log("user:", this.$auth.user);
    },
    async getCards() {
      console.log("Getting Cards");
      var requestPath = "/api/cards/" + this.$auth.user.sub;
      const agent = new https.Agent({
        rejectUnauthorized: false,
      });
      var resp = await this.$axios.get(requestPath, { httpsAgent: agent });
      this.cards = resp.data;
    },
    async saveCard() {
      var requestPath = "/api/cards/";
      var resp = await this.$axios.post(requestPath, {name: this.cardName, userId: this.$auth.user.sub})
      console.log("resp from savecard", resp)
      await this.getCards()
    }
  },
};
</script>
