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
    </v-col>
  </v-row>
</template>
<script>
export default {
  methods: {
    onClick() {
      console.log("user:", this.$auth.user);
    },
  },
};
</script>
