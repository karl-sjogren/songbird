import Delaunator from 'delaunator';

export function initialize() {
  generateNewBackground();
  window.addEventListener('resize', generateNewBackground);
}

function generateNewBackground() {
  const width = window.innerWidth + 150;
  const height = window.innerHeight + 150;

  var points = [
    [0, 0],
    [width, 0],
    [width, height],
    [0, height]
  ];

  const colors = [
    'rgb(240, 248, 255)',
    'rgb(223, 240, 255)',
    'rgb(203, 231, 255)',
    'rgb(179, 219, 255)',
    //'rgb(204, 200, 255)',
    //'rgb(217, 214, 255)'
  ];

  let nubmerOfPoints = 400;
  if(width > 992) {
    nubmerOfPoints = 1000;
  }

  if(width > 1440) {
    nubmerOfPoints = 1500;
  }

  for(let i = 0; i < nubmerOfPoints; i++) {
    var rng1 = Math.random() * width;
    var rng2 = Math.random() * height;

    points.push([rng1, rng2]);
  }

  const delaunay = Delaunator.from(points);
  const canvas = document.createElement('canvas');
  const ctx = canvas.getContext('2d');

  let minX = Infinity;
  let minY = Infinity;
  let maxX = -Infinity;
  let maxY = -Infinity;
  for(let i = 0; i < points.length; i++) {
    let x = points[i][0];
    let y = points[i][1];
    if(x < minX) minX = x;
    if(y < minY) minY = y;
    if(x > maxX) maxX = x;
    if(y > maxY) maxY = y;
  }

  canvas.style.width = width + 'px';
  canvas.style.height = width + 'px';

  canvas.width = width;
  canvas.height = height;

  if(window.devicePixelRatio >= 2) {
    canvas.width = width * 2;
    canvas.height = height * 2;
    ctx.scale(2, 2);
  }

  const ratio = width / Math.max(maxX - minX, maxY - minY);

  function getX(i) {
    return ratio * (points[i][0] - minX);
  }
  function getY(i) {
    return ratio * (points[i][1] - minY);
  }

  ctx.clearRect(0, 0, width, height);

  const triangles = delaunay.triangles;

  for(let i = 0; i < triangles.length; i += 3) {
    ctx.beginPath();
    let p0 = triangles[i];
    let p1 = triangles[i + 1];
    let p2 = triangles[i + 2];
    ctx.moveTo(getX(p0), getY(p0));
    ctx.lineTo(getX(p1), getY(p1));
    ctx.lineTo(getX(p2), getY(p2));
    ctx.closePath();

    ctx.fillStyle = colors[Math.floor(Math.random() * colors.length)];
    ctx.fill();
  }

  canvas.toBlob(blob => {
    const objectURL = URL.createObjectURL(blob);
    document.querySelector('body').style.backgroundImage = `url(${objectURL})`;
  }, 'image/png');

}

export default {
  initialize
};

