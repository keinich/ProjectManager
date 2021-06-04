<template>
  <div id="canvas-wrap" dropzone="true">
    <div id="text-editor" class="card">
      <div class="toolbar">
        <ul class="tool-list">
          <li class="tool">
            <button type="button" data-command="justifyLeft" class="tool--btn">
              <i class="fas fa-align-left"></i>
            </button>
          </li>
          <li class="tool">
            <button
              type="button"
              data-command="justifyCenter"
              class="tool--btn"
            >
              <i class="fas fa-align-center"></i>
            </button>
          </li>
          <li class="tool">
            <button type="button" data-command="bold" class="tool--btn">
              <i class="fas fa-bold"></i>
            </button>
          </li>
          <li class="tool">
            <button type="button" data-command="italic" class="tool--btn">
              <i class="fas fa-italic"></i>
            </button>
          </li>
          <li class="tool">
            <button type="button" data-command="underline" class="tool--btn">
              <i class="fas fa-underline"></i>
            </button>
          </li>
          <li class="tool">
            <button
              type="button"
              data-command="insertOrderedList"
              class="tool--btn"
            >
              <i class="fas fa-list-ol"></i>
            </button>
          </li>
          <li class="tool">
            <button
              type="button"
              data-command="insertUnorderedList"
              class="tool--btn"
            >
              <i class="fas fa-list-ul"></i>
            </button>
          </li>
          <li class="tool">
            <button type="button" data-command="createlink" class="tool--btn">
              <i class="fas fa-link"></i>
            </button>
          </li>
        </ul>
      </div>

      <div id="output" contenteditable="true"></div>
    </div>

    <canvas dropzone="true" ref="jedi"> </canvas>

    <div id="card-templates">
      <section class="draggable-elements">
        <i
          class="fas fa-book draggable"
          draggable="true"
          style="color: grey"
          id="cat"
        ></i>
        <i
          class="fas fa-clipboard draggable"
          draggable="true"
          style="color: grey"
          id="dog"
        ></i>
        <i
          class="fas fa-chart-bar draggable"
          draggable="true"
          style="color: grey"
          id="bear"
        ></i>
      </section>
    </div>
  </div>
</template>

<script>
import https from "https";
import * as cardBoard from "../assets/cardBoard.js";
import * as textEditor from "../assets/textEditor.js";

export default {
  data() {
    return {
      cards: [],
      cardName: "New Card",
    };
  },

  async fetch() {
    await this.getCards();
    console.log(this.cards);

    this.placeCards();
  },

  mounted() {
    console.log("mounted start");
    cardBoard.resizeCanvas();

    textEditor.init();

    var cont = document.getElementById("canvas-wrap");

    cont.addEventListener("drop", this.drop);

    this.placeCards();
  },

  methods: {
    async createCard(posX, posY) {
      console.log("create card");

      var requestPath = "/api/cards/";
      var resp = await this.$axios.post(requestPath, {
        name: "New Card",
        userId: this.$auth.user.sub,
        positionX: posX,
        positionY: posY,
      });
      console.log("resp from savecard", resp);
      this.placeCard(resp.data);
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
    async drop(event) {
      event.preventDefault();
      var cont = document.getElementById("canvas-wrap");
      console.log("event", event);
      await this.createCard(event.layerX, event.layerY);
    },
    placeCards() {
      this.cards.forEach((card) => {
        this.placeCard(card);
      });
    },
    placeCard(card) {
      var cardElement = document.createElement("div");
      cardElement.innerHTML = `<p>${card.name}</p>`;
      cardElement.style.position = "absolute";
      var topS = card.positionY + "px";
      var leftS = card.positionX + "px";
      cardElement.style.top = topS;
      cardElement.style.left = leftS;
      cardElement.style.color = "white";
      cardElement.classList.add("card");
      var cont = document.getElementById("canvas-wrap");
      cont.appendChild(cardElement);
      console.log("card placed");
    },
  },
};
</script>

<style>
@import "https://use.fontawesome.com/releases/v5.9.0/css/all.css";
@import "../assets/style.css";
</style>