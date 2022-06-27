import { defineProperty } from '@ember/object';
import { A } from '@ember/array';
import { tracked } from '@glimmer/tracking';

export default class TrackedModelBase {
  constructor(source) {
    if(!source) {
      return;
    }

    for(let property in source) {
      if(Object.prototype.hasOwnProperty.call(this, property)) {
        console.warn(`Tried to overwrite property ${property}, this will be ignored and data might be unavailable.`);
        continue;
      }

      if(Object.prototype.hasOwnProperty.call(source, property)) {
        let value = source[property];
        if(Array.isArray(value)) {
          // eslint-disable-next-line @babel/new-cap
          value = A(value);
        }

        defineProperty(this, property, tracked({ value }));
      }
    }
  }

  toJSON() {
    let jsonObj = Object.assign({}, this);

    return jsonObj;
  }

  ensureProperty(property, defaultFunc) {
    if(typeof this[property] !== 'undefined' && this[property] !== null) {
      return;
    }

    const defaultValue = defaultFunc();
    try {
      defineProperty(this, property, tracked({ value: defaultValue }));
    } catch(e) {
      console.log(`Failed to properly define property ${property} as tracked. It was probably already defined.`);
    }
  }
}
