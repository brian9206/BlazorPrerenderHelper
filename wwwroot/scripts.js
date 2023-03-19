window.ssrInterop = {
  isSSR() {
    return typeof (ssrHint) !== 'undefined' ? 1 : 0
  },
  getHint(key) {
    if (typeof (ssrHint) === 'undefined' || !Object.keys(ssrHint).includes(key)) {
      return {isFound: false}
    } else {
      return {
        isFound: true,
        result: ssrHint[key]
      }
    }
  },
  getHints() {
    return typeof (ssrHint) !== 'undefined' ? ssrHint : {}
  }
}