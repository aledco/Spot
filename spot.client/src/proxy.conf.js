const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7108';

const PROXY_CONFIG = [
  //{
  //  context: [
  //    "/api/**",
  //  ],
  //  target,
  //  secure: false
  //},
  {
    "/api/**": {
      "target": "https://localhost:7108/api",
      "secure": false,
      "changeOrigin": true,
      "pathRewrite": {
        "^/api": ""
      }
    }
  }
]

module.exports = PROXY_CONFIG;
