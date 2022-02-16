export let domain;
export let credentials;

  if(process.env.NODE_ENV !== 'production') {
      domain = "";
      credentials = "same-origin";
  }
  else {
      domain = "https://api.amigoserver.xyz";
      credentials = "include"; // т.к домен другой, то явно указываем крепеж куки(cors)
  }

