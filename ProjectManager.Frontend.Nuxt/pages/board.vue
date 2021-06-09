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

    <div
      id="card-0"
      class="card"
      style="visibility: hidden"
      draggable="true"
      dropzone="false"
    >
      <div class="texteditor" contenteditable="false" dropzone="false"></div>
      <i class="fas fa-expand resizer" draggable="true"></i>
    </div>

    <canvas dropzone="true" id="maincanvas"> </canvas>

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
      selectedCardIds: [],
      currentDragInfo: {
        dragType: "",
        cardId: "",
        templateId: "",
        startClientX: 0,
        startClientY: 0,
        startCardX: 0,
        startCardY: 0,
        startCardWidth: 0,
        startCardHeight: 0,
      },
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

    var canvasElement = document.getElementById("maincanvas");
    canvasElement.addEventListener("click", this.deselectCards);

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
      var requestPath = "/api/cards/";
      var resp = await this.$axios.put(requestPath, card);
    },

    // drag n drop
    async drop(event) {
      event.preventDefault();
      var dragType = this.currentDragInfo.dragType;
      console.log("dragtype at drop", dragType);
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
              var deltaX = event.clientX - this.currentDragInfo.startClientX;
              var deltaY = event.clientY - this.currentDragInfo.startClientY;
              var newX = this.currentDragInfo.startCardX + deltaX;
              var newY = this.currentDragInfo.startCardY + deltaY;
              c.positionX = newX;
              c.positionY = newY;
              this.updateCard(c);
            }
          });
          break;
      }
    },

    dragover(event) {
      // console.log("drag ev", event);
      event.preventDefault();
      var dragType = this.currentDragInfo.dragType;
      switch (dragType) {
        case "template":
          break;
        case "card":
          var cardElementId = this.getCardElementId(
            this.currentDragInfo.cardId
          );
          var cardElement = document.getElementById(cardElementId);
          var deltaX = event.clientX - this.currentDragInfo.startClientX;
          var deltaY = event.clientY - this.currentDragInfo.startClientY;
          var newX = this.currentDragInfo.startCardX + deltaX;
          var newY = this.currentDragInfo.startCardY + deltaY;
          this.setPosition(cardElement, newX, newY);
          break;
        case "resizeCard":
          var cardElementId = this.getCardElementId(
            this.currentDragInfo.cardId
          );
          var cardElement = document.getElementById(cardElementId);
          var deltaX = event.clientX - this.currentDragInfo.startClientX;
          var deltaY = event.clientY - this.currentDragInfo.startClientY;
          var newWidth = this.currentDragInfo.startCardWidth + deltaX;
          var newHeight = this.currentDragInfo.startCardHeight + deltaY;
          // console.log("newScale", {w: newWidth, h: newHeight});
          cardElement.style.width = newWidth + "px";
          cardElement.style.height = newHeight + "px";
      }
    },
    dragStartTemplate(event) {
      event.dataTransfer.setData("cardType", event.target.id);
      event.dataTransfer.setData("dragType", "template");
      this.currentDragInfo = {
        dragType: "template",
        cardId: "",
        templateId: "note",
      };
    },
    dragStartCard(event, cardId) {
      var img = document.createElement("img");
      img.src =
        "data:image/gif;base64,R0lGODlhAQABAIAAAAUEBAAAACwAAAAAAQABAAACAkQBADs=";
      event.dataTransfer.setDragImage(img, 0, 0);

      event.dataTransfer.setData("cardElementId", event.target.id);
      event.dataTransfer.setData("cardId", cardId);
      event.dataTransfer.setData("dragType", "card");

      var cardElementId = this.getCardElementId(cardId);
      var cardElement = document.getElementById(cardElementId);
      var eventTarget = event.target;
      var startCardX = parseInt(cardElement.style.left, 10);
      var startCardY = parseInt(cardElement.style.top, 10);
      var startCardWidth = parseInt(cardElement.style.width, 10);
      var startCardHeight = parseInt(cardElement.style.height, 10);
      console.log("starScale", {w: cardElement.style.width, h: startCardHeight});
      console.log("cardElement", cardElement);
      var dragType = "";
      if (eventTarget.classList.contains("resizer")) {
        dragType = "resizeCard";
      } else {
        dragType = "card";
      }

      this.currentDragInfo = {
        dragType: dragType,
        cardId: cardId,
        templateId: "",
        startClientX: event.clientX,
        startClientY: event.clientY,
        startCardX: startCardX,
        startCardY: startCardY,
        startCardWidth: startCardWidth,
        startCardHeight: startCardHeight,
      };
    },
    drag(event) {
      event.preventDefault();
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
      var cardId = card.id;
      var cardElementId = "card-" + card.id;
      cardElement.setAttribute("id", cardElementId);

      this.setPosition(cardElement, card.positionX, card.positionY);
      this.setScale(cardElement, 100, 100);
      cardElement.style.visibility = "visible";

      // Set Content
      var texteditorElement =
        cardElement.getElementsByClassName("texteditor")[0];
      texteditorElement.innerHTML = card.content;

      // insert the card
      var cont = document.getElementById("canvas-wrap");
      cont.appendChild(cardElement);

      // set card event listeners
      cardElement.addEventListener("focusout", (event) => {});
      cardElement.addEventListener("click", (event) => {
        this.selectCard(cardId);
      });
      cardElement.addEventListener("dragstart", (event) => {
        this.dragStartCard(event, card.id);
      });
      cardElement.addEventListener("drag", this.drag);
    },

    deselectCards() {
      this.selectedCardIds.forEach((cardId) => {
        this.deselectCard(cardId);
      });
    },
    selectCard(cardId) {
      var cardElementId = this.getCardElementId(cardId);
      var newCardElement = document.getElementById(cardElementId);
      var newTexteditorElement =
        newCardElement.getElementsByClassName("texteditor")[0];
      newCardElement.classList.add("cardSelected");
      newTexteditorElement.contentEditable = true;
      newCardElement.setAttribute("draggable", "false");
      this.selectedCardIds.push(cardId);
    },
    deselectCard(cardId) {
      var cardElementId = this.getCardElementId(cardId);
      var newCardElement = document.getElementById(cardElementId);
      var newTexteditorElement =
        newCardElement.getElementsByClassName("texteditor")[0];
      newTexteditorElement.contentEditable = false;
      var card = this.getCardById(cardId);
      console.log("card found", card);
      newCardElement.setAttribute("draggable", "true");
      card.content = newTexteditorElement.innerHTML;
      newCardElement.classList.remove("cardSelected");
      this.updateCard(card);
      this.removeFromArray(this.selectedCardIds, cardId);
    },

    getCardById(cardId) {
      var card = {};
      this.cards.forEach((c) => {
        if (cardId == c.id) {
          card = c;
        }
      });
      return card;
    },
    getCardElementId(cardId) {
      return "card-" + cardId;
    },
    setPosition(domElement, x, y) {
      var leftS = x + "px";
      var topS = y + "px";
      domElement.style.left = leftS;
      domElement.style.top = topS;
    },
    setScale(domElement, width, height) {
      var widthS = width + "px";
      var heightS = height + "px";
      domElement.style.width = widthS;
      domElement.style.height = heightS;
    }
    ,
    removeFromArray(arr, el) {
      const index = arr.indexOf(el);
      if (index > -1) {
        arr.splice(index, 1);
      }
    },
  },
};
</script>

<style>
@import "https://use.fontawesome.com/releases/v5.9.0/css/all.css";
@import "../assets/style.css";
</style>