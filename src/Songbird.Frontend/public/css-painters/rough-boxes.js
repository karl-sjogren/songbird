/* globals registerPaint */
/**
 * Painter taken from https://css-houdini.rocks/posts/rough-boxes/paint.js
 * with more information available at https://css-houdini.rocks/rough-boxes
 */
class RoughRectangle extends class{constructor(){this._fields={},this._dirty=!1,this._canvas=null,this.z=0,this.roughness=0,this.bowing=1,this._stroke=null,this._strokeWidth=null,this._fill=null,this._fillStyle=null,this._fillWeight=null,this._hachureAngle=null,this._hachureGap=null,this.maxRandomnessOffset=1,this._curveTightness=0}getOffset(t,s){return this.roughness*(Math.random()*(s-t)+t)}drawLine(t,s,e,i,h,f){let l=Math.pow(s-i,2)+Math.pow(s-i,2),n=this.maxRandomnessOffset||0;n*n*100>l&&(n=Math.sqrt(l)/10);let g=n/2,o=.2+.2*Math.random(),r=this.bowing*this.maxRandomnessOffset*(h-e)/200,a=this.bowing*this.maxRandomnessOffset*(s-i)/200;r=this.getOffset(-r,r),a=this.getOffset(-a,a),f||t.beginPath(),t.moveTo(s+this.getOffset(-n,n),e+this.getOffset(-n,n)),t.bezierCurveTo(r+s+(i-s)*o+this.getOffset(-n,n),a+e+(h-e)*o+this.getOffset(-n,n),r+s+2*(i-s)*o+this.getOffset(-n,n),a+e+2*(h-e)*o+this.getOffset(-n,n),i+this.getOffset(-n,n),h+this.getOffset(-n,n)),f||t.stroke(),f||t.beginPath(),t.moveTo(s+this.getOffset(-g,g),e+this.getOffset(-g,g)),t.bezierCurveTo(r+s+(i-s)*o+this.getOffset(-g,g),a+e+(h-e)*o+this.getOffset(-g,g),r+s+2*(i-s)*o+this.getOffset(-g,g),a+e+2*(h-e)*o+this.getOffset(-g,g),i+this.getOffset(-g,g),h+this.getOffset(-g,g)),f||t.stroke()}}{constructor(t,s,e,i){super(["x","y","width","height"]),this.x=t,this.y=s,this.width=e,this.height=i}draw(t){let s=this.x,e=this.x+this.width,i=this.y,h=this.y+this.height;this.fill&&this._doFill(t,s,e,i,h),t.save(),t.strokeStyle=this.stroke,t.lineWidth=this.strokeWidth,this.drawLine(t,s,i,e,i),this.drawLine(t,e,i,e,h),this.drawLine(t,e,h,s,h),this.drawLine(t,s,h,s,i),t.restore()}_doFill(t,s,e,i,h){t.save(),t.fillStyle=this.fill;let f=this.maxRandomnessOffset||0;var l=[[s+this.getOffset(-f,f),i+this.getOffset(-f,f)],[e+this.getOffset(-f,f),i+this.getOffset(-f,f)],[e+this.getOffset(-f,f),h+this.getOffset(-f,f)],[s+this.getOffset(-f,f),h+this.getOffset(-f,f)]];t.beginPath(),t.moveTo(l[0][0],l[0][1]),t.lineTo(l[1][0],l[1][1]),t.lineTo(l[2][0],l[2][1]),t.lineTo(l[3][0],l[3][1]),t.fill(),t.restore()}}; /* eslint-disable-line */

registerPaint('rough-boxes', class {
  static get inputProperties() {
    return [
      '--rough-fill',
      '--rough-stroke-width',
      '--rough-stroke',
      '--rough-roughness'
    ];
  }

  paint(ctx, geom, properties) {
    const padding = 20;
    var rect = new RoughRectangle(padding, padding, geom.width - padding * 2, geom.height - padding * 2);
    rect.roughness = properties.get('--rough-roughness').toString();
    rect.fill = properties.get('--rough-fill').toString();
    rect.stroke = properties.get('--rough-stroke').toString();
    rect.strokeWidth = properties.get('--rough-stroke-width').toString().replace('px', '');
    rect.draw(ctx);
  }
});
