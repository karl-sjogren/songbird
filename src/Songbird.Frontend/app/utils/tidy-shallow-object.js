export default function tidyShallowObject(obj) {
  for(const key of Object.getOwnPropertyNames(obj)) {
    const value = obj[key];

    if(typeof value === 'boolean') {
      continue;
    }

    if(typeof value === 'number') {
      continue;
    }

    if(!value) {
      delete obj[key];
    }
  }
}
