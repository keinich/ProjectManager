<template>
  <div>
    <v-btn @click="login">login</v-btn>
    <v-btn @click="api('test')">test</v-btn>
  </div>
</template>

<script>
import { mapState } from "vuex";
import { UserManager, WebStorageStateStore } from "oidc-client";

export default {
  data: () => ({
    userMgr: null,
  }),
  created() {
    if (!process.server) {
      this.userMgr = new UserManager({
        authority: "http://localhost:5000",
        client_id: "web-client",
        redirect_uri: "http://localhost:3000/login",
        response_type: "code",
        scope: "openid profile IdentityServerApi",
        // loadUserInfo: true,
        post_logout_redirect_uri: "http//localhost:3000",
        // silent_redirect_uri: "http//localhost:3000/",
        userStore: new WebStorageStateStore({
          store: window.localStorage
        })
      });

      this.userMgr.getUser().then(user => {
        if (user) {
          console.log("user from storage", user)
          this.$axios.create({baseUrl: 'http://localhost:5000/'})
          this.$axios.setToken(`Bearer ${user.acces_token}`)
        }
      });

      const { code, scope, session_state, state } = this.$route.query;
      if (code && scope && session_state && state) {
        this.userMgr.signinRedirectCallback().then((user) => {
          console.log(user);
          this.$axios.create({baseUrl: 'http://localhost:5000/'})
          this.$axios.setToken(`Bearer ${user.acces_token}`)
          this.$router.push("/");
        });
      }
    }
  },
  methods: {
    login() {
      return this.userMgr.signinRedirect();
    },
    api(x) {
      this.$axios.create({baseUrl: 'http://localhost:5000/'})
      return this.$axios.$get("api/cards/" + x)
        .then(msg => console.log(msg))
    }
  },
};
</script>
