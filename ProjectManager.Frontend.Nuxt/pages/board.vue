<template>
  <canvas ref="jedi"> </canvas>
</template>

<script>
import * as cardBoard from "../assets/cardBoard.js";

export default {

  async fetch() {   
  },

  mounted() {
    console.log("mounted start")
    cardBoard.resizeCanvas();
  }

};
</script>

<style>
@import "../assets/style.css";
</style>