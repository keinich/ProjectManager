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

    <div id="output" class="card" style="visibility: hidden">
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

    var noteTemplate = document.getElementById("note");

    console.log("noteTemplate", noteTemplate);

    noteTemplate.addEventListener("dragStart", this.dragStart);

    this.placeCards();
    textEditor.init();
  },

  methods: {
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
      var requestPath = "/api/cards/";
      var resp = await this.$axios.put(requestPath, card);
      console.log("resp from updateCard", resp);
    },

    async drop(event) {
      event.preventDefault();
      const cardType = event.dataTransfer.getData("cardType");
      if (cardType === "note") {
        await this.createNote(event.layerX, event.layerY);
      }
    },

    placeCards() {
      this.cards.forEach((card) => {
        this.placeCard(card);
      });
    },
    placeCard(card) {
      var texteditorElement = document.getElementById("output");
      var cardElement = texteditorElement.cloneNode(true);
      cardElement.removeAttribute("id");
      cardElement.setAttribute("id", "output-" + card.id);

      // Set Position
      var topS = card.positionY + "px";
      var leftS = card.positionX + "px";
      cardElement.style.top = topS;
      cardElement.style.left = leftS;
      cardElement.style.visibility = "visible";

      // Set Content
      var texteditorElement1 =
        cardElement.getElementsByClassName("texteditor")[0];
      this.setContent(texteditorElement1, card.content);

      console.log("TextEditor", texteditorElement1);
      var cont = document.getElementById("canvas-wrap");

      cont.appendChild(cardElement);
      cardElement.addEventListener("focusout", (event) => {
        var newOutput = document.getElementById("output-" + card.id);
        var texteditorElement2 =
          newOutput.getElementsByClassName("texteditor")[0];
        card.content = this.convertToString(texteditorElement2);
        console.log("new card content", card.content);
        this.updateCard(card);
      });
      console.log("card placed");
    },

    setContent(domElement, content) {
      domElement.innerHTML = content;
      return;
      var p = document.createElement("p");
      p.innerHTML = content;
      p.style.color = "red";
      domElement.appendChild(p);
    },
    convertToString(domElement) {
      return domElement.innerHTML;
      console.log("domElement", domElement);
      var result = "";
      domElement.children.forEach((c) => {
        console.log("child", c);
        console.log("child innerHTML", c.innerHTML);
        result += c.innerHTML;
      });
      return result;
    },
  },
};
</script>

<style>
@import "https://use.fontawesome.com/releases/v5.9.0/css/all.css";
@import "../assets/style.css";
</style>