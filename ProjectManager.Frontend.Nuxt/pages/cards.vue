<template>
  <v-row>
    <v-col class="text-center">
      <img src="/v.png" alt="Vuetify.js" class="mb-5" />
      <blockquote class="blockquote">
        &#8220;First, solve the problem. Then, write the code.&#8221;
        <footer>
          <small>
            <em>&mdash;John Johnson</em>
          </small>
        </footer>
      </blockquote>
      <button @click="onClick">Log user</button>
      <button @click="getCards">Get Cards</button>
      <button @click="$auth.logout()">Logout</button>
    </v-col>
  </v-row>
</template>
<script>
  import {mapState, mapActions, mapMutations} from 'vuex'

export default {

  computed: mapState({

  }),

  async fetch() {
  },

  methods: {
    onClick() {
      console.log("user:", this.$auth.user);
    },
    getCards() {
      var requestPath = "/api/cards/" + this.$auth.user.sub
      // console.log(this.$axios.get("/api/cards"))
      console.log(this.$axios.get(requestPath))
    },
  },

};
</script>
