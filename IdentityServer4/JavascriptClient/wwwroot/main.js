const IDENTITY_SERVER_URL = "https://localhost:5001";
const API_ONE_URL = "https://localhost:6001";
const JAVASCRIPT_CLIENT_URL = "https://localhost:9001";

const config = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  authority: IDENTITY_SERVER_URL,
  client_id: "client_id_js",
  redirect_uri: `${JAVASCRIPT_CLIENT_URL}/home/signin`,
  post_logout_redirect_uri: `${JAVASCRIPT_CLIENT_URL}/home/index`,
  response_type: "id_token token",
  scope: "openid ApiOne ApiTwo rc.scope",
};

const userManager = new Oidc.UserManager(config);

const signIn = function () {
  userManager.signinRedirect();
};

const signOut = function () {
  userManager.signoutRedirect();
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

var refreshing = false;

axios.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    console.error(error.response);

    // if error response is 401 try to refresh token
    if (error.response.status === 401) {
      console.log("axios error 401");

      // if already refreshing don't make another request
      if (!refreshing) {
        console.log("starting token refresh");
        refreshing = true;

        // do refresh
        return userManager.signinSilent().then((user) => {
          console.log(user);

          // update the http request and client
          axios.defaults.headers.common["Authorization"] =
            "Bearer " + user.access_token;
          axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;

          // retry the http request
          return axios(axiosConfig);
        });
      }
    }

    return Promise.reject(error);
  }
);
