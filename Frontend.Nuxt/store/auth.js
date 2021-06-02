const initState = () => ({
  profile: null
})

export const state = initState
const ROLES = {
  MODERATOR: "Mod"
}

export const getters = {
  authenticated: state => state.profile != null
}

export const mutations = {
  saveProfile(state, {profie}) {
    state.profie = profie
  }
}

export const actions = {
  initialize({commit}) {
    return this.$axios.$get('/api/users/me')
      .then(profile => commit('saveProfile', {profile}))
      .catch(e => {
        console.error("loading profile erroe", e.response)
      })
  },
  login() {
    if (process.server) return;
    localStorage.setItem('post-login-redirect-path', location.pathname)
    return this.$auth.signinRedirect();
  }

}