console.log("Hello from Card Board")

const event = new Event('testEvent');

export var canvas = {}

// elem.dispatchEvent('testEvent');

export function resizeCanvas() {
  if (typeof window !== 'undefined') {
    // We are in the browser
    console.log("document", document);
    canvas = document.querySelector('canvas');
    console.log("canvas", canvas);

    console.log("canvas width", canvas.width);
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    console.log("canvas width after", canvas.width);

    var c = canvas.getContext('2d');

    drawGrid();

    canvas.addEventListener("drop", drop)
    canvas.addEventListener("dragenter", dragEnter)
    canvas.addEventListener("dragover", dragOver);
    canvas.addEventListener("dragleave", dragLeave);

    const draggableElements = document.querySelectorAll(".draggable")
    draggableElements.forEach(elem => {
      elem.addEventListener("dragstart", dragStart);
    });

  }

  function dragStart(event) {
    console.log("drag start")
  }

  function dragEnter(event) {
    event.preventDefault();
    console.log("drag enter")
  }

  function dragOver(event) {
    event.preventDefault();
  }

  function dragLeave(event) {
    event.preventDefault();
  }

  function drop(event) {
    // event.preventDefault();
    console.log("Drop")
  }

  function drawGrid() {

    var dist = 20;
    var size = .5;

    var i = 0, j = 0;
    for (i = 1; i * dist < canvas.width; i++) {
      for (j = 1; j * dist < canvas.height; j++) {

        c.beginPath();
        c.arc(i * dist, j * dist, size, Math.PI * 2, false);
        c.fillStyle = 'beige';
        c.strokeStyle = 'beige';
        c.fill();
      }
    }
  }
}


