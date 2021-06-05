<template>
  <div id="canvas-wrap" dropzone="true">
    <div class="toolbar card" style="height: 5rem">
      <ul class="tool-list">
        <li class="tool">
          <button type="button" data-command="justifyLeft" class="tool--btn">
            <i class="fas fa-align-left"></i>
          </button>
        </li>
        <li class="tool">
          <button type="button" data-command="justifyCenter" class="tool--btn">
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

    <div id="card-0" class="card" style="visibility: hidden" draggable="true">
      <div class="texteditor" contenteditable="true"></div>
    </div>

    <canvas dropzone="true" ref="jedi"> </canvas>

    <div id="card-templates">
      <i
        class="fas fa-sticky-note fa-xs draggable tooltip"
        draggable="true"
        style="color: grey"
        id="note"
        ><span class="tooltiptext">Note</span></i
      >
      <i
        class="fas fa-columns fa-xs draggable tooltip"
        draggable="true"
        style="color: grey"
        id="column"
        ><span class="tooltiptext">Column</span></i
      >
      <i
        class="fas fa-clipboard fa-xs draggable tooltip"
        draggable="true"
        style="color: grey"
        id="board"
        ><span class="tooltiptext">Board</span></i
      >
    </div>
  </div>
</template>

<script>
import https from "https";
import * as cardBoard from "../assets/cardBoard.js";
import * as textEditor from "../assets/textEditor.js";

export default {

  // vue hooks
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

    var cont = document.getElementById("canvas-wrap");

    cont.addEventListener("drop", this.drop);
    cont.addEventListener("dragover", this.dragover);

    var noteTemplate = document.getElementById("note");

    console.log("noteTemplate", noteTemplate);

    noteTemplate.addEventListener("dragstart", this.dragStartTemplate);

    this.placeCards();
    textEditor.init();
  },

  methods: {

    // crud
    async getCards() {
      console.log("Getting Cards");
      var requestPath = "/api/cards/" + this.$auth.user.sub;
      const agent = new https.Agent({
        rejectUnauthorized: false,
      });
      var resp = await this.$axios.get(requestPath, { httpsAgent: agent });
      this.cards = resp.data;
    },
    async createNote(posX, posY) {
      console.log("create note");

      var requestPath = "/api/cards/";
      var resp = await this.$axios.post(requestPath, {
        name: "New Card",
        userId: this.$auth.user.sub,
        positionX: posX,
        positionY: posY,
        type: 0,
        content: "",
      });
      console.log("resp from savecard", resp);
      this.placeCard(resp.data);
    },
    async updateCard(card) {
      console.log("update card", card);
      var requestPath = "/api/cards/";
      var resp = await this.$axios.put(requestPath, card);
      console.log("resp from updateCard", resp);
    },

    // drag n drop
    async drop(event) {
      event.preventDefault();
      var dragType = event.dataTransfer.getData("dragType");
      switch (dragType) {
        case "template":
          const cardType = event.dataTransfer.getData("cardType");
          if (cardType === "note") {
            await this.createNote(event.layerX, event.layerY);
          }
          break;
        case "card":
          var cardId = event.dataTransfer.getData("cardId");
          console.log("cardId", cardId);
          console.log("cards", this.cards);
          var card;
          var card = this.cards.forEach((c) => {
            if (c.id == cardId) {
              c.positionX = event.layerX;
              c.positionY = event.layerY;
              this.updateCard(c);
            }
          });
          break;
      }
    },

    dragover(event) {
      var dragType = event.dataTransfer.getData("dragType");
      switch (dragType) {
        case "template":
          break;
        case "card":
          var cardElementId = event.dataTransfer.getData("cardElementId");
          var cardElement = document.getElementById(cardElementId);
          this.setPosition(cardElement, event.layerX, event.layerY);
          break;
      }
    },
    dragStartTemplate(event) {
      event.dataTransfer.setData("cardType", event.target.id);
      event.dataTransfer.setData("dragType", "template");
      console.log("start dragging template");
    },
    dragStartCard(event, cardId) {
      event.dataTransfer.setData("cardElementId", event.target.id);
      event.dataTransfer.setData("cardId", cardId);
      event.dataTransfer.setData("dragType", "card");
      console.log("start dragging template");
    },

    // cardboard utils
    placeCards() {
      this.cards.forEach((card) => {
        this.placeCard(card);
      });
    },
    placeCard(card) {
      var cardTemplateElement = document.getElementById("card-0");
      var cardElement = cardTemplateElement.cloneNode(true);
      cardElement.removeAttribute("id");
      var cardElementId = "card-" + card.id;
      cardElement.setAttribute("id", cardElementId);
      
      this.setPosition(cardElement, card.positionX, card.positionY);
      cardElement.style.visibility = "visible";

      // Set Content
      var texteditorElement = cardElement.getElementsByClassName("texteditor")[0];
      texteditorElement.innerHTML = card.content;

      // insert the card
      var cont = document.getElementById("canvas-wrap");
      cont.appendChild(cardElement);

      // set card event listeners
      cardElement.addEventListener("focusout", (event) => {
        var newCardElement = document.getElementById(cardElementId);
        var newTexteditorElement =
          newCardElement.getElementsByClassName("texteditor")[0];
        card.content = newTexteditorElement.innerHTML;
        this.updateCard(card);
      });
      cardElement.addEventListener("dragstart", (event) => {
        this.dragStartCard(event, card.id);
      });
    },

    setPosition(domElement, x, y) {
      var leftS = x + "px";
      var topS = y + "px";
      domElement.style.left = leftS;
      domElement.style.top = topS;
    },
  },
};
</script>

<style>
@import "https://use.fontawesome.com/releases/v5.9.0/css/all.css";
@import "../assets/style.css";
</style>