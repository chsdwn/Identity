const extractTokens = function (address) {
  const returnValue = address.split("#")[1];
  const values = returnValue.split("&");

  values.forEach((value) => {
    const keyValuePair = value.split("=");

    localStorage.setItem(keyValuePair[0], keyValuePair[1]);
  });

  window.location.href = "/";
};

extractTokens(window.location.href);
