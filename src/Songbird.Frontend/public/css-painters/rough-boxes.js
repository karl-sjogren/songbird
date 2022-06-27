/* globals registerPaint */
/**
 * Inlined version of https://github.com/davidbau/seedrandom
 * Version 3.05, released under MIT license
*/
/* eslint-disable-next-line */
!function(f,a,c){var s,l=256,p="random",d=c.pow(l,6),g=c.pow(2,52),y=2*g,h=l-1;function n(n,t,r){function e(){for(var n=u.g(6),t=d,r=0;n<g;)n=(n+r)*l,t*=l,r=u.g(1);for(;y<=n;)n/=2,t/=2,r>>>=1;return(n+r)/t}var o=[],i=j(function n(t,r){var e,o=[],i=typeof t;if(r&&"object"==i)for(e in t)try{o.push(n(t[e],r-1))}catch(n){}return o.length?o:"string"==i?t:t+"\0"}((t=1==t?{entropy:!0}:t||{}).entropy?[n,S(a)]:null==n?function(){try{var n;return s&&(n=s.randomBytes)?n=n(l):(n=new Uint8Array(l),(f.crypto||f.msCrypto).getRandomValues(n)),S(n)}catch(n){var t=f.navigator,r=t&&t.plugins;return[+new Date,f,r,f.screen,S(a)]}}():n,3),o),u=new m(o);return e.int32=function(){return 0|u.g(4)},e.quick=function(){return u.g(4)/4294967296},e.double=e,j(S(u.S),a),(t.pass||r||function(n,t,r,e){return e&&(e.S&&v(e,u),n.state=function(){return v(u,{})}),r?(c[p]=n,t):n})(e,i,"global"in t?t.global:this==c,t.state)}function m(n){var t,r=n.length,u=this,e=0,o=u.i=u.j=0,i=u.S=[];for(r||(n=[r++]);e<l;)i[e]=e++;for(e=0;e<l;e++)i[e]=i[o=h&o+n[e%r]+(t=i[e])],i[o]=t;(u.g=function(n){for(var t,r=0,e=u.i,o=u.j,i=u.S;n--;)t=i[e=h&e+1],r=r*l+i[h&(i[e]=i[o=h&o+t])+(i[o]=t)];return u.i=e,u.j=o,r})(l)}function v(n,t){return t.i=n.i,t.j=n.j,t.S=n.S.slice(),t}function j(n,t){for(var r,e=n+"",o=0;o<e.length;)t[h&o]=h&(r^=19*t[h&o])+e.charCodeAt(o++);return S(t)}function S(n){return String.fromCharCode.apply(0,n)}if(j(c.random(),a),"object"==typeof module&&module.exports){module.exports=n;try{s=require("crypto")}catch(n){}}else"function"==typeof define&&define.amd?define(function(){return n}):c["seed"+p]=n}("undefined"!=typeof self?self:this,[],Math);
/**
 * Painter taken from https://css-houdini.rocks/posts/rough-boxes/paint.js
 * with more information available at https://css-houdini.rocks/rough-boxes
 */
/* eslint-disable-next-line */
class RoughRectangle extends class{constructor(rnd){this.rnd=rnd,this._fields={},this._dirty=!1,this._canvas=null,this.z=0,this.roughness=0,this.bowing=1,this._stroke=null,this._strokeWidth=null,this._fill=null,this._fillStyle=null,this._fillWeight=null,this._hachureAngle=null,this._hachureGap=null,this.maxRandomnessOffset=1,this._curveTightness=0}getOffset(t,s){return this.roughness*(this.rnd()*(s-t)+t)}drawLine(t,s,e,i,h,f){let l=Math.pow(s-i,2)+Math.pow(s-i,2),n=this.maxRandomnessOffset||0;n*n*100>l&&(n=Math.sqrt(l)/10);let g=n/2,o=.2+.2*this.rnd(),r=this.bowing*this.maxRandomnessOffset*(h-e)/200,a=this.bowing*this.maxRandomnessOffset*(s-i)/200;r=this.getOffset(-r,r),a=this.getOffset(-a,a),f||t.beginPath(),t.moveTo(s+this.getOffset(-n,n),e+this.getOffset(-n,n)),t.bezierCurveTo(r+s+(i-s)*o+this.getOffset(-n,n),a+e+(h-e)*o+this.getOffset(-n,n),r+s+2*(i-s)*o+this.getOffset(-n,n),a+e+2*(h-e)*o+this.getOffset(-n,n),i+this.getOffset(-n,n),h+this.getOffset(-n,n)),f||t.stroke(),f||t.beginPath(),t.moveTo(s+this.getOffset(-g,g),e+this.getOffset(-g,g)),t.bezierCurveTo(r+s+(i-s)*o+this.getOffset(-g,g),a+e+(h-e)*o+this.getOffset(-g,g),r+s+2*(i-s)*o+this.getOffset(-g,g),a+e+2*(h-e)*o+this.getOffset(-g,g),i+this.getOffset(-g,g),h+this.getOffset(-g,g)),f||t.stroke()}}{constructor(t,s,e,i,rnd){super(rnd),this.x=t,this.y=s,this.width=e,this.height=i}draw(t){let s=this.x,e=this.x+this.width,i=this.y,h=this.y+this.height;this.fill&&this._doFill(t,s,e,i,h),t.save(),t.strokeStyle=this.stroke,t.lineWidth=this.strokeWidth,this.drawLine(t,s,i,e,i),this.drawLine(t,e,i,e,h),this.drawLine(t,e,h,s,h),this.drawLine(t,s,h,s,i),t.restore()}_doFill(t,s,e,i,h){t.save(),t.fillStyle=this.fill;let f=this.maxRandomnessOffset||0;var l=[[s+this.getOffset(-f,f),i+this.getOffset(-f,f)],[e+this.getOffset(-f,f),i+this.getOffset(-f,f)],[e+this.getOffset(-f,f),h+this.getOffset(-f,f)],[s+this.getOffset(-f,f),h+this.getOffset(-f,f)]];t.beginPath(),t.moveTo(l[0][0],l[0][1]),t.lineTo(l[1][0],l[1][1]),t.lineTo(l[2][0],l[2][1]),t.lineTo(l[3][0],l[3][1]),t.fill(),t.restore()}};

registerPaint('rough-boxes', class {
  static get inputProperties() {
    return [
      '--rough-fill',
      '--rough-stroke-width',
      '--rough-stroke',
      '--rough-roughness',
      '--rough-seed'
    ];
  }

  paint(ctx, geom, properties) {
    const seed = properties.get('--rough-seed').toString();
    // eslint-disable-next-line @babel/new-cap
    const random = new Math.seedrandom(seed);

    const padding = 20;
    const rect = new RoughRectangle(padding, padding, geom.width - padding * 2, geom.height - padding * 2, random);
    rect.roughness = properties.get('--rough-roughness').toString();
    rect.fill = properties.get('--rough-fill').toString();
    rect.stroke = properties.get('--rough-stroke').toString();
    rect.strokeWidth = properties.get('--rough-stroke-width').toString().replace('px', '');
    rect.draw(ctx);
  }
});
