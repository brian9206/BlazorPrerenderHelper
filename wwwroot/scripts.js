window.ssrInterop = {
  isSSR() {
    return typeof (ssrHint) !== 'undefined' ? 1 : 0
  },
  getHint(key, preserve) {
    if (typeof (ssrHint) === 'undefined' || !Object.keys(ssrHint).includes(key)) {
      return {isFound: false}
    } else {
      const result = {
        isFound: true,
        result: ssrHint[key]
      }
      
      if (!preserve) {
        delete ssrHint[key]
      }
      
      return result
    }
  },
  getHints() {
    return typeof (ssrHint) !== 'undefined' ? ssrHint : {}
  }
}