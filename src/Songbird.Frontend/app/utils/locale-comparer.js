import { isHTMLSafe } from '@ember/template';

const comparerCache = {};

const getComparer = locale => {
  if(!!comparerCache[locale]) {
    return comparerCache[locale];
  }

  const comparer = new Intl.Collator(locale, { sensitivity: 'accent' }).compare;
  return comparerCache[locale] = comparer;
};

const localeCompareSupportsLocales = (function() {
  try {
    'foo'.localeCompare('bar', 'i');
  } catch(e) {
    return e.name === 'RangeError';
  }
  return false;
})();

export default function localeComparer(a, b, locale = 'sv') {
  if(!a && !!b) {
    return 1;
  } else if(!!a && !b) {
    return -1;
  } else if(!a && !b) {
    return 0;
  }

  let shouldWarn = false;
  if(typeof a !== 'string') {
    if(isHTMLSafe(a)) {
      // This is an Ember SafeString, that is ok
      a = a.string;
    } else {
      a = a.toString();
      shouldWarn = true;
    }
  }

  if(typeof b !== 'string') {
    if(isHTMLSafe(b)) {
      // This is an Ember SafeString, that is ok
      b = b.string;
    } else {
      b = b.toString();
      shouldWarn = true;
    }
  }

  if(shouldWarn) {
    console.warn('Trying to compare non-string objects with localeComparer.', a, b);
  }

  // The regex matches a range from space to tilde, which isn't really that clear
  // but it is bacily the ASCII range
  let asciiStrings = false;
  let asciiRegex = /^[ -~]*$/;
  if(asciiRegex.test(a) && asciiRegex.test(b)) {
    asciiStrings = true;
  }

  if(asciiStrings || !localeCompareSupportsLocales) {
    if(a < b) {
      return -1;
    } else if(a > b) {
      return 1;
    } else {
      return 0;
    }
  }

  const comparer = getComparer(locale);
  return comparer(a, b);
}