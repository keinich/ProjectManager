<template>
  <div>
    <h1>Login</h1>
    <button @click="onClick">Login with Auth0</button>
  </div>
</template>
<script>
export default {
  methods: {
    onClick() {
      console.log("Logging in with auth0")
      this.$auth.loginWith('auth0')
    }
  }
}
</script>