const PROXY_CONFIG = [
  {
    context: [
      "/api/todoitem",  // requests starting with this are not done on the current application...
    ],
    target: "https://localhost:7259",  // ...but are instead sent there
    // so, a request to https://localhost:4200/api/todoitem is instead sent to https://localhost:4200/api/todoitem
    secure: false  // if true, a certificate is required (not needed in development)
  }
]

module.exports = PROXY_CONFIG;
