console.log("Hello from Card Board")

const event = new Event('testEvent');

export var canvas = {};

var selecting = false;

export function init() {
  if (typeof window !== 'undefined') {
    // We are in the browser
    canvas = document.querySelector('canvas');
    var mainWrap = canvas.parentNode.parentNode.parentNode;
    var w = mainWrap.offsetWidth;
    var h = mainWrap.offsetHeight;
    console.log("mainWrap", mainWrap);
    console.log("mainWrap w", w);
    console.log("mainWrap h", h);
    // canvas.width = w - 5;
    // canvas.height = h - 2;

    drawGrid();

    canvas.addEventListener("dragover", dragOver);
    canvas.addEventListener("mousedown", (event) => {selecting = true;});
    canvas.addEventListener("mouseup", (event) => {selecting = false;});
    canvas.addEventListener("mouseleave", canvasMouseLeave);
    window.addEventListener("mousemove", boxSelect);
  }
  
}
  
function canvasMouseLeave(event) {
  console.log("event", event);
  console.log("window dims", {w: window.innerWidth, h: window.innerHeight});
}

function boxSelect(event) {
  if (selecting) {
    console.log("selecting");
  }
}

function dragOver(event) {
  event.preventDefault();
}

function drawGrid() {

  var c = canvas.getContext('2d');

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

