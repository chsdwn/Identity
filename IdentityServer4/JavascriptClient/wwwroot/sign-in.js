const IDENTITY_SERVER_URL = "https://localhost:5001";
const JAVASCRIPT_CLIENT_URL = "https://localhost:9001";

const createNonce = function () {
  return "NonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValueNonceValue";
};

const createState = function () {
  return "SessionValueSessionValueSessionValueSessionValueSessionValueSessionValueSessionValueSessionValue";
};

const signIn = function () {
  const redirectUri = encodeURIComponent(
    `${JAVASCRIPT_CLIENT_URL}/home/signin`
  );
  const responseType = encodeURIComponent("id_token token");
  const scope = encodeURIComponent("openid ApiOne");

  const authUrl =
    `/connect/authorize/callback` +
    `?client_id=client_id_js` +
    `&redirect_uri=${redirectUri}` +
    `&response_type=${responseType}` +
    `&scope=${scope}` +
    `&nonce=${createNonce()}` +
    `&state=${createState()}`;

  const returnUrl = encodeURIComponent(authUrl);

  window.location.href = `${IDENTITY_SERVER_URL}/auth/login?ReturnUrl=${returnUrl}`;
};
