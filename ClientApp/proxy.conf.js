const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "http://localhost:5200";

const PROXY_CONFIG = [
  {
    context: ["/weatherforecast"],
    target: target,
    secure: false,
    headers: {
      Connection: "Keep-Alive",
    },
  },
  {
    "/api": {
      target: "http://backend:5000",
      secure: false,
    },
  },
];

module.exports = PROXY_CONFIG;
