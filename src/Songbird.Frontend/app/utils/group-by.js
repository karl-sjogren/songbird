// https://stackoverflow.com/a/34890276/547640
/**
 * @param {Array} arr The array of items to group
 * @param {String} key The ket to group by
 */
export default function groupBy(arr, key) {
  return arr.reduce(function(rv, x) {
    (rv[x[key]] = rv[x[key]] || []).push(x);
    return rv;
  }, {});
}
