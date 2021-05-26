<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <div class="text-center">
        <logo />
        <vuetify-logo />
      </div>

      <div v-if="cards">
        <p v-for="c in cards">
          {{c.name}}
        </p>
      </div>

      <div>
        <v-text-field label="Card Name" v-model = "cardName"></v-text-field>
        <v-btn @click="saveCard">Save Card</v-btn>
      </div>

      {{message}}
      <v-btn @click="reset">Reset Message</v-btn>
      <v-btn @click="resetCards">Reset Cards</v-btn>
    </v-col>
  </v-row>
</template>

<script>
  import Logo from '~/components/Logo.vue'
  import VuetifyLogo from '~/components/VuetifyLogo.vue'
  import Axios from "axios";
  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    components: {
      Logo,
      VuetifyLogo
    },
    data: () => ({
      cardName: ""
    }),
    computed: {
      ...mapState({
        message: state => state.message
      }),
      ...mapState('cards', {
        cards: state => state.cards
      })
    },
    methods: {
      ...mapMutations([
        'reset'
      ]),
      ...mapMutations('cards', {
        resetCards: 'reset'
      }),
      ...mapActions('cards', ['createCard']),
      async saveCard() {
        await this.createCard({ card: { name: this.cardName } });
      }
    }
    //async fetch() {
    //  await this.$store.dispatch('fetchMessage')
    //}
  }
</script>
