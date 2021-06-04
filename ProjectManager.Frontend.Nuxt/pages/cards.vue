<template>
  <v-row>
    <v-col class="text-center">
      <div v-if="cards">
        <p v-for="c in cards">
          {{ c.name }}
        </p>
      </div>

      <v-btn @click="getCards">Get Cards</v-btn>
    </v-col>
  </v-row>
</template>
<script>
import { mapState, mapActions, mapMutations } from "vuex";
import https from 'https'

export default {
  computed: mapState({}),

  async fetch() {
    this.cards = [{ name: "Dummy Card" }];
    await this.getCards2();
  },

  data() {
    return {
      cards: [],
    };
  },

  methods: {
    onClick() {
      console.log("user:", this.$auth.user);
    },
    getCards() {
      console.log("Getting Cards");
      var requestPath = "/api/cards/" + this.$auth.user.sub;
      this.$axios.get(requestPath).then((resp) => {
        console.log("Receiving cards");
        this.cards = resp.data;
        console.log("cards", this.cards);
      });
    },
    getCards1() {
      console.log("Getting Cards");
      var requestPath = "/api/cards/" + this.$auth.user.sub;
      const agent = new https.Agent({
        rejectUnauthorized: false,
      });
      this.$axios.get(requestPath, {httpsAgent: agent}).then((resp) => {
        console.log("Receiving cards");
        this.cards = resp.data;
        console.log("cards", this.cards);
      });
    },
    async getCards2() {
      console.log("Getting Cards");
      var requestPath = "/api/cards/" + this.$auth.user.sub;
      const agent = new https.Agent({
        rejectUnauthorized: false,
      });
      var resp = await this.$axios.get(requestPath, {httpsAgent: agent})
      console.log("resp", resp)
      console.log("resp.data", resp.data)
      this.cards = resp.data
    },
  },
};
</script>
