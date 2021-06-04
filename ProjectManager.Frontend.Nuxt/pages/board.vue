<template>
  <div id="canvas-wrap" dropzone="true">
    <canvas dropzone="true" ref="jedi"> </canvas>


    
    <div id="overlay" class="main-board">
      <p draggable="true">Test</p>
      <section class="draggable-elements">
        <i
          class="fas fa-cat draggable"
          draggable="true"
          style="color: blueviolet"
          id="cat"
        ></i>
      </section>
      <section class="droppable-elements">
        <div class="droppable" data-draggable-id="cat">
          <span>Cat</span>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
import * as cardBoard from "../assets/cardBoard.js";



export default {
  async fetch() {},

  mounted() {
    console.log("mounted start");
    cardBoard.resizeCanvas();    

    cardBoard.canvas.addEventListener("drop", this.drop)
  },

  methods: {
    createCard() {
      console.log("create card");
    },
    drop(event) {
      event.preventDefault();
      this.createCard();
    }
  }
};
</script>

<style>
@import "https://use.fontawesome.com/releases/v5.9.0/css/all.css";    
@import "../assets/style.css";
</style>