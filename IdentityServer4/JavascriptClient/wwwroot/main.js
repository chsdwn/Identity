const IDENTITY_SERVER_URL = "https://localhost:5001";
const API_ONE_URL = "https://localhost:6001";
const JAVASCRIPT_CLIENT_URL = "https://localhost:9001";

const config = {
  authority: IDENTITY_SERVER_URL,
  client_id: "client_id_js",
  redirect_uri: `${JAVASCRIPT_CLIENT_URL}/Home/SignIn`,
  response_type: "id_token token",
  scope: "openid ApiOne",
};

const userManager = new Oidc.UserManager(config);

const signIn = function () {
  userManager.signinRedirect();
};

userManager.getUser().then((user) => {
  console.log("user", user);
  if (user) {
    axios.defaults.headers.common["Authorization"] =
      "Bearer " + user.access_token;
  }
});

const callApi = function () {
  axios.get(`${API_ONE_URL}/secret`).then(console.log);
};
